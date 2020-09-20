using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Uruntakip.db;
using System.Web.Security;
using System.Web.Configuration;
using System.Web.Routing;
using System.Runtime.Remoting.Lifetime;

namespace Uruntakip.Controllers
{
    public class HomeController : Controller
    {
        uruntakipdbEntities3 db = new uruntakipdbEntities3();
       
        public ActionResult Login()
        {

            return View();
        }
       
        [HttpPost]
       
        public ActionResult Login(string username,string password)
        {

            string sonuc = "";
            tbllogin bilgiler = db.tbllogin.FirstOrDefault(x => x.eposta == username && x.sifre == password);
            if (bilgiler != null)
            {
                if (bilgiler.durum == 1)
                {
                    FormsAuthentication.SetAuthCookie(bilgiler.girisid.ToString(), false);
                    Response.Cookies["user"].Value = bilgiler.eposta.ToString();

                    sonuc = bilgiler.girisid.ToString(); ;

                }

            }

            return Json(sonuc, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult Kayıt( string kullaniciadi,string kullanicisifresi)
        { string sonuc = "0";
            try
            {
                tbllogin epostakontrol = db.tbllogin.FirstOrDefault(x => x.eposta == kullaniciadi);
                if(epostakontrol!=null)
                {
                    sonuc = "2";
                    return Json(sonuc, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    tbllogin t = new tbllogin();
                    t.eposta = kullaniciadi;
                    t.sifre = kullanicisifresi;
                    t.durum = 1;
                    db.tbllogin.Add(t);
                    db.SaveChanges();
                    sonuc = "1";
                    return Json(sonuc, JsonRequestBehavior.AllowGet);

                }
               
            }
            catch (Exception)
            {
              
                return Json(sonuc, JsonRequestBehavior.AllowGet);
            }

           
        }
      
    }
}