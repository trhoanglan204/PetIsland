namespace PetIsland.Controllers;

public class PetsController : Controller
{
    private readonly ApplicationDbContext _context;

    public PetsController(ApplicationDbContext context)
    {
        _context = context;
    }

    //GET: Pets
    public async Task<IActionResult> Index()
    {
        return View(await _context.Pets.ToListAsync());
    }

    // Get: Pets/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pet = await _context.Pets
            .FirstOrDefaultAsync(m => m.PetId == id);
        if (pet == null)
        {
            return NotFound();
        }

        return View(pet);
    }

    // GET: Pets/Add
    public IActionResult Add()
    {
        return View();
    }

    //POST: Pets/Add
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(Pets item)
    {
        if (ModelState.IsValid)
        {
            _context.Pets.Add(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(item);
    }

    [Authorize]
    public async Task<IActionResult> Edit([FromRoute] int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pet = await _context.Pets.FindAsync(id);
        if (pet == null)
        {
            return NotFound();
        }

        return View();
    }

    // POST: Products/Edit/5
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind()] Pets pet)
    {
        if (id != pet.PetId)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            try
            {
                _context.Pets.Update(pet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetsExists(pet.PetId))
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
        return View(pet);
    }

    // GET Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var pet = await _context.Pets.FirstOrDefaultAsync(m => m.PetId == id);
        if (pet == null)
        {
            return NotFound();
        }

        return View(pet);
    }

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var pet = await _context.Pets.FindAsync(id);
        if (pet != null)
        {
            _context.Pets.Remove(pet);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private bool PetsExists(int id)
    {
        return _context.Pets.Any(e => e.PetId == id);
    }
}
