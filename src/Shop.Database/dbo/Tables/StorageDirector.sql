CREATE TABLE [dbo].[StorageDirector]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Surname] NVARCHAR(50) NULL, 
    [Name] NVARCHAR(50) NULL, 
    [Patronymic] NVARCHAR(50) NULL, 
    [Birthday] DATE NULL
)
