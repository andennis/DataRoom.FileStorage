/*
DELETE FROM fs.StorageItem
DELETE FROM fs.FolderItem

INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder1', NULL, 0)
GO
INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder11', @@IDENTITY, 0)
GO
DECLARE @ParentId INT = @@IDENTITY
INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder111', @ParentId, 0)
INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder112', @ParentId, 0)
GO
DECLARE @ParentId INT = @@IDENTITY
INSERT INTO fs.StorageItem (Name, OriginalName, Size, [Status], ItemType, ParentId)
VALUES (N'File111_1', N'File1', 0, 0, 0, @ParentId) 
INSERT INTO fs.StorageItem (Name, OriginalName, Size, [Status], ItemType, ParentId)
VALUES (N'File111_2', N'File1', 0, 0, 0, @ParentId) 
GO
DECLARE @ParentId INT = (SELECT FolderItemId FROM fs.FolderItem WHERE Name = 'Folder111')
INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder111_1', @ParentId, 0)
GO


INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder2', NULL, 0)
GO
INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder21', @@IDENTITY, 0)
GO
INSERT INTO fs.FolderItem (Name, ParentId, ChildFoldersCount)
VALUES (N'Folder211', @@IDENTITY, 0)
GO
DECLARE @ParentId INT = @@IDENTITY
INSERT INTO fs.StorageItem (Name, OriginalName, Size, [Status], ItemType, ParentId)
VALUES (N'File211_1', N'File211_1', 0, 0, 0, @ParentId) 
INSERT INTO fs.StorageItem (Name, OriginalName, Size, [Status], ItemType, ParentId)
VALUES (N'File211_2', N'File211_2', 0, 0, 0, @ParentId) 
INSERT INTO fs.StorageItem (Name, OriginalName, Size, [Status], ItemType, ParentId)
VALUES (N'File211_3', N'File211_3', 0, 0, 0, @ParentId) 
GO
*/
