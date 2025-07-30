CREATE TABLE [dbo].[TaxonomyLink] (
    [FilmId]     VARCHAR (100) NOT NULL,
    [TaxonomyId] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([FilmId] ASC, [TaxonomyId] ASC),
    FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Film] ([ID]),
    FOREIGN KEY ([TaxonomyId]) REFERENCES [dbo].[Taxonomy] ([Id])
);

