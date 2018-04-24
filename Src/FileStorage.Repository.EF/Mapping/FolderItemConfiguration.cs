using Common.Repository.EF;
using FileStorage.Repository.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Repository.EF.Mapping
{
    public class FolderItemConfiguration : BaseEntityTypeConfiguration<FolderItem>
    {
        public FolderItemConfiguration(string dbScheme)
            : base(dbScheme)
        {
        }

        public override void Configure(EntityTypeBuilder<FolderItem> builder)
        {
            builder.ToTable("FolderItem", _dbScheme);
            builder.HasMany(x => x.ChildFolders).WithOne(x => x.Parent).HasForeignKey(x => x.ParentId);
            builder.HasMany(x => x.ChildStorageItems).WithOne(x => x.Parent).HasForeignKey(x => x.ParentId);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
        }
    }
}
