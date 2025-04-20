using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;
using PetIslandWeb.Controllers;

#pragma warning disable IDE0290

namespace PetIslandWeb.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/User")]
[Authorize(Roles = SD.Role_Admin)]
public class UserController : Controller
{
    private readonly UserManager<AppUserModel> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<UserController> _logger;
    public UserController(UserManager<AppUserModel> userManager,ApplicationDbContext context, 
        RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment, ILogger<UserController> logger)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        var usersWithRoles = await (from u in _context.Users
                                    join ur in _context.UserRoles on u.Id equals ur.UserId
                                    join r in _context.Roles on ur.RoleId equals r.Id
                                    select new UserWithRoleVM { User = u, RoleName = r.Name })
                    .GroupBy(x =>x.User)
                    .Select(g => new UserWithRoleVM 
                    { 
                        User = g.Key,
                        RoleName = string.Join(", ",g.Select(x=>x.RoleName))
                    })
                   .ToListAsync();
        return View(usersWithRoles);
    }

    [HttpGet]
    [Route("Create")]
    public async Task<IActionResult> Create()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        ViewBag.Roles = new SelectList(roles, "Id", "Name");
        return View(roles);
    }

    [HttpGet]
    [Route("Edit")]
    public async Task<IActionResult> Edit(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var roles = await _roleManager.Roles.ToListAsync();
        ViewBag.Roles = new SelectList(roles, "Id", "Name");
        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Edit")]
    public async Task<IActionResult> Edit(string id, AppUserModel user)
    {
        var existingUser = await _userManager.FindByIdAsync(id);
        if (existingUser == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Update other user properties (excluding password)
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;

            var updateUserResult = await _userManager.UpdateAsync(existingUser);
            if (updateUserResult.Succeeded)
            {
                return RedirectToAction("Index", "User");
            }
            else
            {
                AddIdentityErrors(updateUserResult);
                return View(existingUser);
            }
        }
        var roles = await _roleManager.Roles.ToListAsync();
        ViewBag.Roles = new SelectList(roles, "Id", "Name");

        TempData["error"] = "Model validate failed";
        var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();
        string errorMessage = string.Join("\n", errors);
        return View(existingUser);
    }

    private void AddIdentityErrors(IdentityResult identityResult)
    {
        foreach (var error in identityResult.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("Create")]
    public async Task<IActionResult> Create(UserVM user, string role)
    {
        if (ModelState.IsValid)
        {
            var newUser = new AppUserModel
            {
                UserName = user.Username,
                Email = user.Email,
                Role = SD.Role_Customer,
                Avatar = user.Image,
            };
            var createUserResult = await _userManager.CreateAsync(newUser, user.Password); //tạo user
            if (createUserResult.Succeeded)
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(newUser, role);
                if (!addToRoleResult.Succeeded)
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                return RedirectToAction("Index", "User");
            }
            else
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(user);
            }
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

    [HttpGet]
    [Route("Delete")]
    public async Task<IActionResult> Delete(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return NotFound();
        }
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var deleteResult = await _userManager.DeleteAsync(user);
        if (!deleteResult.Succeeded)
        {
            return View("Error");
        }
        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/users");
        string oldfilePath = Path.Combine(uploadsDir, user.Avatar!);
        if (System.IO.File.Exists(oldfilePath))
        {
            try
            {
                System.IO.File.Delete(oldfilePath);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Không thể xóa file cũ {FilePath}: {Error}", oldfilePath, ex.Message);
            }
        }
        TempData["success"] = "User đã được xóa thành công";
        return RedirectToAction("Index");
    }
}