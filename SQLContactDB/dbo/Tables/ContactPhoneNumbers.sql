CREATE TABLE [dbo].[ContactPhoneNumbers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NOT NULL, 
    [PhoneNumber] INT NOT NULL
)
