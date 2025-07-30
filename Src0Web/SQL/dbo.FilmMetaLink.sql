CREATE TABLE [dbo].[FilmMetaLink] (
    [FilmId] VARCHAR (100) NOT NULL,
    [MetaId] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([FilmId] ASC, [MetaId] ASC),
    FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Film] ([ID]),
    FOREIGN KEY ([MetaId]) REFERENCES [dbo].[FilmMetadata] ([Id])
);

