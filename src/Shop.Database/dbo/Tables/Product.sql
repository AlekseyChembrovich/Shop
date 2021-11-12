CREATE TABLE [dbo].[Product]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Name] NVARCHAR(100) NULL, 
    [Balance] INT NULL, 
    [Price] MONEY NULL, 
    [MinValue] INT NULL, 
    [TypeProductId] INT NULL, 
    [StorageId] INT NULL,
    CONSTRAINT FK_TypeProduct FOREIGN KEY ([TypeProductId]) REFERENCES [dbo].[TypeProduct] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Storage FOREIGN KEY ([StorageId]) REFERENCES [dbo].[Storage] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
)
