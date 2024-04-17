using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using linkApi.Entities;
using linkApi.Repositories;
using linkApi.Services;

namespace linkApi.Controllers;

[Route("api/short")]
[ApiController]
public class ShortController : ControllerBase
{
    private UrlRepo _repo;
    private HashingService _hashingService;
    public ShortController(UrlRepo repo)
    {
        _repo = repo;
        _hashingService = new HashingService();
    }

    [HttpPost]
    [ProducesResponseType(201, Type = typeof(string))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Generate([FromBody] string ogUrl)
    {
        //generate a shortened link
        //save it to a db
        return Created(ogUrl, "huj");
    }

    [HttpGet("{hashkey}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(302)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetOriginal(string hashkey)
    {
        Url? url = await _repo.RetreiveByIdAsync(hashkey);

        if(url is not null)
        {
            //return Redirect(url.OriginalUrl);
            return Ok(_hashingService.EncodeBase64(url.OriginalUrl));
        }

        return Ok("NOT FOUND");
    }
}
