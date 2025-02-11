using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlShifa_Task.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public DateTime Date { get; set; }
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
        public decimal GrossTotal { get; set; }
    }

}