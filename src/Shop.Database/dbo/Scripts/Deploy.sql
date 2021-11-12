GO
USE [Shop]

GO
INSERT INTO [dbo].[StorageDirector] ([dbo].[StorageDirector].[Surname], [dbo].[StorageDirector].[Name], [dbo].[StorageDirector].[Patronymic], [dbo].[StorageDirector].[Birthday])
	VALUES ('test1', 'test1', 'test1', '2021-09-01'),
		   ('test2', 'test2', 'test2', '2021-09-01'),
		   ('test3', 'test3', 'test3', '2021-09-01'),
		   ('test4', 'test4', 'test4', '2021-09-01'),
		   ('test5', 'test5', 'test5', '2021-09-01')

GO
INSERT INTO [dbo].[Storage] ([dbo].[Storage].[Name], [dbo].[Storage].[Address], [dbo].[Storage].[StorageDirectorId])
	VALUES ('test1', 'test1', 1), ('test2', 'test2', 2), ('test3', 'test3', 3), ('test4', 'test4', 4), ('test5', 'test5', 5)

GO
INSERT INTO [dbo].[TypeProduct] ([dbo].[TypeProduct].[Name], [dbo].[TypeProduct].[ExtraCharge])
	VALUES ('test1', 18), ('test2', 20), ('test3', 18), ('test4', 20), ('test5', 18)

GO
INSERT INTO [dbo].[Product] ([dbo].[Product].[Name], [dbo].[Product].[Balance], [dbo].[Product].[Price], 
			[dbo].[Product].[MinValue], [dbo].[Product].[StorageId], [dbo].[Product].[TypeProductId])
	VALUES ('test1', 1000, 3000, 100, 1, 1),
		   ('test2', 2000, 4000, 200, 2, 2),
		   ('test3', 3000, 5000, 300, 3, 3),
		   ('test4', 4000, 6000, 400, 4, 4),
		   ('test5', 5000, 7000, 500, 5, 5)

GO
INSERT INTO [dbo].[Driver] ([dbo].[Driver].[Surname], [dbo].[Driver].[Name], [dbo].[Driver].[Patronymic], [dbo].[Driver].[Experience])
	VALUES ('test1', 'test1', 'test1', 10),
		   ('test2', 'test2', 'test2', 11),
		   ('test3', 'test3', 'test3', 12),
		   ('test4', 'test4', 'test4', 13),
		   ('test5', 'test5', 'test5', 14)

GO
INSERT INTO [dbo].[Client] ([dbo].[Client].[Name], [dbo].[Client].[Address], [dbo].[Client].[TaxpayerIdentification], [dbo].[Client].[Phone])
		VALUES ('test1', 'test1', 'test1', '+375-33-111-11-11'), 
			   ('test2', 'test2', 'test2', '+375-33-222-22-22'),
			   ('test3', 'test3', 'test3', '+375-33-333-33-33'),
			   ('test4', 'test4', 'test4', '+375-33-444-44-44'),
			   ('test5', 'test5', 'test5', '+375-33-555-55-55')

GO
INSERT INTO [dbo].[Provider] ([dbo].[Provider].[Name], [dbo].[Provider].[Address], [dbo].[Provider].[TaxpayerIdentification], [dbo].[Provider].[Phone])
		VALUES ('test1', 'test1', 'test1', '+375-33-111-11-11'), 
			   ('test2', 'test2', 'test2', '+375-33-222-22-22'),
			   ('test3', 'test3', 'test3', '+375-33-333-33-33'),
			   ('test4', 'test4', 'test4', '+375-33-444-44-44'),
			   ('test5', 'test5', 'test5', '+375-33-555-55-55')

GO
INSERT INTO [dbo].[Order] ([dbo].[Order].[Number], [dbo].[Order].[Date], [dbo].[Order].[ClientId], [dbo].[Order].[DriverId], [dbo].[Order].[ProviderId])
		VALUES (123, '2021-09-01', 1, 1, 1), 
			   (333, '2021-09-01', 2, 2, 2), 
			   (222, '2021-09-01', 3, 3, 3), 
			   (111, '2021-09-01', 4, 4, 4), 
			   (321, '2021-09-01', 5, 5, 5)

GO
INSERT INTO [dbo].[ProvideProduct] ([dbo].[ProvideProduct].[Count], [dbo].[ProvideProduct].[Price], [dbo].[ProvideProduct].[TypeProvide],
									[dbo].[ProvideProduct].[OrderId], [dbo].[ProvideProduct].[ProductId])
		VALUES (100, 1000, 'test1', 1, 1),
			   (200, 2000, 'test2', 2, 2),
			   (300, 3000, 'test1', 3, 3),
			   (400, 4000, 'test2', 4, 4),
			   (500, 5000, 'test1', 5, 5)
