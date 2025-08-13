CREATE TABLE [dbo].[PeopleLinks] (
  [FilmId] [varchar](100) NOT NULL,
  [PersonId] [bigint] NOT NULL,
  PRIMARY KEY CLUSTERED ([FilmId], [PersonId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[PeopleLinks]
  ADD FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Films] ([ID])
GO

ALTER TABLE [dbo].[PeopleLinks]
  ADD FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([Id])
GO