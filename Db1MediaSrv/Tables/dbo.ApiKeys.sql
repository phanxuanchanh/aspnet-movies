CREATE TABLE [dbo].[ApiKeys] (
  [ClientId] [nvarchar](200) NOT NULL,
  [SecretKey] [nvarchar](500) NOT NULL,
  CONSTRAINT [PK_ApiKeys] PRIMARY KEY CLUSTERED ([ClientId])
)
ON [PRIMARY]
GO