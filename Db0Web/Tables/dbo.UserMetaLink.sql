CREATE TABLE [dbo].[UserMetaLink] (
  [MetaId] [int] NOT NULL,
  [UserId] [varchar](100) NOT NULL,
  CONSTRAINT [PK_UserMetaLink] PRIMARY KEY CLUSTERED ([MetaId], [UserId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserMetaLink]
  ADD CONSTRAINT [FK_UserMetaLink_User_ID] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([ID])
GO

ALTER TABLE [dbo].[UserMetaLink]
  ADD CONSTRAINT [FK_UserMetaLink_UserMetadata_MetadataId] FOREIGN KEY ([MetaId]) REFERENCES [dbo].[UserMetadata] ([MetadataId])
GO