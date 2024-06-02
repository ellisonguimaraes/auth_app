using AuthApp.Application;
using Microsoft.AspNetCore.Mvc;

namespace AuthApp.API;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactAppServices _contactAppServices;

    public ContactController(IContactAppServices contactAppServices)
    {
        _contactAppServices = contactAppServices;
    }
    
    /// <summary>
    /// Send contact email to collegiate
    /// </summary>
    /// <param name="request">Email data</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> SendContactEmailAsync([FromBody] ContactEmailRequest request)
    {   
        await _contactAppServices.SendEmailContactFormAsync(request);
        return NoContent();
    }
}
