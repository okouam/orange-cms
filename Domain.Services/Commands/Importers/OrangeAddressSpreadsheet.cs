using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CsvHelper;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands.Importers
{
    internal class OrangeAddressSpreadsheet : Spreadsheet
    {
        protected override Customer Read(CsvReader csv, string[] headers, Customer customer)
        {
            if (headers.Contains("NOM"))
            {
                customer.Name = csv.GetField<string>("NOM");
            }

            if (headers.Contains("BP1"))
            {
                // do nothing
            }

            if (headers.Contains("VOIE"))
            {
                // do nothing
            }

            if (headers.Contains("COMMUNE"))
            {
                // do nothing
            }

            if (headers.Contains("NVOIE"))
            {
                // do nothing
            }

            if (csv.FieldHeaders.Contains("KADR"))
            {
                // do nothing
            }

            if (csv.FieldHeaders.Contains("COMPL_ADR"))
            {
                // do nothing
            }

            return customer;
        }
    }
}
