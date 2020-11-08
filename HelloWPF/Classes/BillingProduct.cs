using System;

namespace HelloWPF.Classes
{
    public class BillingProduct
    {
        public int Serial { get; set; }
        public String Barcode { get; set; }
        public String Name { get; set; }
        public String PrintName { get; set; }
        public int SellingPrice { get; set; }
        public int MRP { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
    }
}
