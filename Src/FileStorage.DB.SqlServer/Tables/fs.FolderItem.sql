CREATE TABLE [fs].[FolderItem] (
    [FolderItemId] BIGINT            IDENTITY (1, 1) NOT NULL,
    [ParentId]     BIGINT            NULL,
    [Name]         NVARCHAR (128) NOT NULL,
    [RowVersion]   INT            NOT NULL,
    [CreatedDate]  DATETIME       NOT NULL,
    [UpdatedDate]  DATETIME       NOT NULL,
    CONSTRAINT [PK_fs.FolderItem] PRIMARY KEY CLUSTERED ([FolderItemId] ASC),
    CONSTRAINT [FK_fs.FolderItem_fs.FolderItem_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [fs].[FolderItem] ([FolderItemId])
);






GO
CREATE NONCLUSTERED INDEX [IX_ParentId]
    ON [fs].[FolderItem]([ParentId] ASC);

