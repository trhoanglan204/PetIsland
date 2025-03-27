using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class PetController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PetController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            List<PetModel> objCatagoryList = (await _unitOfWork.Pet.GetAllAsync(includeProperties: "PetCategory")).ToList();
            return View(objCatagoryList);
        }
        //Update and Insert  
        public async Task<IActionResult> Upsert(int? id)
        {
            PetVM petVM = new()
            {
                CategoryList = (await _unitOfWork.PetCategory.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Pet = new PetModel()
            };
            if (id == null || id == 0)
            {
                //Create
                return View(petVM);
            }
            else
            {
                //Update
                petVM.Pet = await _unitOfWork.Pet.GetAsync(u => u.Id == id, includeProperties: "PetImages");
                return View(petVM);
            }

        }
        [HttpPost]
        public async Task<IActionResult> Upsert(PetVM petVM, List<IFormFile> files)
        {
            //if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
            //{
            //    //Delete Old Image 
            //    var oldImagePath =
            //        Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

            //    if (System.IO.File.Exists(oldImagePath))
            //    {
            //        System.IO.File.Delete(oldImagePath);
            //    }

            //}
            //using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
            //{
            //    file.CopyTo(fileStream);
            //}
            //productVM.Product.ImageUrl = @"\images\product\" + fileName;

            if (ModelState.IsValid)
            {
                if (petVM.Pet.Id == 0)
                {
                    _unitOfWork.Pet.Add(petVM.Pet);

                    TempData["success"] = "Product Created Successfully";
                }
                else
                {
                    _unitOfWork.Pet.Update(petVM.Pet);

                    TempData["success"] = "Product Updated Successfully";
                }
                await _unitOfWork.SaveAsync();
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (files != null)
                {

                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\pets\pet-" + petVM.Pet.Id;
                        string finalPath = Path.Combine(wwwRootPath, productPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        PetImageModel petImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            PetId = petVM.Pet.Id,
                        };

                        if (petVM.Pet.PetImages == null)
                            petVM.Pet.PetImages = [];

                        petVM.Pet.PetImages.Add(petImage);

                    }

                    _unitOfWork.Pet.Update(petVM.Pet);
                    await _unitOfWork.SaveAsync();

                }

                TempData["success"] = "Product created/updated successfully";

                return RedirectToAction("Index");
            }
            else
            {
                petVM.CategoryList = (await _unitOfWork.PetCategory.GetAllAsync()).Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(petVM);
            }
        }

        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var imageToBeDeleted = await _unitOfWork.ProductImage.GetAsync(u => u.Id == imageId);
            int productId = imageToBeDeleted.ProductId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                   imageToBeDeleted.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.ProductImage.Remove(imageToBeDeleted);
                await _unitOfWork.SaveAsync();

                TempData["success"] = "Deleted successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = productId });
        }


        #region API CALLS
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<PetModel> objProductList = (await _unitOfWork.Pet.GetAllAsync(
                includeProperties: "PetCategory")).ToList();
            return Json(new { data = objProductList });

        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            var petToBeDeleted = await _unitOfWork.Pet.GetAsync(u => u.Id == id);
            if (petToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error While Deleting" });
            }

            string petPath = @"images\pets\pet-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, petPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }

            _unitOfWork.Pet.Remove(petToBeDeleted);
            await _unitOfWork.SaveAsync();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }

}
