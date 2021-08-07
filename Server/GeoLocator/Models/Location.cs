using System.Runtime.InteropServices;

namespace GeoLocator.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Location
    {
        private fixed sbyte _buffer[96];
        // public fixed sbyte CountryBytes[8];
        // public fixed sbyte RegionBytes[12];
        // public fixed sbyte PostalCodeBytes[12];
        // public fixed sbyte CityBytes[24];
        // public fixed sbyte OrganizationBytes[32];
        // public float Latitude;
        // public float Longitude;

        public string Country
        {
            get
            {
                fixed(sbyte* ptr = &_buffer[0])
                {
                    return new string(ptr);
                }
            }
        }

        public string Region
        {
            get
            {
                fixed(sbyte* ptr = &_buffer[8])
                {
                    return new string(ptr);
                }
            }
        }

        public string PostalCode
        {
            get
            {
                fixed(sbyte* ptr = &_buffer[20])
                {
                    return new string(ptr);
                }
            }
        }

        public string City
        {
            get
            {
                fixed(sbyte* ptr = &_buffer[32])
                {
                    return new string(ptr);
                }
            }
        }

        public string Organization
        {
            get
            {
                fixed(sbyte* ptr = &_buffer[56])
                {
                    return new string(ptr);
                }
            }
        }

        public float Latitude
        {
            get
            {
                fixed(sbyte* ptr = &_buffer[88])
                {
                    return *(float*) &ptr;
                }
            }
        }

        public float Longitude
        {
            get
            {
                fixed(sbyte* ptr = &_buffer[92])
                {
                    return *(float*) &ptr;
                }
            }
        }
    }
}