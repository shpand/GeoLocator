namespace GeoLocator.Models
{
    public class Location
    {
        public string Country;        // название страны (случайная строка с префиксом "cou_")
        public string Region;        // название области (случайная строка с префиксом "reg_")
        public string PostalCode;        // почтовый индекс (случайная строка с префиксом "pos_")
        public string City;          // название города (случайная строка с префиксом "cit_")
        public string Organization;  // название организации (случайная строка с префиксом "org_")
        public float Latitude;          // широта
        public float Longitude;         // долгота
    }
}