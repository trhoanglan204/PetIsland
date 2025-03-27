using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PetIsland.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
//[Authorize(Roles = SD.Role_Admin)]
[Route("Admin/Product")]
public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        //List<ProductModel> objCatagoryList = (await _unitOfWork.Product.GetAllAsync(includeProperties:"ProductCategory")).ToList();
        //return View(objCatagoryList);
        return View(await _context.Products.OrderByDescending(p => p.Id).Include(c => c.ProductCategory).ToListAsync());
    }

    [Route("Create")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name");
        return View();
    }

    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductModel product)
    {
        ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);

        if (ModelState.IsValid)
        {
            product.Slug = product.Name.Replace(" ", "-");
            var slug = await _context.Products.FirstOrDefaultAsync(p => p.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "Sản phẩm đã có trong database");
                return View(product);
            }

            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                var fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                product.Image = imageName;
            }

            _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["success"] = "Thêm sản phẩm thành công";
            return RedirectToAction("Index");

        }
        else
        {
            TempData["error"] = "Model có một vài thứ đang lỗi";
            List<string> errors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            string errorMessage = string.Join("\n", errors);
            return BadRequest(errorMessage);
        }
    }

    [Route("Edit")]
    public async Task<IActionResult> Edit(long Id)
    {
        ProductModel? product = await _context.Products.FindAsync(Id);
        if (product == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);

        return View(product);
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductModel product)
    {
        var existed_product = _context.Products.Find(product.Id)!; //tìm sp theo id product
        ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);

        if (ModelState.IsValid)
        {
            product.Slug = product.Name.Replace(" ", "-");

            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                existed_product.Image = imageName;
            }


            existed_product.Name = product.Name;
            existed_product.Description = product.Description;
            existed_product.Price = product.Price;
            existed_product.ProductCategoryId = product.ProductCategoryId;
            _context.Update(existed_product);
            await _context.SaveChangesAsync();
            TempData["success"] = "Cập nhật sản phẩm thành công";
            return RedirectToAction("Index");

        }
        else
        {
            TempData["error"] = "Model có một vài thứ đang lỗi";
            var errors = new List<string>();
            foreach (var value in ModelState.Values)
            {
                foreach (var error in value.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }
            }
            string errorMessage = string.Join("\n", errors);
            return BadRequest(errorMessage);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Delete(long Id)
    {
        ProductModel? product = await _context.Products.FindAsync(Id);
        if (product == null)
        {
            return NotFound();
        }
        if (!string.Equals(product.Image, "noname.jpg"))
        {
            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/products");
            string oldfilePath = Path.Combine(uploadsDir, product.Image);
            if (System.IO.File.Exists(oldfilePath))
            {
                System.IO.File.Delete(oldfilePath);
            }
        }
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        TempData["success"] = "sản phẩm đã được xóa thành công";
        return RedirectToAction("Index");
    }
    //Update and Insert  
    public async Task<IActionResult> Upsert(int? id)
    {
        ProductVM productVM = new()
        {
            CategoryList = (await _unitOfWork.ProductCategory.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Product = new ProductModel()
        };
        if (id==null || id == 0)
        {
            //Create
            return View(productVM);
        }
        else
        {
            //Update
            productVM.Product = (await _unitOfWork.Product.GetAsync(u=>u.Id==id, includeProperties: "ProductImages"))!;
            return View(productVM);
        }

    }
    [HttpPost]
    public async Task<IActionResult> Upsert(ProductVM productVM, List<IFormFile> files)
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
            if (productVM.Product.Id==0)
            {
                _unitOfWork.Product.Add(productVM.Product);

                TempData["success"] = "Product Created Successfully";
            }
            else
            {
                await _unitOfWork.Product.UpdateAsync(productVM.Product);

                TempData["success"] = "Product Updated Successfully";
            }
            await _unitOfWork.SaveAsync();
            string wwwRootPath = _webHostEnvironment.WebRootPath;

            if (files != null)
            {

                foreach (IFormFile file in files)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = @"images\products\product-" + productVM.Product.Id;
                    string finalPath = Path.Combine(wwwRootPath, productPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    ProductImageModel productImage = new()
                    {
                        ImageUrl = @"\" + productPath + @"\" + fileName,
                        ProductId = productVM.Product.Id,
                    };

                    if (productVM.Product.ProductImages == null)
                        productVM.Product.ProductImages = [];

                    productVM.Product.ProductImages.Add(productImage);

                }

                await _unitOfWork.Product.UpdateAsync(productVM.Product);
                await _unitOfWork.SaveAsync();

            }

            TempData["success"] = "Product created/updated successfully";

            return RedirectToAction("Index");
        }
        else
        {
            productVM.CategoryList = (await _unitOfWork.ProductCategory.GetAllAsync()).Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            return View(productVM);
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
        List<ProductModel> objProductList = (await _unitOfWork.Product.GetAllAsync(
            includeProperties:"Category")).ToList();
        return Json(new { data = objProductList });

    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int? id)
    {
        var productToBeDeleted = await _unitOfWork.Product.GetAsync(u=>u.Id == id);
        if ( productToBeDeleted == null )
        {
            return Json(new { success = false, Message = "Error While Deleting" });
        }

        string productPath = @"images\products\product-" + id;
        string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, productPath);

        if (Directory.Exists(finalPath))
        {
            string[] filePaths = Directory.GetFiles(finalPath);
            foreach (string filePath in filePaths)
            {
                System.IO.File.Delete(filePath);
            }

            Directory.Delete(finalPath);
        }

        _unitOfWork.Product.Remove(productToBeDeleted);
        await _unitOfWork.SaveAsync();

        return Json(new { success = true, message = "Delete Successful" });
    }
    #endregion
}
