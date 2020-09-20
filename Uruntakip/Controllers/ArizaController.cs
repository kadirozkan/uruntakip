using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Uruntakip.db;
using Uruntakip.Models;

namespace Uruntakip.Controllers
{
    public class ArizaController : Controller
    {
        // GET: Ariza
        uruntakipdbEntities3 db = new uruntakipdbEntities3();
        public ActionResult Index()
        {
            return View();
        }
        //-----------------------------------------------------ARIZALAR ICIN YAPILAN ISLEMLER GIRILIYOR---------
        public ActionResult _arizaislemleri(int arizano,string islem,DateTime tarih,int islemtipi)
        {
            string sonuc = "0";
            try
            {
                tblarizaislemleri t = new tblarizaislemleri();
                t.aciklama = islem;
                t.ariza_id = arizano;
                t.islemtarihi = tarih;
                t.islemtipi = islemtipi;
                db.tblarizaislemleri.Add(t);
                db.SaveChanges();
                sonuc = "1";

            }
            catch (Exception)
            {

                sonuc = "0";
            }
            return Json(sonuc, JsonRequestBehavior.AllowGet);

            

        }
        //--------------------------------------------------------MAKINAYA AIT BUTUN ARIZALAR CEKILIYOR------------------
        public ActionResult _arizagecmisi(int makinaid)
        {
            List<cls_arizalistesi> gidicekliste = new List<cls_arizalistesi>();
            List<tblarizalar> arizalistesi = db.tblarizalar.Where(x => x.makina_id == makinaid).ToList();
            foreach (var item in arizalistesi)
            {
                var liste = (from a in db.tblarizalar
                             join k in db.tblarizakategorileri on a.kategori_id equals k.ariza_kategori_ID
                             join i in db.tblarizaislemleri on a.arizaid equals i.ariza_id
                             join s in db.tblarızasonuctipleri on i.islemtipi equals s.arizasonuc_id
                             where a.makina_id == makinaid&&a.arizaid==item.arizaid
                             orderby s.arizasonuc_id descending
                             select (new cls_arizalistesi { _arizano = a.arizaid, tarih2 = a.tarih.ToString(), _arizatanimi = a.tanım, _durum = s.durum, _kategoriadi = k.kategoriadi })).ToList().Take(1);
                
                
                foreach (var itm in liste)
                {
                    cls_arizalistesi c = new cls_arizalistesi();
                    c._arizano = itm._arizano;
                    c.tarih2 = itm.tarih2;
                    c._arizatanimi = itm._arizatanimi;
                    c._durum = itm._durum;
                    c._kategoriadi = itm._kategoriadi;

                    gidicekliste.Add(c);
                }
                
                

            }

            foreach (var item in gidicekliste)
            {
                item.tarih2 = tarihduzelt_gun_ay_yıl(item.tarih2);
            }

            return Json(gidicekliste, JsonRequestBehavior.AllowGet);
        }
        //---------------------------------------------------------ARIZA BILGILERI GUNCELLENIYOR-----------------------

        public ActionResult _arizaguncelle(int arizano, int kategori, string tanim, DateTime tarih)
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

        // ---------------------------------------------------------TARIH FORMATI DEGISTIRILIYOR----------------
        public string tarihduzelt_yıl_ay_gun(string tt)
        {
            string v = Convert.ToDateTime(tt).ToShortDateString();  // 19.09.2020 seklınde gelen tarıhı 2020-09-19 formatına cevırdık datetımepickere yazmak ıcın
            var t = v.Split('.');
          string tarih = t[2] + '-' + t[1] + '-' + t[0];
            return tarih;
        }
        public string tarihduzelt_gun_ay_yıl(string m)
        {
            string tarih = Convert.ToDateTime(m).ToShortDateString();  // 2020.08.19 seklınde gelen tarıhı 19.08.2020 formatına cevırdık datetıme yazmak ıcın
           
            return tarih;
        }
        //-------------------------------------------------------------ARIZAYA AIT BILGILER CEKILIYOR--------------------
        public ActionResult _arizabilgileri(int arizano)
        {
            tblarizalar aranan = db.tblarizalar.FirstOrDefault(x => x.arizaid == arizano);
            cls_arizalistesi c = new cls_arizalistesi();
            c._makinaid = (int)aranan.makina_id;
            c._kategoriid = (int)aranan.kategori_id;
            c._arizatanimi = aranan.tanım;
            c.tarih2 = tarihduzelt_yıl_ay_gun(aranan.tarih.ToString());  // 19.09.2020 seklınde gelen tarıhı 2020-09-19 formatına cevırdık datetıme pickera yazmak ıcın
            
            return Json(c, JsonRequestBehavior.AllowGet);

        }
       // -----------------------------------------------------------MAKINA TIPINE GORE ARIZA KATEGORILERI CEKILIYOR----------
        public ActionResult _arizakategorileri(int makinaid)
        {
            tblmakina mc = db.tblmakina.FirstOrDefault(x => x.makinaid == makinaid);
            List<tblarizakategorileri> liste = db.tblarizakategorileri.Where(x => x.makinatipi == mc.makinatip_id).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }

        //----------------------------------------------------------ARIZA KAYDI OLUSTURULUYOR--------------
        public ActionResult yeniarizakaydi(int makinaid, int kategori, string tanım, DateTime tarih)
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
                //işlem tablosunada verileri ekledik.
                tblarizaislemleri islem = new tblarizaislemleri();
                islem.ariza_id = arz.arizaid;
                islem.aciklama = arz.tanım;
                islem.islemtarihi = arz.tarih;
                islem.islemtipi = arz.durum;
                db.tblarizaislemleri.Add(islem);
                db.SaveChanges();
                sonuc = 1;
            }
            catch (Exception)
            {

                sonuc = 0;
            }

            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }



        //----------------------------------------------ARIZA ICIN YAPILAN ISLEM DETAYLARI CEKILIYOR---------

        [Authorize]
        public ActionResult arizadetayı(int id)
        {
            List<cls_arizalistesi> liste = (from i in db.tblarizaislemleri
                       join a in db.tblarizalar on i.ariza_id equals a.arizaid
                       join k in db.tblarizakategorileri on a.kategori_id equals k.ariza_kategori_ID
                       join s in db.tblarızasonuctipleri on i.islemtipi equals s.arizasonuc_id
                       join m in db.tblmakina on a.makina_id equals m.makinaid
                       where i.ariza_id == id
                       orderby i.islem_id descending
                       select( new cls_arizalistesi{_islemid= i.islem_id,tarih2= i.islemtarihi.ToString(),_arizatanimi= i.aciklama,_durum= s.durum })).ToList();



            


            return View(liste);
        }
        //-----------------------------------------------ACIK OLAN ARIZALAR CEKILIYOR-------------------
        public ActionResult _arizalistesinicek()
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
            return Json(listeler, JsonRequestBehavior.AllowGet);
        }

        //--------------------------------------------------İŞLEM TİPLERİNI CEKIYORUZ----------------------
        public ActionResult _islemtipleri()
        {
            List<tblarızasonuctipleri> liste = db.tblarızasonuctipleri.ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }


        public ActionResult getaktifariza(int id)
        {
            List<cls_arizalistesi> liste = (from a in db.tblarizalar join k in db.tblarizakategorileri on a.kategori_id equals k.ariza_kategori_ID where a.makina_id == id && (a.durum != 4 && a.durum != 5) select (new cls_arizalistesi { _arizano = a.arizaid, _kategoriadi = k.kategoriadi })).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);

        }

    }
}