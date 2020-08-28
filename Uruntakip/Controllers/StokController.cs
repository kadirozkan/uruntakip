using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.WebPages.Html;
using Uruntakip.db;

namespace Uruntakip.Controllers
{
    public class StokController : Controller
    {
        // GET: Urun
        uruntakipdbEntities3 db = new uruntakipdbEntities3();
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult tedarikci()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult tedarikci(FormCollection frm)
        {
            
            try
            {
                tblCustomer t = new tblCustomer();
                t.adres = frm["adres"].ToString();
                t.firmaadi = frm["ad"].ToUpper().ToString();
                t.email = frm["email"].ToString();
                t.telefon = frm["telefon"].ToString();
                t.musteritipi = 0;

                db.tblCustomer.Add(t);
                db.SaveChanges();
                ViewBag.mesaj = 1;
            }
            catch (Exception)
            {

                ViewBag.mesaj = 0;
            }
            
            return View();
        }
        [Authorize]
        public ActionResult stokkarti()
        {
           List< tblkategori> urunkategorisi = db.tblkategori.ToList();
            ViewBag.tedarikci = db.tblCustomer.Where(x => x.musteritipi == 0).ToList();
            
            return View(urunkategorisi);
        }
        
        [HttpPost]
        public ActionResult stokkarti(FormCollection frm)
        {
            List<tblkategori> urunkategorisi = db.tblkategori.ToList();
            ViewBag.tedarikci = db.tblCustomer.Where(x => x.musteritipi == 0).ToList();

            string urunadi = frm["ad"].ToUpper().ToString().Trim();
            tblurunler urun = db.tblurunler.FirstOrDefault(x => x.urunadi ==urunadi );
            if(urun==null)
            {
                try
                {
                    tblurunler yeniurun = new tblurunler();
                    yeniurun.tedarikci_id =Convert.ToInt32( frm["tedarikci"].ToString().Trim());
                    yeniurun.urunadi = frm["ad"].ToUpper().Trim();
                    yeniurun.urun_fiyati = Convert.ToDecimal(frm["fiyat"].ToString().Trim());
                    yeniurun.urunkategori = Convert.ToInt32(frm["kategori"]);
                    db.tblurunler.Add(yeniurun);
                    db.SaveChanges();
                    ViewBag.mesaj = 1;
                }
                catch (Exception)
                {

                    ViewBag.mesaj = 2;
                }
            }
            else
            {
                ViewBag.mesaj = 0;
            }
            return View(urunkategorisi);
        }
        public ActionResult _urunlericek()
        {
            var liste = db.tblurunler.ToList().Take(10);
            List<tblurunler> t = new List<tblurunler>();
            foreach (var item in liste)
            {
                tblurunler n = new tblurunler();
                n = (tblurunler)item;
                t.Add(n);
            }
            return Json(t, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult urunekle()
        {
          var  liste = db.tblurunler.ToList().Take(10);
            List<tblurunler> t = new List<tblurunler>();
            foreach (var item in liste)
            {
                tblurunler n = new tblurunler();
                n =(tblurunler) item;
                t.Add(n); 
            }

            ViewBag.urunler = t;

            return View();

        }
        [HttpPost]
        public ActionResult urunekle(FormCollection frm)
        {
            var liste = db.tblurunler.ToList().Take(10);
            List<tblurunler> t = new List<tblurunler>();
            foreach (var item in liste)
            {
                tblurunler n = new tblurunler();
                n = (tblurunler)item;
                t.Add(n);
            }
            ViewBag.urunler = t;

            if(string.IsNullOrEmpty(frm["serino"].ToString()))
            {
                int urunid = Convert.ToInt32(frm["urun"].ToString());
                tblurundetayları a = db.tblurundetayları.FirstOrDefault(x => x.urun_id == urunid);

                if(a==null)
                {
                    try
                    {
                        tblurundetayları yeniurun = new tblurundetayları();
                        yeniurun.urun_id = Convert.ToInt32(frm["urun"].ToString());
                        yeniurun.urunserino = frm["serino"].ToString();
                        yeniurun.adet = Convert.ToInt32(frm["adet"].ToString());
                        db.tblurundetayları.Add(yeniurun);
                        db.SaveChanges();
                        ViewBag.mesaj = 1;

                    }
                    catch (Exception)
                    {

                        ViewBag.mesaj =0;
                    }
                    
                }
                else
                {
                    try
                    {
                        a.adet = a.adet + Convert.ToInt32(frm["adet"].ToString());
                        db.SaveChanges();
                        ViewBag.mesaj = 1;
                    }
                    catch (Exception)
                    {

                        ViewBag.mesaj = 0;
                    }
                    
                }
            }
            else
            {
                try
                {
                    tblurundetayları yeniurun = new tblurundetayları();
                    yeniurun.urun_id = Convert.ToInt32(frm["urun"].ToString());
                    yeniurun.urunserino = frm["serino"].ToString();
                    yeniurun.adet = Convert.ToInt32(frm["adet"].ToString());
                    db.tblurundetayları.Add(yeniurun);
                    db.SaveChanges();
                    ViewBag.mesaj = 1;
                }
                catch (Exception)
                {

                    ViewBag.mesaj = 0;
                }
               
            }
            

         
            return View();
        }
        public ActionResult _urunarama(string name)
        {
            List<tblurunler> liste = db.tblurunler.Where(x => x.urunadi.StartsWith(name.ToUpper())||x.urunadi.Contains(name.ToUpper())).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
    }
}