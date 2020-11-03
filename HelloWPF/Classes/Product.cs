using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace HelloWPF.Classes
{
    public class Product
    {
        [PrimaryKey]
        public String Barcode { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public int MRP { get; set; }

    }
}
