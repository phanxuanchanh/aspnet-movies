CREATE TABLE [dbo].[FilmMetaLinks] (
  [FilmId] [varchar](100) NOT NULL,
  [MetaId] [int] NOT NULL,
  PRIMARY KEY CLUSTERED ([FilmId], [MetaId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[FilmMetaLinks]
  ADD FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Films] ([ID])
GO

ALTER TABLE [dbo].[FilmMetaLinks]
  ADD FOREIGN KEY ([MetaId]) REFERENCES [dbo].[FilmMetadata] ([Id])
GO