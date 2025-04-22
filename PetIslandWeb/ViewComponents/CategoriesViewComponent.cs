using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
namespace PetIslandWeb.ViewComponents;
#pragma warning disable IDE0290
public class CategoriesViewComponent : ViewComponent
{
	private readonly ApplicationDbContext _dataContext;

	public CategoriesViewComponent(ApplicationDbContext context)
	{
		_dataContext = context;
	}
	public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.ProductCategory.ToListAsync());
}
