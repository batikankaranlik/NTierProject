using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Areas.Admin.Controllers
{
    public class AdminLogOutController : Controller
    {
        // GET: Admin/AdminLogOut
        public ActionResult AdLogOut()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "MainPage",new {Area="" });
         
        }
    }
}
