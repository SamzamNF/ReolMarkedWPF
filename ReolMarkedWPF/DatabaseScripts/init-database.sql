-- Sikrer, at de følgende kommandoer køres på den korrekte database
USE ReolmarkedDB;
GO

-- Opretter SHELF_VENDOR tabellen
CREATE TABLE [dbo].[SHELF_VENDOR]
(
    [ShelfVendorID] INT IDENTITY (1, 1) NOT NULL,
    [FirstName]     NVARCHAR(30)    NOT NULL,
    [LastName]      NVARCHAR(30)    NOT NULL,
    [PhoneNumber]   NVARCHAR(18)    NULL,
    [Email]         NVARCHAR(50)    NOT NULL,
    CONSTRAINT [PK_SHELF_VENDOR] PRIMARY KEY CLUSTERED ([ShelfVendorID] ASC)
);

-- Opretter PAYMENT_METHOD tabellen
CREATE TABLE [dbo].[PAYMENT_METHOD]
(
    [PaymentMethodID] INT IDENTITY (1, 1) NOT NULL,
    [ShelfVendorID]   INT                 NOT NULL,
    [PaymentOption]   NVARCHAR(50)        NOT NULL,
    [PaymentInfo]     NVARCHAR(100)       NOT NULL,
    CONSTRAINT [PK_PAYMENT_METHOD] PRIMARY KEY CLUSTERED ([PaymentMethodID] ASC),
    CONSTRAINT [FK_Payment_ShelfVendor] FOREIGN KEY ([ShelfVendorID]) REFERENCES [dbo].[SHELF_VENDOR] ([ShelfVendorID]) ON DELETE CASCADE
);

-- Opretter RENT_AGREEMENT tabellen
CREATE TABLE [dbo].[RENT_AGREEMENT]
(
    [RentAgreementID] INT  IDENTITY (1, 1) NOT NULL,
    [ShelfVendorID]   INT                  NOT NULL,
    [StartDate]       DATE                 NOT NULL,
    [EndDate]         DATE                 NOT NULL,
    CONSTRAINT [PK_RENT_AGREEMENT] PRIMARY KEY CLUSTERED ([RentAgreementID] ASC),
    CONSTRAINT [FK_Rent_ShelfVendor] FOREIGN KEY ([ShelfVendorID]) REFERENCES [dbo].[SHELF_VENDOR] ([ShelfVendorID]) ON DELETE CASCADE
);

-- Opretter SHELF tabellen
CREATE TABLE [dbo].[SHELF]
(
    [ShelfNumber]     INT IDENTITY (1, 1) NOT NULL,
    [RentAgreementID] INT                 NULL,
    [ShelfType]       NVARCHAR(40)        NOT NULL,
    [Price]           DECIMAL(10, 2)      NOT NULL,
    CONSTRAINT [PK_SHELF] PRIMARY KEY CLUSTERED ([ShelfNumber] ASC),
    CONSTRAINT [FK_Shelf_RentAgreement] FOREIGN KEY ([RentAgreementID]) REFERENCES [dbo].[RENT_AGREEMENT] ([RentAgreementID]) ON DELETE SET NULL
);

-- Opretter TRANSACTION tabellen
CREATE TABLE [dbo].[TRANSACTION]
(
    [TransactionID]   INT IDENTITY (1, 1) NOT NULL,
    [TransactionDate] DATE                NOT NULL,
    [TotalPrice]      DECIMAL(10, 2)      NOT NULL,
    CONSTRAINT [PK_TRANSACTION] PRIMARY KEY CLUSTERED ([TransactionID] ASC)
);

-- Opretter PRODUCT tabellen
CREATE TABLE [dbo].[PRODUCT]
(
    [ProductID]   INT IDENTITY (1, 1) NOT NULL,
    [ShelfNumber] INT                 NOT NULL,
    [ProductName] NVARCHAR(100)       NOT NULL,
    [UnitPrice]   DECIMAL(10, 2)      NOT NULL,
    [Quantity]    INT                 NOT NULL,
    CONSTRAINT [PK_PRODUCT] PRIMARY KEY CLUSTERED ([ProductID] ASC),
    CONSTRAINT [FK_Product_Shelf] FOREIGN KEY ([ShelfNumber]) REFERENCES [dbo].[SHELF] ([ShelfNumber])
);

-- Opretter TRANSACTION_PRODUCT (koblingstabel)
CREATE TABLE [dbo].[TRANSACTION_PRODUCT]
(
    [TransactionID] INT            NOT NULL,
    [ProductID]     INT            NOT NULL,
    [UnitPrice]     DECIMAL(10, 2) NOT NULL,
    [Amount]        INT            NOT NULL,
    CONSTRAINT [PK_TRANSACTION_PRODUCT] PRIMARY KEY CLUSTERED ([TransactionID] ASC, [ProductID] ASC),
    CONSTRAINT [FK_TP_Transaction] FOREIGN KEY ([TransactionID]) REFERENCES [dbo].[TRANSACTION] ([TransactionID]) ON DELETE CASCADE,
    CONSTRAINT [FK_TP_Product] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[PRODUCT] ([ProductID]) ON DELETE CASCADE
);