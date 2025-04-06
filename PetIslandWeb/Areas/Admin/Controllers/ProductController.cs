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
//[Authorize(Roles = SD.Role_Employee)]
[Route("Admin/Product")]
public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ProductController(IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var objCatagoryList = await _context.Products.OrderByDescending(p => p.Id).Include(c => c.ProductCategory).Include(b => b.Brand).ToListAsync();
        return View(objCatagoryList);
    }

    [Route("CreateProductQuantity")]
    [HttpGet]
    public async Task<IActionResult> CreateProductQuantity(long Id)
    {
        var productbyquantity = await _context.ProductQuantities.Where(pq => pq.ProductId == Id).ToListAsync();
        ViewBag.ProductByQuantity = productbyquantity;
        ViewBag.ProductId = Id;
        return View();
    }

    [Route("UpdateMoreQuantity")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateMoreQuantity(ProductQuantityModel productQuantityModel)
    {
        var product = _context.Products.Find(productQuantityModel.ProductId);

        if (product == null)
        {
            return NotFound(); // Handle product not found scenario
        }
        product.Quantity += productQuantityModel.Quantity;

        productQuantityModel.Quantity = productQuantityModel.Quantity;
        productQuantityModel.ProductId = productQuantityModel.ProductId;
        productQuantityModel.DateTime = DateTime.Now;

        _context.Add(productQuantityModel);
        _context.SaveChangesAsync();
        TempData["success"] = "Thêm số lượng sản phẩm thành công";
        return RedirectToAction("CreateProductQuantity", "Product", new { Id = productQuantityModel.ProductId });
    }

    [Route("Create")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name");
        ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name");
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

    [Route("Edit")]
    public async Task<IActionResult> Edit(long Id)
    {
        ProductModel? product = await _context.Products.FindAsync(Id);
        if (product == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);
        ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name", product.BrandId);
        return View(product);
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductModel product)
    {
        var existed_product = _context.Products.Find(product.Id)!; //tìm sp theo id product
        ViewBag.Categories = new SelectList(_context.ProductCategory, "Id", "Name", product.ProductCategoryId);
        ViewBag.Brands = new SelectList(_context.Brands, "Id", "Name", product.BrandId);

        if (ModelState.IsValid)
        {
            product.Slug = product.Name.Replace(" ", "-");

            if (product.ImageUpload != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadsDir, imageName);

                var fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                existed_product.Image = imageName;
            }

            existed_product.Name = product.Name;
            existed_product.Description = product.Description;
            existed_product.Price = product.Price;
            existed_product.ProductCategoryId = product.ProductCategoryId;
            existed_product.BrandId = product.BrandId;
            existed_product.CreatedDate = DateTime.Now;
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
        if (!string.Equals(product.Image, "null.jpg"))
        {
            string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
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
}
