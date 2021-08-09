using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using GeoLocator.Models;
using GeoLocator.Repositories.DataReaders;

namespace GeoLocator.Repositories
{
    public class InMemoryLocationRepository: ILocationRepository
    {
        private readonly IpRange[] _ipRanges;
        private readonly Location[] _locations;
        private readonly uint[] _locationIdsSortedByCity;

        public InMemoryLocationRepository(IGeobaseDataReader geobaseDataReader)
        {
            _ipRanges = geobaseDataReader.ReadIpRanges();
            _locations = geobaseDataReader.ReadLocations();
            var locationOffsetsSortedByCity = geobaseDataReader.ReadLocationIndexes();

            ConvertLocationOffsetsToLocationIds(locationOffsetsSortedByCity);
            _locationIdsSortedByCity = locationOffsetsSortedByCity;
        }

        /// <summary>
        /// return Location by ip address or null if there's no location for the given ip
        /// </summary>
        /// <param name="ip">valid ip v4 address</param>
        public Location? GetLocationByIp(string ip)
        {
            var intNotationIp = ConvertIpToInt(ip);
            var locationIndex = FindLocationIndexByIp(intNotationIp);

            if (locationIndex < 0)
            {
                return null;
            }

            return _locations[locationIndex];
        }

        /// <summary>
        /// return location[] for the given city.
        /// Return empty array if no location corresponds to the given city
        /// </summary>
        public Location[] GetLocationsByCity(string city)
        {
            var locationIds = FindLocationIdsByCity(city);

            return locationIds.Select(id => _locations[id]).ToArray();
        }

        private int FindLocationIndexByIp(uint ip)
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
                    return (int)_ipRanges[i].LocationIndex;
                }
            }

            return -1;
        }

        private List<int> FindLocationIdsByCity(string city)
        {
            var locationIds = new List<int>();

            var start = 0;
            var end = _locationIdsSortedByCity.Length - 1;

            while (start <= end)
            {
                var i = (start + end) / 2;
                var locationId = _locationIdsSortedByCity[i];
                var comparisonResult = string.Compare(city, _locations[locationId].City);
                switch (comparisonResult)
                {
                    case < 0:
                        end = i - 1;
                        break;

                    case > 0:
                        start = i + 1;
                        break;

                    default://found one of multiple locations. Search adjacent ids to the both sides
                    {
                        var current = i;
                        while (current >= 0 && _locations[_locationIdsSortedByCity[current]].City == city)
                        {
                            locationIds.Add((int)_locationIdsSortedByCity[current]);
                            --current;
                        }

                        current = i + 1;
                        while (current < _locationIdsSortedByCity.Length && _locations[_locationIdsSortedByCity[current]].City == city)
                        {
                            locationIds.Add((int)_locationIdsSortedByCity[current]);
                            ++current;
                        }

                        return locationIds;
                    }
                }
            }

            return locationIds;
        }

        private static uint ConvertIpToInt(string ip)
        {
            var ipSections = ip.Split('.').Select(section => int.Parse(section)).ToArray();

            return (uint)
                (ipSections[0] * (1 << 24) + //256 * 256 * 256
                 ipSections[1] * (1 << 16) + //256 * 256
                 ipSections[2] * 256 +
                 ipSections[3]);
        }

        private static void ConvertLocationOffsetsToLocationIds(uint[] locationOffsetsSortedByCity)
        {
            //divide every offset by location size in order to find a proper index of location in the array.
            //could use raw byte offsets together with raw location bytes deserializing into Location with every request.
            //that would speed up database reading but slow down request handling.
            var recordSize = (uint)Marshal.SizeOf(typeof(Location));
            for (int i = 0; i < locationOffsetsSortedByCity.Length; i++)
            {
                locationOffsetsSortedByCity[i] /= recordSize;
            }
        }
    }
}