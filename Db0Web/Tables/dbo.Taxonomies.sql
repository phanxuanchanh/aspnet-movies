CREATE TABLE [dbo].[Taxonomies] (
  [Id] [int] IDENTITY,
  [Name] [nvarchar](255) NOT NULL,
  [Description] [nvarchar](max) NULL,
  [Custom] [nvarchar](max) NULL,
  [Type] [nvarchar](50) NOT NULL,
  [CreatedAt] [datetime] NOT NULL DEFAULT (getdate()),
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  PRIMARY KEY CLUSTERED ([Id])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO