using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace project
{
    class CheckSingleQuotationMark
    {
        public string checkForSingleQuotationMark(string test)
        {
            bool withQuote = false;
            withQuote = test.Contains("'");
            if (withQuote)
            {
                string doubleQuotes = test.Replace("'","''");
                Console.WriteLine("הכיל סימן ציטוט ושונה ל:");
                Console.WriteLine(doubleQuotes);
                return doubleQuotes;
            }
            else
            {
                Console.WriteLine("ללא סימן ציטוט והוחזר המקור:");
                Console.WriteLine(test);
                return test;
            }
        }
    }
}
