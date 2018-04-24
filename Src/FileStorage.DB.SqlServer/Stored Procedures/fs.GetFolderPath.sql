--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[fs].[GetFolderPath]') AND type in (N'P', N'PC'))
--DROP PROCEDURE [fs].[GetFolderPath]
--GO

-- =============================================
-- Author:		Denis Andreev
-- Create date: 01/25/2014
-- Description:	
-- =============================================
CREATE PROCEDURE [fs].[GetFolderPath]
	@FolderItemId INT
AS
BEGIN
	;WITH folders(FolderItemId, ParentId, FolderPath) AS
	(
		SELECT FolderItemId, ParentId, CAST(Name as NVARCHAR(MAX)) as FolderPath
		FROM fs.FolderItem WHERE FolderItemId = @FolderItemId
		UNION ALL
		SELECT f2.FolderItemId, f2.ParentId, f2.Name + '\' + f1.FolderPath
		FROM folders f1
		INNER JOIN fs.FolderItem f2 ON f1.ParentId = f2.FolderItemId
	)
    SELECT f1.FolderPath FROM folders f1
    WHERE f1.ParentId IS NULL
END