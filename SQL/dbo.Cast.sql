CREATE TABLE [dbo].[Cast] (
    [ID]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [name]        NVARCHAR (50) NOT NULL,
    [description] NTEXT         NULL,
    [createdAt]    DATETIME      NULL,
    [updatedAt]    DATETIME      NULL,
    [deletedAt] DATETIME NULL, 
    CONSTRAINT [PK_Cast] PRIMARY KEY CLUSTERED ([ID] ASC)
);

