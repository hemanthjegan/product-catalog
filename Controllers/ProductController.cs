using product_catalog_ecommerce.Data.Repositories;
using product_catalog_ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourProject.Data.Repositories;

namespace product_catalog_ecommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepo = new ProductRepository();
        private readonly CategoryRepository _categoryRepo = new CategoryRepository();
        private readonly CategoryAttributeRepository _catAttrRepo = new CategoryAttributeRepository();

        public ActionResult Index()
        {
            var list = _productRepo.GetAll();
            return View(list);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = _categoryRepo.GetAll().ToList();
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product model, FormCollection form)
        {
            // Read posted attribute values: the view should post values named attr_{categoryAttributeId}
            var catAttrs = _catAttrRepo.GetByCategoryId(model.CategoryId).ToList();
            foreach (var ca in catAttrs)
            {
                var formKey = $"attr_{ca.CategoryAttributeId}";
                if (form.AllKeys.Contains(formKey))
                {
                    model.Attributes.Add(new ProductAttributeValue
                    {
                        CategoryAttributeId = ca.CategoryAttributeId,
                        Value = form[formKey],
                        AttributeName = ca.AttributeName
                    });
                }
            }

            _productRepo.Create(model);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var p = _productRepo.GetById(id);
            if (p == null) return HttpNotFound();
            ViewBag.Categories = _categoryRepo.GetAll().ToList();
            // Category attributes for current category
            ViewBag.CategoryAttributes = _catAttrRepo.GetByCategoryId(p.CategoryId).ToList();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product model, FormCollection form)
        {
            var catAttrs = _catAttrRepo.GetByCategoryId(model.CategoryId).ToList();
            model.Attributes.Clear();
            foreach (var ca in catAttrs)
            {
                var formKey = $"attr_{ca.CategoryAttributeId}";
                if (form.AllKeys.Contains(formKey))
                {
                    model.Attributes.Add(new ProductAttributeValue
                    {
                        CategoryAttributeId = ca.CategoryAttributeId,
                        Value = form[formKey],
                        AttributeName = ca.AttributeName
                    });
                }
            }
            _productRepo.Update(model);
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var p = _productRepo.GetById(id);
            return View(p);
        }
    }
}