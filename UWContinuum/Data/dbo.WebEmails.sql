USE [Leads]
GO

CREATE TABLE [dbo].[WebEmails] (
    [Id]           BIGINT         NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [FirstName]    NVARCHAR (50)  NOT NULL,
    [LastName]     NVARCHAR (50)  NOT NULL,
    [EmailAddress] NVARCHAR (100) NOT NULL,
    [HasOptedIn]   BIT            NOT NULL,
    [PhoneNumber]  NVARCHAR (50)  NULL,
    [SignupDate]   DATETIME       NULL
);