using Common.Repository;

namespace FileStorage.Repository.Core.Entities
{
    public class StorageItem : EntityVersionable
    {
        public long StorageItemId { get; set; }
        public string Name { get; set; }

        public FolderItem Parent { get; set; }
        public long ParentId { get; set; }

        public string OriginalName { get; set; }
        public long Size { get; set; }
        public ItemStatus Status { get; set; }
        public StorageItemType ItemType { get; set; }
    }
}
