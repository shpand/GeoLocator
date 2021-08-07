using System.Runtime.InteropServices;

namespace GeoLocator.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Location
    {
        public fixed sbyte CountryBytes[8];
        public fixed sbyte RegionBytes[12];
        public fixed sbyte PostalCodeBytes[12];
        public fixed sbyte CityBytes[24];
        public fixed sbyte OrganizationBytes[32];
        public float Latitude;
        public float Longitude;

        public string Country
        {
            get
            {
                fixed(sbyte* ptr = &CountryBytes[0])
                {
                    return new string(ptr);
                }
            }
        }

        public string Region
        {
            get
            {
                fixed(sbyte* ptr = &RegionBytes[0])
                {
                    return new string(ptr);
                }
            }
        }

        public string PostalCode
        {
            get
            {
                fixed(sbyte* ptr = &PostalCodeBytes[0])
                {
                    return new string(ptr);
                }
            }
        }

        public string City
        {
            get
            {
                fixed(sbyte* ptr = &CityBytes[0])
                {
                    return new string(ptr);
                }
            }
        }

        public string Organization
        {
            get
            {
                fixed(sbyte* ptr = &OrganizationBytes[0])
                {
                    return new string(ptr);
                }
            }
        }
    }
}