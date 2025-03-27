using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductCategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ProductCategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IActionResult> Index()
    {
        List<ProductCategoryModel> objCatagoryList = (await _unitOfWork.ProductCategory.GetAllAsync()).ToList();

        return View(objCatagoryList);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ProductCategoryModel obj)
    {
        if (obj.Name != null && obj.Name.Equals("test", StringComparison.CurrentCultureIgnoreCase))
        {
            ModelState.AddModelError("Name", "Test is invalid value");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.ProductCategory.Add(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Category Created Successfully";
            return RedirectToAction("Index");
        }
        return View();
    }
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        ProductCategoryModel? categoryFromDb = await _unitOfWork.ProductCategory.GetAsync(u => u.Id == id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(ProductCategoryModel obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.ProductCategory.Update(obj);
            await _unitOfWork.SaveAsync();
            TempData["success"] = "Category Updated Successfully";

            return RedirectToAction("Index");
        }
        return View();
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id == 0)
        {
            return NotFound();
        }
        ProductCategoryModel? categoryFromDb = await _unitOfWork.ProductCategory.GetAsync(u => u.Id == id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        ProductCategoryModel? obj = await _unitOfWork.ProductCategory.GetAsync(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }
        _unitOfWork.ProductCategory.Remove(obj);
        await _unitOfWork.SaveAsync();
        TempData["success"] = "Category Deleted Successfully";

        return RedirectToAction("Index");
    }
}
