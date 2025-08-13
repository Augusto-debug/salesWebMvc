using System.ComponentModel.DataAnnotations;

namespace SalesWebMvc.Models;

public class Seller{
    public int Id{ get; init; }

    [Display(Name = "Nome")]
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [StringLength(60, MinimumLength = 3, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.")]
    public string? Name{ get; set; }

    [Display(Name = "Email")]
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [EmailAddress(ErrorMessage = "O campo {0} deve ser um email válido.")]
    [StringLength(60, MinimumLength = 3)]
    public string? Email{ get; set; }

    [Display(Name = "Data de Nascimento")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public DateTime BirthDate{ get; set; }

    [Display(Name = "Salário Base")]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    [Range(100.0, 50000.0, ErrorMessage = "O campo {0} deve estar entre {1} e {2}.")]
    public double BaseSalary{ get; set; }

    [Display(Name = "Departamento")] public int DepartmentId{ get; set; }

    public Department? Department{ get; set; }
    public List<SalesRecord> Sales{ get; init; } = new List<SalesRecord>();

    public Seller(){
    }

    public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department){
        Id = id;
        Name = name;
        Email = email;
        BirthDate = birthDate;
        BaseSalary = baseSalary;
        DepartmentId = department.Id;
        Department = department;
    }

    public void AddSales(SalesRecord sale){
        Sales.Add(sale);
    }

    public void RemoveSales(SalesRecord sale){
        Sales.Remove(sale);
    }

    public double TotalSales(DateTime startDate, DateTime endDate){
        return Sales
            .Where(sale => sale.Date >= startDate && sale.Date <= endDate)
            .Sum(sale => sale.Amount);
        // double total = 0.0;
        // for( int i = 0; i < Sales.Count; i++){
        //     if(Sales[i].Date >= startDate && Sales[i].Date <= endDate){
        //         total += Sales[i].Amount;
        //     }
        // }
        // return total;
    }

    // public void f(){
    //     var lista = new List<string>();
    //     lista.Where(f => f.Contains("a"));
    // }
    //
    // public void f2(Func<string, bool> filtro){
    //     IEnumerable<string> lista = new List<string>();
    //     lista = lista.Where(filtro);
    // }
    //
    // public void f3(Expression<Func<string, bool>> filtro){
    //     IEnumerable<string> lista = new List<string>();
    //     lista = lista.Where(f => f.Contains("a"));
    // }

    public override string ToString(){
        return
            $"Id: {Id}, Name: {Name}, Email: {Email}, BirthDate: {BirthDate.ToShortDateString()}, BaseSalary: {BaseSalary:C}, Department: {Department?.Name}";
    }
}