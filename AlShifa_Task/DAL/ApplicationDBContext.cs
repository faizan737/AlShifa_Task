using AlShifa_Task.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AlShifa_Task.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() : base("name=DefaultConnection")
        {
            
        }
       public DbSet<Invoice> Invoices { get; set; }
       public DbSet<InvoiceItem> InvoiceItems { get; set; }
       public DbSet<Item> Items { get; set; }
       public DbSet<Vendor> Vendors { get; set; }
    }
}