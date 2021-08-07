using System.Collections.Generic;
using GeoLocator.Models;

namespace GeoLocator.Repositories
{
    public interface ILocationRepository
    {
        Location? GetLocationByIp(string ip);
        List<Location> GetLocationsByCity(string city);
    }
}