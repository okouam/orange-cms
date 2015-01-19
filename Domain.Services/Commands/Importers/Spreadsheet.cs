using System;
using System.Collections.Generic;
using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CsvHelper;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands.Importers
{
    internal abstract class Spreadsheet
    {
        protected abstract Customer Read(CsvReader csv, string[] headers, Customer customer);

        public static Customer ReadCustomerData(CsvReader csv, IEnumerable<Customer> customers)
        {
            string key;

            Spreadsheet spreadsheet;

            if (IsCollectionSpreadsheet(csv.FieldHeaders))
            {
                key = csv.GetField<string>("Numéro de Téléphone").Trim();
                spreadsheet = new CollectionSpreadsheet();
            }
            else if (IsOrangeAddressSpreadsheet(csv.FieldHeaders))
            {
                key = csv.GetField<string>("ND").Trim();
                spreadsheet = new OrangeAddressSpreadsheet();
            }
            else if (IsOrangeSubscriptionSpreadsheet(csv.FieldHeaders))
            {
                key = csv.GetField<string>("NUM_ADSL").Trim();
                spreadsheet = new OrangeSubscriptionSpreadsheet();
            }
            else
            {
                throw new Exception("A column called [NUM_ADSL], [ND] or [\"Numéro de Téléphone\"] must be provided.");
            }

            var customer = customers.FirstOrDefault(x => x.Telephone == key) ?? new Customer { Telephone = key };

            return spreadsheet.Read(csv, csv.FieldHeaders, customer);
        }

        static bool IsOrangeAddressSpreadsheet(IEnumerable<string> headers)
        {
            return headers.Contains("ND");
        }

        static bool IsOrangeSubscriptionSpreadsheet(IEnumerable<string> headers)
        {
            return headers.Contains("NUM_ADSL");
        }

        static bool IsCollectionSpreadsheet(IEnumerable<string> headers)
        {
            return headers.Contains("Numéro de Téléphone");
        }
    }
}