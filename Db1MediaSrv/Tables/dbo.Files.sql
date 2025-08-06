CREATE TABLE [dbo].[Files] (
  [Id] [varchar](100) NOT NULL,
  [PartitionKey] [varchar](100) NOT NULL,
  [FileName] [varchar](255) NOT NULL,
  [Type] [varchar](20) NOT NULL,
  [Description] [nvarchar](1000) NULL,
  [Title] [nvarchar](255) NOT NULL DEFAULT (N''),
  CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([Id])
)
ON [PRIMARY]
GO