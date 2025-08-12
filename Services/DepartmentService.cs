using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services;

public class DepartmentService{
    
    private readonly SalesWebMvcContext _context;

    public DepartmentService(SalesWebMvcContext context){
        _context = context;
    }

    public async Task<List<Department>> FindAllAsync(){
        return await _context.Department.Include(x => x.Sellers).OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Department?> FindByIdAsync(int id){
        return await _context.Department
            .Include(d => d.Sellers)
            .FirstOrDefaultAsync(obj => obj.Id == id);
    }

    public async Task InsertAsync(Department department){
        _context.Add(department);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Department department){
        _context.Update(department);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(int id){
        var department = await _context.Department.FindAsync(id);
        if (department != null) {
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id){
        return await _context.Department.AnyAsync(e => e.Id == id);
    }
}