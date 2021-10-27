﻿using Project.BLL.DesignPatterns.genericRepository.ConcRep;
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
    public class CategoryController : Controller
    {
        CategoryRepository _cRep;
        public CategoryController()
        {
            _cRep = new CategoryRepository();
        }
        // GET: Admin/Category
        public ActionResult CategoryList(int? id)
        {
            CategoryVM cvm = id == null ? new CategoryVM
            {
                Categories = _cRep.GetActives()
            } : new CategoryVM { Categories = _cRep.Where(x=>x.ID==id) };
            return View(cvm);
        }
        public ActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            _cRep.Add(category);
            return RedirectToAction("CategoryList");
        }
        public ActionResult UpdateCategory(int id)
        {
            CategoryVM cvm = new CategoryVM
            {
                Category = _cRep.Find(id)
            };
            return View(cvm);
        }
        [HttpPost]
        public ActionResult Updatecategory(Category category)
        {
            _cRep.Update(category);
            return RedirectToAction("Categorylist");
        }
        public ActionResult DeleteCategory(int id)
        {
            _cRep.Delete(_cRep.Find(id));
            return RedirectToAction("CategoryList");
        }
    }
}