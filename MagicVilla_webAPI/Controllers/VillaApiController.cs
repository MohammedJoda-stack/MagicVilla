using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_webAPI.Controllers;
//[Route("api/[controller]")]
[Route("api/VillaApi")]
[ApiController]

public class VillaApiController : ControllerBase
{
    [HttpGet]
    public IEnumerable<VillaDTO> GetVillas()
    {
        return new List<VillaDTO> 
        {
          new VillaDTO { Id = 1, Name = "Pool View"},
          new VillaDTO { Id = 2, Name = "Beach View"}
        };
    }
}