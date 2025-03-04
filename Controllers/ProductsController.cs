namespace PetIsland.Controllers;

#pragma warning disable IDE0290

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        this._context = context;
        this._env = env;
    }

    //GET: Products
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }

    // Get: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // GET: Products/Add
    public IActionResult Add()
    {
        return View();
    }

    //POST: Products/Add
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(ProductDto item)
    {
        if (item.ImageFile == null)
        {
            ModelState.AddModelError("ImageFile", "The image file is required");
        }
        if (!ModelState.IsValid)
        {
            return View(item);

        }
        string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(item.ImageFile!.FileName);
        string imageFilePath = _env.WebRootPath + "/images/products/" + newFileName;
        using (var stream = System.IO.File.Create(imageFilePath))
        {
            await item.ImageFile!.CopyToAsync(stream);
        }
        Products product = new Products
        {
            Name = item.Name,
            ImageUrl = newFileName,
            Tags = item.Tags,
            Price = item.Price,
            Description = item.Description,
            CreatedDate = DateTime.Now
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit([FromRoute] int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var item = await _context.Products.FindAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        return View();
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind()] Products item)
    {
        if (id != item.Id)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(item.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(item);
    }

    // GET Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var item = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);
        if (item == null)
        {
            return NotFound();
        }

        return View(item);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.Products.FindAsync(id);
        if (item != null)
        {
            _context.Products.Remove(item);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool ProductsExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
