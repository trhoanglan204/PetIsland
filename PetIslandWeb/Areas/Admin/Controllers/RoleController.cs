﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/Role")]
[Authorize(Roles = "Admin")]
public class RoleController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    public RoleController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _roleManager = roleManager;
    }
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Roles.OrderByDescending(p => p.Id).ToListAsync());
    }
    [HttpGet]
    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpGet]
    [Route("Edit")]

    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound(); // Handle missing Id
        }
        var role = await _roleManager.FindByIdAsync(id);

        return View(role);
    }

    [Route("Edit")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, IdentityRole model)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound(); // Handle missing Id
        }
        if (ModelState.IsValid) // Validate model state before proceeding
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound(); // Handle role not found
            }
            role.Name = model.Name; // Update role properties with model data
            try
            {
                await _roleManager.UpdateAsync(role);
                TempData["success"] = "Role updated successfully!";
                return RedirectToAction("Index"); // Redirect to the index action
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while updating the role.");
            }

        }
        return View(model ?? new IdentityRole { Id = id });
    }

    [Route("Create")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IdentityRole model)
    {
        //avoid duplicate role
        if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
        }
        return Redirect("Index");
    }

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound(); // Handle missing Id
        }

        var role = await _roleManager.FindByIdAsync(id);

        if (role == null)
        {
            return NotFound(); // Handle role not found
        }

        try
        {
            await _roleManager.DeleteAsync(role);
            TempData["success"] = "Role deleted successfully!";
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "An error occurred while deleting the role.");
        }

        return Redirect("Index");
    }

}
