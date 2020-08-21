using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uruntakip.Models;
using Uruntakip.db;
using System.Security.Cryptography;

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

        public ActionResult _arizakategorileri(int makinaid)
        {
            tblmakina mc = db.tblmakina.FirstOrDefault(x => x.makinaid == makinaid);
            List<tblarizakategorileri> liste = db.tblarizakategorileri.Where(x => x.makinatipi == mc.makinatip_id).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        public ActionResult yeniarizakaydi(int makinaid,int kategori,string tanım,DateTime tarih)
        {
            int sonuc = 0;
            try
            {
                tblarizalar arz = new tblarizalar();
                arz.makina_id = makinaid;
                arz.kategori_id = kategori;
                arz.tanım = tanım;
                arz.tarih = tarih.Date;
                arz.durum = 1;
                db.tblarizalar.Add(arz);
                db.SaveChanges();
                sonuc = 1;
            }
            catch (Exception)
            {

                sonuc = 0;
            }

            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult _arizabilgileri(int arizano)
        {
            
           

            tblarizalar aranan = db.tblarizalar.FirstOrDefault(x => x.arizaid == arizano);
            cls_arizalistesi c = new cls_arizalistesi();
            c._makinaid = (int)aranan.makina_id;
            c._kategoriid =(int) aranan.kategori_id;
           c._arizatanimi=aranan.tanım;
            c.tarih2 = Convert.ToDateTime(aranan.tarih).ToShortDateString();  // 19.09.2020 seklınde gelen tarıhı 2020-09-19 formatına cevırdık datetıme yazmak ıcın
            var t = c.tarih2.Split('.');
            c.tarih2 = t[2] + '-' + t[1] + '-' + t[0];

            return Json(c, JsonRequestBehavior.AllowGet);

        }
        public ActionResult _arizagecmisi(int makinaid)
        {

            List<cls_arizalistesi> liste = (from m in db.tblmakina
                                            join a in db.tblarizalar on m.makinaid equals a.makina_id
                                            join s in db.tblarızasonuctipleri on a.durum equals s.arizasonuc_id
                                            join k in db.tblarizakategorileri on a.kategori_id equals k.ariza_kategori_ID
                                            where m.makinaid==makinaid
                                            select (new cls_arizalistesi { _arizano = a.arizaid, _kategoriadi=k.kategoriadi,_arizatanimi=a.tanım,_durum=s.durum })).ToList();

            

            return Json(liste,JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult arizadetay(int id)
        {
            return View();

        }
        [Authorize]
        public ActionResult arizalistesi()
        {
            List<cls_arizalistesi> liste = (from f in db.tblCustomer
                                            join m in db.tblmakina on f.firmaid equals m.musteri_id
                                            join mt in db.tblmakinatipi on m.makinatip_id equals mt.tipid
                                            join a in db.tblarizalar on m.makinaid equals a.makina_id
                                            join s in db.tblarızasonuctipleri on a.durum equals s.arizasonuc_id
                                            select (new cls_arizalistesi { _arizano = a.arizaid,_arizatanimi=a.tanım,_tarih=(DateTime)a.tarih,_firmaadi=f.firmaadi,_makinatipi=mt.makinatipi,_serino=m.serino,_durum=s.durum })).ToList();

            return View(liste);
        }
       
        public ActionResult _arizaguncelle(int arizano,int kategori,string tanim,DateTime tarih)
        {
            string sonuc = "0";
            try
            {
                tblarizalar t = db.tblarizalar.FirstOrDefault(x => x.arizaid == arizano);
                t.kategori_id = kategori;
                t.tanım = tanim;
                t.tarih = tarih;
                db.SaveChanges();
                sonuc = "1";

            }
            catch (Exception)
            {

                sonuc = "0";
            }

            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
    

    }
}