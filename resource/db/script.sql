﻿USE [master]
GO
/****** Object:  Database [PetIsland]    Script Date: 4/29/2025 7:19:47 PM ******/
IF DB_ID('PetIsland') IS NULL
BEGIN
    CREATE DATABASE [PetIsland];
END
GO
USE [PetIsland]
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PetIsland].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PetIsland] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PetIsland] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PetIsland] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PetIsland] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PetIsland] SET ARITHABORT OFF 
GO
ALTER DATABASE [PetIsland] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PetIsland] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PetIsland] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PetIsland] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PetIsland] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PetIsland] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PetIsland] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PetIsland] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PetIsland] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PetIsland] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PetIsland] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PetIsland] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PetIsland] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PetIsland] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PetIsland] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PetIsland] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PetIsland] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PetIsland] SET RECOVERY FULL 
GO
ALTER DATABASE [PetIsland] SET  MULTI_USER 
GO
ALTER DATABASE [PetIsland] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PetIsland] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PetIsland] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PetIsland] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PetIsland] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PetIsland] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PetIsland', N'ON'
GO
ALTER DATABASE [PetIsland] SET QUERY_STORE = OFF
GO
USE [PetIsland]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[StreetAddress] [nvarchar](max) NULL,
	[City] [nvarchar](max) NULL,
	[State] [nvarchar](max) NULL,
	[PostalCode] [nvarchar](max) NULL,
	[Token] [nvarchar](max) NULL,
	[Role] [nvarchar](max) NOT NULL,
	[Avatar] [nvarchar](max) NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Brands]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Brands](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Slug] [nvarchar](max) NULL,
	[Status] [int] NULL,
	[Image] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Brands] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contact]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contact](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[ORS_Key] [nvarchar](max) NULL,
	[ORS_lat] [float] NOT NULL,
	[ORS_lon] [float] NOT NULL,
	[Map] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Contact] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[DateStart] [datetime2](7) NOT NULL,
	[DateExpired] [datetime2](7) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MomoInfo]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MomoInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [nvarchar](max) NULL,
	[OrderInfo] [nvarchar](max) NULL,
	[FullName] [nvarchar](max) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[DatePaid] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_MomoInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[OrderCode] [nvarchar](max) NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[ProductId] [bigint] NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderCode] [nvarchar](max) NOT NULL,
	[ShippingCost] [decimal](18, 2) NOT NULL,
	[CouponCode] [nvarchar](max) NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[Status] [int] NOT NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[GrandTotal] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PetCategory]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PetCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Slug] [nvarchar](max) NULL,
 CONSTRAINT [PK_PetCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pets]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pets](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Sex] [int] NOT NULL,
	[Slug] [nvarchar](max) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
	[PetCategoryId] [int] NOT NULL,
	[Age] [datetime2](7) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Pets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Slug] [nvarchar](max) NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductQuantities]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductQuantities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Quantity] [int] NOT NULL,
	[DateTime] [datetime2](7) NOT NULL,
	[ProductId] [bigint] NOT NULL,
 CONSTRAINT [PK_ProductQuantities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[SoldOut] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[Slug] [nvarchar](max) NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
	[BrandId] [int] NULL,
	[ProductCategoryId] [int] NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RatingEntries]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RatingEntries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[RatingDate] [datetime2](7) NOT NULL,
	[Star] [int] NOT NULL,
 CONSTRAINT [PK_RatingEntries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ratings]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ratings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TotalRated] [bigint] NOT NULL,
	[Star] [decimal](3, 1) NOT NULL,
	[ProductId] [bigint] NOT NULL,
 CONSTRAINT [PK_Ratings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shippings]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shippings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Ward] [nvarchar](max) NOT NULL,
	[District] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Shippings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sliders]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sliders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
	[Image] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Sliders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Statisticals]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Statisticals](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[revenue] [decimal](18, 2) NOT NULL,
	[orders] [int] NOT NULL,
	[date] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Statisticals] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VnpayInfo]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VnpayInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [nvarchar](max) NULL,
	[OrderDescription] [nvarchar](max) NULL,
	[TransactionId] [nvarchar](max) NULL,
	[PaymentId] [nvarchar](max) NULL,
	[PaymentMethod] [nvarchar](max) NULL,
	[DatePaid] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_VnpayInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Wishlist]    Script Date: 4/29/2025 7:19:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlist](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[UserId] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Wishlist] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250420135101_first_init', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250421194240_add_contact', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250421213859_update_contact', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250422184040_update_contact_map', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250426162424_add_payment_order', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250427195214_update-coupon', N'8.0.11')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250428143216_update_brand', N'8.0.11')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'28c9470e-ec4e-4605-a8cf-e8480f9d0a1b', N'Customer', N'CUSTOMER', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'70a4700b-dc5b-448d-ad59-d8a61b8332a6', N'Admin', N'ADMIN', NULL)
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'b4b393ee-cea1-423a-997d-39d32154488b', N'Employee', N'EMPLOYEE', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'228f0653-c5e8-475b-a63b-cff55fcaabd8', N'28c9470e-ec4e-4605-a8cf-e8480f9d0a1b')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'069d2711-f385-4c3c-afb8-05d39fc89966', N'70a4700b-dc5b-448d-ad59-d8a61b8332a6')
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'250ebcab-b8cf-4a65-b61e-bc91b572678b', N'b4b393ee-cea1-423a-997d-39d32154488b')
GO
INSERT [dbo].[AspNetUsers] ([Id], [Name], [StreetAddress], [City], [State], [PostalCode], [Token], [Role], [Avatar], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'069d2711-f385-4c3c-afb8-05d39fc89966', N'AT19_MaThieuFamily', N'17A Cong Hoa', N'HCM', N'DK', N'90001', NULL, N'Admin', N'Admin.jpg', N'admin@kma.com', N'ADMIN@KMA.COM', N'admin@kma.com', N'ADMIN@KMA.COM', 0, N'AQAAAAIAAYagAAAAELQhInuJFQ1vq2BGG8kI9oyiw9pGuOeSWcyhB22cj/Qa41UHXeZctZTMIUpS/PLRxQ==', N'QYQT5EQB64UFKPO5NMS6KKNFTVZTXLYY', N'90689f72-ef59-4d96-8cbb-328cafd7ad3e', N'0123456789', 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Name], [StreetAddress], [City], [State], [PostalCode], [Token], [Role], [Avatar], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'228f0653-c5e8-475b-a63b-cff55fcaabd8', N'AT19_Gang', N'Đầu đường xó chợ', N'HaNoi', N'DK', N'90001', NULL, N'Customer', NULL, N'vip_customer', N'VIP_CUSTOMER', N'customer@kma.com', N'CUSTOMER@KMA.COM', 0, N'AQAAAAIAAYagAAAAELcuuVVuR3emVwVS09qdsP+OxC4Fgm2qdq4Koz1yRiGLlqlW74qGCKSiO6p8Gp8C3A==', N'6AETYS5HIK3WVW5RSSVX66V3L23TIIXW', N'07bf5cd3-c961-46b3-9d90-72f312481f22', N'001122334455', 0, 0, NULL, 1, 0)
INSERT [dbo].[AspNetUsers] ([Id], [Name], [StreetAddress], [City], [State], [PostalCode], [Token], [Role], [Avatar], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'250ebcab-b8cf-4a65-b61e-bc91b572678b', N'AT19_Slave', N'Ut Tich', N'HCM', N'DK', N'90001', NULL, N'Employee', N'staffA.jpg', N'staffA', N'STAFFA', N'staff@kma.com', N'STAFF@KMA.COM', 0, N'AQAAAAIAAYagAAAAEKyUX1xwhynnHmgu1+hVWrOZigBOKa5IFiOgzXalExJGuSPgrIXfB7hCCbI+rovKmA==', N'MG42HTBXL245CZADRUEB4HUOLD7PQ5FR', N'8dee6fea-8d54-424b-9112-c212373ca1de', N'0987654321', 0, 0, NULL, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[Contact] ON 

INSERT [dbo].[Contact] ([Id], [Name], [Address], [Email], [Phone], [ORS_Key], [ORS_lat], [ORS_lon], [Map]) VALUES (1, N'Phân hiệu Học viện Kỹ thuật Mật mã', N'17A Cộng Hòa, Quận Tân Bình, TP. Hồ Chí Minh', N'petisland41425@gmail.com', N'028 123456789', N'5b3ce3597851110001cf624800a5fa4327ed4c2a9c96df705bf8b6c0', 10.81691, 106.63832, N'<iframe src="https://www.google.com/maps/embed?pb=!1m14!1m8!1m3!1d3919.1374071775494!2d106.6571839!3d10.8007864!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x31752937159dbd15%3A0x286f2a16b253c64b!2zSOG7jWMgVmnhu4duIEvhu7kgVGh14bqtdCBN4bqtdCBNw6MgLSBNaeG7gW4gTmFtIChLTVAp!5e0!3m2!1svi!2s!4v1745349371090!5m2!1svi!2s" width="600" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="no-referrer-when-downgrade"></iframe>')
SET IDENTITY_INSERT [dbo].[Contact] OFF
GO
SET IDENTITY_INSERT [dbo].[Coupons] ON 

INSERT [dbo].[Coupons] ([Id], [Name], [Description], [DateStart], [DateExpired], [Quantity], [Status], [Price]) VALUES (2, N'50NAMGIAIPHONG', N'ki niem 50 nam giai phong mien nam thong nhat dat nuoc', CAST(N'2025-04-29T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-05T00:00:00.0000000' AS DateTime2), 3004, 1, CAST(0.50 AS Decimal(18, 2)))
INSERT [dbo].[Coupons] ([Id], [Name], [Description], [DateStart], [DateExpired], [Quantity], [Status], [Price]) VALUES (3, N'1THANG5', N'giftcode quoc te lao dong', CAST(N'2025-05-01T00:00:00.0000000' AS DateTime2), CAST(N'2025-05-05T00:00:00.0000000' AS DateTime2), 1052025, 1, CAST(150000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Coupons] OFF
GO
SET IDENTITY_INSERT [dbo].[MomoInfo] ON 

INSERT [dbo].[MomoInfo] ([Id], [OrderId], [OrderInfo], [FullName], [Amount], [DatePaid]) VALUES (1, N'638812879418950901', N'Khách hàng: admin@kma.com. Nội dung: ', N'admin@kma.com', CAST(40000.00 AS Decimal(18, 2)), CAST(N'2025-04-27T01:14:09.4219158' AS DateTime2))
INSERT [dbo].[MomoInfo] ([Id], [OrderId], [OrderInfo], [FullName], [Amount], [DatePaid]) VALUES (2, N'638814615974022139', N'Khách hàng: vip_customer. Nội dung: ', N'customer@kma.com', CAST(150000.00 AS Decimal(18, 2)), CAST(N'2025-04-29T01:27:00.4238561' AS DateTime2))
SET IDENTITY_INSERT [dbo].[MomoInfo] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([Id], [UserName], [OrderCode], [Price], [Quantity], [ProductId]) VALUES (1, N'customer@kma.com', N'55c9924e-c5da-4782-a733-dd37d319b5c4', CAST(150000.00 AS Decimal(18, 2)), 2, 1)
INSERT [dbo].[OrderDetails] ([Id], [UserName], [OrderCode], [Price], [Quantity], [ProductId]) VALUES (2, N'admin@kma.com', N'1cca610a-ce61-4229-9cab-654c50a94cb4', CAST(40000.00 AS Decimal(18, 2)), 1, 2)
INSERT [dbo].[OrderDetails] ([Id], [UserName], [OrderCode], [Price], [Quantity], [ProductId]) VALUES (3, N'customer@kma.com', N'28e528df-95ec-45a5-a954-2cb600a1096e', CAST(45000.00 AS Decimal(18, 2)), 3, 8)
INSERT [dbo].[OrderDetails] ([Id], [UserName], [OrderCode], [Price], [Quantity], [ProductId]) VALUES (4, N'customer@kma.com', N'0dac07a9-d111-4d87-90a1-6c9b64209d7f', CAST(250000.00 AS Decimal(18, 2)), 1, 7)
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([Id], [OrderCode], [ShippingCost], [CouponCode], [UserName], [CreatedDate], [Status], [PaymentMethod], [GrandTotal]) VALUES (1, N'55c9924e-c5da-4782-a733-dd37d319b5c4', CAST(50000.00 AS Decimal(18, 2)), NULL, N'customer@kma.com', CAST(N'2025-04-24T23:57:53.3141094' AS DateTime2), 1, NULL, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Orders] ([Id], [OrderCode], [ShippingCost], [CouponCode], [UserName], [CreatedDate], [Status], [PaymentMethod], [GrandTotal]) VALUES (2, N'1cca610a-ce61-4229-9cab-654c50a94cb4', CAST(50000.00 AS Decimal(18, 2)), NULL, N'admin@kma.com', CAST(N'2025-04-27T01:14:10.0064069' AS DateTime2), 1, N'MOMO 638812879418950901', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Orders] ([Id], [OrderCode], [ShippingCost], [CouponCode], [UserName], [CreatedDate], [Status], [PaymentMethod], [GrandTotal]) VALUES (3, N'28e528df-95ec-45a5-a954-2cb600a1096e', CAST(50000.00 AS Decimal(18, 2)), N'50NAMGIAIPHONG | ki niem 50 nam giai phong mien nam thong nhat dat nuoc', N'customer@kma.com', CAST(N'2025-04-29T01:25:27.1661463' AS DateTime2), 1, N' ', CAST(184999.50 AS Decimal(18, 2)))
INSERT [dbo].[Orders] ([Id], [OrderCode], [ShippingCost], [CouponCode], [UserName], [CreatedDate], [Status], [PaymentMethod], [GrandTotal]) VALUES (4, N'0dac07a9-d111-4d87-90a1-6c9b64209d7f', CAST(50000.00 AS Decimal(18, 2)), NULL, N'customer@kma.com', CAST(N'2025-04-29T01:27:00.8532583' AS DateTime2), 1, N'MOMO 638814615974022139', CAST(300000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[PetCategory] ON 

INSERT [dbo].[PetCategory] ([Id], [Name], [Slug]) VALUES (1, N'Dog', N'Dog')
INSERT [dbo].[PetCategory] ([Id], [Name], [Slug]) VALUES (2, N'Cat', N'Cat')
INSERT [dbo].[PetCategory] ([Id], [Name], [Slug]) VALUES (4, N'Bird', N'Bird')
INSERT [dbo].[PetCategory] ([Id], [Name], [Slug]) VALUES (5, N'Reptile', N'Reptile')
INSERT [dbo].[PetCategory] ([Id], [Name], [Slug]) VALUES (6, N'Hamster', N'Hamster')
INSERT [dbo].[PetCategory] ([Id], [Name], [Slug]) VALUES (7, N'Rabbit', N'Rabbit')
INSERT [dbo].[PetCategory] ([Id], [Name], [Slug]) VALUES (8, N'Uncategorized', N'Uncategorized')
SET IDENTITY_INSERT [dbo].[PetCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[Pets] ON 

INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (1, N'Ooe Meo', 0, N'Ooe-Meo', N'cat_79358e86-4b3d-448c-adf6-9de1544e068f.jpg', 2, CAST(N'2024-10-07T00:00:00.0000000' AS DateTime2), N'<p>meo meo</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (2, N'Jerry', 0, N'Jerry', N'dog_323c653c-f358-4863-911f-bed59a67393f.jpg', 1, CAST(N'2024-09-04T00:00:00.0000000' AS DateTime2), N'<p>Tom&amp;Jerry?</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (4, N'Turtle', 0, N'Turtle', N'turtle_db9a77b8-01f8-4f05-9588-8509296c4234.jpg', 5, CAST(N'2024-10-08T00:00:00.0000000' AS DateTime2), N'<p>huhhh??</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (5, N'Hamster', 1, N'Hamster', N'hamster_04ac53d8-910c-4816-b670-e9e41f14661e.jpg', 6, CAST(N'2024-12-25T00:00:00.0000000' AS DateTime2), N'<p>hiiiii</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (6, N'huh cat', 0, N'huh-cat', N'huh-cat_1190ef1d-101d-4da7-97e5-9030e149377b.jpg', 2, CAST(N'2025-02-04T00:00:00.0000000' AS DateTime2), N'<p>huhhhh</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (7, N'My Dieu', 1, N'My-Dieu', N'mydieu_ed74aac5-445e-4abc-9788-8a43339a6e71.jpeg', 2, CAST(N'2024-06-11T00:00:00.0000000' AS DateTime2), N'<p>tướng bầy hầy</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (8, N'shiba', 0, N'shiba', N'shiba_dcaee15d-6be9-41d2-8fa7-c58e755075f7.jpg', 1, CAST(N'2024-08-14T00:00:00.0000000' AS DateTime2), N'<p>lovely boy</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (9, N'cat with middle finger up', 0, N'cat-with-middle-finger-up', N'cat-middle-finger_d44bd2c0-0626-45bb-b3fa-c18789e39cf4.jpg', 2, CAST(N'2025-01-13T00:00:00.0000000' AS DateTime2), N'<p>hey you !!!!</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (10, N'cat crying', 0, N'cat-crying', N'cat-cry_412c99e8-f028-4b7a-adc8-2d00356be1d6.jpg', 2, CAST(N'2024-08-06T00:00:00.0000000' AS DateTime2), N'<p>why r u cry</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (11, N'fish', 0, N'fish', N'fish_35e24d9f-4fc9-4ffc-b8f3-e655d5db6843.jpg', 8, CAST(N'2025-02-28T00:00:00.0000000' AS DateTime2), N'<p>shhhhhhh</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (12, N'bormbadilo dogzimelio', 0, N'bormbadilo-dogzimelio', N'dog-smile_b0515716-789d-4d0a-8a97-b1acc736d09f.jpg', 1, CAST(N'2024-11-12T00:00:00.0000000' AS DateTime2), N'<p>tung tung tung sahur</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (13, N'catalia', 1, N'catalia', N'dolia_8e6617f3-6607-48fc-b1fb-f918676f174c.jpg', 2, CAST(N'2024-10-29T00:00:00.0000000' AS DateTime2), N'<p>mamamia</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (14, N'kittenia', 0, N'kittenia', N'valhein_16266f6a-a29e-4795-8452-712f57a7ad1d.jpg', 2, CAST(N'2025-02-12T00:00:00.0000000' AS DateTime2), N'<p>vanheintilo</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (15, N'violetemeo', 1, N'violetemeo', N'violet_8e6a0c1e-9b73-4b68-ab0b-99039a6cb202.jpg', 2, CAST(N'2024-10-14T00:00:00.0000000' AS DateTime2), N'<p>rose are red, violet are blue</p>

<p>i&#39;m your boss, you are sen</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (16, N'ladygaga', 1, N'ladygaga', N'toro_5f677123-995b-4b12-8b00-e486092140da.jpg', 2, CAST(N'2024-08-20T00:00:00.0000000' AS DateTime2), N'<p>meomeo mama</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (17, N'momopita', 0, N'momopita', N'lego_371f5ad2-fdfc-4914-a77b-d8aa6963d88c.jpg', 2, CAST(N'2024-12-17T00:00:00.0000000' AS DateTime2), N'<p>banachoco</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (18, N'puppyes', 0, N'puppyes', N'krixi_0a4ac388-4f30-4464-ad0e-ae571b1f28e3.jpg', 1, CAST(N'2024-08-26T00:00:00.0000000' AS DateTime2), N'<p>zzzz</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (19, N'birdicola', 0, N'birdicola', N'bird_9de84159-cef7-444d-9f59-c655e2cfb1b7.jpg', 4, CAST(N'2025-02-24T00:00:00.0000000' AS DateTime2), N'<p>tung tung suhar rakakata</p>
')
INSERT [dbo].[Pets] ([Id], [Name], [Sex], [Slug], [Image], [PetCategoryId], [Age], [Description]) VALUES (20, N'ego', 0, N'ego', N'rabbit_c02c9b61-ff07-4d2c-af39-45104502f31c.jpg', 7, CAST(N'2024-10-27T00:00:00.0000000' AS DateTime2), N'<p>super hero</p>
')
SET IDENTITY_INSERT [dbo].[Pets] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductCategory] ON 

INSERT [dbo].[ProductCategory] ([Id], [Name], [Slug]) VALUES (1, N'Accessories', N'Accessories')
INSERT [dbo].[ProductCategory] ([Id], [Name], [Slug]) VALUES (2, N'Medicines', N'Medicines')
INSERT [dbo].[ProductCategory] ([Id], [Name], [Slug]) VALUES (3, N'Hygiene', N'Hygiene')
INSERT [dbo].[ProductCategory] ([Id], [Name], [Slug]) VALUES (4, N'Foods', N'Foods')
INSERT [dbo].[ProductCategory] ([Id], [Name], [Slug]) VALUES (5, N'Toys', N'Toys')
SET IDENTITY_INSERT [dbo].[ProductCategory] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (1, N'Cat Meo', N'<p>Thom phuc</p>
', 2, 24, N'Cat-Meo', N'cat-litter_f8ac87e7-b25d-45ad-8e3f-d6e7233b58b0.jpg', NULL, 1, CAST(150000.00 AS Decimal(18, 2)), CAST(N'2025-04-20T14:21:56.5971902' AS DateTime2))
INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (2, N'Xuong', N'<p>for puppy</p>
', 1, 13, N'Xuong', N'chew-bone_6b8b1474-0bba-43cf-acd3-5d4d6faa7c0c.jpg', NULL, 1, CAST(40000.00 AS Decimal(18, 2)), CAST(N'2025-04-20T14:23:33.1461597' AS DateTime2))
INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (3, N'bikini for dog', N'<p>make your dog sexier</p>
', 0, 100, N'bikini-for-dog', N'bikini_for_dog_7a0923d9-f1ad-47d7-bddc-5aee506a53e4.jpg', NULL, 1, CAST(80000.00 AS Decimal(18, 2)), CAST(N'2025-04-28T17:23:39.0959420' AS DateTime2))
INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (4, N'beeeeeeeeer', N'<p>don&#39;t be overdrunk, buddy !!!</p>
', 0, 70, N'beeeeeeeeer', N'beerfordog_eb2277bf-f3e2-4879-b35e-3b9c9bc85019.jpg', NULL, 4, CAST(35000.00 AS Decimal(18, 2)), CAST(N'2025-04-28T17:25:08.3086597' AS DateTime2))
INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (5, N'nailing for puppy', N'<p>wanna see dog nail?</p>
', 0, 150, N'nailing-for-puppy', N'look_at_my_nails_22b714a2-55ff-4db3-a774-009e2615019b.jpg', NULL, 1, CAST(20000.00 AS Decimal(18, 2)), CAST(N'2025-04-28T17:26:26.0430020' AS DateTime2))
INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (6, N'hair salona', N'<p>smooth more than sunsilk</p>
', 0, 48, N'hair-salona', N'thewig_ff82eb8e-07d6-4a8d-83b6-38125c4496cb.jpg', NULL, 1, CAST(60000.00 AS Decimal(18, 2)), CAST(N'2025-04-28T17:28:26.0039292' AS DateTime2))
INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (7, N'toys model', N'<p>partner for relaxing???</p>

<p>for mature dog only !!!!</p>
', 1, 54, N'toys-model', N'lovetoy_7919cac7-1c46-4600-82ff-37214bdc50e3.jpg', NULL, 5, CAST(250000.00 AS Decimal(18, 2)), CAST(N'2025-04-28T17:30:28.5837071' AS DateTime2))
INSERT [dbo].[Products] ([Id], [Name], [Description], [SoldOut], [Quantity], [Slug], [Image], [BrandId], [ProductCategoryId], [Price], [CreatedDate]) VALUES (8, N'skidibi toilet', N'<p>for ...</p>
', 3, 77, N'skidibi-toilet', N'toilet_skibidi_f04a0868-c453-4d9d-ae63-d2bd21acc2ef.jpg', NULL, 5, CAST(45000.00 AS Decimal(18, 2)), CAST(N'2025-04-28T17:33:25.4330686' AS DateTime2))
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[RatingEntries] ON 

INSERT [dbo].[RatingEntries] ([Id], [ProductId], [Email], [Comment], [RatingDate], [Star]) VALUES (1, 3, N'vip_customer', N'wow amazing good chop', CAST(N'2025-04-28T18:00:49.6825741' AS DateTime2), 4)
INSERT [dbo].[RatingEntries] ([Id], [ProductId], [Email], [Comment], [RatingDate], [Star]) VALUES (2, 4, N'vip_customer', N'tasty', CAST(N'2025-04-28T18:03:12.4968151' AS DateTime2), 5)
INSERT [dbo].[RatingEntries] ([Id], [ProductId], [Email], [Comment], [RatingDate], [Star]) VALUES (3, 7, N'vip_customer', N'uhh no comment', CAST(N'2025-04-28T18:09:03.4304353' AS DateTime2), 4)
SET IDENTITY_INSERT [dbo].[RatingEntries] OFF
GO
SET IDENTITY_INSERT [dbo].[Ratings] ON 

INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (1, 0, CAST(0.0 AS Decimal(3, 1)), 1)
INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (2, 0, CAST(0.0 AS Decimal(3, 1)), 2)
INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (3, 1, CAST(4.0 AS Decimal(3, 1)), 3)
INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (4, 1, CAST(5.0 AS Decimal(3, 1)), 4)
INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (5, 0, CAST(0.0 AS Decimal(3, 1)), 5)
INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (6, 0, CAST(0.0 AS Decimal(3, 1)), 6)
INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (7, 1, CAST(4.0 AS Decimal(3, 1)), 7)
INSERT [dbo].[Ratings] ([Id], [TotalRated], [Star], [ProductId]) VALUES (8, 0, CAST(0.0 AS Decimal(3, 1)), 8)
SET IDENTITY_INSERT [dbo].[Ratings] OFF
GO
SET IDENTITY_INSERT [dbo].[Sliders] ON 

INSERT [dbo].[Sliders] ([Id], [Name], [Description], [Status], [Image]) VALUES (4, N'banner', N'<p>game</p>
', 1, N'banner_ddbe3fa6-4bc4-4003-aeec-562d86bc6778.jpg')
INSERT [dbo].[Sliders] ([Id], [Name], [Description], [Status], [Image]) VALUES (5, N'game', N'<p>banner</p>
', 1, N'game-banner_e34b466f-7860-40a3-be98-261e5417a994.jpg')
INSERT [dbo].[Sliders] ([Id], [Name], [Description], [Status], [Image]) VALUES (6, N'puppy', N'<p>dog</p>
', 1, N'puppy_cf196bdb-36e2-473f-a682-32d02c2b0678.jpg')
SET IDENTITY_INSERT [dbo].[Sliders] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetRoleClaims_RoleId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [RoleNameIndex]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] ON [dbo].[AspNetRoles]
(
	[NormalizedName] ASC
)
WHERE ([NormalizedName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserLogins_UserId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] ON [dbo].[AspNetUserLogins]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserRoles_RoleId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] ON [dbo].[AspNetUserRoles]
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_OrderDetails_ProductId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_OrderDetails_ProductId] ON [dbo].[OrderDetails]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pets_PetCategoryId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_Pets_PetCategoryId] ON [dbo].[Pets]
(
	[PetCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_BrandId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_BrandId] ON [dbo].[Products]
(
	[BrandId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_ProductCategoryId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_ProductCategoryId] ON [dbo].[Products]
(
	[ProductCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_RatingEntries_ProductId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_RatingEntries_ProductId] ON [dbo].[RatingEntries]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Ratings_ProductId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Ratings_ProductId] ON [dbo].[Ratings]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Wishlist_ProductId]    Script Date: 4/29/2025 7:19:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_Wishlist_ProductId] ON [dbo].[Wishlist]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Brands] ADD  DEFAULT (N'') FOR [Image]
GO
ALTER TABLE [dbo].[Contact] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [ORS_lat]
GO
ALTER TABLE [dbo].[Contact] ADD  DEFAULT ((0.0000000000000000e+000)) FOR [ORS_lon]
GO
ALTER TABLE [dbo].[Contact] ADD  DEFAULT (N'') FOR [Map]
GO
ALTER TABLE [dbo].[Coupons] ADD  DEFAULT ((0.0)) FOR [Price]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((0.0)) FOR [GrandTotal]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Products_ProductId]
GO
ALTER TABLE [dbo].[Pets]  WITH CHECK ADD  CONSTRAINT [FK_Pets_PetCategory_PetCategoryId] FOREIGN KEY([PetCategoryId])
REFERENCES [dbo].[PetCategory] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pets] CHECK CONSTRAINT [FK_Pets_PetCategory_PetCategoryId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Brands_BrandId] FOREIGN KEY([BrandId])
REFERENCES [dbo].[Brands] ([Id])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Brands_BrandId]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_ProductCategory_ProductCategoryId] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_ProductCategory_ProductCategoryId]
GO
ALTER TABLE [dbo].[RatingEntries]  WITH CHECK ADD  CONSTRAINT [FK_RatingEntries_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RatingEntries] CHECK CONSTRAINT [FK_RatingEntries_Products_ProductId]
GO
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_Products_ProductId]
GO
ALTER TABLE [dbo].[Wishlist]  WITH CHECK ADD  CONSTRAINT [FK_Wishlist_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Wishlist] CHECK CONSTRAINT [FK_Wishlist_Products_ProductId]
GO
USE [master]
GO
ALTER DATABASE [PetIsland] SET  READ_WRITE 
GO
