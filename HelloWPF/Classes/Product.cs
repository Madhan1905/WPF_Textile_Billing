using System;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace HelloWPF.Classes
{
    public class Product
    {
        //public ObjectId id { get; set; }
        [BsonId]
        public String Barcode { get; set; }
        public String Name { get; set; }
        public String PrintName { get; set; }
        public String SellingPrice { get; set; }
        public String Cost { get; set; }
        public String MRP { get; set; }
        public long stock { get; set; }

    }
}
