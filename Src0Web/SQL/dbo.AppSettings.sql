CREATE TABLE [dbo].[AppSettings] (
    [Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Name]  VARCHAR (100)  NOT NULL,
    [Value] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);

