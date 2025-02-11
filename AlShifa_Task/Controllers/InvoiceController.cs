using AlShifa_Task.BLL;
using AlShifa_Task.DAL;
using AlShifa_Task.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace AlShifa_Task.Controllers
{
 
    public class InvoiceController : Controller
    {
        private readonly ApplicationDBContext _context;

        public InvoiceController()
        {
            _context = new ApplicationDBContext();
        }


        /// <summary>
        /// TO get all Invoices
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var invoices = _context.Invoices
                .Include(i => i.Vendor)
                .Include(i => i.InvoiceItems)
                .Include("InvoiceItems.Item")
                .ToList();

            return View(invoices);
        }

        public ActionResult Preview(int id)
        {
            var invoice = _context.Invoices.Include(i => i.Vendor)
                .Include(i => i.InvoiceItems)
                .Include("InvoiceItems.Item")
                .FirstOrDefault(i => i.InvoiceId == id);

            if(invoice ==null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoice/Create
        public ActionResult Create()
        {
            ViewBag.Vendors = new SelectList(_context.Vendors, "VendorId", "Name");//_context.Vendors.ToList();
            ViewBag.Items = _context.Items.ToList();


            return View();
        }

        //// POST: Invoice/Create (Save Invoice)
        //[HttpPost]
        //public ActionResult Create(Invoice invoice, List<int> itemIds, List<int> quantities)
        //{
        //    if (invoice.VendorId == 0 || itemIds == null || quantities == null)
        //    {
        //        ViewBag.Vendors = _context.Vendors.ToList();
        //        ViewBag.Items = _context.Items.ToList();
        //        ViewBag.ErrorMessage = "Please select a vendor and at least one item.";
        //        return View(invoice);
        //    }

        //    // Create invoice record
        //    invoice.Date = DateTime.Now;
        //    invoice.InvoiceItems = new List<InvoiceItem>();

        //    decimal grossTotal = 0;

        //    for (int i = 0; i < itemIds.Count; i++)
        //    {
        //        var item = _context.Items.Find(itemIds[i]);
        //        if (item != null)
        //        {
        //            var totalPrice = item.Price * quantities[i];
        //            grossTotal += totalPrice;

        //            var invoiceItem = new InvoiceItem
        //            {
        //                ItemId = itemIds[i],
        //                Quantity = quantities[i],
        //                TotalPrice = totalPrice
        //            };
        //            invoice.InvoiceItems.Add(invoiceItem);

        //            // Deduct stock quantity
        //            item.QuantityInStock -= quantities[i];
        //        }
        //    }

        //    invoice.GrossTotal = grossTotal;
        //    _context.Invoices.Add(invoice);
        //    _context.SaveChanges();

        //    TempData["SuccessMessage"] = "Invoice created successfully!";
        //    return RedirectToAction("Index", "Item");
        //}

        // POST: Invoice/Create (Save Invoice)
        [HttpPost]
        public JsonResult Create(Invoice invoiceDto)
        {
            try
            {
                
                if (invoiceDto == null || invoiceDto.InvoiceItems == null || !invoiceDto.InvoiceItems.Any())
                {
                    return Json(new { success = false, message = "Invoice must have items!" });
                }

                // Create new Invoice
                var invoice = new Invoice
                {
                    Date = DateTime.Now,
                    VendorId = invoiceDto.VendorId,
                    GrossTotal = invoiceDto.InvoiceItems.Sum(i => i.TotalPrice),
                    InvoiceItems = new List<InvoiceItem>()
                };

                foreach (var item in invoiceDto.InvoiceItems)
                {
                    var dbItem = _context.Items.Find(item.ItemId);
                    if (dbItem == null || dbItem.QuantityInStock < item.Quantity)
                    {
                        return Json(new { success = false, message = $"Insufficient stock for item ID {item.ItemId}!" });
                    }

                    // Deduct stock
                    dbItem.QuantityInStock -= item.Quantity;

                    // Add invoice item
                    invoice.InvoiceItems.Add(new InvoiceItem
                    {
                        ItemId = item.ItemId,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    });
                }

                // Save to database
                _context.Invoices.Add(invoice);
                _context.SaveChanges();

                return Json(new { success = true, message = "Invoice created successfully!" , invoiceId = invoice.InvoiceId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
    }

}