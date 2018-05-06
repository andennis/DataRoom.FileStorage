using Common.Configuration;
using Common.Repository;

namespace FileStorage.Repository.EF.Tests
{
    public static class FsRepTestHelper
    {
        private class TestDbConfig : AppConfigBase
        {
            public override string ConnectionString => GetConnectionString("FileStorageConnection");
        }

        public static IDbConfig DbConfig => new TestDbConfig();
    }
}
