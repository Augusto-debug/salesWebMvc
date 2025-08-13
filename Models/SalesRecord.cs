using System.ComponentModel.DataAnnotations;
using SalesWebMvc.Models.Enums;

namespace SalesWebMvc.Models;

public class SalesRecord{
    public int Id{ get; init; }
    
    [Display(Name = "Data")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime Date{ get; init; }
    
    [Display(Name = "Valor")]
    [DisplayFormat(DataFormatString = "{0:F2}")]
    public double Amount{ get; init; }
    
    [Display(Name = "Status")]
    public SaleStatus Status{ get; init; }
    
    [Display(Name = "Vendedor")]
    public Seller? Seller{ get; init; }

    public SalesRecord(){
    }

    public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller){
        Id = id;
        Date = date;
        Amount = amount;
        Status = status;
        Seller = seller;
    }
}