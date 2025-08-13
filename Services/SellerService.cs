using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services;

public class SellerService(SalesWebMvcContext context){
    public async Task<List<Seller>> FindAllAsync(){
        return await context.Seller.Include(x => x.Department).ToListAsync();
    }

    public async Task InsertAsync(Seller obj){
        context.Add(obj);
        await context.SaveChangesAsync();
    }

    public async Task<Seller?> FindByIdAsync(int id){
        return await context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id);
    }

    public async Task UpdateAsync(Seller obj){
        var existingSeller = await context.Seller.FindAsync(obj.Id);
        if (existingSeller == null){
            throw new NotFoundException("Id not found");
        }
        try{
            existingSeller.Name = obj.Name;
            existingSeller.Email = obj.Email;
            existingSeller.BirthDate = obj.BirthDate;
            existingSeller.BaseSalary = obj.BaseSalary;
            existingSeller.DepartmentId = obj.DepartmentId;
            
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException e){
            throw new DbConcurrencyException(e.Message);
        }
    }
    
    public async Task RemoveAsync(int id)
    {
        try
        {
            var obj = await context.Seller.FindAsync(id);
            if (obj == null)
            {
                throw new NotFoundException("Id not found");
            }
            context.Seller.Remove(obj);
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException )
        {
            throw new IntegrityException("Can't delete seller because he/she has sales");
        }
    }
}   