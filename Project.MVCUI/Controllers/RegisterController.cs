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
    public class RegisterController : Controller
    {
        AppUserRepository _apRep;
        ProfileRepository _proRep;
        public RegisterController()
        {
            _apRep = new AppUserRepository();
            _proRep = new ProfileRepository();
        }

        // GET: Register
        public ActionResult RegisterNow()
        { 
            return View();
        }
        //kayıt için post hazırlıyoruz
        [HttpPost]
        public ActionResult RegisterNow(AppUserVM apvm)
        {
            AppUser appUser = apvm.AppUser;
            UserProfile profile = apvm.Profile;

            
            appUser.Password = DantexCrypt.Crypt(appUser.Password);//şifreyi kriptoladık
            appUser.ConfirmPassword = DantexCrypt.Crypt(apvm.AppUser.ConfirmPassword);
            //kontroller yapıyoruz aynı isim ve mail olmaması için
            if (_apRep.Any(x=>x.UserName==appUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanıcı ismi daha önce alınmış";
                return View();
            }
            else if (_apRep.Any(x=>x.Email==appUser.Email))
            {
                ViewBag.ZatenVar = "Email zaten kayıtlı";
                return View();
            }
            string gonderilecekEmail = "Tebrikler ... Hesabınış oluşturuldu.Aktive İçin https://localhost:44379/Register/Activation/"+appUser.ActivationCode+" linkine tıklayınız ";
            MailService.send(appUser.Email, body: gonderilecekEmail, subject: "Hesap aktivasyon");
            _apRep.Add(appUser);
            profile.ID = appUser.ID;
             _proRep.Add(profile);


            //  if (!string.IsNullOrEmpty(profile.FirstName) || !string.IsNullOrEmpty(profile.LastName) || !string.IsNullOrEmpty(profile.Address))
            //  {
            //    profile.ID = appUser.ID;
            //   _proRep.Add(profile);
            //  }

            return View("RegisterOk");
        }
        public ActionResult Activation(Guid id)
        {
            AppUser aktifEdilecek = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            if (aktifEdilecek!=null)
            {
                aktifEdilecek.Active = true;
                _apRep.Update(aktifEdilecek);
                TempData["hesapAktifMi"] = "Hesabınız aktif hale geldi";
                return RedirectToAction("Login", "Home");
            }
            TempData["HesapAktifMi"] = "Hesabınız bulunamadı";
            return RedirectToAction("Login", "Home");
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}