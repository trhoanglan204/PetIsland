using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;
using PetIsland.Utility;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class PetCatergoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public PetCatergoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<IActionResult> Index()
    {
        List<PetCategoryModel> objCatagoryList = (await _unitOfWork.PetCategory.GetAllAsync()).ToList();

        return View(objCatagoryList);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(PetCategoryModel obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
        }

        if (obj.Name != null && obj.Name.Equals("test", StringComparison.CurrentCultureIgnoreCase))
        {
            ModelState.AddModelError("Name", "Test is invalid value");
        }

        if (ModelState.IsValid)
        {
            _unitOfWork.PetCategory.Add(obj);
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
        PetCategoryModel? categoryFromDb = await _unitOfWork.PetCategory.GetAsync(u => u.Id == id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(PetCategoryModel obj)
    {

        if (ModelState.IsValid)
        {
            _unitOfWork.PetCategory.Update(obj);
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
        PetCategoryModel? categoryFromDb = await _unitOfWork.PetCategory.GetAsync(u => u.Id == id);

        if (categoryFromDb == null)
        {
            return NotFound();
        }
        return View(categoryFromDb);
    }
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        PetCategoryModel? obj = await _unitOfWork.PetCategory.GetAsync(u => u.Id == id);
        if (obj == null)
        {
            return NotFound();
        }
        _unitOfWork.PetCategory.Remove(obj);
        await _unitOfWork.SaveAsync();
        TempData["success"] = "Category Deleted Successfully";

        return RedirectToAction("Index");
    }
}
