using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CsvHelper;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands.Importers
{
    class CollectionSpreadsheet : Spreadsheet
    {
        protected override Customer Read(CsvReader csv, string[] headers, Customer customer)
        {
            double? longitude = null, latitude = null;

            if (headers.Contains("Coordonées GPS Longitude"))
            {
                longitude = csv.GetField<double>("Coordonées GPS Longitude");
            }

            if (headers.Contains("Coordonées GPS Latitude"))
            {
                latitude = csv.GetField<double>("Coordonées GPS Latitude");
            }

            if (longitude.HasValue && latitude.HasValue)
            {
                customer.Coordinates = Coordinates.Create(longitude.Value, latitude.Value);
            }

            if (headers.Contains("Photo de l'entrée"))
            {
                customer.ImageUrl = csv.GetField<string>("Photo de l'entrée");
            }

            return customer;
        }
    }
}
