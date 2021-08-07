using GeoLocator.Models;

namespace GeoLocator.Repositories.DataReaders
{
    public interface IGeobaseDataReader
    {
        IpRange[] ReadIpRanges();
        Location[] ReadLocations();
        byte[] ReadLocationsBytes();
        uint[] ReadLocationIndexes();
    }
}