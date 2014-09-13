using System.Data.Entity.Spatial;

namespace Codeifier.OrangeCMS.Domain.Models
{
    public static class Coordinates
    {
        public static DbGeography Create(double longitude, double latitude)
        {
            var wkt = string.Format("POINT({1} {0})", latitude, longitude);
            return DbGeography.FromText(wkt);
        }
    }
}
