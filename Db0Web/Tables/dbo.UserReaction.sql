CREATE TABLE [dbo].[UserReaction] (
  [filmId] [varchar](100) NOT NULL,
  [userId] [varchar](100) NOT NULL,
  [upvoted] [bit] NULL,
  [downvoted] [bit] NULL,
  [createAt] [datetime] NULL,
  [updateAt] [datetime] NULL,
  CONSTRAINT [PK_UserReaction] PRIMARY KEY CLUSTERED ([filmId], [userId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserReaction]
  ADD CONSTRAINT [FK_UserReaction_Film] FOREIGN KEY ([filmId]) REFERENCES [dbo].[Film] ([ID])
GO

ALTER TABLE [dbo].[UserReaction]
  ADD CONSTRAINT [FK_UserReaction_User] FOREIGN KEY ([userId]) REFERENCES [dbo].[User] ([ID])
GO