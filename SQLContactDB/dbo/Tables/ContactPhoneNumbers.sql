CREATE TABLE [dbo].[ContactPhoneNumbers] (
    [Id]          INT IDENTITY (1, 1) NOT NULL,
    [ContactId]   INT NOT NULL,
    [PhoneNumberId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

