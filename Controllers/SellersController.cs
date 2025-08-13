using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModel;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers{
    public class SellersController(SellerService sellerService, DepartmentService departmentService) : Controller{
        public async Task<IActionResult> Index(){
            var sellers = await sellerService.FindAllAsync();
            return View(sellers);
        }

        public async Task<IActionResult> Details(int? id){
            if (id == null){
                return RedirectToAction("Error", new{ message = "Id não pode ser nulo" });
            }

            try{
                var seller = await sellerService.FindByIdAsync(id.Value);
                return View(seller);
            }
            catch (NotFoundException){
                return RedirectToAction("Error", new{ message = "Id não encontrado" });
            }
        }

        public async Task<IActionResult> Create(){
            var departments = await departmentService.FindAllAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>
            Create([Bind("Name,Email,BirthDate,BaseSalary,DepartmentId")] Seller seller){
            ModelState.Remove("Department");

            if (ModelState.IsValid){
                await sellerService.InsertAsync(seller);
                return RedirectToAction(nameof(Index));
            }

            var departments = await departmentService.FindAllAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", seller.DepartmentId);
            return View(seller);
        }

        public async Task<IActionResult> Edit(int? id){
            if (id == null){
                return RedirectToAction("Error", new{ message = "Id não pode ser nulo" });
            }

            try{
                var seller = await sellerService.FindByIdAsync(id.Value);
                if (seller == null){
                    return RedirectToAction("Error", new{ message = "Vendedor não encontrado" });
                }

                var departments = await departmentService.FindAllAsync();
                ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", seller.DepartmentId);
                return View(seller);
            }
            catch (NotFoundException){
                return RedirectToAction("Error", new{ message = "Id não encontrado" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Name,Email,BirthDate,BaseSalary,DepartmentId")]
            Seller seller){
            if (id != seller.Id){
                return RedirectToAction("Error", new{ message = "Id não corresponde ao vendedor informado" });
            }

            ModelState.Remove("Department");

            if (ModelState.IsValid){
                try{
                    await sellerService.UpdateAsync(seller);
                    return RedirectToAction(nameof(Index));
                }
                catch (NotFoundException){
                    return RedirectToAction("Error", new{ message = "Vendedor não encontrado" });
                }
                catch (DbConcurrencyException){
                    return RedirectToAction("Error", new{ message = "Erro de concorrência" });
                }
            }

            var departments = await departmentService.FindAllAsync();
            ViewBag.DepartmentId = new SelectList(departments, "Id", "Name", seller.DepartmentId);
            return View(seller);
        }

        public async Task<IActionResult> Delete(int? id){
            if (id == null){
                return RedirectToAction("Error", new{ message = "Id não pode ser nulo" });
            }

            try{
                var seller = await sellerService.FindByIdAsync(id.Value);
                return View(seller);
            }
            catch (NotFoundException){
                return RedirectToAction("Error", new{ message = "Vendedor não encontrado" });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id){
            try{
                await sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException){
                return RedirectToAction("Error", new{ message = "Vendedor não encontrado" });
            }
        }

        public IActionResult Error(string message){
            var errorViewModel = new ErrorViewModel{
                Message = message,
                RequestId = HttpContext.TraceIdentifier
            };
            return View(errorViewModel);
        }
    }
}