using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using linkApi.Entities;
using linkApi.Repositories;
using linkApi.Services;
using linkApi.Factories;

namespace linkApi.Controllers;

[Route("api/short")]
[ApiController]
public class ShortController : ControllerBase
{
    private UrlRepo _repo;
    private HashingService _hashingService;
    private UrlFactory _urlFactory;
    public ShortController(UrlRepo repo)
    {
        _repo = repo;
        _hashingService = new HashingService();
        _urlFactory = new UrlFactory();
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(string))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Generate([FromBody] string ogUrl)
    {
        if (!_urlFactory.IsValidUrl(ogUrl))
        {
            return BadRequest("Incorrect url format");
        }

        Url? url;
        string baseUri = $"{Request.Scheme}://{Request.Host}";

        url = await _repo.FindByOgUrl(ogUrl);

        //url already exists in a db
        if(url is not null)
        {
            if(url.Hash != _hashingService.EncodeBase10To62(url.Id, out string newHash))
            {
                url.Hash = newHash;

                var result = await _repo.UpdateAsync(url);
                if (result is null) WriteLine("Unable to add hash a url record");

                return Ok(value: $"{baseUri}/api/short/{url.Hash}");
            }

            return Ok(value: $"{baseUri}/api/short/{url.Hash}");
        }

        url = _urlFactory.Create(ogUrl, ensureValidity: false);
        url = await _repo.CreateRecordAsync(url!);
        
        if(url is null)
        {
            return BadRequest("DB error create, change error type in the future");
        }

        // Need to save url to a DB first for an id to be assigned to it.
        // Then use this id to generate a hash for the url's record.

        string hash = _hashingService.EncodeBase10To62(url.Id);
        url.Hash = hash;
        url = await _repo.UpdateAsync(url);

        if(url is null)
        {
            return BadRequest("DB error update, change error type in the future");
        }

        return Created(uri: $"{baseUri}/api/short/{hash}", value: hash);
    }   

    [HttpGet("{hash}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(302)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOriginal(string hash)
    {

        int urlId = _hashingService.DecodeBase62To10(hash);
        
        Url? url = await _repo.FindById(urlId);

        if (url is null)
        {
            return BadRequest("Provided hash is not assigned to any url at the moment.");
        }

        url.AccessCount++;
        await _repo.UpdateAsync(url);
        return Redirect(url.OriginalUrl);
        
    }
}
