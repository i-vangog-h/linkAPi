using Microsoft.AspNetCore.Mvc;
using linkApi.Entities;
using linkApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace linkApi.Controllers;

[Route("api")]
[ApiController]
public class ShortController : ControllerBase
{
    private IUrlRepo _repo;
    private IHashingService _hashingService;
    private IUrlFactory _urlFactory;
    public ShortController(IUrlRepo repo, IHashingService hashingService, IUrlFactory urlFactory)
    {
        _repo = repo;
        _hashingService = hashingService;
        _urlFactory = urlFactory;
    }
    
    [HttpPost("generate")]
    [ProducesResponseType(200)] // Ok
    [ProducesResponseType(201)] // Created
    [ProducesResponseType(400)] // Bad Request
    public async Task<IActionResult> Generate([FromBody] string ogUrl)
    {
        if (!_urlFactory.IsValidUrl(ogUrl))
        {
            return BadRequest("Incorrect url format"); //400
        }

        Url? url;
        string baseUri = $"{Request.Scheme}://{Request.Host}";

        url = await _repo.FindByOgUrl(ogUrl);

        //url already exists in a db
        if (url is not null)
        {
            if (url.Hash != _hashingService.EncodeBase10To62(url.Id, out string newHash))
            {
                url.Hash = newHash;

                var result = await _repo.UpdateAsync(url);
                if (result is null) WriteLine("Unable to add hash to a url record");
            }

            return Ok(value: $"{baseUri}/api/get-original/{url.Hash}"); //200
        }

        url = _urlFactory.Create(ogUrl, ensureValidity: false);
        url = await _repo.CreateAsync(url!);

        if (url is null)
        {
            return BadRequest("DB error create, change error type in the future"); //400
        }

        // Need to save url to a DB first for an id to be assigned to it.
        // Then use this id to generate a hash for the url's record.

        string hash = _hashingService.EncodeBase10To62(url.Id);
        url.Hash = hash;
        url = await _repo.UpdateAsync(url);

        if (url is null)
        {
            return BadRequest("DB error update, change error type in the future"); //400
        }

        return Created(uri: $"{baseUri}/api/get-original/{hash}", value: $"{baseUri}/api/get-original/{hash}"); //201
    }

    private string getRoutePattern = "get-original";

    [HttpGet("get-original/{hash}")]
    [ProducesResponseType(302)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOriginal(string hash)
    {
        int urlId = _hashingService.DecodeBase62To10(hash);

        Url? url = await _repo.FindByIdAsync(urlId);

        if (url is null)
        {
            return NotFound("Provided hash is not assigned to any url at the moment.");
        }

        url.AccessCount++;
        await _repo.UpdateAsync(url);

        // just return ogUrl to a caller, let it redirect itself
        return Ok(url.OriginalUrl);

        //temp
        //return Redirect(url.OriginalUrl); //302

    }

    [HttpDelete("remove-record/{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> RemoveRecord(int id)
    {
        bool? deleted = await _repo.DeleteAsync(id);

        if(deleted is null)
        {
            return NotFound();
        }

        if (deleted.Value)
        {
            return NoContent(); //204
        }
        else
        {
            return BadRequest($"Url {id} was found but failed to delete."); //400
        }
    }
}
