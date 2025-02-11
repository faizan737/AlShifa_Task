using AlShifa_Task.DAL;
using AlShifa_Task.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlShifa_Task.BLL
{
    
    public class ItemService
    {
        private readonly ApplicationDBContext _context;

        public ItemService()
        {
            _context = new ApplicationDBContext();
        }

        // Get an item by ID
        public Item GetItemById(int id)
        {
            return _context.Items.Find(id);
        }

        /// <summary>
        /// To get all the Items details
        /// </summary>
        /// <returns></returns>
        public List<Item> GetAllItems()
        {
            return _context.Items.ToList();
        }

        /// <summary>
        /// TO add new item
        /// </summary>
        /// <param name="item"></param>
        public void AddItem(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        /// <summary>
        /// TO Update Stock 
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="quantity"></param>
        public void UpdateStock(int itemId, int quantity)
        {
            var item = _context.Items.Find(itemId);
            if (item != null)
            {
                item.QuantityInStock -= quantity;
                _context.SaveChanges();
            }
        }
        //public List<Invoice> GetAllItems()
        //{
        //    return _context.Items.ToList();
        //}
        public bool DeleteStock(int itemId)
        {
            var item = _context.Items.Find(itemId);
            if (item != null)
            {
                _context.Items.Remove(item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public bool EditStock(Item item)
        {
            var existingItem = _context.Items.Find(item.ItemId);
            if (existingItem == null)
                return false;

            existingItem.Name = item.Name;
            existingItem.Price = item.Price;
            existingItem.QuantityInStock = item.QuantityInStock;

            _context.SaveChanges();
            return true;
        }
    }

}