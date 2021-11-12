CREATE TABLE [dbo].[Storage]
(
	[Id] INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, 
    [Name] NVARCHAR(100) NULL, 
    [Address] NVARCHAR(100) NULL, 
    [StorageDirectorId] INT NULL,
    CONSTRAINT FK_StorageDirector FOREIGN KEY ([StorageDirectorId]) REFERENCES [dbo].[StorageDirector] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
)
