using MagicVilla_webAPI.Data;
using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Models.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_webAPI.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaApi")]
[ApiController]

public class VillaApiController : ControllerBase
{
    private readonly ApplicationDbContext db;
     private readonly ILogger<VillaApiController> logger;
    public VillaApiController( ILogger<VillaApiController> _logger, ApplicationDbContext _db)
    {
        logger = _logger;
        db = _db;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        logger.LogInformation("Get All Villas");
        return Ok(db.Villas.ToList());
    }
    
    [HttpGet("{id:int}", Name="GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<VillaDTO> GetVilla(int id)
    {
        if (id == 0)
        {
            logger.LogError("Error with id " + id);
            return BadRequest();
        }

        var villa = db.Villas.FirstOrDefault(u => u.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(villa);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);

        }
        if (villaDto == null)
        {
            return BadRequest(villaDto);
        }

        if (villaDto.Id > 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        villaDto.Id = db.Villas.OrderByDescending(u=>u.Id).FirstOrDefault().Id + 1;
        VillaStore.VillaDTOs.Add(villaDto);
        return CreatedAtRoute("GetVilla", new { id = villaDto.Id}, villaDto);
        
    }

    [HttpDelete("{id:int}", Name = "DeleteVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult DeleteVilla(int id)
    {
        if (id == 0 )
        {
            return BadRequest(StatusCodes.Status400BadRequest);
        }

        var villaDto = db.Villas.FirstOrDefault(u => u.Id == id);
        if (villaDto == null)
        {
            return NotFound(StatusCodes.Status404NotFound);

            
        }

        db.Villas.Remove(villaDto);
        return NoContent();

    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateVilla(int id, [FromBody] Villa villaObj)
    {
        if (villaObj== null || villaObj.Id != id )
        {
            return BadRequest(StatusCodes.Status400BadRequest);
        }

        var villa = db.Villas.FirstOrDefault(u => u.Id == id);
        villa.Name = villaObj.Name;
        villa.Occupancy = villaObj.Occupancy;
        villa.Sqft = villaObj.Sqft;

        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<Villa> patchDto)
    {
        if (patchDto == null || id == 0)
        {
            return BadRequest();
        }

        var villa = db.Villas.FirstOrDefault(u => u.Id == id);
        if (villa == null)
        {
            return BadRequest();
        }
        patchDto.ApplyTo(villa, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return NoContent();
        
    }
    
}