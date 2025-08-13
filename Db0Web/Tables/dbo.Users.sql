CREATE TABLE [dbo].[Users] (
  [ID] [varchar](100) NOT NULL,
  [UserName] [varchar](50) NOT NULL,
  [SurName] [nvarchar](50) NULL,
  [MiddleName] [nvarchar](50) NULL,
  [Name] [nvarchar](50) NULL,
  [Email] [nvarchar](100) NOT NULL,
  [PhoneNumber] [char](11) NULL,
  [Password] [varchar](100) NOT NULL,
  [Salt] [varchar](100) NOT NULL,
  [Description] [ntext] NULL,
  [Activated] [bit] NOT NULL,
  [RoleId] [varchar](100) NOT NULL,
  [CreatedAt] [datetime] NULL,
  [UpdatedAt] [datetime] NULL,
  [DeletedAt] [datetime] NULL,
  CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([ID])
)
ON [PRIMARY]
TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Users]
  ADD CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([ID])
GO