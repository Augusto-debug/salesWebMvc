using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services{
    public class SalesRecordService(SalesWebMvcContext context){
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate){
            var result = from obj in context.SalesRecord select obj;

            if (minDate.HasValue){
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue){
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller!.Department)
                .AsSplitQuery()
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate,
            DateTime? maxDate){
            var result = from obj in context.SalesRecord select obj;

            if (minDate.HasValue){
                result = result.Where(x => x.Date >= minDate.Value);
            }

            if (maxDate.HasValue){
                result = result.Where(x => x.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)
                .Include(x => x.Seller!.Department)
                .AsSplitQuery()
                .GroupBy(x => x.Seller!.Department!)
                .ToListAsync();
        }
    }
}