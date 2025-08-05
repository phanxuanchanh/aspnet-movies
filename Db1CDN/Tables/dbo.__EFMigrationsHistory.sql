CREATE TABLE [dbo].[__EFMigrationsHistory] (
  [MigrationId] [nvarchar](150) NOT NULL,
  [ProductVersion] [nvarchar](32) NOT NULL,
  CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId])
)
ON [PRIMARY]
GO