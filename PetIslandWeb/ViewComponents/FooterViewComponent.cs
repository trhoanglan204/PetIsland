using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;

namespace PetIslandWeb.ViewComponents;

#pragma warning disable IDE0290

public class FooterViewComponent : ViewComponent
{
	private readonly ApplicationDbContext _dataContext;
	public FooterViewComponent(ApplicationDbContext context)
	{
		_dataContext = context;
	}
	public async Task<IViewComponentResult> InvokeAsync() => View(await _dataContext.Contact.FirstOrDefaultAsync());
}
