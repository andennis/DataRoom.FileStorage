using Common.Repository.EF;
using FileStorage.Repository.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileStorage.Repository.EF.Mapping
{
    public class StorageItemConfiguration : BaseEntityTypeConfiguration<StorageItem>
    {
        public StorageItemConfiguration(string dbScheme)
            :base(dbScheme)
        {
        }

        public override void Configure(EntityTypeBuilder<StorageItem> builder)
        {
            builder.ToTable("StorageItem", _dbScheme);
            //builder.HasOne(x => x.Parent).WithMany(x => x.ChildStorageItems).HasForeignKey(x => x.ParentId);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(128);
            builder.Property(x => x.OriginalName).HasMaxLength(128);
            builder.Property(x => x.Size);
        }
    }
}
