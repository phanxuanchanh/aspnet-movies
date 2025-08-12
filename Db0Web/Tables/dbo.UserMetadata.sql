CREATE TABLE [dbo].[UserMetadata] (
  [MetadataId] [int] NOT NULL,
  [Name] [varchar](50) NOT NULL,
  [Description] [varchar](100) NULL,
  [Custom] [varchar](30) NULL,
  [Type] [datetime] NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [varchar](50) NULL,
  [DeletedAt] [varchar](50) NULL,
  CONSTRAINT [PK_PaymentInfo] PRIMARY KEY CLUSTERED ([MetadataId])
)
ON [PRIMARY]
GO