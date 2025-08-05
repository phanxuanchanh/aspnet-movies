CREATE TABLE [dbo].[PaymentMethod] (
  [ID] [int] IDENTITY,
  [name] [varchar](30) NOT NULL,
  [createAt] [datetime] NULL,
  [updateAt] [datetime] NULL,
  CONSTRAINT [PK_PaymentMethod] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
GO