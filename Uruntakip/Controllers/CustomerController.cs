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
using System.EnterpriseServices;
using System.Net;
using Newtonsoft.Json.Linq;

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
        //-----------------------------------musterı ekleme ıslemlerı
        public ActionResult musteriekle(string adi,string soyadi,string eposta,string adres,string telefon,string firmaadi)
        {
            string sonuc ="0";
            try
            {
                tblCustomer t = new tblCustomer();
                t.ad = adi.ToUpper();
                t.soyad = soyadi.ToUpper();
                t.email = eposta;
                t.adres = adres;
                t.telefon = telefon;
                t.firmaadi = firmaadi.ToUpper();
                t.musteritipi = 1;
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
        //-------------------------------------------musterı lıstesı gonderılıyor
        public ActionResult musterilistesi()
        {
            List<tblCustomer> liste = db.tblCustomer.Where(x => x.musteritipi == 1).ToList();
            return View(liste);
           
            
        }
        
        
       
       //-------------------------------------------------id numarasına aıt musterı bılgılerı----------------
        public ActionResult musteribilgileri(int id)
        {
            tblCustomer musteri = db.tblCustomer.FirstOrDefault(x => x.firmaid == id);
            return Json(musteri, JsonRequestBehavior.AllowGet);
        }

        //--------------------------------------------------musterı bılgılerı guncellenıyor---------------------
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
       [Authorize]
        public ActionResult yeniteklif(int id)
        {
           
            ViewBag.sevkiyat = db.tblsevkiyat.ToList();
            ViewBag.para = db.tblparabirimleri.ToList();
            ViewBag.makina = db.tblmakina.Where(x => x.musteri_id == id);

            
            return View();
        }
        
       public ActionResult getgonderimtip(int id)
        {
            List<tblgonerimtipleri> liste = db.tblgonerimtipleri.Where(x => x.sevkiyat_id == id).ToList();
            foreach (var item in liste)
            {
                var acıklama = item.aciklama.Split('-');
                string yeniacıklama = "";
                for (int i = 1; i < acıklama.Length; i++)
                {
                    yeniacıklama += "\n-"+acıklama[i].Trim();

                }
                item.aciklama = yeniacıklama.Trim();
            }
          return  Json(liste, JsonRequestBehavior.AllowGet);
        }
        public ActionResult getaciklama(int id)
        {
            tblgonerimtipleri t = db.tblgonerimtipleri.FirstOrDefault(x => x.gonderiID == id);
            var info = t.aciklama.Split('-');
            string aciklama = "";
            for (int i = 1; i < info.Length; i++)
            {
                aciklama += "\n-" + info[i].Trim();
            }
            return Json(aciklama, JsonRequestBehavior.AllowGet);
            

           
        }
        public ActionResult teklifolustur(int id,DateTime tarih,string teklifno,string not,int sevkiyat,int teslimat,string teslimatnotu)
        {
           int sonuc = 0;
            try
            {
                tblteklif sorgu = db.tblteklif.FirstOrDefault(x => x.teklifno == teklifno);
                if (sorgu == null)
                {
                    tblteklif t = new tblteklif();
                    t.musteri_id = id;
                    t.sevkiyat_id = sevkiyat;
                    t.tarih = tarih;
                    t.teklifno = teklifno;
                    t.teklifnotu = not;
                    t.teslimatid = teslimat;
                    t.teslimat_notu = teslimatnotu;
                    db.tblteklif.Add(t);
                    db.SaveChanges();
                    sonuc = 2;
                }
                else
                {
                    sonuc = 1;
                }
               

            }
            catch (Exception)
            {

                sonuc = 0;
            }
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult teklifdetay(string teklifno,string serino,string arizano,int parabirimi,int iskonto)
        {
            int sonuc = 0;
            try
            {
                tblteklif t = db.tblteklif.FirstOrDefault(x => x.teklifno == teklifno);
                t.arizano = arizano;
                t.makinserino = serino;
                t.parabirimi = parabirimi;
                t.iskonto = iskonto;
                db.SaveChanges();
                sonuc = 1;
            }
            catch (Exception)
            {
                sonuc = 0;
                
            }
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult teklifurunleri(string data,string teklifno)
        {
            tblteklif t = db.tblteklif.FirstOrDefault(x => x.teklifno == teklifno);
            List<tblteklif_urunler> pr = db.tblteklif_urunler.Where(x => x.teklif_id == t.teklifid).ToList();
            if(pr!=null)
            {
                foreach (tblteklif_urunler item in pr)
                {
                    db.tblteklif_urunler.Remove(item);
                }
            }
            
           var veri = data.Split('/');
            for (int i = 0; i < veri.Length; i++)
            {
                var gelen = veri[i].Split('-');
                tblteklif_urunler yeniurun = new tblteklif_urunler();
                yeniurun.teklif_id = t.teklifid;
                yeniurun.teklifurun_id =Convert.ToInt32(gelen[0]);
                yeniurun.urunadi = gelen[1];
                yeniurun.urunadeti =Convert.ToInt32( gelen[2]);
                yeniurun.birimfiyat = Convert.ToDecimal(gelen[3]);
                yeniurun.total = Convert.ToDecimal(gelen[4]);
                db.tblteklif_urunler.Add(yeniurun);
                
            }
            db.SaveChanges();
            return View();
        }

    }
}