using System.ComponentModel.DataAnnotations;

namespace MagicVilla_webAPI.Models.DTO;

public class VillaDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string Name { get; set; }
}