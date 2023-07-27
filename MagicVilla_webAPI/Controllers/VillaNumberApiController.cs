using System.Net;
using AutoMapper;
using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_webAPI.Controllers;

[Route("api/VillaNumberApi")]
[ApiController]
public class VillaNumberApiController : ControllerBase
{
    protected APiResponse response;
    private readonly IVillaNumberRepository dbVillaNumber;
    
    private readonly IMapper mapper;

    public VillaNumberApiController(IVillaNumberRepository _dbVillaNumber, IMapper _mapper)
    {
        dbVillaNumber = _dbVillaNumber;
        mapper = _mapper;
        response = new();
    }

    [HttpGet]
    [ProducesResponseType(HttpStatusCode.OK)]
    public async ActionResult<APiResponse> GetVillaNumbers()
    {
        
    }
}