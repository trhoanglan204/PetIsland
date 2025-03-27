using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.DataAccess.Repository.IRepository;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;

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
    private readonly IUnitOfWork _unitOfWork;
    public UserController(UserManager<AppUserModel> userManager,ApplicationDbContext context, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("Index")]
    public async Task<IActionResult> Index()
    {
        //var usersWithRoles = await _unitOfWork.ApplicationUser.GetUsersWithRoles();
        var usersWithRoles = await (from u in _context.Users
                                    join ur in _context.UserRoles on u.Id equals ur.UserId
                                    join r in _context.Roles on ur.RoleId equals r.Id
                                    select new { User = u, RoleName = r.Name })
                   .ToListAsync();
        return View(usersWithRoles);
    }

    [HttpGet]
    [Route("Create")]
    public async Task<IActionResult> Create()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        ViewBag.Roles = new SelectList(roles, "Id", "Name");
        return View(new AppUserModel());
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
    public async Task<IActionResult> Create(AppUserModel user)
    {
        if (ModelState.IsValid)
        {
            var createUserResult = await _userManager.CreateAsync(user, user.PasswordHash); //tạo user
            if (createUserResult.Succeeded)
            {
                var createUser = await _userManager.FindByEmailAsync(user.Email); //tìm user dựa vào email
                var userId = createUser.Id; // lấy user Id
                var role = _roleManager.FindByIdAsync(user.RoleId); //lấy RoleId
                var addToRoleResult = await _userManager.AddToRoleAsync(createUser, role.Result.Name);
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
        TempData["success"] = "User đã được xóa thành công";
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RoleManagment(string userId)
    {
        var RoleVM = new RoleManagmentVM()
        {
            ApplicationUser = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == userId, includeProperties: "Company"),
            RoleList = _roleManager.Roles.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Name
            }),
        };

        RoleVM.ApplicationUser.Role =
            _userManager.GetRolesAsync(await _unitOfWork.ApplicationUser
            .GetAsync(u => u.Id == userId)).GetAwaiter().GetResult().FirstOrDefault();

        return View(RoleVM);
    }

    [HttpPost]
    public async Task<IActionResult> RoleManagment(RoleManagmentVM roleManagmentVM)
    {

        string oldRole =
            _userManager.GetRolesAsync(await _unitOfWork.ApplicationUser
            .GetAsync(u => u.Id == roleManagmentVM.ApplicationUser.Id))
            .GetAwaiter().GetResult().FirstOrDefault();

        AppUserModel applicationUser = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == roleManagmentVM.ApplicationUser.Id);


        if (!(roleManagmentVM.ApplicationUser.Role == oldRole))
        {
            //a role was updated
            _unitOfWork.ApplicationUser.Update(applicationUser);
            await _unitOfWork.SaveAsync();

            _userManager.RemoveFromRoleAsync(applicationUser, oldRole).GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(applicationUser, roleManagmentVM.ApplicationUser.Role).GetAwaiter().GetResult();

        }

        return RedirectToAction("Index");
    }

    #region API CALLS

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        List<AppUserModel> objUserList = (await _unitOfWork.ApplicationUser.GetAllAsync(includeProperties: "Company")).ToList();

        foreach (var user in objUserList)
        {
            user.Role = _userManager.GetRolesAsync(user).GetAwaiter().GetResult().FirstOrDefault();
        }

        return Json(new { data = objUserList });
    }


    [HttpPost]
    public async Task<IActionResult> LockUnlock([FromBody] string id)
    {

        var objFromDb = await _unitOfWork.ApplicationUser.GetAsync(u => u.Id == id);
        if (objFromDb == null)
        {
            return Json(new { success = false, message = "Error while Locking/Unlocking" });
        }

        if (objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
        {
            //user is currently locked and we need to unlock them
            objFromDb.LockoutEnd = DateTime.Now;
        }
        else
        {
            objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
        }
        _unitOfWork.ApplicationUser.Update(objFromDb);
        await _unitOfWork.SaveAsync();

        return Json(new { success = true, message = "Operation Successful" });
    }

    #endregion
}