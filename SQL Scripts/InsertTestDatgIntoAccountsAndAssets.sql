USE [ae_code_challange]
GO


-- If you want to start clean, uncomment and run these deletes. Run them in order, one at a time to avoid fk constraint errors.
--delete from Assets
--delete from accounts


-- Populate some data in the Accounts and Assets tables. We'll add 50 companies. Then we'll add 50 assets to random companies. 

declare @companyCounter int = 0
declare @assetCounter int = 0
declare @assetRandomCompanyAccountID int = 0
declare @companyBaseName varchar(255) = 'ABC Co.'
declare @companyBaseID varchar(255) = 'ABC'
declare @assetBaseName varchar(255) = 'Asset #'

WHILE (SELECT count(*) from [dbo].[Accounts]) < 50 
BEGIN  
select @companyCounter = @companyCounter + 1 
INSERT INTO [dbo].[Accounts]
           ([AccountID]
           ,[CompanyName])
     VALUES
           (@companyBaseID + convert(varchar(255), @companyCounter),
		   @companyBaseName + convert(varchar(255), @companyCounter))
END  


WHILE (SELECT count(*) from [dbo].[Assets]) < 100 
BEGIN  
-- Add 100 assets randomly to the 50 companies.
-- For explanation of this random 1-50 number generator, see stackoverflow:  https://stackoverflow.com/questions/7878287/generate-random-int-value-from-3-to-6
select @assetRandomCompanyAccountID = ABS(Checksum(NewID()) % 50) + 1  
select @assetCounter = @assetCounter + 1 -- sequential number we'll use for the asset name
INSERT INTO [dbo].[Assets]
           ([X]
           ,[Y]
           ,[Name]
           ,[AccountID])
     VALUES
		(rand(),
		rand(),
		@assetBaseName + convert(varchar(255), @assetCounter),
		@companyBaseID + convert(varchar(255), @assetRandomCompanyAccountID)
		)
END  

