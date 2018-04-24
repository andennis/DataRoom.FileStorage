CREATE TABLE [fs].[StorageItem] (
    [StorageItemId] BIGINT            IDENTITY (1, 1) NOT NULL,
    [ParentId]      BIGINT            NOT NULL,
    [Name]          NVARCHAR (128) NOT NULL,
    [OriginalName]  NVARCHAR (128) NULL,
    [Size]          BIGINT         NULL,
    [Status]        INT            NOT NULL,
    [ItemType]      INT            NOT NULL,
    [RowVersion]    INT            NOT NULL,
    [CreatedDate]   DATETIME       NOT NULL,
    [UpdatedDate]   DATETIME       NOT NULL,
    CONSTRAINT [PK_fs.StorageItem] PRIMARY KEY CLUSTERED ([StorageItemId] ASC),
    CONSTRAINT [FK_fs.StorageItem_fs.FolderItem_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [fs].[FolderItem] ([FolderItemId]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_ParentId]
    ON [fs].[StorageItem]([ParentId] ASC);

