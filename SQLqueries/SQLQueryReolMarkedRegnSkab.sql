CREATE PROCEDURE uspCreateAccounting
	@StartDate DATE,
	@EndDate DATE
AS
BEGIN

	-- Beregner den totale lejepris per shelf_vendor, for alle deres reoler
	-- Opretter midlertige tabeller (CTE( Common Table Expression)) som bruges til at samle lejepervendor
	-- som kan bruges til sidst for at lave regneskab med (TotalRent)
	;WITH RentPerVendor AS (
		SELECT
			RENT_AGREEMENT.ShelfVendorID,
			SUM(SHELF.Price) AS TotalRent
		FROM SHELF
			-- Linker RentAgreement med reolen
			JOIN RENT_AGREEMENT 
			ON SHELF.RentAgreementID = RENT_AGREEMENT.RentAgreementID
		
		-- Tager kun kun de lejeaftaler, hvor en StartDato er mindre end den indtastede slutdato
		-- og slutdatoen er størrere end den indtastede startdato
		WHERE RENT_AGREEMENT.StartDate <= @EndDate
			AND RENT_AGREEMENT.EndDate >= @StartDate
		
		-- Gruppere det samlet under den enkelte lejeaftale, ud fra en shelfvendors ID
		GROUP BY RENT_AGREEMENT.ShelfVendorID
		),


	-- Salg per shelf_vendor
	-- Opretter midlertige tabeller (CTE( Common Table Expression)) som bruges til at samle SalePerVendor
	-- som kan bruges til sidst for at lave regnskab med (TotalSale)
	SalePerVendor AS (
		SELECT 
			RENT_AGREEMENT.ShelfVendorID,
			SUM(TRANSACTION_PRODUCT.Amount * TRANSACTION_PRODUCT.UnitPrice) AS TotalSale
		FROM [TRANSACTION]
		
		-- Arbejder igennem tabellener, for at linke transaktion_produkter til en lejeaftale og dermed en shelfvendor
		JOIN TRANSACTION_PRODUCT
			on [TRANSACTION].TransactionID = TRANSACTION_PRODUCT.TransactionID
		JOIN [PRODUCT]
			on TRANSACTION_PRODUCT.ProductID = [PRODUCT].ProductID
		JOIN SHELF
			on [PRODUCT].ShelfNumber = SHELF.ShelfNumber
		JOIN RENT_AGREEMENT
			on SHELF.RentAgreementID = RENT_AGREEMENT.RentAgreementID

		-- Kun de salg mellem den indtastede dato skal bruges
		WHERE [TRANSACTION].TransactionDate BETWEEN @StartDate AND @EndDate
		
		-- Gruppere alle salgene til den enkelte kunde
		GROUP BY RENT_AGREEMENT.ShelfVendorID

		)
	
	-- Samlet Regnskab
	SELECT 
		SHELF_VENDOR.ShelfVendorID,
		SHELF_VENDOR.FirstName,
		SHELF_VENDOR.LastName,
		PAYMENT_METHOD.PaymentInfo,
		-- Gør så hvis TotalRent / TotalSale er NULL (ingen salg fx) at de bliver sat til 0
		-- Dette gøres, så regnestykket stadig virker (Fx TotalSale = NULL ville ikke virke)
		-- Bruger vores data fra de midlertidige tabeller der blev oprettet før
		ISNULL(TotalRent, 0) AS TotalRent,
		ISNULL(TotalSale, 0) AS TotalSale,
		-- Runder tallet til 2 decimaler ( * 0.9, >2< her speciferes det)
		-- Tager "TotalSale" som defineret over, ganger med 0.9 med maks 2 decimaler og kalder det AfterComission
		ROUND(ISNULL(Totalsale, 0) * 0.9, 2) AS AfterComission,
		-- Tager TotalSale igen, ganger med 0.9 og trækker lejen fra, maks 2 decimaler og kalder det Payment
		ROUND(ISNULL(TotalSale, 0) * 0.9 - ISNULL(TotalRent, 0), 2) AS Payment

		FROM SHELF_VENDOR
		-- Linker de to midlertidige tabeller sammen, ved at bruge ShelfVendorID - Dette kan bruges fordi CTE'er 
		-- brugte RentAgreement.ShelfVendorID
		LEFT JOIN RentPerVendor
			on SHELF_VENDOR.ShelfVendorID = RentPerVendor.ShelfVendorID
		LEFT JOIN SalePerVendor
			on SHELF_VENDOR.ShelfVendorID = SalePerVendor.ShelfVendorID
		INNER JOIN PAYMENT_METHOD 
			ON SHELF_VENDOR.ShelfVendorID = PAYMENT_METHOD.ShelfVendorID




END


EXEC uspCreateAccounting '2025-08-01', '2025-09-30';