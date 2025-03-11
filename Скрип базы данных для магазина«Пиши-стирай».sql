create table Role
(
	RoleID serial primary key,
	RoleName varchar(100) not null
);

create table Users
(
	UserID serial primary key,
	UserSurname varchar(100) not null,
	UserName varchar(100) not null,
	UserPatronymic varchar(100) not null,
	UserLogin varchar(100) not null,
	UserPassword varchar not null,
	UserRole int not null,
	constraint UserRole_fk foreign key (UserRole) references Role(RoleID),
	constraint unique_user_login unique (UserLogin)
);

create table Status
(
	StatusID serial primary key,
	StatusName varchar(100) not null
);

create table PickupPoint
(
	PickupPointID serial primary key,
	PickupPointName varchar(100) not null
);

create table Orders
(
	OrderID serial primary key,
	OrderStatus int not null,
	OrderDate timestamp not null,
	OrderDeliveryDate timestamp not null,
	OrderPickupPoint int not null,
	OrderClient int not null DEFAULT 1,
	OrderCodeToReceive int not null,
	constraint UserRole_fk foreign key (OrderStatus) references Status(StatusID),
	constraint OrderPickupPoint_fk foreign key (OrderPickupPoint) references PickupPoint(PickupPointID),
	constraint OrderClient_fk foreign key (OrderClient) references Users(UserID)
);

create table Categories
(
	CategoryID serial primary key,
	CategoryName varchar(100) not null
);

create table UnitOfMeasurement
(
	UnitOfMeasurementID serial primary key,
	UnitOfMeasurementName varchar(100) not null
);

create table Product
(
	ProductArticleNumber varchar(100) primary key,
	ProductName varchar not null,
	ProductUnitOfMeasurement int not null,
	ProductCost decimal(19,4) not null,
	ProductMaximumPossibleDiscountAmount smallint null,
	ProductCategory int not null,
	ProductDiscountAmount smallint null,
	ProductQuantityInStock int not null,
	ProductDescription varchar not null,
	ProductPhoto varchar not null,
	ProductStatus varchar null,
	constraint ProductUnitOfMeasurement_fk foreign key (ProductUnitOfMeasurement) references UnitOfMeasurement(UnitOfMeasurementID),
	constraint ProductCategory_fk foreign key (ProductCategory) references Categories(CategoryID)
);

create table OrderProduct
(
	OrderProductID serial primary key,
	OrderID int not null,
	ProductArticleNumber varchar(100) not null,	
	ProductQuantity int not null,
	constraint Order_OrderProduct_fk foreign key (OrderID) references Orders(OrderID),
	constraint Product_OrderProduct_fk foreign key (ProductArticleNumber) references Product(ProductArticleNumber)
);

create table Manufacturers
(
	ManufacturerID serial primary key,
	ManufacturerName varchar(100) not null
);

create table ProductManufacturer
(
	ProductManufacturerID serial primary key,
	ProductArticleNumber varchar(100) not null,	
	ManufacturerID int not null,
	constraint Product_ProductManufacturer_fk foreign key (ProductArticleNumber) references Product(ProductArticleNumber),
	constraint Manufacturer_ProductManufacturer_fk foreign key (ManufacturerID) references Manufacturers(ManufacturerID)	
);

create table Suppliers
(
	SupplierID serial primary key,
	SupplierName varchar(100) not null
);

create table ProductSupplier
(
	ProductSupplierID serial primary key,
	ProductArticleNumber varchar(100) not null,	
	SupplierID int not null,
	constraint Product_ProductSupplier_fk foreign key (ProductArticleNumber) references Product(ProductArticleNumber),
	constraint Supplier_ProductSupplier_fk foreign key (SupplierID) references Suppliers(SupplierID)	
);
