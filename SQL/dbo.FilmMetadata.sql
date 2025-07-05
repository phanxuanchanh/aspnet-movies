CREATE TABLE [dbo].[FilmMetadata] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [Custom]      NVARCHAR (MAX) NULL,
    [CreatedAt]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [UpdatedAt]   DATETIME       NULL,
    [DeletedAt]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

