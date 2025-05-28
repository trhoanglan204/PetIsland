# 🔍 Vulnerability Reference Guide

This file maps all known intentional vulnerabilities in this branch to their source code locations for easier study and demo.

---

## 🐞 SQL Injection (SQLi)
### 📂 File: [HomeController.cs](PetIslandWeb/Controllers/HomeController.cs)
- using raw sql query

```csharp
var petSql = $"SELECT * FROM Pets WHERE Name LIKE '%{searchString}%' OR Description LIKE '%{searchString}%'";
var productSql = $"SELECT * FROM Products WHERE Name LIKE '%{searchString}%' OR Description LIKE '%{searchString}%'";
var pets = await _context.Pets.FromSqlRaw(petSql).ToListAsync();
var products = await _context.Products.FromSqlRaw(productSql).ToListAsync();
```
<details>
<summary> To Fix </summary>

- using LINQ

```csharp
var pets = await _context.Pets
    .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%") || EF.Functions.Like(p.Description, $"%{searchString}%"))
    .ToListAsync();
var products = await _context.Products
    .Where(p => EF.Functions.Like(p.Name, $"%{searchString}%") || EF.Functions.Like(p.Description, $"%{searchString}%"))
    .ToListAsync();
```

</details>


## 🐞 Cross-Site Scripting (XSS)
### 📂 File: [Search.cshtml](PetIslandWeb/Views/Home/Search.cshtml)
- using html raw render

```csharp
<h3>Search Results for @Html.Raw(@ViewBag.KeyWord)</h3>
```
<details>
<summary> To Fix </summary>

- avoid ``Html.Raw()``

```cshtml
<h3>Search Results for @ViewBag.KeyWord</h3>
```

</details>

## 🐞 Insecure Cookie Configuration
### 📂 File: [Program.cs](PetIslandWeb/Program.cs)
- **Vulnerabilities**:
  - Cookie accessible by JavaScript (XSS + session hijack)
  - Cookie sent over HTTP (MITM risk)
  - Cookie lacks CSRF protection

```csharp
builder.Services.ConfigureApplicationCookie(options =>
{
    // Redirects settings.
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    // Cookie settings.
    //options.Cookie.HttpOnly = true;
    //options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //Use Always for HTTPS | None for dev mode
    //options.Cookie.SameSite = SameSiteMode.None; //Prevent Cross-Site Request Forgery (CSRF)
});
```

<details>
<summary> Explanation </summary>

By disabling important cookie flags (`HttpOnly`, `Secure`, `SameSite`), this config allows an attacker to:
- Steal cookies via XSS
- Capture cookies over unencrypted traffic
- Exploit CSRF via cross-site form submissions

</details>