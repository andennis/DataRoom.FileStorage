--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[fs].[GetFreeFolder]') AND type in (N'P', N'PC'))
--DROP PROCEDURE [fs].[GetFreeFolder]
--GO

-- =============================================
-- Author:		Denis Andreev
-- Create date: 01/21/2014
-- Description:	
-- =============================================
CREATE PROCEDURE [fs].[GetFreeFolder]
	@FolderLevel INT,
	@MaxItemsNumber INT
AS
BEGIN
    DECLARE @FolderItemId INT = NULL;

	;WITH folders(FolderItemId, ParentId, FolderLevel) AS
	(
		SELECT FolderItemId, ParentId, 0 AS FolderLevel
		FROM fs.FolderItem WHERE ParentId IS NULL
		UNION ALL
		SELECT f2.FolderItemId, f2.ParentId, f1.FolderLevel + 1 
		FROM folders f1
		INNER JOIN fs.FolderItem f2 on f1.FolderItemId = f2.ParentId
	)
    SELECT TOP 1 @FolderItemId = t.FolderItemId FROM 
    (
        SELECT f.FolderItemId, COUNT(si.ParentId) AS Number FROM folders f
        LEFT JOIN fs.StorageItem si ON si.ParentId = f.FolderItemId
        WHERE f.FolderLevel = @FolderLevel
        GROUP BY f.FolderItemId
        UNION ALL
        SELECT f1.FolderItemId, COUNT(f2.ParentId) AS  Number FROM folders f1
        LEFT JOIN fs.FolderItem f2 ON f2.ParentId = f1.FolderItemId
        WHERE f1.FolderLevel = @FolderLevel
        GROUP BY f1.FolderItemId
    )t
    GROUP BY t.FolderItemId
    HAVING SUM(t.Number) < @MaxItemsNumber
    ORDER BY SUM(t.Number)

    SELECT * FROM fs.FolderItem 
    WHERE FolderItemId = @FolderItemId
END