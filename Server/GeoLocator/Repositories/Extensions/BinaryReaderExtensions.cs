using System;
using System.IO;
using System.Runtime.CompilerServices;

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

        public static string ReadString(this byte[] buffer, int offset, int byteCount)
        {
            var newBuffer = new byte[byteCount];
            Buffer.BlockCopy(buffer, offset, newBuffer, 0, byteCount);

            return System.Text.Encoding.UTF8.GetString(newBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(this byte[] buffer, int offset, int byteCount)
        {
            var newBuffer = new byte[byteCount];
            Buffer.BlockCopy(buffer, offset, newBuffer, 0, byteCount);

            return newBuffer;
        }
    }
}