CREATE TABLE [dbo].[Film] (
    [ID]                VARCHAR (100) NOT NULL,
    [name]              NVARCHAR (50) NOT NULL,
    [description]       NTEXT         NULL,
    [productionCompany] NVARCHAR (50) NOT NULL,
    [releaseDate]       VARCHAR (10)  NOT NULL,
    [upvote]            BIGINT        NULL,
    [downvote]          BIGINT        NULL,
    [views]             BIGINT        NULL,
    [duration]          VARCHAR (20)  NULL,
    [thumbnail]         VARCHAR (100) NULL,
    [source]            VARCHAR (100) NULL,
    [createdAt]         DATETIME      NULL,
    [updatedAt]         DATETIME      NULL,
    [deletedAt]         DATETIME      NULL,
    CONSTRAINT [PK_Film] PRIMARY KEY CLUSTERED ([ID] ASC)
);

