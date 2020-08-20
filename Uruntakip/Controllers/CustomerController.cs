using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uruntakip.db;
using Newtonsoft.Json;
using PagedList;
using WebGrease.Css.ImageAssemblyAnalysis.LogModel;
using Uruntakip.Models;

namespace Uruntakip.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Machine
        uruntakipdbEntities3 db = new uruntakipdbEntities3();
        [Authorize]
        public ActionResult Index()
        {
            
            return View();
        }
        
        [Authorize]
        public ActionResult musteriekle()
        {
            return View();
        }
        
        [Authorize]
        [HttpPost]
        public ActionResult musteriekle(string adi,string soyadi,string eposta,string adres,string telefon,string firmaadi)
        {
            string sonuc ="0";
            try
            {
                tblCustomer t = new tblCustomer();
                t.ad = adi;
                t.soyad = soyadi;
                t.email = eposta;
                t.adres = adres;
                t.telefon = telefon;
                t.firmaadi = firmaadi;
                db.tblCustomer.Add(t);
                db.SaveChanges();
                sonuc = "1";
                return Json(sonuc, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                sonuc = "0";
                return Json(sonuc, JsonRequestBehavior.AllowGet);
            }

           

          
        }
        [Authorize]
        public ActionResult musterilistesi()
        {
            return View();
            
        }
        
        public ActionResult getcustomer()
        {
            List<tblCustomer> liste = db.tblCustomer.ToList();

            return Json(liste, JsonRequestBehavior.AllowGet);


        } 
        public ActionResult adresgoster(int id)
        {
         
            tblCustomer m = db.tblCustomer.FirstOrDefault(x => x.firmaid ==id);
            return Json(m.adres, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult musteribilgileri(int id)
        {
            tblCustomer musteri = db.tblCustomer.FirstOrDefault(x => x.firmaid == id);
            return Json(musteri, JsonRequestBehavior.AllowGet);
        }
        public ActionResult musteriguncelle(int id,string firmaadi,string ad,string soyad,string eposta,string telefon,string adres)
        {
            string kontrol;
            try
            {
                tblCustomer veri = db.tblCustomer.FirstOrDefault(x => x.firmaid == id);
                veri.adres = adres;
                veri.ad = ad.ToUpper();
                veri.soyad = soyad.ToUpper();
                veri.telefon = telefon;
                veri.firmaadi = firmaadi.ToUpper();
                veri.email = eposta;
                db.SaveChanges();
                kontrol = "1";
            }
            catch (Exception)
            {
                kontrol = "0";
               
            }

            return Json(kontrol, JsonRequestBehavior.AllowGet);
        }
       
        

        
    }
}