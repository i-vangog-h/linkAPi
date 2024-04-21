using linkApi.DataContext;
using linkApi.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace linkApi.Repositories;

public class UrlRepo
{
    private readonly LinkShortenerContext _db;
    public UrlRepo(LinkShortenerContext db)
    {
        _db = db;
    }

    public async Task<Url?> FindById(int id)
    {
        Url? url = await _db.Urls.SingleOrDefaultAsync(u => u.Id == id);

        if (url is null)
        {
            return null;
        }

        return url; 
    }

    public async Task<Url?> RetreiveByHashAsync(string urlEncoded)
    {
        Url? url = await _db.Urls.FirstOrDefaultAsync(u => u.Hash == urlEncoded);

        if (url is null)
        {
            WriteLine("Failed to retreive the url object");
            return null;
        }

        return url;
    }

    public async Task<Url?> FindByOgUrl(string ogUrl)
    {
        Url? url = await _db.Urls.SingleOrDefaultAsync(u => u.OriginalUrl == ogUrl);

        if (url is null)
        {
            WriteLine("Record with such url is not present in a db");
            return null;
        }

        return url;
    }

    public async Task<Url?> CreateRecordAsync(Url url)
    {
        await _db.Urls.AddAsync(url);
        var affected = await _db.SaveChangesAsync();
        if (affected == 1)
        {
            return await _db.Urls.FirstAsync(u => u.OriginalUrl == url.OriginalUrl);
        }

        return null;
    }

    public async Task<Url?> UpdateAsync(Url url)
    {
        _db.Update(url);

        int affected = await _db.SaveChangesAsync();

        if (affected == 1)
        {
            return url;
        }

        return null;

    }
}
