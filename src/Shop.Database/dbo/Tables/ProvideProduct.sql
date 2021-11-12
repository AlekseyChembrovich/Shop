CREATE TABLE [dbo].[ProvideProduct]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Count] INT NULL, 
    [Price] MONEY NULL,
    [TypeProvide] NVARCHAR(100) NULL, 
    [ProductId] INT NULL, 
    [OrderId] INT NULL,
    CONSTRAINT FK_Product FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT FK_Order FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Order] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
)
