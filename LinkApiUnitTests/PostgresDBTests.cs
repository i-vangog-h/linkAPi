using linkApi.DataContext;
using linkApi.Entities;

namespace linkApi.Tests;

public class PostgresDBTests
{  
    [Fact]
    public void DatabaseConnectionTest()
    {
        using LinkShortenerContext db = new();
        Assert.True(db.Database.CanConnect());
    }

    [Fact]
    public void AllRecordsContainUrl()
    {   
        using LinkShortenerContext db = new();

        Url? missing = db.Urls.Where(u => u.OriginalUrl == null) as Url;

        Assert.Null(missing);
    }


}