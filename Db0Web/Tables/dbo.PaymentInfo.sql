CREATE TABLE [dbo].[PaymentInfo] (
  [userId] [varchar](100) NOT NULL,
  [paymentMethodId] [int] NOT NULL,
  [cardNumber] [varchar](50) NOT NULL,
  [cvv] [varchar](5) NOT NULL,
  [owner] [varchar](100) NOT NULL,
  [expirationDate] [varchar](30) NOT NULL,
  [createAt] [datetime] NULL,
  [updateAt] [datetime] NULL,
  CONSTRAINT [PK_PaymentInfo] PRIMARY KEY CLUSTERED ([userId], [paymentMethodId])
)
ON [PRIMARY]
GO

ALTER TABLE [dbo].[PaymentInfo]
  ADD CONSTRAINT [FK_PaymentInfo_PaymentMethod] FOREIGN KEY ([paymentMethodId]) REFERENCES [dbo].[PaymentMethod] ([ID])
GO

ALTER TABLE [dbo].[PaymentInfo]
  ADD CONSTRAINT [FK_PaymentInfo_User] FOREIGN KEY ([userId]) REFERENCES [dbo].[User] ([ID])
GO