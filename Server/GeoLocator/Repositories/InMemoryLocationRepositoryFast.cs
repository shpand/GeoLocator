using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using GeoLocator.Models;
using GeoLocator.Repositories.DataReaders;

namespace GeoLocator.Repositories
{
    public class InMemoryLocationRepositoryFast : ILocationRepository
    {
        private IpRange[] _ipRanges;
        private Location[] _locations;
        private int[] _cityIndexes;
        public InMemoryLocationRepositoryFast(IGeobaseDataReader geobaseDataReader)
        {
            _ipRanges = geobaseDataReader.ReadIpRanges();
            _locations = geobaseDataReader.ReadLocations();
            _cityIndexes = geobaseDataReader.ReadLocationIndexes();
        }

        public Location GetLocationByIp(string ip)
        {
            throw new System.NotImplementedException();
        }

        public List<Location> GetLocationsByCity(string city)
        {
            throw new System.NotImplementedException();
        }


    }
}