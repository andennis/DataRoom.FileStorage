using System.Collections.Generic;
using Common.Repository;

namespace FileStorage.Repository.Core.Entities
{
    public class FolderItem : EntityVersionable
    {
        public long FolderItemId { get; set; }
        public string Name { get; set; }

        public FolderItem Parent { get; set; }
        public long? ParentId { get; set; }

        public ICollection<FolderItem> ChildFolders { get; set; }
        public ICollection<StorageItem> ChildStorageItems { get; set; }
    }
}
