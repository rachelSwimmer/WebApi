-- Insert Categories with hierarchical structure
-- Declare variables to store generated IDs
DECLARE @ElectronicsId INT, @ClothingId INT, @HomeGardenId INT;
DECLARE @ComputersId INT, @MobilePhonesId INT, @AudioId INT;
DECLARE @LaptopsId INT, @DesktopsId INT, @AccessoriesId INT;
DECLARE @GamingLaptopsId INT, @BusinessLaptopsId INT, @UltrabooksId INT;
DECLARE @MenId INT, @WomenId INT, @KidsId INT;
DECLARE @ShirtsId INT, @PantsId INT, @ShoesId INT;

-- Root Categories (no parent)
INSERT INTO Categories (Name, ParentId) VALUES ('Electronics', NULL);
SET @ElectronicsId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Clothing', NULL);
SET @ClothingId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Home & Garden', NULL);
SET @HomeGardenId = SCOPE_IDENTITY();

-- Electronics Sub-Categories
INSERT INTO Categories (Name, ParentId) VALUES ('Computers', @ElectronicsId);
SET @ComputersId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Mobile Phones', @ElectronicsId);
SET @MobilePhonesId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Audio', @ElectronicsId);
SET @AudioId = SCOPE_IDENTITY();

-- Computers Sub-Categories
INSERT INTO Categories (Name, ParentId) VALUES ('Laptops', @ComputersId);
SET @LaptopsId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Desktops', @ComputersId);
SET @DesktopsId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Accessories', @ComputersId);
SET @AccessoriesId = SCOPE_IDENTITY();

-- Laptops Sub-Categories
INSERT INTO Categories (Name, ParentId) VALUES ('Gaming Laptops', @LaptopsId);
SET @GamingLaptopsId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Business Laptops', @LaptopsId);
SET @BusinessLaptopsId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Ultrabooks', @LaptopsId);
SET @UltrabooksId = SCOPE_IDENTITY();

-- Clothing Sub-Categories
INSERT INTO Categories (Name, ParentId) VALUES ('Men', @ClothingId);
SET @MenId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Women', @ClothingId);
SET @WomenId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Kids', @ClothingId);
SET @KidsId = SCOPE_IDENTITY();

-- Men Clothing Sub-Categories
INSERT INTO Categories (Name, ParentId) VALUES ('Shirts', @MenId);
SET @ShirtsId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Pants', @MenId);
SET @PantsId = SCOPE_IDENTITY();

INSERT INTO Categories (Name, ParentId) VALUES ('Shoes', @MenId);
SET @ShoesId = SCOPE_IDENTITY();

-- Insert Products
-- Gaming Laptops
INSERT INTO Products (Name, CategoryId) VALUES ('ASUS ROG Strix G15', @GamingLaptopsId);
INSERT INTO Products (Name, CategoryId) VALUES ('MSI Raider GE76', @GamingLaptopsId);
INSERT INTO Products (Name, CategoryId) VALUES ('Alienware m15 R7', @GamingLaptopsId);

-- Business Laptops
INSERT INTO Products (Name, CategoryId) VALUES ('Dell Latitude 5430', @BusinessLaptopsId);
INSERT INTO Products (Name, CategoryId) VALUES ('Lenovo ThinkPad X1 Carbon', @BusinessLaptopsId);
INSERT INTO Products (Name, CategoryId) VALUES ('HP EliteBook 840', @BusinessLaptopsId);

-- Ultrabooks
INSERT INTO Products (Name, CategoryId) VALUES ('MacBook Air M2', @UltrabooksId);
INSERT INTO Products (Name, CategoryId) VALUES ('Dell XPS 13', @UltrabooksId);
INSERT INTO Products (Name, CategoryId) VALUES ('HP Spectre x360', @UltrabooksId);

-- Desktops
INSERT INTO Products (Name, CategoryId) VALUES ('Dell OptiPlex 7090', @DesktopsId);
INSERT INTO Products (Name, CategoryId) VALUES ('HP Pavilion Desktop', @DesktopsId);
INSERT INTO Products (Name, CategoryId) VALUES ('Lenovo IdeaCentre', @DesktopsId);

-- Accessories
INSERT INTO Products (Name, CategoryId) VALUES ('Logitech MX Master 3', @AccessoriesId);
INSERT INTO Products (Name, CategoryId) VALUES ('Razer BlackWidow Keyboard', @AccessoriesId);
INSERT INTO Products (Name, CategoryId) VALUES ('Dell UltraSharp Monitor 27"', @AccessoriesId);

-- Mobile Phones
INSERT INTO Products (Name, CategoryId) VALUES ('iPhone 15 Pro', @MobilePhonesId);
INSERT INTO Products (Name, CategoryId) VALUES ('Samsung Galaxy S24', @MobilePhonesId);
INSERT INTO Products (Name, CategoryId) VALUES ('Google Pixel 8', @MobilePhonesId);

-- Audio
INSERT INTO Products (Name, CategoryId) VALUES ('Sony WH-1000XM5 Headphones', @AudioId);
INSERT INTO Products (Name, CategoryId) VALUES ('Bose QuietComfort 45', @AudioId);
INSERT INTO Products (Name, CategoryId) VALUES ('JBL Flip 6 Speaker', @AudioId);

-- Men's Shirts
INSERT INTO Products (Name, CategoryId) VALUES ('Calvin Klein Dress Shirt', @ShirtsId);
INSERT INTO Products (Name, CategoryId) VALUES ('Ralph Lauren Polo Shirt', @ShirtsId);
INSERT INTO Products (Name, CategoryId) VALUES ('Nike Dri-FIT T-Shirt', @ShirtsId);

-- Men's Pants
INSERT INTO Products (Name, CategoryId) VALUES ('Levi''s 501 Jeans', @PantsId);
INSERT INTO Products (Name, CategoryId) VALUES ('Dockers Chinos', @PantsId);
INSERT INTO Products (Name, CategoryId) VALUES ('Adidas Track Pants', @PantsId);

-- Men's Shoes
INSERT INTO Products (Name, CategoryId) VALUES ('Nike Air Max 270', @ShoesId);
INSERT INTO Products (Name, CategoryId) VALUES ('Adidas Ultraboost', @ShoesId);
INSERT INTO Products (Name, CategoryId) VALUES ('Clarks Desert Boots', @ShoesId);
