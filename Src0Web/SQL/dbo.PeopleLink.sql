CREATE TABLE [dbo].[PeopleLink] (
    [FilmId]   VARCHAR (100) NOT NULL,
    [PersonId] BIGINT        NOT NULL,
    PRIMARY KEY CLUSTERED ([FilmId] ASC, [PersonId] ASC),
    FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Film] ([ID]),
    FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([Id])
);

