using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using product_catalog_ecommerce.Data.Repositories;
using product_catalog_ecommerce.Models;

namespace product_catalog_ecommerce.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _repo = new CategoryRepository();

        public ActionResult Index()
        {
            var cats = _repo.GetAll();
            return View(cats);
        }

        public ActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                _repo.Create(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var cat = _repo.GetById(id);
            if (cat == null) return HttpNotFound();
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                _repo.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}