using System;
using System.Collections.Generic;

namespace k190144_Q3
{
    public class JsonObjectClass
    {
        public DateTime lastUpdated { get; set; }
        public List<DateAndPrice> dateAndPrice { get; set; }

        public JsonObjectClass()
        {
            dateAndPrice = new List<DateAndPrice>();
        }
    }

    public class DateAndPrice
    {
        public float Price { get; set; }
        public DateTime Date { get; set; }
    }
}
