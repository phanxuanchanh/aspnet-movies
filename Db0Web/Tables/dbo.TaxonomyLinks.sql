CREATE TABLE [dbo].[TaxonomyLinks] (
  [FilmId] [varchar](100) NOT NULL,
  [TaxonomyId] [int] NOT NULL,
  PRIMARY KEY CLUSTERED ([FilmId], [TaxonomyId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[TaxonomyLinks]
  ADD FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Films] ([ID])
GO

ALTER TABLE [dbo].[TaxonomyLinks]
  ADD FOREIGN KEY ([TaxonomyId]) REFERENCES [dbo].[Taxonomy] ([Id])
GO