CREATE TABLE [dbo].[Doctors] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (MAX) NOT NULL,
    [LastName]  NVARCHAR (MAX) NOT NULL,
	[UserId]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Doctors] PRIMARY KEY CLUSTERED ([Id] ASC)
);