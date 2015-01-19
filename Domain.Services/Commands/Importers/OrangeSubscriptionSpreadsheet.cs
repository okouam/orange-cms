using System;
using System.Linq;
using CodeKinden.OrangeCMS.Domain.Models;
using CsvHelper;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands.Importers
{
    internal class OrangeSubscriptionSpreadsheet : Spreadsheet
    {
        protected override Customer Read(CsvReader csv, string[] headers, Customer customer)
        {
            if (headers.Contains("NOM"))
            {
                customer.Name = csv.GetField<string>("NOM");
            }

            if (headers.Contains("LOGIN"))
            {
                customer.Login = csv.GetField<string>("LOGIN");
            }

            if (headers.Contains("FORMULE"))
            {
                customer.Formula = csv.GetField<string>("FORMULE");
            }

            if (headers.Contains("DEBIT"))
            {
                customer.Speed = csv.GetField<string>("DEBIT");
            }

            if (headers.Contains("DATE_EXPIRATION"))
            {
                customer.ExpiryDate = GetDate(csv, "DATE_EXPIRATION");
                customer.NeverExpires = csv.GetField<string>("DATE_EXPIRATION") == "Illimite";
            }

            if (csv.FieldHeaders.Contains("ETAT"))
            {
                customer.Status = csv.GetField<string>("ETAT");
            }

            return customer;
        }

        private static DateTime? GetDate(CsvReader csv, string header)
        {
            var dateString = csv.GetField<string>(header);
            DateTime date;
            return DateTime.TryParse(dateString, out date) ? date : (DateTime?)null;
        }
    }
}
