using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            _locationRepository = new InMemoryLocationRepositoryFast(new GeobaseDataReader("Data/geobase.dat"));
            var t = sw.ElapsedMilliseconds;
            Console.Write(t);

            sw.Restart();
            for (int i = 0; i < 10; i++)
            {
                new InMemoryLocationRepositoryFast(new GeobaseDataReader("Data/geobase.dat"));
            }
            var  k  = sw.ElapsedMilliseconds;
            Console.Write(k);
        }

        [HttpGet("ip/location")]
        //todo: add parameters check
        public Location GetLocationByIp(string ip)
        {
            return _locationRepository.GetLocationByIp(ip);
        }

        [HttpGet("city/locations")]
        public List<Location> GetLocationsByCity(string city)
        {
            return _locationRepository.GetLocationsByCity(city);
        }
    }
}