namespace GeoLocator.Models
{
    public class FileHeader
    {
        public int Version;           // версия база данных
        public string Name;          // название/префикс для базы данных
        public ulong Timestamp;         // время создания базы данных
        public int   RecordCount;           // общее количество записей
        public uint  OffsetRanges;     // смещение относительно начала файла до начала списка записей с геоинформацией
        public uint  OffsetCities;     // смещение относительно начала файла до начала индекса с сортировкой по названию городов
        public uint  OffsetLocations;  // смещение относительно начала файла до начала списка записей о местоположении
    }
}