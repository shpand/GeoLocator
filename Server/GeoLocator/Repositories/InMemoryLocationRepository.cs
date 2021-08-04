using System;
using System.Collections.Generic;
using System.IO;
using GeoLocator.Models;
using GeoLocator.Repositories.Extensions;

namespace GeoLocator.Repositories
{
    public class InMemoryLocationRepository : ILocationRepository
    {
        public InMemoryLocationRepository(string geobasePath)
        {
            using (var fs = new FileStream(geobasePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (var reader = new BinaryReader(fs))
                {
                    var fileHeader = ReadFileHeader(reader);
                    var ipRanges = ReadIpRanges(reader, fileHeader.RecordCount);
                    var locations = ReadLocations(reader, fileHeader.RecordCount);

                    Console.WriteLine(fileHeader);
                }
            }
        }

        public Location GetLocationByIp(string ip)
        {
            throw new System.NotImplementedException();
        }

        public List<Location> GetLocationsByCity(string city)
        {
            throw new System.NotImplementedException();
        }

        private FileHeader ReadFileHeader(BinaryReader reader)
        {
            var fileHeader = new FileHeader();
            fileHeader.Version = reader.ReadInt32();

            //though test description says that we have sbyte array it doesn't really matter when converting it to a string.
            //so we read bytes as unsigned bytes in order to convert it to a string
            fileHeader.Name = reader.ReadStringCustom(32);

            fileHeader.Timestamp = reader.ReadUInt64();
            fileHeader.RecordCount = reader.ReadInt32();
            fileHeader.OffsetRanges = reader.ReadUInt32();
            fileHeader.OffsetCities = reader.ReadUInt32();
            fileHeader.OffsetLocations = reader.ReadUInt32();

            return fileHeader;
        }

        private IpRange[] ReadIpRanges(BinaryReader reader, int recordCount)
        {
            var ipRanges = new IpRange[recordCount];

            for (int i = 0; i < recordCount; i++)
            {
                var ipRange = new IpRange
                {
                    From = reader.ReadUInt32(),
                    To = reader.ReadUInt32(),
                    LocationIndex = reader.ReadUInt32()
                };

                ipRanges[i] = ipRange;
            }

            return ipRanges;
        }

        private Location[] ReadLocations(BinaryReader reader, int recordCount)
        {
            var locations = new Location[recordCount];

            for (int i = 0; i < recordCount; i++)
            {
                var location = new Location
                {
                    // Country = reader.ReadStringCustom(8),
                    // Region = reader.ReadStringCustom(12),
                    // PostalCode = reader.ReadStringCustom(12),
                    // City = reader.ReadStringCustom(24),
                    // Organization = reader.ReadStringCustom(32),
                    Latitude = reader.ReadSingle(),
                    Longitude = reader.ReadSingle()
                };

                locations[i] = location;
            }

            return locations;
        }
    }
}