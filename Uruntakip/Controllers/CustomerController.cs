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
using Microsoft.Reporting.WebForms;

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
        public ActionResult getcustomer()
        {
            List<tblCustomer> liste = db.tblCustomer.Where(x=>x.musteritipi==1).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        //-------------------------------------------musterı lıstesı gonderılıyor
        public ActionResult musterilistesi()
        {
            List<tblCustomer> liste = db.tblCustomer.Where(x => x.musteritipi == 1).ToList();
            return View(liste);
           
            
        }
        [Authorize]
       //-------------------------------------------------id numarasına aıt musterı bılgılerı----------------
        public ActionResult musteribilgileri(int id)
        {
            tblCustomer musteri = db.tblCustomer.FirstOrDefault(x => x.firmaid == id);
            return Json(musteri, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        //--------------------------------------------------musterı bılgılerı guncellenıyor---------------------
        public ActionResult musteriguncelle(int id,string firmaadi,string ad,string soyad,string eposta,string telefon,string adres)
        {
            string kontrol;
            try
            {
                tblCustomer veri = db.tblCustomer.FirstOrDefault(x => x.firmaid == id);
                veri.adres = adres.Trim();
                veri.ad = ad.ToLower().Trim();
                veri.soyad = soyad.ToLower().Trim();
                veri.telefon = telefon.Trim();
                veri.firmaadi = firmaadi.ToUpper().Trim();
                veri.email = eposta.Trim();
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
        [Authorize]
       public ActionResult getteslimattip(int id)
        {
            List<tblteslimattipleri> liste = db.tblteslimattipleri.Where(x => x.sevkiyat_id == id).ToList();
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
        [Authorize]
        public ActionResult getaciklama(int id)
        {
            tblteslimattipleri t = db.tblteslimattipleri.FirstOrDefault(x => x.teslimatID == id);
            var info = t.aciklama.Split('-');
            string aciklama = "";
            for (int i = 1; i < info.Length; i++)
            {
                aciklama += "\n-" + info[i].Trim();
            }
            return Json(aciklama, JsonRequestBehavior.AllowGet);
            

           
        }
        [Authorize]
        public ActionResult teklifolustur(int id,DateTime tarih,string teklifno,string not,int sevkiyat,int teslimat,string teslimatnotu)
        {
           int sonuc = 0;
            try
            {
                tblCustomer c = db.tblCustomer.FirstOrDefault(x => x.firmaid == 1007);
                tblteklif sorgu = db.tblteklif.FirstOrDefault(x => x.teklifno == teklifno);
                if (sorgu == null)
                {
                    tblteklif t = new tblteklif();
                    t.musteri_id = id;
                    t.sevkiyat_id = sevkiyat;
                    t.tarih = tarih;
                    t.teklifno = teklifno;
                    t.teklifnotu = not.Trim();
                    t.teslimatid = teslimat;
                    t.teslimat_notu = teslimatnotu.Trim();
                    t.gdr_adres = c.adres;
                    t.gdr_email = c.email;
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
        [Authorize]
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
        [Authorize]
        public ActionResult teklifurunleri(string data,string teklifno)
        { string teklifid= null;
            try
            {
                tblteklif t = db.tblteklif.FirstOrDefault(x => x.teklifno == teklifno);
                List<tblteklif_urunler> pr = db.tblteklif_urunler.Where(x => x.teklif_id == t.teklifid).ToList();
                if (pr != null)
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
                    yeniurun.urun_id= Convert.ToInt32(gelen[0]);
                    yeniurun.urunadi = gelen[1];
                    yeniurun.urunadeti = Convert.ToInt32(gelen[2]);
                    yeniurun.birimfiyat = Convert.ToDecimal(gelen[3].Replace('.',','));
                    yeniurun.total = Convert.ToDecimal(gelen[4].Replace('.',','));
                    db.tblteklif_urunler.Add(yeniurun);
                    teklifid = t.teklifid.ToString();
                }
                db.SaveChanges();
            }
            catch (Exception)
            {

                teklifid = null;
            }

            return Json(teklifid, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult pdfgoruntule(string id)
        { 
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                int teklifid = Convert.ToInt32(id);
                tblteklif gelenteklif = db.tblteklif.FirstOrDefault(x => x.teklifid ==teklifid);
                if(gelenteklif!=null)
                {
                    LocalReport localReport = new LocalReport();
                    localReport.ReportPath = Server.MapPath("~/pdfreports/teklif.rdlc");

                    var liste = from m in db.tblCustomer
                                join t in db.tblteklif on m.firmaid equals t.musteri_id
                                join tu in db.tblteklif_urunler on t.teklifid equals tu.teklif_id
                                join p in db.tblparabirimleri on t.parabirimi equals p.paraid
                                where t.teklifid == gelenteklif.teklifid
                                select new { m.firmaadi, m.ad, m.soyad, m.adres, m.email, m.telefon, t.iskonto, p.parabirimi, p.sembol, t.gdr_adres, t.gdr_email, t.tarih, t.teklifno, t.teslimat_notu, tu.birimfiyat, tu.urun_id, tu.urunadi, tu.total, tu.urunadeti };
                    List<vwyeniteklif> tur = new List<vwyeniteklif>();
                    
                
                    foreach (var item in liste)
                      { 
                        
                        vwyeniteklif t = new vwyeniteklif();
                        t.firmaadi = item.firmaadi;
                        t.adres = item.adres;
                        t.email = item.email;
                        t.telefon = item.telefon;
                        t.urunadi = item.urunadi;
                        t.urunadeti = item.urunadeti;
                        t.total =decimal.Round( (decimal)item.total,2);
                        t.birimfiyat =decimal.Round((decimal) item.birimfiyat,2);
                        t.teklifno = item.teklifno;
                        t.urun_id = item.urun_id;
                        t.tarih = item.tarih;
                        t.ad = item.ad;
                        t.soyad = item.soyad;
                        t.teslimat_notu = item.teslimat_notu;
                        t.gdr_adres = item.gdr_adres;
                        t.gdr_email = item.gdr_email;
                        t.iskonto = item.iskonto;
                        t.parabirimi = item.parabirimi;
                        t.sembol = item.sembol;

                        tur.Add(t);
                    };
                    ReportDataSource reportDataSource = new ReportDataSource("musteriteklifleri", tur);

                    localReport.DataSources.Add(reportDataSource);
                    string reportType = "PDF";
                    string mimeTpe;
                    string encoding;
                    string fileNameExtension;




                    Warning[] warnings;
                    string[] streams;
                    byte[] renderedBytes;

                    renderedBytes = localReport.Render(
                                               reportType,
                                              "",
                                               out mimeTpe,
                                               out encoding,
                                               out fileNameExtension,
                                               out streams,
                                               out warnings);

                    //Response.AddHeader("content-disposition", "attachment; filename="+gelenteklif.teklifno+"." + fileNameExtension);

                    return File(renderedBytes, mimeTpe);
                }
                else
                {
                    return View();
                }
                
                
            }

        }

      
    }
}