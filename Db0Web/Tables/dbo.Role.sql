CREATE TABLE [dbo].[Role] (
  [ID] [varchar](100) NOT NULL,
  [name] [nvarchar](50) NOT NULL,
  [createAt] [datetime] NULL,
  [updateAt] [datetime] NULL,
  CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO