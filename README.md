### Members:
- AT19N0101 Nguyễn Thị Hồng Anh
- AT19N0119 Nguyễn Anh Khôi
- AT19N0123 Trương Hoàng Lân
- AT19N0138 Trương Văn Thiệu

---
<p align="center">
  <img src="resource/readme/image.png" />
</p>

---

# 🐾 PetIsland

[![Build & Test](https://github.com/trhoanglan204/petisland/actions/workflows/dotnet.yml/badge.svg)](https://github.com/trhoanglan204/petisland/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-7.0-blue.svg)](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
[![EF Core](https://img.shields.io/badge/Entity%20Framework-Core%207.0-blue.svg)](https://learn.microsoft.com/en-us/ef/core/)
[![Platform](https://img.shields.io/badge/Platform-Web%20MVC%20App-orange.svg)](https://learn.microsoft.com/en-us/aspnet/core/mvc/overview)

**PetIsland** is an e-commerce platform for pets, where users can buy and sell pets, pet care products, and manage orders easily. The project is built using .NET Core with an MVC architecture and Entity Framework Core for data access.

---

## 📁 Project Structure

The project is organized with a layered architecture:

- **`PetIsland.Models`**: Contains data models, view models, and helper libraries such as VnPay and Momo.
- **`PetIsland.DataAccess`**: Manages data access, including the Repository pattern, DbContext, and Migrations.
- **`PetIsland.Utility`**: Contains utilities like email sending, session handling, and system constants.
- **`PetIslandWeb`**: The main MVC web application, including controllers, views, SignalR, and payment configurations.

---

## ⚙️ System Requirements

- [.NET SDK 8.0 or higher](https://dotnet.microsoft.com/en-us/download)
- [SQL Server 2019 or higher](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- Visual Studio 2022 (or any IDE that supports .NET Core)
- (Optional) Node.js if you need to rebuild JS files

---

## I. Installation and Running the Project

### 1. Clone the repository

```bash
git clone https://github.com/trhoanglan204/petisland.git
cd petisland
```

### 2. Configure the connection string
Open PetIslandWeb/appsettings.json and update the connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=localhost;Initial Catalog=PetIsland;Integrated Security=True;Trust Server Certificate=True"
}
```
⚠️ Make sure you've created the database PetIslandDb on SQL Server, or let EF create it automatically.

#### Momo and VnPay Payment Integration
- Momo Payment: You can integrate Momo's payment gateway by following the official [Momo](https://developers.momo.vn/v3/vi/download/) documentation. It provides a full API for payment processing.
> ⚠️ for demo and test, the fail result when applied paid in Momo also set to success, change if needed

- VnPay Payment: VnPay integration is set up using their sandbox environment. You can read more and get API details here: [VnPay Sandbox](https://sandbox.vnpayment.vn/apis/vnpay-demo/).

#### Google logged in
Make sure to check fallback in your url setting: ``console.cloud.google.com/auth/clients``
> ex: https://localhost:7021/signin-google

### 3. Create the database
Run the migration to create the database:

```bash
cd PetIsland.DataAccess
dotnet ef database update
```

### 4. Run the application
```bash
cd ../PetIslandWeb
dotnet run
```

Once the application is running, visit: https://localhost:7021 or http://localhost:5140

## II. Default Accounts (if seeding is enabled)
Admin:

- username: admin@kma.com
- password: Admin@123*

Employee: (test)

- username: staffA
- password: Employee@123*

Customer: (test)

- username: vip_customer
- password: Customer@123*

> for other user: can create your custom account

If not seeded automatically, you can add seeding logic in `DbInitializer.cs.`

## III. Explanation of Key Features
- **User Registration**: Users can sign up, login, and manage their profile.
- **Product Management**: Admin users can create, update, or delete pet and product listings.
- **Shopping Cart**: Users can add products to their shopping cart and proceed to checkout.
- **Order Management**: Track orders, manage order details, and apply coupons.
- **Payment Integration**: With payment gateways like Momo and VnPay, users can complete their purchases.
- **Chatting anonymous**: With whatever identical name, you can chat to everyone

## IV. Deployment (Hosting)
### Manual Deployment
1. Publish WebApp
```bash
cd PetIslandWeb
dotnet publish -c Release -o ./publish
```

2. Deploy on IIS or upload the /publish folder to the server ([myasp](https://www.myasp.net/))
- Configure web.config for IIS if necessary.
- Update the connection string and environment variables.

---

## Drawbacks & Potential Vulnerabilities
- Old Version of CKEditor: The project uses CKEditor version 4.22.1, which might have some security vulnerabilities due to its age. You should consider upgrading it to the latest version for better security.
    - [CKEditor](https://cdn.ckeditor.com/4.22.1/standard/ckeditor.js)

## Further Updates
- Dynamic Shipping Calculation: A future update will implement dynamic shipping cost calculation based on the delivery distance. This feature is already being worked on with the GeoService and will be included in future versions.
- AI Chat bot: Feel boring when browsing our shop? Don't worry, we will add an "artificial friend" to chit chat in the future.
## Report a Bug
If you find any bugs or issues while using the PetIsland app, please report them on the GitHub issues page

## 📝 License

This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).

## Contribute

You want to contribute your code? See [TODO.md](/TODO.md)

## Contact

If you have any questions, suggestions, or feedback, feel free to reach out via:

- [GitHub Issues](https://github.com/trhoanglan204/petisland/issues) — for bug reports or feature requests