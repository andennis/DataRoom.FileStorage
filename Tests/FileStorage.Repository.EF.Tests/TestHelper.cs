using Common.Configuration;
using Common.Repository;

namespace FileStorage.Repository.EF.Tests
{
    public static class TestHelper
    {
        private class TestDbConfig : AppConfigBase, IDbConfig
        {
            public TestDbConfig()
            :base("ConnectionStrings")
            {
            }
            public string ConnectionString => GetValue<string>("FileStorageConnection");
        }

        public static IDbConfig DbConfig => new TestDbConfig();
    }
}
