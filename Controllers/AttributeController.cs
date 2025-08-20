using product_catalog_ecommerce.Data.Repositories;
using product_catalog_ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace product_catalog_ecommerce.Controllers
{
    public class AttributeController : Controller
    {
        private readonly AttributeRepository _repo = new AttributeRepository();

        public ActionResult Index()
        {
            var list = _repo.GetAll();
            return View(list);
        }

        public ActionResult Create()
        {
            return View(new AttributeModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AttributeModel model)
        {
            if (ModelState.IsValid)
            {
                _repo.Create(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}