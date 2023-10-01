CREATE TABLE [dbo].[ContactEmail]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ContactId] INT NOT NULL, 
    [EmailAddressId] NCHAR(10) NULL
)
