using Microsoft.AspNetCore.Mvc;


[ApiController]
[Route("api/v1/[controller]")]
public class FabricController : ControllerBase
{
    private readonly FabricService _fabricService;
    public FabricController(FabricService fabricService)
    {
        _fabricService = fabricService;
    }


}