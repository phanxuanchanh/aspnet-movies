CREATE TABLE [dbo].[TaxonomyLink] (
  [FilmId] [varchar](100) NOT NULL,
  [TaxonomyId] [int] NOT NULL,
  PRIMARY KEY CLUSTERED ([FilmId], [TaxonomyId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaxonomyLink]
  ADD FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Film] ([ID])
GO

ALTER TABLE [dbo].[TaxonomyLink]
  ADD FOREIGN KEY ([TaxonomyId]) REFERENCES [dbo].[Taxonomy] ([Id])
GO