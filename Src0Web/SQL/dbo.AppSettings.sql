CREATE TABLE [dbo].[AppSettings] (
    [Id]    INT            NOT NULL,
    [Name]  VARCHAR (100)  NOT NULL,
    [Value] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

