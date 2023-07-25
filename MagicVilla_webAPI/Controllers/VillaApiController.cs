using AutoMapper;
using MagicVilla_webAPI.Data;
using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Models.DTO;
using MagicVilla_webAPI.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_webAPI.Controllers;

//[Route("api/[controller]")]
[Route("api/VillaApi")]
[ApiController]

public class VillaApiController : ControllerBase
{
    private readonly IVillaRepository dbVilla;
     private readonly ILogger<VillaApiController> logger;
     private readonly IMapper mapper;
    public VillaApiController( ILogger<VillaApiController> _logger, IVillaRepository _dbVilla, IMapper _mapper)
    {
        logger = _logger;
        dbVilla = _dbVilla;
        mapper = _mapper;
    }
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
    {
        logger.LogInformation("Get All Villas");
        IEnumerable<Villa> villaList = await dbVilla.GetAllAsync(); 
        return Ok( mapper.Map<List<VillaDTO>>(villaList) );
    }
    
    [HttpGet("{id:int}", Name="GetVilla")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VillaDTO>> GetVilla(int id)
    {
        if (id == 0)
        {
            logger.LogError("Error with id " + id);
            return BadRequest();
        }

        var villa = await dbVilla.GetAsync(u => u.Id == id);
        if (villa == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<VillaDTO>(villa));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaCreateDto createDto)
    {
       // if (!ModelState.IsValid)
        //{
          //  return BadRequest(ModelState);

        //}

        if (await dbVilla.GetAsync(u=>u.Name.ToLower() == createDto.Name.ToLower()) !=null)
        {
            ModelState.AddModelError("Custom Errors", "Customer already Exists");
        }
        if (createDto == null)
        {
            return BadRequest(createDto);
        }

        //if (villaDto.Id > 0)
        //{
          //  return StatusCode(StatusCodes.Status500InternalServerError);

       // }

        //villaDto.Id = db.Villas.OrderByDescending(u=>u.Id).FirstOrDefault().Id + 1;

        Villa model = mapper.Map<Villa>(createDto);
        // Villa model = new ()
        // {
        //   
        //     Name = createDto.Name,
        //     Amenity = createDto.Amenity,
        //     Occupancy = createDto.Occupancy,
        //     Rate = createDto.Rate,
        //     Sqft = createDto.Sqft,
        //     Details = createDto.Details,
        //     ImageUrl = createDto.ImageUrl
        // };
        await dbVilla.CreateAsync(model);
        await dbVilla.SaveAsync();
            // VillaStore.VillaDTOs.Add(villaDto);
        return CreatedAtRoute("GetVilla", new { id = model.Id}, createDto);
        
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

        //var villa=  dbVilla.GetAsync(u => u.Id == id);
        var villa =  dbVilla.GetAsync(u => u.Id == id);
        var villaResult = villa.Result;
        if (villaResult == null)
        {
            return NotFound(StatusCodes.Status404NotFound);

            
        }

        dbVilla.RemoveAsync(villaResult);
      
        return NoContent();

    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateVilla(int id, [FromBody] VillaUpdateDto updateDto)
    {
        if (updateDto== null || updateDto.Id != id )
        {
            return BadRequest(StatusCodes.Status400BadRequest);
        }

        // var villa = db.Villas.FirstOrDefault(u => u.Id == id);
        Villa model = mapper.Map<Villa>(updateDto);
        // Villa model = new Villa()
        // {
        //     Id = updateDto.Id,
        //     Name = updateDto.Name,
        //     Amenity = updateDto.Amenity,
        //     Occupancy = updateDto.Occupancy,
        //     Rate = updateDto.Rate,
        //     Sqft = updateDto.Sqft,
        //     Details = updateDto.Details,
        //     ImageUrl = updateDto.ImageUrl
        // };
       // villa.Name = villaObj.Name;
       //  villa.Occupancy = villaObj.Occupancy;
        //villa.Sqft = villaObj.Sqft;
        dbVilla.UpdateAsync(model);
        
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult >UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
    {
        if (patchDto == null || id == 0)
        {
            return BadRequest();
        }

        var villa = await dbVilla.GetAsync(u => u.Id == id, tracked:false);
        if (villa == null)
        {
            return BadRequest();
        }

        VillaUpdateDto villaDto = mapper.Map<VillaUpdateDto>(villa);
        // VillaUpdateDto villaDto = new()
        // {
        //     Id = villa.Id,
        //     Name = villa.Name,
        //     Details = villa.Details,
        //     Occupancy = villa.Occupancy,
        //     Amenity = villa.Amenity,
        //     ImageUrl = villa.ImageUrl,
        //     Rate = villa.Rate,
        //     Sqft = villa.Sqft
        // };
        patchDto.ApplyTo(villaDto, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Villa model = mapper.Map<Villa>(villaDto);
        // Villa model = new Villa()
        // {
        //     Id = villaDto.Id,
        //     Name = villaDto.Name,
        //     Amenity = villaDto.Amenity,
        //     Occupancy = villaDto.Occupancy,
        //     Rate = villaDto.Rate,
        //     Sqft = villaDto.Sqft,
        //     Details = villaDto.Details,
        //     ImageUrl = villaDto.ImageUrl
        // };
        await dbVilla.UpdateAsync(model);
        
        return NoContent();
        
    }
    
}