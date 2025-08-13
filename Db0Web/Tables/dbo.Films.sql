CREATE TABLE [dbo].[Films] (
  [ID] [varchar](100) NOT NULL,
  [name] [nvarchar](50) NOT NULL,
  [description] [ntext] NULL,
  [productionCompany] [nvarchar](50) NOT NULL,
  [releaseDate] [varchar](10) NOT NULL,
  [upvote] [bigint] NULL,
  [downvote] [bigint] NULL,
  [views] [bigint] NULL,
  [duration] [varchar](20) NULL,
  [thumbnail] [varchar](100) NULL,
  [source] [varchar](100) NULL,
  [createdAt] [datetime] NULL,
  [updatedAt] [datetime] NULL,
  [deletedAt] [datetime] NULL,
  CONSTRAINT [PK_Film] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO