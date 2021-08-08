using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using GeoLocator.Models;
using GeoLocator.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GeoLocator.Controllers
{
    [ApiController]
    public class GeoLocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public GeoLocationController(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        [HttpGet("ip/location")]
        public ActionResult<Location> GetLocationByIp(string ip)
        {
            if (!ValidateIpV4(ip))
            {
                return BadRequest($"Provided ip address {ip} is not valid");
            }

            var location = _locationRepository.GetLocationByIp(ip);

            return Ok(location);
        }

        [HttpGet("city/locations")]
        public Location[] GetLocationsByCity([StringLength(24)]string city)
        {
            return _locationRepository.GetLocationsByCity(city);
        }

        private static bool ValidateIpV4(string ipString)
        {
            return ipString.Count(c => c == '.') == 3 && IPAddress.TryParse(ipString, out _);
        }
    }
}