using linkApi.DataContext;
using linkApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace linkApi.Repositories;

public class UrlRepo
{
    private readonly LinkShortenerContext _db;
    public UrlRepo(LinkShortenerContext db)
    {
        _db = db;
    }

    public async Task<Url?> RetreiveByIdAsync(string id)
    {
        Url? Url = await _db.Urls.FirstAsync(u => u.HashKey == id);

        if (Url is null)
        {
            WriteLine("Failed to retreive the Url");
            return null;
        }

        return Url;
    }
}
