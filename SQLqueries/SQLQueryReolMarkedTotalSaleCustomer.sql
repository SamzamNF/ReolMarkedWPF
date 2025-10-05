CREATE PROCEDURE uspSalesForShelfVendor
	@ShelfVendorID int = NULL
AS
BEGIN

-- Hvis man vil have dem alle
IF @ShelfVendorID is NULL SELECT
	SHELF_VENDOR.ShelfVendorID,
	SHELF_VENDOR.FirstName,
	SHELF_VENDOR.LastName,
	SUM(TRANSACTION_PRODUCT.Amount * TRANSACTION_PRODUCT.UnitPrice) AS TotalSale

	FROM SHELF_VENDOR
	
	-- Forbinder
	INNER JOIN RENT_AGREEMENT
		on SHELF_VENDOR.ShelfVendorID = RENT_AGREEMENT.ShelfVendorID
	INNER JOIN SHELF
		on RENT_AGREEMENT.RentAgreementID = SHELF.RentAgreementID
	INNER JOIN [PRODUCT]
		on [PRODUCT].ShelfNumber = SHELF.ShelfNumber
	INNER JOIN TRANSACTION_PRODUCT
		on TRANSACTION_PRODUCT.ProductID = [PRODUCT].ProductID

	GROUP BY
		SHELF_VENDOR.ShelfVendorID,
		SHELF_VENDOR.FirstName,
		SHELF_VENDOR.LastName

ELSE SELECT
	SHELF_VENDOR.ShelfVendorID,
	SHELF_VENDOR.FirstName,
	SHELF_VENDOR.LastName,
	SUM(TRANSACTION_PRODUCT.Amount * TRANSACTION_PRODUCT.UnitPrice) AS TotalSale

	FROM SHELF_VENDOR
	
	INNER JOIN RENT_AGREEMENT
		on SHELF_VENDOR.ShelfVendorID = RENT_AGREEMENT.ShelfVendorID
	INNER JOIN SHELF
		on RENT_AGREEMENT.RentAgreementID = SHELF.RentAgreementID
	INNER JOIN [PRODUCT]
		on [PRODUCT].ShelfNumber = SHELF.ShelfNumber
	INNER JOIN TRANSACTION_PRODUCT
		on TRANSACTION_PRODUCT.ProductID = [PRODUCT].ProductID

	-- Filtrere efter den person som vi vil have
	WHERE SHELF_VENDOR.ShelfVendorID = @ShelfVendorID

	GROUP BY
		SHELF_VENDOR.ShelfVendorID,
		SHELF_VENDOR.FirstName,
		SHELF_VENDOR.LastName
END

exec uspSalesForShelfVendor '1018'