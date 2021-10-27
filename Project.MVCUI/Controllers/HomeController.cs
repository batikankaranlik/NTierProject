using MVCMailService.Tools;
using Project.BLL.DesignPatterns.genericRepository.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.MVCUI.VMClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class HomeController : Controller
    {
        AppUserRepository _apRep;
        public HomeController()
        {
            _apRep = new AppUserRepository();
        }
        // GET: Home
        public ActionResult Login()
        {
            if (Session["member"] != null)
            {
                //giriş yapan biri giriş yapmaya tekrar çalışırsa
                TempData["msg"] = "<script>alert('Zaten Giriş Yaptınız');</script>";
                AppUser user = Session["member"] as AppUser;
                
            }
            return View();
        }
        [HttpPost]
        //login Yapabilmek için kontroller ve role göre session verimi
        public ActionResult Login(AppUser appUser)
        {

            AppUser yakUser = _apRep.FirstOrDefault(x => x.UserName == appUser.UserName);
            if (yakUser == null)
            {
                ViewBag.Kullanici = "Kullanıcı bulunamadı";
                return View();
            }
            string decrypted = DantexCrypt.DeCrypt(yakUser.Password);
            if (appUser.Password == decrypted && yakUser.Role == ENTITIES.Enums.UserRole.Admin)
            {
                if (!yakUser.Active)
                {
                    return AktifKontrol();
                }
                Session["admin"] = yakUser;
                return RedirectToAction("CategoryList", "Category", new { Area = "Admin" });

            }
            else if (appUser.Password==decrypted && yakUser.Role == ENTITIES.Enums.UserRole.Member)
            {
                if (!yakUser.Active)
                {
                    return AktifKontrol();
                }
                TempData["yakalanan"] = yakUser.UserName;
                Session["member"] = yakUser;
                return RedirectToAction("HomePage", "MainPage");
            }

            ViewBag.Kullanici = "Kullanıcı adı ve ya Şifre yanlış";
            return View();

        }
        //hesap aktif mi diye kontrol
        private ActionResult AktifKontrol()
        {
            ViewBag.AktifDegil = "Lütfen hesabı aktif hale getiriniz.";
            return View("Login");
        }
        //hesaptan Çıkış yapmak İçin
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("HomePage", "MainPage");
        }
        //şifre değiştirmek için gönderilcek mail
        public ActionResult ResPasswordMailSend()
        {
            return View();

        }
        [HttpPost]
        public ActionResult ResPasswordMailSend(AppUserVM apvm)
        {
            AppUser mail = _apRep.FirstOrDefault(x => x.Email == apvm.AppUser.Email);
            if (mail != null)
            {
                string sendMail = "Şifre Sıfırlamak için . https://localhost:44379/Home/ResetPassword/" + mail.ActivationCode;
                MailService.send(mail.Email, body: sendMail, subject: "Şifre Değiştirme");
                ViewBag.info = "Şifre sıfırlama isteğiniz Mailinize Gönderildi";
                return View();

            }
            ViewBag.info = "Mail Adresi bulunamadı";
            return View();


        }
        //Şifre değiştirme sayfası
        public ActionResult ResetPassword(Guid id)
        {
            AppUserVM apvm = new AppUserVM();
            apvm.AppUser = _apRep.FirstOrDefault(x => x.ActivationCode == id);

            return View(apvm);
        }
        [HttpPost]
        public ActionResult ResetPassword(AppUser appUser)
        {
            AppUser changePas = _apRep.FirstOrDefault(x => x.ActivationCode == appUser.ActivationCode);
            changePas.Password = DantexCrypt.Crypt(appUser.Password);
            changePas.ConfirmPassword = DantexCrypt.Crypt(appUser.ConfirmPassword);
            _apRep.Update(changePas);
            TempData["msg"] = "<script>alert('Şifreniz değiştirildi');</script>";
            return RedirectToAction("Login", "Home");
        }
    }
}