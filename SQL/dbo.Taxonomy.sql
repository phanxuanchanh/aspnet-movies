CREATE TABLE [dbo].[Taxonomy] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Custom]      NVARCHAR (MAX) NULL,
    [Type]        NVARCHAR (50)  NOT NULL,
    [CreatedAt]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [UpdatedAt]   DATETIME       NULL,
    [DeletedAt]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

