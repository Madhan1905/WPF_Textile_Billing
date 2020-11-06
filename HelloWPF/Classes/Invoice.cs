﻿using SQLite;
using System;
using System.Collections.Generic;

namespace HelloWPF.Classes
{
    public class Invoice
    {
        [PrimaryKey] [AutoIncrement]
        public long Number { get; set; }
        public String Date { get; set; }
        public String Total { get; set; }
        public String BillingProducts { get; set; }
    }
}
