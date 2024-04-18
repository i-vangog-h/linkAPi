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
        string encoded = "";

        url = await _repo.FindByOgUrl(ogUrl);

        if(url is not null)
        {
            encoded = url.Hash;
        }

        url = _urlFactory.Create(ogUrl, ensureValidity: false);

        url = await _repo.CreateRecordAsync(url!);
        
        if(url is null)
        {
            return BadRequest("DB error create, change error type in the future");
        }

        url = await _repo.FindByOgUrl(ogUrl);

        encoded = _hashingService.EncodeBase10To62(url!.Id);

        url.Hash = encoded;

        url = await _repo.UpdateAsync(url);

        if(url is null)
        {
            return BadRequest("DB error update, change error type in the future");
        }

        return Created(uri: $"api/short/{encoded}", value: encoded);
    }

    [HttpGet("{urlEncoded}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(302)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOriginal(string urlEncoded)
    {
        Url? url = await _repo.RetreiveByHashAsync(urlEncoded);

        if(url is not null)
        {
            url.AccessCount++;
            await _repo.UpdateAsync(url);
            return Redirect(url.OriginalUrl);
        }

        return Ok("NOT FOUND");
    }
}
