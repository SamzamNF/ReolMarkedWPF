/****************************************************************************************
    Dette script er en "data seeder", designet til at fylde ReolmarkedDB-databasen
    med et komplet s�t dummy-data til udvikling og test.

    S�DAN BRUGES SCRIPTET:
    1. S�rg for at du har k�rt 'init-database.sql' f�rst, s� alle tabeller eksisterer.
    2. K�r hele dette script i SQL Server Management Studio (SSMS).
    3. Scriptet kan k�res igen og igen for at nulstille data til udgangspunktet.
****************************************************************************************/

-- Sikrer, at vi arbejder i den korrekte database
USE ReolmarkedDB;
GO

-- TRIN 1: RYD EKSISTERENDE DATA
-- R�kkef�lgen er vigtig pga. fremmedn�gler. Vi sletter fra de tabeller,
-- der bliver refereret til af andre, f�rst.
DELETE FROM [dbo].[TRANSACTION_PRODUCT];
DELETE FROM [dbo].[PRODUCT];
DELETE FROM [dbo].[SHELF];
DELETE FROM [dbo].RENT_AGREEMENT;
DELETE FROM [dbo].[PAYMENT_METHOD];
DELETE FROM [dbo].[SHELF_VENDOR];
GO

-- Nulstil IDENTITY-kolonner korrekt til 0.
DBCC CHECKIDENT ('[SHELF_VENDOR]', RESEED, 0);
DBCC CHECKIDENT ('[RENT_AGREEMENT]', RESEED, 0);
DBCC CHECKIDENT ('[SHELF]', RESEED, 0);
GO


-- TRIN 2: INDS�T HYLDE-S�LGERE (SHELF VENDORS)
-- Vi opretter 20 fiktive s�lgere.
INSERT INTO [dbo].[SHELF_VENDOR] (FirstName, LastName, PhoneNumber, Email) VALUES
('Mette', 'Jensen', '22334455', 'mette.j@example.com'),
('Lars', 'Nielsen', '33445566', 'lars.n@example.com'),
('Sofie', 'Hansen', '44556677', 'sofie.h@example.com'),
('Frederik', 'Pedersen', '55667788', 'frederik.p@example.com'),
('Ida', 'Andersen', '66778899', 'ida.a@example.com'),
('Mads', 'Christensen', '77889900', 'mads.c@example.com'),
('Clara', 'Larsen', '88990011', 'clara.l@example.com'),
('Emil', 'S�rensen', '99001122', 'emil.s@example.com'),
('Laura', 'Rasmussen', '12345678', 'laura.r@example.com'),
('Viktor', 'J�rgensen', '23456789', 'viktor.j@example.com'),
('Emma', 'Petersen', '34567890', 'emma.p@example.com'),
('Oliver', 'Madsen', '45678901', 'oliver.m@example.com'),
('Josefine', 'Kristensen', '56789012', 'josefine.k@example.com'),
('Noah', 'Olsen', '67890123', 'noah.o@example.com'),
('Karla', 'Thomsen', '78901234', 'karla.t@example.com'),
('August', 'Christiansen', '89012345', 'august.c@example.com'),
('Frida', 'Poulsen', '90123456', 'frida.p@example.com'),
('Aksel', 'Johansen', '11234567', 'aksel.j@example.com'),
('Agnes', 'M�ller', '22345678', 'agnes.m@example.com'),
('Valdemar', 'Mortensen', '33456789', 'valdemar.m@example.com');
GO


-- TRIN 3: INDS�T BETALINGSMETODER
-- Vi tildeler en betalingsmetode til hver af de 20 s�lgere.
INSERT INTO [dbo].[PAYMENT_METHOD] (ShelfVendorID, PaymentOption, PaymentInfo) VALUES
(1, 'MobilePay', '22334455'),
(2, 'AccountNumber', '1111-0001234567'),
(3, 'MobilePay', '44556677'),
(4, 'MobilePay', '55667788'),
(5, 'AccountNumber', '2222-0002345678'),
(6, 'MobilePay', '77889900'),
(7, 'AccountNumber', '3333-0003456789'),
(8, 'MobilePay', '99001122'),
(9, 'MobilePay', '12345678'),
(10, 'AccountNumber', '4444-0004567890'),
(11, 'MobilePay', '34567890'),
(12, 'MobilePay', '45678901'),
(13, 'AccountNumber', '5555-0005678901'),
(14, 'MobilePay', '67890123'),
(15, 'AccountNumber', '6666-0006789012'),
(16, 'MobilePay', '89012345'),
(17, 'AccountNumber', '7777-0007890123'),
(18, 'MobilePay', '11234567'),
(19, 'MobilePay', '22345678'),
(20, 'AccountNumber', '8888-0008901234');
GO


-- TRIN 4: INDS�T HYLDER (SHELVES)
-- Vi opretter 80 hylder med en fast pris og en blanding af typer.
-- 'RentAgreementID' er NULL i f�rste omgang, da de ikke er udlejet endnu.
DECLARE @i INT = 1;
WHILE @i <= 80
BEGIN
    DECLARE @shelfType NVARCHAR(40);
    IF @i % 2 = 0
        SET @shelfType = 'Med b�jle';
    ELSE
        SET @shelfType = 'Uden b�jle';

    INSERT INTO [dbo].[SHELF] (RentAgreementID, ShelfType, Price) VALUES
    (NULL, @shelfType, 850.00);

    SET @i = @i + 1;
END;
GO


-- TRIN 5: INDS�T LEJEAFTALER (RENT AGREEMENTS)
-- Vi opretter 20 lejeaftaler, �n for hver s�lger, for en periode p� en m�ned.
-- Vi bruger GETDATE() til at skabe realistiske, aktuelle datoer.
DECLARE @vendorId INT = 1;
WHILE @vendorId <= 20
BEGIN
    -- Vi varierer startdatoerne lidt for realismens skyld
    DECLARE @startDate DATE = DATEADD(DAY, -@vendorId, GETDATE());
    DECLARE @endDate DATE = DATEADD(MONTH, 1, @startDate);

    INSERT INTO [dbo].[RENT_AGREEMENT] (ShelfVendorID, StartDate, EndDate) VALUES
    (@vendorId, @startDate, @endDate);

    SET @vendorId = @vendorId + 1;
END;
GO


-- TRIN 6: FORBIND HYLDER TIL LEJEAFTALER
-- Nu hvor lejeaftalerne findes, opdaterer vi 20 af hylderne,
-- s� de afspejler, at de er udlejet.
DECLARE @agreementIdToAssign INT = 1;
WHILE @agreementIdToAssign <= 20
BEGIN
    UPDATE [dbo].[SHELF]
    SET RentAgreementID = @agreementIdToAssign
    WHERE ShelfNumber = @agreementIdToAssign;

    SET @agreementIdToAssign = @agreementIdToAssign + 1;
END;
GO

PRINT 'Database seeding er fuldf�rt! Der er nu oprettet 20 s�lgere, 80 hylder og 20 lejeaftaler.';
GO