using System.IO;
using System.Runtime.InteropServices;
using GeoLocator.Models;

namespace GeoLocator.Repositories.DataReaders
{
    public class GeobaseDataReader : IGeobaseDataReader
    {
        private readonly string _geobaseFilePath;
        private readonly FileHeader _fileHeader;

        public GeobaseDataReader(string geobaseFilePath)
        {
            _geobaseFilePath = geobaseFilePath;
            _fileHeader = ReadFileHeader();
        }

        public IpRange[] ReadIpRanges()
        {
            return ReadData<IpRange>(_fileHeader.RecordCount, _fileHeader.OffsetRanges);
        }

        public Location[] ReadLocations()
        {
            return ReadData<Location>(_fileHeader.RecordCount, _fileHeader.OffsetLocations);
        }

        public uint[] ReadLocationIndexes()
        {
            return ReadData<uint>(_fileHeader.RecordCount, _fileHeader.OffsetCities);
        }

        private T[] ReadData<T>(int recordCount, uint offset) where T: unmanaged
        {
            var recordSize = Marshal.SizeOf(typeof(T));
            var data = ReadFromFile(recordCount * recordSize, offset);

            var array = new T[recordCount];

            unsafe
            {
                fixed (byte* ptr = &data[0])
                {
                    var i = 0;
                    while (i < recordCount)
                    {
                        array[i] = *(T*) &ptr[i * recordSize];
                        ++i;
                    }
                }
            }

            return array;
        }

        private byte[] ReadFromFile(int count, uint offset)
        {
            using var fs = new FileStream(_geobaseFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, count);
            var data = new byte[count];
            FillBuffer(fs, data, count, offset);

            return data;
        }

        private FileHeader ReadFileHeader()
        {
            var recordSize = Marshal.SizeOf(typeof(FileHeader));;
            var data = ReadFromFile(recordSize, 0);
            unsafe
            {
                fixed (byte* ptr = &data[0])
                {
                    return *(FileHeader*)ptr;
                }
            }
        }

        private static void FillBuffer(Stream stream, byte[] buffer, int count, uint streamOffset)
        {
            var read = 0;
            var totalRead = 0;
            stream.Seek(streamOffset, SeekOrigin.Begin);
            do
            {
                read = stream.Read(buffer, totalRead, count - totalRead);
                totalRead += read;

            } while (read > 0 && totalRead < buffer.Length);
        }
    }
}