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
using System.Xml;

namespace Uruntakip.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Machine
        uruntakipdbEntities3 db = new uruntakipdbEntities3();
        public string tarihduzelt_yıl_ay_gun(string tt)
        {
            string v = Convert.ToDateTime(tt).ToShortDateString().Replace('.','/');  // 19.09.2020 seklınde gelen tarıhı 2020-09-19 formatına cevırdık datetımepickere yazmak ıcın
            var t = v.Split('/');
            if (t[1].Count() == 1)
            {
                t[1] = "0" + t[1];
            }
            if (t[0].Count() == 1)
            {
                t[0] = "0" + t[0];
            }
            string tarih = t[2] + '-' + t[1] + '-' + t[0];
            return tarih;
        }

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
        public ActionResult getcustomer(string name)
        {  if (string.IsNullOrEmpty(name))
            {
                List<tblCustomer> liste = db.tblCustomer.Where(x => x.musteritipi == 1).ToList();
                return Json(liste, JsonRequestBehavior.AllowGet);
            }
            else
            {
                name = name.ToUpper();
                List<tblCustomer> liste = db.tblCustomer.Where(x => x.musteritipi == 1&&(x.firmaadi.StartsWith(name)==true||x.firmaadi.Contains(name)==true||x.firmaadi.EndsWith(name)==true)).ToList();
                return Json(liste, JsonRequestBehavior.AllowGet);
            }
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
                    t.durum = 0;
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
        public ActionResult teklifdetay(string teklifno,string makinaid,string arizano,int parabirimi,int iskonto)
        {
            int sonuc = 0;
            try
            {
              
                tblteklif t = db.tblteklif.FirstOrDefault(x => x.teklifno == teklifno);
                if (string.IsNullOrEmpty(makinaid))
                {
                    t.makinaid = null;
                }
                else
                {
                    t.makinaid =Convert.ToInt32( makinaid);
                }
                if (string.IsNullOrEmpty(arizano))
                {
                    t.arizano = null;

                }
                else {

                    t.arizano =Convert.ToInt32( arizano);
                }
                
               
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

     
        [Authorize]
        public ActionResult gettekliflistesi(int musteriid)
        {
            List<cls_teklif> liste = (from c in db.tblCustomer join t in db.tblteklif on c.firmaid equals t.musteri_id where t.musteri_id == musteriid select (new cls_teklif { teklifid = t.teklifid, tarih = t.tarih.ToString(), acıklama = t.teklifnotu, firmaadi = c.firmaadi,durum= (int)t.durum})).ToList();
            return Json(liste, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult teklifduzenle(string id)
        {
            if (string.IsNullOrEmpty(id) == false)
            {
                int _id = Convert.ToInt32(id);
                tblteklif gelenteklif = db.tblteklif.FirstOrDefault(x => x.teklifid == _id);
                List<tblarizalar> acikarızalar = new List<tblarizalar>();

                if (gelenteklif != null)
                {
                    if(gelenteklif.makinaid!=null)
                    {
                        var arizalar = db.tblarizalar.Where(x => x.makina_id == gelenteklif.makinaid).ToList();
                       
                        foreach (var item in arizalar)
                        {
                            var liste = (from i in db.tblarizaislemleri join s in db.tblarızasonuctipleri on i.islemtipi equals s.arizasonuc_id where i.ariza_id == item.arizaid  orderby i.islem_id descending select new { i.ariza_id,s.durum}).Take(1);
                            foreach ( var item2 in liste)
                            { 
                                if (item2.durum != "Tamamlandı" && item2.durum != "İptal Edildi")
                                {
                                    tblarizalar t = db.tblarizalar.FirstOrDefault(x => x.arizaid == item2.ariza_id);
                                    acikarızalar.Add(t);

                                }
                               
                            }
                        }
                    }

                    ViewBag.makinalar = db.tblmakina.Where(x => x.musteri_id == gelenteklif.musteri_id).ToList();
                    ViewBag.teslimat = db.tblteslimattipleri.ToList();
                    ViewBag.sevkiyat = db.tblsevkiyat.ToList();
                    ViewBag.arizalar = acikarızalar;
                    ViewBag.parabirimleri = db.tblparabirimleri.ToList();

                    cls_teklif tek = new cls_teklif();
                    tek.teklifno = gelenteklif.teklifno;
                    tek.tarih = tarihduzelt_yıl_ay_gun(gelenteklif.tarih.ToString());
                    tek.teklifnotu = gelenteklif.teklifnotu;
                    tek.sevkiyatid =(int) gelenteklif.sevkiyat_id;
                    if (gelenteklif.makinaid != null)
                    {
                        tek.makinaid = (int)gelenteklif.makinaid;
                    }
                    else 
                    {
                        tek.makinaid = 0;
                    }
                    if (gelenteklif.arizano != null)
                    {
                        tek.makinaid = (int)gelenteklif.makinaid;
                    }
                    else
                    {
                        tek.arizano = 0;
                    }
                    
                    tek.teslimatid =(int) gelenteklif.teslimatid;
                    tek.teslimatnotu = gelenteklif.teslimat_notu;
                    if (gelenteklif.iskonto == null)
                    {
                        tek.iskonto = 0;
                    }
                    else
                    {
                        tek.iskonto = (int)gelenteklif.iskonto;
                    }
                    if (gelenteklif.parabirimi == null)
                    {
                        tblparabirimleri p = db.tblparabirimleri.FirstOrDefault(x => x.parabirimi == "EURO");
                        tek.paraid = p.paraid;
                    }
                    else 
                    {
                        tek.paraid = (int)gelenteklif.parabirimi;
                    }
                    

                    return View(tek);
                }
            }
           
           return View();
            
        }
        public ActionResult teklifbilgileriniguncelle(int id,string sayfa,string sevkiyat,string teslimat,string teslimatnotu,string teklifnotu,string makinaid,string arizano,string iskonto,string paraid,string urun)
        {
            int sonuc = 0;
            try
            {
                tblteklif teklif = db.tblteklif.FirstOrDefault(x => x.teklifid == id);
               
                if(teklif !=null)
                {
                    try
                    {
                        switch (sayfa)
                        {
                            case "1":
                                teklif.sevkiyat_id = Convert.ToInt32(sevkiyat);
                                teklif.teslimatid = Convert.ToInt32(teslimat);
                                teklif.teslimat_notu = teslimatnotu;
                                teklif.teklifnotu = teklifnotu;
                                break;
                            case "2":
                                if (string.IsNullOrEmpty(makinaid))
                                {
                                    teklif.makinaid = null;
                                }
                                else
                                {
                                    teklif.makinaid = Convert.ToInt32(makinaid);
                                }
                                if (string.IsNullOrEmpty(arizano))
                                {
                                    teklif.makinaid = null;
                                }
                                else
                                {
                                    teklif.makinaid = Convert.ToInt32(arizano);
                                }
                                teklif.iskonto = Convert.ToInt32(iskonto);
                                if (teklif.parabirimi != Convert.ToInt32(paraid))
                                {
                                   
                                    urunfiyatlarınıguncelle(teklif, Convert.ToInt32(paraid));
                                    teklif.parabirimi = Convert.ToInt32(paraid);
                                }
                                break;
                            case "3":
                                if(string.IsNullOrEmpty(urun)==false)
                                {
                                    var yeniurun = urun.Split('-');
                                    tblteklif_urunler t = new tblteklif_urunler();
                                    t.teklif_id = id;
                                    t.urun_id = Convert.ToInt32(yeniurun[0]);
                                    t.urunadi = yeniurun[1];
                                    t.urunadeti = Convert.ToInt32(yeniurun[2]);
                                    t.birimfiyat = Convert.ToDecimal(yeniurun[3].Replace('.',','));
                                    t.total = Convert.ToDecimal(yeniurun[4].Replace('.',','));
                                    db.tblteklif_urunler.Add(t);

                                        


                                }
                                break;
                         
                        }


                        db.SaveChanges();
                        sonuc = 1;
                    }
                    catch (Exception)
                    {

                        sonuc = 0;
                    }
                   
                }
            }
            catch (Exception)
            {

                sonuc = 0;
            }
            return Json(sonuc, JsonRequestBehavior.AllowGet);
        }
        public ActionResult teklifeaiturunler(string teklif,string sorgu,string urunid)
        {
            if (string.IsNullOrEmpty(teklif) == false && sorgu == "0")
            {
                int id = Convert.ToInt32(teklif);
                var liste = from t in db.tblteklif
                            join p in db.tblparabirimleri on t.parabirimi equals p.paraid
                            join u in db.tblteklif_urunler on t.teklifid equals u.teklif_id
                            where t.teklifid == id
                            select new { u.urun_id, u.birimfiyat, u.urunadeti, u.urunadi, u.total, p.parabirimi };
                return Json(liste, JsonRequestBehavior.AllowGet);

            }
            else 
            {
                int id = Convert.ToInt32(urunid);
                int teklifid = Convert.ToInt32(teklif);
                tblteklif_urunler tt = db.tblteklif_urunler.FirstOrDefault(x => x.teklif_id == teklifid && x.urun_id == id);
                db.tblteklif_urunler.Remove(tt);
                db.SaveChanges();
                var liste = from t in db.tblteklif
                            join p in db.tblparabirimleri on t.parabirimi equals p.paraid
                            join u in db.tblteklif_urunler on t.teklifid equals u.teklif_id
                            where t.teklifid == teklifid
                            select new { u.urun_id, u.birimfiyat, u.urunadeti, u.urunadi, u.total, p.parabirimi };
                return Json(liste, JsonRequestBehavior.AllowGet);

            }
            
        }

        public void urunfiyatlarınıguncelle(tblteklif teklif,int paraid)
        {
            tblparabirimleri eskipara = db.tblparabirimleri.FirstOrDefault(x => x.paraid == teklif.parabirimi);
            tblparabirimleri yenipara = db.tblparabirimleri.FirstOrDefault(x => x.paraid == paraid);
          List<tblteklif_urunler> liste = db.tblteklif_urunler.Where(x => x.teklif_id == teklif.teklifid).ToList();
            foreach (tblteklif_urunler item in liste)
            {
                item.birimfiyat = (decimal)fiyatduzenle(eskipara.parabirimi, yenipara.parabirimi, (double)item.birimfiyat);
                item.total = item.birimfiyat * item.urunadeti;
                tblteklif_urunler urun = db.tblteklif_urunler.FirstOrDefault(x => x.teklifurun_id == item.teklifurun_id);
                urun.birimfiyat = item.birimfiyat;
                urun.total = item.total;
            }
            db.SaveChanges();
           
        }
        public double fiyatduzenle(string eskipara,string yenipara, double tutar)
        {
            string bugun = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var xmldoc = new XmlDocument();
            xmldoc.Load(bugun);
            string usd = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='USD']/BanknoteSelling").InnerXml;
            string eur = xmldoc.SelectSingleNode("Tarih_Date/Currency [@Kod='EUR']/BanknoteSelling").InnerXml;
            usd = usd.Replace('.', ',');
            eur = eur.Replace('.', ',');
            double dolar = Convert.ToDouble(usd);
            double euro = Convert.ToDouble(eur);
            double deger = 0;
            switch (eskipara)
            {
                case "DOLAR":
                    switch (yenipara)
                    {
                        case "EURO":
                            deger = tutar * (dolar / euro);
                            break;
                        case "TÜRK LİRASI":
                            deger = tutar * dolar;
                            break;
                    }
                    break;
                case "EURO":
                    switch (yenipara)
                    {
                        case "DOLAR":
                            deger = tutar * (euro / dolar);
                            break;
                        case "TL":
                            deger = tutar * euro;
                            break;
                    }
                    break;
                case "TÜRK LİRASI":
                    switch (yenipara)
                    {
                        case "DOLAR":
                            deger = tutar * dolar;
                            break;
                        case "TÜRK LİRASI":
                            deger = tutar * euro;
                            break;
                    }
                    break;
            }
            deger = Math.Round(deger, 3);
            return deger;
        }
        [Authorize]
        public ActionResult tekliflistesi(string id)
        {
            int musteriid;
            List<cls_teklif> teklifler = new List<cls_teklif>();
            if(string.IsNullOrEmpty(id)==false)
            {

                musteriid = Convert.ToInt32(id);
                List<tblteklif> tt = db.tblteklif.Where(x => x.musteri_id == musteriid).OrderByDescending(y=>y.tarih).ToList();
                foreach (tblteklif item in tt)
                {
                    cls_teklif c = new cls_teklif();
                    c.teklifnotu = item.teklifnotu;
                    if(item.arizano==null)
                    {
                        c.arizano = 0;
                    }
                    else
                    {
                        c.arizano =(int) item.arizano;
                    }
                    if (item.makinaid == null)
                    {
                        c.serino = "";

                    }
                    else
                    {
                        tblmakina m = db.tblmakina.FirstOrDefault(x => x.makinaid == item.makinaid);
                        c.serino = m.serino;
                    }
                    c.teklifno = item.teklifno;
                    c.teklifid = item.teklifid;
                    c.tarih2 =(DateTime)item.tarih;
                    c.durum =(int) item.durum;
                    teklifler.Add(c);
                }
            }

            return View(teklifler);
        }
    }
}