CREATE TABLE [dbo].[FilmMetaLink] (
  [FilmId] [varchar](100) NOT NULL,
  [MetaId] [int] NOT NULL,
  PRIMARY KEY CLUSTERED ([FilmId], [MetaId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[FilmMetaLink]
  ADD FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Film] ([ID])
GO

ALTER TABLE [dbo].[FilmMetaLink]
  ADD FOREIGN KEY ([MetaId]) REFERENCES [dbo].[FilmMetadata] ([Id])
GO