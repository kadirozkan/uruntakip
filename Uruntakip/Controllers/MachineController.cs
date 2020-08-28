using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uruntakip.Models;
using Uruntakip.db;
using System.Security.Cryptography;
using System.Diagnostics;

namespace Uruntakip.Controllers
{
    public class MachineController : Controller
    {
        // GET: Machine

        uruntakipdbEntities3 db = new uruntakipdbEntities3();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _makinalar()
        {
            List<cls_makinalar> liste = (from c in db.tblCustomer join m in db.tblmakina on c.firmaid equals m.musteri_id join t in db.tblmakinatipi on m.makinatip_id equals t.tipid select (new cls_makinalar { _firmaadi = c.firmaadi, _makinaid = m.makinaid, _makinaserino = m.serino, _makinatipi = t.makinatipi })).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);

        }
        public ActionResult _makinasahibi(int makinaid)
        {
            tblmakina m = db.tblmakina.FirstOrDefault(x => x.makinaid == makinaid);
            tblCustomer musteri = db.tblCustomer.FirstOrDefault(x => x.firmaid == m.musteri_id);


            return Json(musteri, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _makinabul(int makinaid)
        {
            tblmakina t = db.tblmakina.FirstOrDefault(x => x.makinaid == makinaid);

            return Json(t, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _makinatipi()
        {

            List<tblmakinatipi> makinatipi = db.tblmakinatipi.ToList();
            return Json(makinatipi, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _makinaguncelle(int makinaid, int firmaid, int makinatipi, string makinaserino)
        {
            string sonuc = "";
            try
            {
                tblmakina guncellenen = db.tblmakina.FirstOrDefault(x => x.makinaid == makinaid);
                guncellenen.makinatip_id = makinatipi;
                guncellenen.musteri_id = firmaid;
                guncellenen.serino = makinaserino;
                db.SaveChanges();
                sonuc = "1";
            }
            catch (Exception)
            {

                sonuc = "0";
            }
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult makinalistesi()
        {
            return View();
        }
        
        
        [Authorize]
        public ActionResult arizalistesi()
        {
            List<cls_arizalistesi> listeler = new List<cls_arizalistesi>();
            List<tblarizalar> arızalar = db.tblarizalar.ToList();
            foreach (tblarizalar item in arızalar)
            {
                var liste = (from f in db.tblCustomer
                             join m in db.tblmakina on f.firmaid equals m.musteri_id
                             join mt in db.tblmakinatipi on m.makinatip_id equals mt.tipid
                             join a in db.tblarizalar on m.makinaid equals a.makina_id
                             join n in db.tblarizaislemleri on a.arizaid equals n.ariza_id
                             join s in db.tblarızasonuctipleri on n.islemtipi equals s.arizasonuc_id
                             where a.arizaid == item.arizaid
                             orderby n.islem_id descending
                             select new { a.arizaid, a.tanım, a.tarih, f.firmaadi, mt.makinatipi, m.serino, s.durum }).Take(1);

                foreach (var itm in liste)
                {
                    if (itm.durum != "Tamamlandı" && itm.durum != "İptal Edildi")
                    {
                        cls_arizalistesi c = new cls_arizalistesi();
                        c._arizano = itm.arizaid;
                        c.tarih2 = Convert.ToDateTime(item.tarih).ToShortDateString();
                        c._arizatanimi = itm.tanım;
                        c._durum = itm.durum;
                        c._firmaadi = itm.firmaadi;
                        c._makinatipi = itm.makinatipi;
                        c._serino = itm.serino;
                        listeler.Add(c);
                    }
                    
                    
                }
                                          
            }
           

            return View(listeler);
        }
       [Authorize]
       public ActionResult parcalistesi()
        {
            return View();
        }
    

    }
}