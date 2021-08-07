using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using GeoLocator.Models;
using GeoLocator.Repositories.DataReaders;

namespace GeoLocator.Repositories
{
    public class InMemoryLocationRepository: ILocationRepository
    {
        private IpRange[] _ipRanges;
        private Location[] _locations;
        private int[] _cityIndexes;

        public InMemoryLocationRepository(IGeobaseDataReader geobaseDataReader)
        {
            _ipRanges = geobaseDataReader.ReadIpRanges();
            _locations = geobaseDataReader.ReadLocations();
            _cityIndexes = geobaseDataReader.ReadLocationIndexes();
        }

        public Location GetLocationByIp(string ip)
        {
            var intNotationIp = ConvertIpToInt(ip);
            var locationIndex = FindLocationIndexByIp(intNotationIp);

            return _locations[locationIndex];
        }

        public List<Location> GetLocationsByCity(string city)
        {
            throw new System.NotImplementedException();
        }

        private static uint ConvertIpToInt(string ip)
        {
            var ipSections = ip.Split('.').Select(section => int.Parse(section)).ToArray();

            return (uint)
                (ipSections[0] * (1 << 24) +
                ipSections[1] * (1 << 16) +
                ipSections[2] * 256 +
                ipSections[3]);
        }

        private uint FindLocationIndexByIp(uint ip)
        {
            var start = 0;
            var end = _ipRanges.Length - 1;

            while (start <= end)
            {
                var i = (start + end) / 2;

                if (ip < _ipRanges[i].From)
                {
                    end = i - 1;
                }
                else if (ip > _ipRanges[i].To)
                {
                    start = i + 1;
                }
                else
                {
                    return _ipRanges[i].LocationIndex;
                }
            }

            throw new ArgumentOutOfRangeException("Specified ip address does not fall into a known range of ip addresses.");
        }
    }
}