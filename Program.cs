using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("SalesWebMvcContext"),
        new MySqlServerVersion(new Version(8, 0, 21))
    ));
builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<SalesRecordService>();

builder.Services.AddControllersWithViews();


var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(enUS),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
};

var app = builder.Build();

app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment()){
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<SalesWebMvcContext>();
    var seedingService = scope.ServiceProvider.GetRequiredService<SeedingService>();

    try{
        context.Database.Migrate();
        seedingService.Seed();
    }
    catch (Exception ex){
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrações ou seeding do banco de dados.");
    }
}

if (!app.Environment.IsDevelopment()){
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRequestLocalization(localizationOptions);
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();