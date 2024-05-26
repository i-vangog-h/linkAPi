using linkApi.DataContext;
using linkApi.Entities;
using Microsoft.Extensions.Configuration;

namespace linkApi.Tests;

public class PostgresDBTests
{
    private static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();
        return config;
    }
   
    [Fact]
    public void DatabaseConnectionTest()
    {
        IConfiguration config = InitConfiguration();
        using LinkShortenerContext db = new(config);
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void AllRecordsContainUrl()
    {
        IConfiguration config = InitConfiguration();
        using LinkShortenerContext db = new(config);

        Url? missing = db.Urls.Where(u => u.OriginalUrl == null) as Url;

        Assert.Null(missing);
    }


}