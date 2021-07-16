using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeoLocator.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class GeoLocationController : ControllerBase
    {
        private readonly ILogger<GeoLocationController> _logger;

        public GeoLocationController(ILogger<GeoLocationController> logger)
        {
            _logger = logger;
        }

        [HttpGet("ip/location")]
        public Location Get(string ip)
        {
            return new Location
            {
                Country = "Belarus",
                Region = "Minsk"
            };
        }
    }
}