using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers{
    public class DepartmentsController(DepartmentService departmentService) : Controller{
        
        public async Task<IActionResult> Index(){
            return View(await departmentService.FindAllAsync());
        }

        public async Task<IActionResult> Details(int? id){
            if (id == null){
                return NotFound();
            }

            var department = await departmentService.FindByIdAsync(id.Value);
            if (department == null){
                return NotFound();
            }

            return View(department);
        }

        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department){
            if (ModelState.IsValid){
                await departmentService.InsertAsync(department);
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        public async Task<IActionResult> Edit(int? id){
            if (id == null){
                return NotFound();
            }

            var department = await departmentService.FindByIdAsync(id.Value);
            if (department == null){
                return NotFound();
            }

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department){
            if (id != department.Id){
                return NotFound();
            }

            if (ModelState.IsValid){
                try{
                    await departmentService.UpdateAsync(department);
                }
                catch (DbUpdateConcurrencyException){
                    if (!await DepartmentExists(id)){
                        return NotFound();
                    }
                    else{
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        public async Task<IActionResult> Delete(int? id){
            if (id == null){
                return NotFound();
            }

            var department = await departmentService.FindByIdAsync(id.Value);
            if (department == null){
                return NotFound();
            }

            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id){
            await departmentService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> DepartmentExists(int id){
            return await departmentService.ExistsAsync(id);
        }
    }
}