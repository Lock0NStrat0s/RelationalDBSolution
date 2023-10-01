CREATE TABLE [dbo].[ContactPhoneNumbers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ContactId] INT NOT NULL, 
    [PhoneNumber] VARCHAR(20) NOT NULL
)
