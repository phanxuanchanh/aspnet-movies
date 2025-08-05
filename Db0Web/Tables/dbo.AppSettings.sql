CREATE TABLE [dbo].[AppSettings] (
  [Id] [int] IDENTITY,
  [Name] [varchar](100) NOT NULL,
  [Value] [nvarchar](max) NULL,
  PRIMARY KEY CLUSTERED ([Id]),
  UNIQUE ([Name])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO