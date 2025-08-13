using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services;

public class DepartmentService (SalesWebMvcContext context){
    public async Task<List<Department>> FindAllAsync(){
        return await context.Department.Include(x => x.Sellers).OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Department?> FindByIdAsync(int id){
        return await context.Department
            .Include(d => d.Sellers)
            .FirstOrDefaultAsync(obj => obj.Id == id);
    }

    public async Task InsertAsync(Department department){
        context.Add(department);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department){
        context.Update(department);
        await context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id){
        var department = await context.Department.FindAsync(id);
        if (department != null) {
            context.Department.Remove(department);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id){
        return await context.Department.AnyAsync(e => e.Id == id);
    }
}