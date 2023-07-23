using AutoMapper;
using MagicVilla_webAPI.Models;
using MagicVilla_webAPI.Models.DTO;

namespace MagicVilla_webAPI;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDTO>();
        CreateMap<VillaDTO, Villa>();

        CreateMap<VillaUpdateDto, Villa>().ReverseMap();
        CreateMap<VillaCreateDto, Villa>().ReverseMap();
    }
}