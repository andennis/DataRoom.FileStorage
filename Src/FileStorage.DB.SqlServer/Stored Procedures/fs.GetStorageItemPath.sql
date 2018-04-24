--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[fs].[GetStorageItemPath]') AND type in (N'P', N'PC'))
--DROP PROCEDURE [fs].[GetStorageItemPath]
--GO

-- =============================================
-- Author:		Denis Andreev
-- Create date: 01/27/2014
-- Description:	
-- =============================================
CREATE PROCEDURE [fs].[GetStorageItemPath]
	@StorageItemId INT
AS
BEGIN
	;WITH folders(FolderItemId, ParentId, FilePath) AS
	(
		SELECT fi.FolderItemId, fi.ParentId, CAST(fi.Name + '\' + si.Name AS NVARCHAR(MAX)) AS FilePath
		FROM fs.FolderItem fi
        INNER JOIN fs.StorageItem si ON si.ParentId = fi.FolderItemId AND si.[Status] = 1/*Active*/
        WHERE si.StorageItemId = @StorageItemId 
		UNION ALL
		SELECT f2.FolderItemId, f2.ParentId, f2.Name + '\' + f1.FilePath
		FROM folders f1
		INNER JOIN fs.FolderItem f2 ON f1.ParentId = f2.FolderItemId
	)
    SELECT f1.FilePath FROM folders f1
    WHERE f1.ParentId IS NULL
END