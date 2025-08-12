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

    public async Task<Department> FindByIdAsync(int id){
        return await _context.Department
            .Include(d => d.Sellers)
            .FirstOrDefaultAsync(obj => obj.Id == id);
    }
}
