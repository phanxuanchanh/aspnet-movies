CREATE TABLE [dbo].[Country] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [name]        NVARCHAR (50) NOT NULL,
    [description] NTEXT         NULL,
    [createdAt]    DATETIME      NULL,
    [updatedAt]    DATETIME      NULL,
    [deletedAt] NCHAR(10) NULL, 
    CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED ([ID] ASC)
);

