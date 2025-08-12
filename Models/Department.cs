namespace SalesWebMvc.Models;

public class Department{
    public int Id{ get; set; }
    public string Name{ get; set; }
    public List<Seller> Sellers{ get; set; } = new List<Seller>();

    public Department(){
    }

    public Department(int id, string name){
        Id = id;
        Name = name;
    }

    public void AddSeller(Seller seller){
        Sellers.Add(seller);
        seller.Department = this; 
    }

    public double TotalSales(DateTime startDate, DateTime endDate){
        // double total = 0.0;
        // foreach (var seller in Sellers){
        //     total += seller.Sales
        //         .Where(sale => sale.Date >= startDate && sale.Date <= endDate)
        //         .Sum(sale => sale.Amount);
        // }
        // return total;
        // var result2 = Sellers
        //     .SelectMany(sellers => sellers.Sales)
        //     .Where(sale => sale.Date >= startDate && sale.Date <= endDate)
        //     .Sum(sale => sale.Amount);
        return Sellers.Sum(sale => sale.TotalSales(startDate, endDate));
    }


    public override string ToString(){
        return $"Id: {Id}, Name: {Name}, Sellers Count: {Sellers.Count}";
    }
}