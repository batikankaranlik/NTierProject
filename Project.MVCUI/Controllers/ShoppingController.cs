using MVCMailService.Tools;
using PagedList;
using Project.BLL.DesignPatterns.genericRepository.ConcRep;
using Project.ENTITIES.Models;
using Project.MVCUI.Models.ShoppingTools;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class ShoppingController : Controller
    {
        OrderRepository _oRep;
        ProductRepository _pRep;
        CategoryRepository _cRep;
        OrderDetailRepository _odRep;
        public ShoppingController()
        {
            _oRep = new OrderRepository();
            _pRep = new ProductRepository();
            _cRep = new CategoryRepository();
            _odRep = new OrderDetailRepository();
            
        }
        
        // GET: Shopping
        public ActionResult ShoppingList(int? page,int? categoryID)
        {
            
            PAVM pavm = new PAVM()
            {
                //sayfalama çalışmadığı için PagedList kütüphanesinden yararlandık
                PagedProducts = categoryID == null ? _pRep.GetActives().ToPagedList(page ?? 1, 9) : _pRep.Where(x => x.CategoryID == categoryID ).ToPagedList(page ?? 1, 9), 
                Categories = _cRep.GetActives()
            };
            if (categoryID != null) TempData["catID"] = categoryID;
            return View(pavm);
        }

        public ActionResult AddToCart(int id)
        {
            Cart c = Session["scart"] == null ? new Cart() : Session["scart"] as Cart;  //session object tutuyor içinde kompleks bir veri var o yüzden unboxsing yapmalıyız

            Product eklenecekUrun = _pRep.Find(id);
            CartItem ci = new CartItem
            {
                ID = eklenecekUrun.ID,
                Name = eklenecekUrun.ProductName,
                Price = eklenecekUrun.UnitPrice,
                ImagePath = eklenecekUrun.ImagePath
            };
            c.SepeteEkle(ci);
            Session["scart"] = c;
            return RedirectToAction("ShoppingList");
        }
        public ActionResult CartPage()
        {
           
            if (Session["scart"]!=null)
            {
                CartPageVM cpvm = new CartPageVM();
                Cart c = Session["scart"] as Cart;
                cpvm.Cart = c;
                return View(cpvm);
            }
            TempData["sepetBos"] = "Sepetinizde ürün bulunmamaktadır";
            return RedirectToAction("ShoppingList");
        }
        public ActionResult DeleteFromCart(int id)
        {
            if (Session["scart"]!=null)
            {
                Cart c = Session["scart"] as Cart;
                c.SepettenSil(id);
                if (c.Sepetim.Count==0)
                {
                    Session.Remove("scart");
                    TempData["sepetBos"] = "sepetinizde ürün bulunmamaktadır";
                    return RedirectToAction("ShoppingList");
                }
                return RedirectToAction("CartPage");
            }
          
            return RedirectToAction("ShoppingList");
        }
        

        public ActionResult SiparisiOnayla()
        {
            AppUser mevcutKullanıcı;
            if (Session["member"] != null)
            {
                mevcutKullanıcı = Session["member"] as AppUser;
                return View();
            }
            else 
            {
                TempData["anonim"] = "Üye olunuz ve ya giriş yapınız";
                
                return RedirectToAction("RegisterNow", "Register");
            } 
            
            
        }
        //https://localhost:44391/api/Payment/ReceivePayment
        [HttpPost]
        public ActionResult SiparisiOnayla(OrderVM ovm)
        {
            //WebApiRestService.WrbApiClient indirmeliyiz
            bool result;
            Cart sepet = Session["scart"] as Cart;
            ovm.Order.TotalPrice = ovm.PaymentDTO.ShoppingPrice = sepet.TotalPrice;

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44391/api/");
                Task<HttpResponseMessage> posTask = client.PostAsJsonAsync("Payment/ReceivePayment", ovm.PaymentDTO);
                HttpResponseMessage sonuc;
                try
                {
                    sonuc = posTask.Result;
                }
                catch (Exception ex)
                {

                    TempData["baglantiRed"] = "banka baglantıyı reddetti";
                    return RedirectToAction("ShoppingList");
                }
                if (sonuc.IsSuccessStatusCode)
                {
                    result = true;
                }
                else result = false;
                if (result)
                {
                    if (Session["member"]!=null)
                    {
                        AppUser kullanici = Session["member"] as AppUser;
                        ovm.Order.AppUserID = kullanici.ID;
                        ovm.Order.UserName = kullanici.UserName;
                    }
                    
                    _oRep.Add(ovm.Order);
                    foreach (CartItem item in sepet.Sepetim) 
                    {
                        OrderDetail od = new OrderDetail();
                        od.OrderID = ovm.Order.ID;
                        od.ProductID = item.ID;
                        od.TotalPrice = item.SubTotal;
                        od.Quantity = item.Amount;
                        _odRep.Add(od);

                        Product stokDus = _pRep.Find(item.ID);
                        stokDus.UnitsInStock -= item.Amount;
                        _pRep.Update(stokDus);
                    }
                    Session["scart"] = null;
                    TempData["odeme"] = "siparişiniz bize ulaştı teşekkür ederiz";
                    MailService.send(ovm.Order.Email,body:$"Siparişiniz başarılı{ovm.Order.TotalPrice}",subject:"Sipariş detay");
                    return RedirectToAction("HomePage","MainPage");
                }
                else
                {
                    TempData["sorun"] = "Odeme ile ilgili sorun oluştu lütfen bankanızla iletişime geçiniz";
                    return RedirectToAction("ShoppingList");
                }
            }

            
        }
       

    }
}