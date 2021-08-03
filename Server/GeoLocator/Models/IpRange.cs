namespace GeoLocator.Models
{
    public class IpRange
    {
        public uint From;           // начало диапазона IP адресов
        public uint To;             // конец диапазона IP адресов
        public uint LocationIndex;    // индекс записи о местоположении
    }
}