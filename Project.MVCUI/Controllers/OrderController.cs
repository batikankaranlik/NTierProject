using Project.BLL.DesignPatterns.genericRepository.ConcRep;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class OrderController : Controller
    {
        OrderRepository _oRep;
        
        public OrderController()
        {
            _oRep = new OrderRepository();
        }
        // GET: Order
        public ActionResult Orders(int id)
        {
            OrdersVM ovm = new OrdersVM()
            {

                Orders = _oRep.Where(x => x.AppUserID == id),
                
                
            };
            return View(ovm);
    }
}
}