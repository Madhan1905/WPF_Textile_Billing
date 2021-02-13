using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace HelloWPF.Classes
{
    public class Invoice
    {
        [BsonId]
        public long Number { get; set; }
        public String Date { get; set; }
        public String Time { get; set; }
        public String Total { get; set; }
        public String Discount { get; set; }
        public String BillingProducts { get; set; }
    }
}
