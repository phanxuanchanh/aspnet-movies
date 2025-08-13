CREATE TABLE [dbo].[UserHistories] (
  [Id] [bigint] NOT NULL,
  [UserId] [varchar](100) NOT NULL,
  [ActionType] [varchar](20) NOT NULL,
  [ActionContent] [varchar](max) NULL,
  [FilmId] [varchar](100) NULL,
  [CreatedAt] [datetime] NULL,
  CONSTRAINT [PK_UserReaction] PRIMARY KEY CLUSTERED ([Id])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserHistories]
  ADD CONSTRAINT [FK_UserHistory_Film_ID] FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Films] ([ID])
GO

ALTER TABLE [dbo].[UserHistories]
  ADD CONSTRAINT [FK_UserHistory_User_ID] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([ID])
GO