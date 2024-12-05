using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.External
{
    //https://senate-stock-watcher-data.s3-us-west-2.amazonaws.com/aggregate/all_transactions.json
    public class SenateWatch
    {
        public string transaction_date { get; set; }
        public DateTime transactionDate {
            get{
                string format = "MM/dd/yyyy";
                if (DateTime.TryParseExact(transaction_date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)) { 
                    return date; 
                } else { 
                    throw new FormatException("The date string is not in the correct format."); 
                }
            }
        }
             // Define the expected date format  // Convert the string to DateTime using ParseExact 
        public string owner { get; set; }
        public string ticker { get; set; }
        public string asset_description { get; set; }
        public string asset_type { get; set; }
        public string type { get; set; }
        public string amount { get; set; }

        public int[] MinMax {
            get {
                // Remove dollar signs and commas, then split the string by " - " 
                string[] parts = amount.Replace("$", "").Replace(",", "").Trim().Split(" - "); 

                // Parse the numbers 
                if (int.TryParse(parts[0], out int number1) && int.TryParse(parts[1], out int number2)) { 
                    return new int[2] { number1, number2 };
                }
                return new int[2] { 0,0};
            }
       
        }
        public string comment { get; set; }
        public string party { get; set; }
        public string state { get; set; }
        public string industry { get; set; }
        public string sector { get; set; }
        public string senator { get; set; }
        public string ptr_link { get; set; }
        public string disclosure_date { get; set; }
        public DateTime disclosureDate {
            get{
                string format = "MM/dd/yyyy";
                if (DateTime.TryParseExact(disclosure_date, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date)) { 
                    return date; 
                } else { 
                    throw new FormatException("The date string is not in the correct format."); 
                }
            }
        }
    }
}