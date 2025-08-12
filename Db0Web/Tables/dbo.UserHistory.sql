CREATE TABLE [dbo].[UserHistory] (
  [UserId] [varchar](100) NOT NULL,
  [ActionType] [varchar](20) NOT NULL,
  [ActionContent] [varchar](max) NULL,
  [FilmId] [varchar](100) NULL,
  [CreatedAt] [datetime] NULL,
  CONSTRAINT [PK_UserReaction] PRIMARY KEY CLUSTERED ([UserId])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserHistory]
  ADD CONSTRAINT [FK_UserHistory_Film_ID] FOREIGN KEY ([FilmId]) REFERENCES [dbo].[Film] ([ID])
GO

ALTER TABLE [dbo].[UserHistory]
  ADD CONSTRAINT [FK_UserHistory_User_ID] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([ID])
GO