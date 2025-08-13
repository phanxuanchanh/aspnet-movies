CREATE TABLE [dbo].[Role] (
  [ID] [varchar](100) NOT NULL,
  [name] [nvarchar](50) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [varchar](50) NULL,
  CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO