using AlShifa_Task.BLL;
using AlShifa_Task.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlShifa_Task.Controllers
{
   
    public class ItemController : Controller
    {
        private readonly ItemService _itemService;

        public ItemController()
        {
            _itemService = new ItemService();
        }

        /// <summary>
        /// TO get all items
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var items = _itemService.GetAllItems();
            return View(items);
        }
        public ActionResult Edit(int id)
        {
            var item = _itemService.GetItemById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (ModelState.IsValid)
            {
                bool updated = _itemService.EditStock(item);
                if(updated)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Unable to update item");
                
            }
            return View(item);
        }
        //Get : item/Delete/id
        public ActionResult Delete(int id)
        {
            var item = _itemService.GetItemById(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Item model)
        {
            bool deleted = _itemService.DeleteStock(model.ItemId);
            if (deleted)
            {
                return RedirectToAction("Index");
            }
            return HttpNotFound();
        }
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// TO create or add new items
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Item item)
        {
            if (ModelState.IsValid)
            {
                _itemService.AddItem(item);
                return RedirectToAction("Index");
            }
            return View(item);
        }
    }

}