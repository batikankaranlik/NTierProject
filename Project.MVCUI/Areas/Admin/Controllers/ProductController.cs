﻿using Project.BLL.DesignPatterns.genericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.AuthenticationClasses;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    [AdminAuthentication]
    public class ProductController : Controller
    {
        ProductRepository _pRep;
        CategoryRepository _cRep;
        public ProductController()
        {
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
        }
        // GET: Admin/Product
        public ActionResult ProductList(int? id)
        {
            ProductVM pvm = new ProductVM()
            {
                Products = id == null ? _pRep.GetActives() : _pRep.Where(x => x.CategoryID == id)
            };
            return View(pvm);
        }
        public ActionResult AddProduct()
        {
            ProductVM pvm = new ProductVM()
            {
                Product = new Product(),
                Categories = _cRep.GetActives()
            };
            return View(pvm);
        }
        [HttpPost]
        public ActionResult AddProduct(Product product,HttpPostedFileBase resim)
        {
            product.ImagePath = ImageUploader.UploadImage("/Pictures/", resim);
            _pRep.Add(product);
            return RedirectToAction("ProductList");
        }
        public ActionResult UpdateProduct(int id)
        {
            ProductVM pvm = new ProductVM()
            {
                Categories = _cRep.GetActives(),
                Product=_pRep.Find(id)
            
            };
            return View(pvm);
        }
        [HttpPost]
        public ActionResult UpdateProduct(Product product)
        {
            
            _pRep.Update(product);
            return RedirectToAction("ProductList");
        }
        public ActionResult DeleteProduct(int id)
        {
            _pRep.Delete(_pRep.Find(id));
            return RedirectToAction("ProductList");

        }
    }
}