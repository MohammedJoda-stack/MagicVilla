using System.Net;
using AutoMapper;
using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Models.DTO;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APiResponse>> GetVillaNumbers()
    {
        try
        {
            IEnumerable<VillaNumber> villaNumberList = await dbVillaNumber.GetAllAsync();
            response.Result = mapper.Map<List<VillaNumberDto>>(villaNumberList);
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>()
            {
                e.ToString()
            };
            return response;
        }
        
        

    }

    [HttpGet("{id:int}", Name = "GetVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APiResponse>> GetVillaNumber(int id)
    {
        if (id == 0)
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(response);
        }
        var villaNumber = await dbVillaNumber.GetAsync(u => u.VillaNo == id);
        if (villaNumber == null)
        {
            response.IsSuccess = false;
            response.StatusCode = HttpStatusCode.NotFound;
            return NotFound(response); 
        }

        response.IsSuccess = true;
        response.Result = mapper.Map<VillaNumberDto>(villaNumber);
        response.StatusCode = HttpStatusCode.OK;
        return Ok(response);
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APiResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto createDto)
    {
        if (await dbVillaNumber.GetAsync(u => u.VillaNo == createDto.VillaNo) != null)
        {
            ModelState.AddModelError("Custom Errors", "Customer already Exists");
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>()
            {
                new string("Customer already Exists")
            };

            return BadRequest(response);
        }

        if (createDto == null)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>()
            {
                new string("not found")
            };
        }

        VillaNumber villaNumber = mapper.Map<VillaNumber>(createDto);
        await dbVillaNumber.CreateAsync(villaNumber);
        
        
        
        await dbVillaNumber.SaveAsync();
        response.Result = mapper.Map<VillaNumberDto>(villaNumber);
        response.StatusCode = HttpStatusCode.Created;
        response.IsSuccess = true;
        // VillaStore.VillaDTOs.Add(villaDto);
        return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo}, response);

    }

    [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<APiResponse> DeleteVillaNumber(int id)
    {
        if (id == 0)
        {
            return BadRequest(StatusCodes.Status400BadRequest);
        }

        var villaNumber = dbVillaNumber.GetAsync(u => u.VillaNo == id);
        var villaNumberResult = villaNumber.Result;
        if (villaNumberResult == null)
        {
            return NotFound(StatusCodes.Status404NotFound);
        }

         dbVillaNumber.RemoveAsync(villaNumberResult);
         response.StatusCode = HttpStatusCode.NoContent;
         response.IsSuccess = true;
         return Ok(response);
    }

    [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<APiResponse> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDto updateDto)
    {
        if (id != updateDto.VillaNo || updateDto == null)
        {
            return BadRequest(StatusCodes.Status400BadRequest);
        }

        VillaNumber model = mapper.Map<VillaNumber>(updateDto);
        dbVillaNumber.UpdateAsync(model);
        response.StatusCode = HttpStatusCode.NoContent;
        response.IsSuccess = true;
        
        return Ok(response);
        
    }

}