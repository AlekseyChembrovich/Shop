CREATE TABLE [dbo].[Provider]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY,
	[Name] NVARCHAR(100) NULL, 
    [Address] NVARCHAR(100) NULL, 
    [TaxpayerIdentification] NCHAR(12) NULL, 
    [Phone] NCHAR(25) NULL
)
