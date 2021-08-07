using System.IO;
using System.Runtime.InteropServices;
using GeoLocator.Models;

namespace GeoLocator.Repositories.DataReaders
{
    public class GeobaseDataReader : IGeobaseDataReader
    {
        private readonly byte[] _data;
        private readonly FileHeader _fileHeader;

        public GeobaseDataReader(string geobaseFilePath)
        {
            //TODO: here I use a little trick of knowing beforehand the size of the array.
            //Should I rewrite the solution to read the file section by section?
            //Won't be hard but don't see any sense in it now
            using var fs = new FileStream(geobaseFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 100000 * 112 + 60);
            _data = new byte[100000 * 112 + 60];
            FillBuffer(fs, _data, _data.Length);

            _fileHeader = ReadFileHeader(_data);
        }

        public IpRange[] ReadIpRanges()
        {
            return ReadData<IpRange>(_fileHeader.RecordCount, _fileHeader.OffsetRanges);
        }

        public Location[] ReadLocations()
        {
            return ReadData<Location>(_fileHeader.RecordCount, _fileHeader.OffsetLocations);
        }

        public int[] ReadLocationIndexes()
        {
            return ReadData<int>(_fileHeader.RecordCount, _fileHeader.OffsetCities);
        }

        private T[] ReadData<T>(int recordCount, uint offset) where T: unmanaged
        {
            var array = new T[recordCount];

            unsafe
            {
                fixed (byte* ptr = &_data[offset])
                {
                    var i = 0;
                    var offsetSize = Marshal.SizeOf(typeof(T));;
                    while (i < recordCount)
                    {
                        var t = *(T*) &ptr[i * offsetSize];
                        array[i] = t;

                        ++i;
                    }
                }
            }

            return array;
        }

        private static void FillBuffer(Stream stream, byte[] buffer, int count)
        {
            int read = 0;
            int totalRead = 0;
            do
            {
                read = stream.Read(buffer, totalRead, count - totalRead);
                totalRead += read;

            } while (read > 0 && totalRead < buffer.Length);
        }

        private static FileHeader ReadFileHeader(byte[] buffer)
        {
            unsafe
            {
                fixed (byte* ptr = &buffer[0])
                {
                    return *(FileHeader*)ptr;
                }
            }
        }
    }
}