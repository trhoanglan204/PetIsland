using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetIsland.DataAccess.Data;
using PetIsland.Models;
using PetIsland.Models.ViewModels;
using PetIsland.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using PetIslandWeb.Areas.Admin.Controllers;
using System.Drawing;

#pragma warning disable IDE0290

namespace PetIslandWeb.Controllers;

public class AccountController : Controller
{
	private readonly UserManager<AppUserModel> _userManager;
	private readonly SignInManager<AppUserModel> _signInManager;
	private readonly IEmailSender _emailSender;
	private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<AccountController> _logger;
    public AccountController(IEmailSender emailSender, UserManager<AppUserModel> userManage,
		SignInManager<AppUserModel> signInManager, ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<AccountController> logger)
	{
		_userManager = userManage;
		_signInManager = signInManager;
		_emailSender = emailSender;
		_context = context;
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }
	[HttpGet]
	public IActionResult Login(string returnUrl)
	{
		return View(new LoginVM { ReturnUrl = returnUrl });
	}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByNameAsync(loginVM.Username!)
            ?? await _userManager.FindByEmailAsync(loginVM.Username!);
            if (user == null)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại.");
                return View(loginVM);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName!, loginVM.Password!, isPersistent: false, lockoutOnFailure: true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Tài khoản bị khóa trong 5 phút do đăng nhập sai quá nhiều lần.");
                return View(loginVM);
            }
            if (result.Succeeded)
            {
				TempData["success"] = "Đăng nhập thành công";
				if (user!.Role != SD.Role_Admin && user.Role != SD.Role_Employee)
				{
					var receiver = user.Email;
					if (!string.IsNullOrEmpty(receiver))
					{
                        var subject = "Đăng nhập trên thiết bị thành công.";
                        var message = "Đăng nhập thành công, trải nghiệm dịch vụ nhé.";
                        await _emailSender.SendEmailAsync(receiver, subject, message);
                    }
				}
                return Redirect(loginVM.ReturnUrl ?? "/");
            }
            ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
        }
        return View(loginVM);
    }

    [HttpPost]
	public async Task<IActionResult> SendMailForgotPass(AppUserModel user)
	{
		var checkuser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

		if (checkuser == null)
		{
			TempData["error"] = "Email not found";
			return RedirectToAction("ForgotPass", "Account");
		}
		else
		{
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            checkuser.Token = token;
			_context.Update(checkuser);
			await _context.SaveChangesAsync();
			var receiver = checkuser.Email!;
			var subject = "Change password for user " + checkuser.Email;
            var message = $"Click on link to change password: <a href='{Request.Scheme}://{Request.Host}/Account/ResetPassword?email={checkuser.Email}&token={token}'>";
            await _emailSender.SendEmailAsync(receiver, subject, message);
		}

		TempData["success"] = "An email has been sent to your registered email address with password reset instructions.";
		return RedirectToAction("ForgotPass", "Account");
	}
	public IActionResult ForgotPass()
	{
		return View();
	}
	public async Task<IActionResult> NewPass(AppUserModel user, string token)
	{
		var checkuser = await _userManager.Users
			.Where(u => u.Email == user.Email)
			.Where(u => u.Token == user.Token).FirstOrDefaultAsync();

		if (checkuser != null)
		{
			ViewBag.Email = checkuser.Email;
			ViewBag.Token = token;
		}
		else
		{
			TempData["error"] = "Email not found or token is not right";
			return RedirectToAction("ForgotPass", "Account");
		}
		return View();
	}
	//TODO: implement trang doi mat khau
	public async Task<IActionResult> UpdateNewPassword(AppUserModel user)
	{
		var checkuser = await _userManager.Users
			.Where(u => u.Email == user.Email)
			.Where(u => u.Token == user.Token).FirstOrDefaultAsync();

		if (checkuser != null)
		{
			string newtoken = Guid.NewGuid().ToString();
            var passwordHasher = new PasswordHasher<AppUserModel>();
			var passwordHash = passwordHasher.HashPassword(checkuser, user.PasswordHash!);

			checkuser.PasswordHash = passwordHash;
			checkuser.Token = newtoken;

			await _userManager.UpdateAsync(checkuser);
			TempData["success"] = "Password updated successfully.";
			return RedirectToAction("Login", "Account");
		}
		else
		{
			TempData["error"] = "Email not found or token is not right";
			return RedirectToAction("ForgotPass", "Account");
		}
	}
	public async Task<IActionResult> History()
	{
		if (!(User?.Identity?.IsAuthenticated ?? false))
		{
			return RedirectToAction("Login", "Account");
		}
		var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
		var userEmail = User.FindFirstValue(ClaimTypes.Email);

		var Orders = await _context.Orders
			.Where(od => od.UserName == userEmail).OrderByDescending(od => od.Id).ToListAsync();

        ViewBag.UserEmail = userEmail;
		return View(Orders);
	}

	public async Task<IActionResult> CancelOrder(string ordercode)
	{
		if (!(User?.Identity?.IsAuthenticated ?? false))
		{
			return RedirectToAction("Login", "Account");
		}
		try
		{
			var order = await _context.Orders.Where(o => o.OrderCode == ordercode).FirstAsync();
			order.Status = 3;
			_context.Update(order);
			await _context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			return BadRequest($"An error occurred while canceling the order: {ex.Message}");
		}
		return RedirectToAction("History", "Account");
	}

	[HttpGet]
	public IActionResult Create()
	{
		return View();
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(UserVM user)
	{
		if (ModelState.IsValid)
		{
			var newUser = new AppUserModel
			{
				UserName = user.Username,
				Email = user.Email,
				Role = SD.Role_Customer,
				Avatar = user.Image
			};
			IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);
			if (result.Succeeded)
			{
				TempData["success"] = "Tạo thành viên thành công";
				return Redirect("/Account/Login");
			}
			foreach (IdentityError error in result.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}
		}
		return View(user);
	}

    [HttpGet]
	[Authorize]
    public async Task<IActionResult> AccountInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return NotFound();
        }

        var model = new UserVM
        {
            Username = user.UserName,
            Email = user.Email,
            Image = user.Avatar ?? "blank_avatar.jpg",
			Password = new string('*', user.PasswordHash?.Length ?? 10), //censored password, to modify, use another action,
			Name = user.Name,
			StreetAddress = user.StreetAddress,
			PostalCode = user.PostalCode,
			State = user.State,
			City = user.City
        };

        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AccountInfo(UserVM model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }
            bool isModified = false;
            if (user.UserName != model.Username)
            {
                user.UserName = model.Username;
                isModified = true;
            }
            if (user.Email != model.Email)
            {
                user.Email = model.Email;
                isModified = true;
            }
            if (user.Name != model.Name)
            {
                user.Name = model.Name;
                isModified = true;
            }
            if (user.StreetAddress != model.StreetAddress)
            {
                user.StreetAddress = model.StreetAddress;
                isModified = true;
            }
            if (user.PostalCode != model.PostalCode)
            {
                user.PostalCode = model.PostalCode;
                isModified = true;
            }
            if (user.City != model.City)
            {
                user.City = model.City;
                isModified = true;
            }
            if (user.State != model.State)
            {
                user.State = model.State;
                isModified = true;
            }
            if (model.ImageUpload != null)
            {
                if (model.ImageUpload.Length > 5 * 1024 * 1024) // Giới hạn 5MB
                {
                    ModelState.AddModelError("", "File ảnh không được lớn hơn 5MB.");
                    return View(model);
                }
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images/users");
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }
                string baseName = Path.GetFileNameWithoutExtension(model.ImageUpload.FileName);
                if (string.IsNullOrEmpty(baseName))
                {
					baseName = "avatar_" + model.Username;
                }
                baseName = CommonHelpers.SanitizeFileName(baseName);
                baseName = baseName.Length > 30 ? baseName[..30] : baseName;
                string imageName = baseName + "_" + Guid.NewGuid().ToString() + Path.GetExtension(model.ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);
                var fs = new FileStream(filePath, FileMode.Create);
                await model.ImageUpload.CopyToAsync(fs);
                fs.Close();
                var oldImage = Path.Combine(uploadsDir, user.Avatar!);
                if (System.IO.File.Exists(oldImage))
                {
                    try
                    {
                        System.IO.File.Delete(oldImage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning("Không thể xóa file cũ {FilePath}: {Error}", oldImage, ex.Message);
                    }
                }
                user.Avatar = imageName;
                isModified = true;
            }
            if (isModified)
            {
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["success"] = "Cập nhật thông tin thành công";
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                TempData["info"] = "Không có thay đổi nào được thực hiện.";
            }
        }

        return View(model);
    }

    public async Task<IActionResult> Logout(string returnUrl = "/")
	{
		await _signInManager.SignOutAsync();
		return Redirect(returnUrl);
	}

	public async Task LoginByGoogle()
	{
		// Use Google authentication scheme for challenge
		await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
			new AuthenticationProperties
			{
				RedirectUri = Url.Action("GoogleResponse")
			});
	}

	public async Task<IActionResult> GoogleResponse()
	{
		var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

		if (!result.Succeeded)
		{
			return RedirectToAction("Login");
		}

		var claims = result.Principal.Identities.FirstOrDefault()!.Claims.Select(claim => new
		{
			claim.Issuer,
			claim.OriginalIssuer,
			claim.Type,
			claim.Value
		});

		var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            TempData["error"] = "Không thể lấy email từ Google. Vui lòng thử lại.";
            return RedirectToAction("Login", "Account");
        }
        string emailName = email.Split('@')[0];
		var existingUser = await _userManager.FindByEmailAsync(email);

		if (existingUser == null)
		{
			var passwordHasher = new PasswordHasher<AppUserModel>();
            var randomPassword = Guid.NewGuid().ToString("N");
            var hashedPassword = passwordHasher.HashPassword(null!, randomPassword);
            var newUser = new AppUserModel
            {
                UserName = emailName,
                Email = email,
                PasswordHash = hashedPassword // Set the hashed password cho user
            };
            var createUserResult = await _userManager.CreateAsync(newUser);
			if (!createUserResult.Succeeded)
			{
				TempData["error"] = "Đăng ký tài khoản thất bại. Vui lòng thử lại sau.";
				return RedirectToAction("Login", "Account");
			}
			else
			{
				await _signInManager.SignInAsync(newUser, isPersistent: false);
				TempData["success"] = "Đăng ký tài khoản thành công.";
				return RedirectToAction("Index", "Home");
			}
		}

		await _signInManager.SignInAsync(existingUser, isPersistent: false);
		return Redirect("/");
	}


    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
