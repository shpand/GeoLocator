using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GeoLocator.Models;
using GeoLocator.Repositories;
using GeoLocator.Repositories.DataReaders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GeoLocator.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class GeoLocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;

        public GeoLocationController()
        {
            var sw = new Stopwatch();
            sw.Start();
            _locationRepository = new InMemoryLocationRepository(new GeobaseDataReader("Data/geobase.dat"));
            var t = sw.ElapsedMilliseconds;
            Console.Write(t);

            sw.Restart();
            for (int i = 0; i < 10; i++)
            {
                new InMemoryLocationRepository(new GeobaseDataReader("Data/geobase.dat"));
            }
            var  k  = sw.ElapsedMilliseconds;
            Console.Write(k);
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