using MagicVilla_webAPI.Data;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<VillaDTO>> GetVillas()
    {
        return Ok(VillaStore.VillaDTOs);
    }
    
    [HttpGet("{id:int}", Name="GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<VillaDTO> GetVilla(int id)
    {
        if (id == 0)
        {
            return BadRequest();
        }

        var villa = VillaStore.VillaDTOs.FirstOrDefault(u => u.Id == id);
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

        villaDto.Id = VillaStore.VillaDTOs.OrderByDescending(u=>u.Id).FirstOrDefault().Id + 1;
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

        var villaDto = VillaStore.VillaDTOs.FirstOrDefault(u => u.Id == id);
        if (villaDto == null)
        {
            return NotFound(StatusCodes.Status404NotFound);

            
        }

        VillaStore.VillaDTOs.Remove(villaDto);
        return NoContent();

    }
}