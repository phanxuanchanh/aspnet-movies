CREATE TABLE [dbo].[User] (
  [ID] [varchar](100) NOT NULL,
  [userName] [varchar](50) NOT NULL,
  [surName] [nvarchar](50) NULL,
  [middleName] [nvarchar](50) NULL,
  [name] [nvarchar](50) NULL,
  [email] [nvarchar](100) NOT NULL,
  [phoneNumber] [char](11) NULL,
  [password] [varchar](100) NOT NULL,
  [salt] [varchar](100) NOT NULL,
  [description] [ntext] NULL,
  [activated] [bit] NOT NULL,
  [roleId] [varchar](100) NOT NULL,
  [createAt] [datetime] NULL,
  [updateAt] [datetime] NULL,
  CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[User]
  ADD CONSTRAINT [FK_User_Role] FOREIGN KEY ([roleId]) REFERENCES [dbo].[Role] ([ID])
GO