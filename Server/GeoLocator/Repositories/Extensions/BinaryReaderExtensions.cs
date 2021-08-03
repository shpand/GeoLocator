using System.IO;

namespace GeoLocator.Repositories.Extensions
{
    internal static class BinaryReaderExtensions
    {
        public static byte[] ReadBytes(this BinaryReader reader, int byteCount)
        {
            var bytes = new byte[byteCount];

            for (int i = 0; i < byteCount; i++)
            {
                bytes[i] = reader.ReadByte();
            }

            return bytes;
        }

        public static string ReadStringCustom(this BinaryReader reader, int byteCount)
        {
            var bytes = ReadBytes(reader, byteCount);

            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}