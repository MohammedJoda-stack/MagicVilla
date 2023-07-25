using System.Net;

namespace MagicVilla_webAPI.Models;

public class APiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public List<string>? ErrorMessages { get; set; }
    public Object Result { get; set; }
    
}