using MagicVilla_webAPI.Models.DTO;

namespace MagicVilla_webAPI.Data;

public static class VillaStore
{
    public static List<VillaDTO> VillaDTOs = new List<VillaDTO>
    {
        new VillaDTO { Id = 1, Name = "Pool View", Occupancy = 4, Sqft = 100},
        new VillaDTO { Id = 2, Name = "Beach View", Occupancy = 3, Sqft = 400}

    };
}