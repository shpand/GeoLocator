using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GeoLocator.Models;
using GeoLocator.Repositories.Extensions;

namespace GeoLocator.Repositories
{
    public class InMemoryLocationRepositoryFast : ILocationRepository
    {

        private const int chunkSize = 100;
        public InMemoryLocationRepositoryFast(string geobasePath)
        {
            using (var fs = new FileStream(geobasePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var fileHeader = ReadFileHeader(fs);
                var sw = new Stopwatch();
                sw.Start();

                var ipRanges = ReadIpRanges(fs, fileHeader.RecordCount);

                var t1 = sw.ElapsedMilliseconds;
                Console.Write(t1);
                sw.Restart();
                var locations = ReadLocations(fs, fileHeader.RecordCount);
                var t2 = sw.ElapsedMilliseconds;
                Console.Write(t2);
                sw.Restart();
                var indexes = ReadLocationIndexes(fs, fileHeader.RecordCount);

                var t3 = sw.ElapsedMilliseconds;
                Console.Write(t3);

                Console.WriteLine(fileHeader);
            }
        }

        private static int FillBuffer(Stream stream, byte[] buffer, int count)
        {
            int read = 0;
            int totalRead = 0;
            do
            {
                read = stream.Read(buffer, totalRead, count - totalRead);
                totalRead += read;

            } while (read > 0 && totalRead < buffer.Length);

            return totalRead;
        }

        public Location GetLocationByIp(string ip)
        {
            throw new System.NotImplementedException();
        }

        public List<Location> GetLocationsByCity(string city)
        {
            throw new System.NotImplementedException();
        }

        private FileHeader ReadFileHeader(Stream stream)
        {
            unsafe
            {
                var fileHeader = new FileHeader();
                var buffer = new byte[60];
                FillBuffer(stream, buffer, buffer.Length);

                fixed (byte* numRef = &buffer[0])
                {
                    fileHeader.Version = *(int*)&numRef[0];

                    //though test description says that we have sbyte array it doesn't really matter when converting it to a string.
                    //so we read bytes as unsigned bytes in order to convert it to a string
                    fileHeader.Name = buffer.ReadString(4, 32);

                    fileHeader.Timestamp = *(ulong*)&numRef[36];
                    fileHeader.RecordCount = *(int*)&numRef[44];
                    fileHeader.OffsetRanges = *(uint*)&numRef[48];
                    fileHeader.OffsetCities = *(uint*)&numRef[52];
                    fileHeader.OffsetLocations = *(uint*)&numRef[56];
                }

                return fileHeader;
            }
        }

        private IpRange[] ReadIpRanges(Stream stream, int recordCount)
        {
            var ipRanges = new IpRange[recordCount];
            const int recordSize = 12;
            int recordsRead = 0;

            var buffer = new byte[recordSize * chunkSize];

            while (recordsRead < recordCount)
            {
                var recordsLeft = recordCount - recordsRead;
                var recordsReadSize = Math.Min(recordsLeft, chunkSize);
                FillBuffer(stream, buffer, recordsReadSize * recordSize);

                unsafe
                {
                    fixed (byte* numRef = &buffer[0])
                    {

                        int offset = 0;
                        while (recordsReadSize > 0)
                        {

                            var ipRange = new IpRange
                            {
                                From = *(uint*) &numRef[offset],
                                To = *(uint*) &numRef[offset + 4],
                                LocationIndex = *(uint*) &numRef[offset + 8]
                            };

                            ipRanges[recordsRead] = ipRange;

                            offset += recordSize;
                            ++recordsRead;

                            --recordsReadSize;
                        }
                    }

                }
            }

            return ipRanges;
        }

        private Location[] ReadLocations(Stream stream, int recordCount)
        {
            var locations = new Location[recordCount];
            const int recordSize = 96;
            int recordsRead = 0;

            var buffer = new byte[recordSize * chunkSize];

            while (recordsRead < recordCount)
            {
                var recordsLeft = recordCount - recordsRead;
                var recordsReadSize = Math.Min(recordsLeft, chunkSize);
                FillBuffer(stream, buffer, recordsReadSize * recordSize);

                int offset = 0;
                unsafe
                {
                    fixed (byte* numRef = &buffer[offset])
                    {
                        while (recordsReadSize > 0)
                        {

                            var location = new Location
                            {
                                Country = buffer.GetBytes(offset, 8),
                                Region = buffer.GetBytes(offset + 8, 12),
                                PostalCode = buffer.GetBytes(offset + 20, 12),
                                City = buffer.GetBytes(offset + 32, 24),
                                Organization = buffer.GetBytes(offset + 56, 32),
                                Latitude = *(float*) &numRef[offset + 88],
                                Longitude = *(float*) &numRef[offset + 92]
                            };

                            locations[recordsRead] = location;

                            offset += recordSize;
                            ++recordsRead;

                            --recordsReadSize;
                        }
                    }
                }
            }

            return locations;
        }



        private int[] ReadLocationIndexes(Stream stream, int recordCount)
        {
            var indexes = new int[recordCount];
            const int recordSize = 4;
            int recordsRead = 0;

            var buffer = new byte[recordSize * chunkSize];

            while (recordsRead < recordCount)
            {
                var recordsLeft = recordCount - recordsRead;
                var recordsReadSize = Math.Min(recordsLeft, chunkSize);
                FillBuffer(stream, buffer, recordsReadSize * recordSize);

                int offset = 0;
                unsafe
                {
                    fixed (byte* numRef = &buffer[offset])
                    {
                        while (recordsReadSize > 0)
                        {

                            indexes[recordsRead] = *(int*) &numRef[offset];

                            offset += recordSize;
                            ++recordsRead;

                            --recordsReadSize;
                        }
                    }
                }
            }

            return indexes;
        }
    }
}