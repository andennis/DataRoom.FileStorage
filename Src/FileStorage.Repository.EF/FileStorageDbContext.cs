using Common.Repository.EF;
using FileStorage.Repository.Core.Entities;
using FileStorage.Repository.EF.Mapping;
using Microsoft.EntityFrameworkCore;

namespace FileStorage.Repository.EF
{
    public sealed class FileStorageDbContext : DbContextBase
    {
        public FileStorageDbContext(string connectionString)
            :base(connectionString)
        {
            //Database.SetInitializer<FileStorageDbContext>(null);
            //this.Configuration.LazyLoadingEnabled = false;
        }

        public override string DbScheme => "fs";

        //public DbSet<FolderItem> FolderItems { get; set; }
        public DbSet<StorageItem> StorageItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FolderItemConfiguration(DbScheme));
            modelBuilder.ApplyConfiguration(new StorageItemConfiguration(DbScheme));

            base.OnModelCreating(modelBuilder);
        }
    }
}
