CREATE TABLE [dbo].[PeopleLink] (
  [FilmId] [varchar](100) NOT NULL,
  [PersonId] [bigint] NOT NULL,
  PRIMARY KEY CLUSTERED ([FilmId], [PersonId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[PeopleLink]
  ADD FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Film] ([ID])
GO

ALTER TABLE [dbo].[PeopleLink]
  ADD FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([Id])
GO