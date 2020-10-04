

//------musteri ekleme-----
function musteriekle()
{
    var _adi = $("#personeladi").val();
    var _soyadi = $("#personelsoyadi").val();
    var _eposta = $("#personeleposta").val();
    var _telefon = $("#personeltelefon").val();
    var _adres = $("#adres").val();
    var _firmadı = $("#firmaadi").val();
    
    if (_adi == "" || _soyadi == "" || _eposta == "" || _telefon == "" || _adres == "" || _eposta.indexOf("@")<0) {
        swal("Bilgilerinizi kontrol ediniz", "", "info");
        return;

    }

    var gidendata = { adi: _adi, soyadi: _soyadi, eposta: _eposta, telefon: _telefon, adres: _adres, firmaadi: _firmadı };
    $.ajax({
        type: "POST",
        url: "/Customer/musteriekle",
        data: gidendata,
        success: function (sonuc)
        {
            if (sonuc == "0") {
                swal("İşlem başarısız", "", "warning");
                $("#Registration")[0].reset();
            }
            else
            {
                swal("İşlem başarılı", "", "success");
                $("#Registration")[0].reset();
            }

        }

    });
}


//------------------------------------musteri bilgileri gösteriliyor--------------

function musteribilgileri()
{  
    
    $("#tblmusteri tbody").on("click", "#guncelle", function () {
        var musteriid = $(this).val();
        $.ajax({
            type: "POST",
            data: { id: musteriid },
            url: "/Customer/musteribilgileri",
            success: function (data) {

                $("#firmaid").text(data.firmaid);
                document.getElementById("_firmaad").value = data.firmaadi;
                document.getElementById("_ad").value = data.ad;
                document.getElementById("_soyad").value = data.soyad;
                document.getElementById("_email").value = data.email;

                document.getElementById("_telefon").value = data.telefon;
                document.getElementById("_adres").value = data.adres;
            }


        });
        $("#frmmusteri").modal();
        
    })
    

}

//--------------musterı bılgılerını guncelleme------------------------
function bilgileriguncelle()
{
        var musteriid = $("#firmaid").text();
        var _firmaad = $("#_firmaad").val();
        var _personelad = $("#_ad").val();
        var _personelsoyad = $("#_soyad").val();
        var _email = $("#_email").val();
        var _telefon = $("#_telefon").val();
        var _adres = $("#_adres").val();
        var gidendata = { id: musteriid, firmaadi: _firmaad, ad: _personelad, soyad: _personelsoyad, eposta: _email, telefon: _telefon, adres: _adres };
        $.ajax({
            type: "POST",
            data: gidendata,
            url: "/Customer/musteriguncelle",
            success: function (data) {
                if (data == "1")
                {
                    $("#frmmusteri").modal("hide");
                    swal("İşlem başarılı", "", "success");
                    // ıslem basarılı olduktan sonra tablo ıcı bosaltılıp musterıler fonksıyonu ıle tekrar yenı bılgıler dolduruyor.
                    var table = $("#tblmusteri").DataTable();

                    table
                        .clear()
                        .draw();
                    $.get("/Customer/getcustomer", {}, function (gelendata)
                    {

                        $.each(gelendata, function (i, v1)
                        {

                            table.row.add([
                                v1.firmaadi,
                                 v1.ad+' '+v1.soyad,
                                 v1.email,
                                 v1.telefon,
                                v1.adres,
                                '<button value="' + v1.firmaid + '" onclick="' + musteribilgileri() + '" class= "btn btn-success" id="guncelle">' + 'Güncelle' + '</button>' + ' ' + '<button id="teklif" onclick="teklifformu(' + v1.firmaid + ')" class="btn btn-danger">' + 'Teklif' + '</button>' + ' ' + '<button id="sevk" value="' + v1.firmaid + '" class="btn btn-warning">' + 'Sevk' + '</button>' + ' ' + '<button class="btn btn-primary" onclick="tekliflistesi(' + v1.firmaid + ')" id="goster">' + 'Göster' + '</button>',
                                ]).draw();
                        });

                    });
                }
                else
                {

                    swal("İşlem başarısız", "", "warning");
                }

            }


        });


}
function musterilistesi()
{
    var table = $("#tblmusteri").DataTable();

    table
        .clear()
        .draw();
    $.get("/Customer/getcustomer", {}, function (gelendata) {

        $.each(gelendata, function (i, v1) {

            table.row.add([
                v1.firmaadi,
                v1.ad + ' ' + v1.soyad,
                v1.email,
                v1.telefon,
                v1.adres,
                '<button value="' + v1.firmaid + '" onclick="' + musteribilgileri() + '" class= "btn btn-success" id="guncelle">' + 'Güncelle' + '</button>' + ' ' + '<button id="teklif" onclick="teklifformu(' + v1.firmaid + ')" class="btn btn-danger">' + 'Teklif' + '</button>' + ' ' + '<button id="sevk" value="' + v1.firmaid + '" class="btn btn-warning">' + 'Sevk' + '</button>' + ' ' + '<button class="btn btn-primary" onclick="tekliflistesi(' + v1.firmaid + ')" id="goster">' + 'Göster' + '</button>',
            ]).draw();
        });

    });
}

//---------------------------------------teklif olusturma sayfasına yönlendırme 

function teklifformu(id)
{
    location.href = '/Customer/yeniteklif/'+id;
}
//----------------------------------------sayfa yuklenınce teklif için default verılerı cekıyoruz---------------------
function teslimattiplerinicek() 
{
   
    $.post("/Customer/getteslimattip", { id: $("#sevkiyattip :selected").val() }, function (data) {
        var item = "";
        var a = 0;
        $.each(data, function (v, k) {
            if (a == 0)
            {
                a++;
                item += '<option value="' + k.teslimatID + '">' + k.teslimatadi + '</option>'
                $("#acıklama").empty();
                $("#acıklama").val(k.aciklama);
            }
            else {
                item += '<option value="' + k.teslimatID + '">' + k.teslimatadi  + '</option>'
            }

        })
        $("#teslimattip").empty();
        $("#teslimattip").append(item);
    })
}
//--------------------------------------urun bulma işlemi-----------------
function urunbul()
{
    document.getElementById("basarılı").style.display = "none";
    document.getElementById("urungecmisi").style.display = "none";
    document.getElementById("loding").style.display = "";
    var table = $("#urunstokları").DataTable();
    table
        .clear()
        .draw();
    
    $.post("/Stok/teklifurunu", { name: $("#arama").val(), teklifno: $("#_teklifno2").text() }, function (data) {

        $.each(data, function (v, k) {
            table.row.add([
                k._stokkodu,
                k._urunadi,
                k._urunadedi,
                k.urunfiyati,
                k.parabirimi,
                '<buton class="btn btn-danger" id="_sec">' + 'Ürün Seç' + '</buton>'

            ]).draw();

        })
        document.getElementById("loding").style.display = "none";
    })

}

//---------------------------------------teklıf olusturma ıslemlerı------------------------------
function teklifhazırla()
{
    teslimattiplerinicek();

    // sevkiyat tıp degistikce ona uygun gonderım tıplerını cekıyoruz
    $("#sevkiyattip").change(function () {
        teslimattiplerinicek();
    })

    // gonderım tıpıne gore acıklama bılgısı cekılıyor
    $("#teslimattip").change(function () {
        $.post("/Customer/getaciklama", { id: $(this).val() }, function (data) {

            $("#acıklama").empty();
            $("#acıklama").val(data);
        })

    })

    //------------------------------------varsa makına arızalarını cekıyoruz
    $("#makina").change(function () {
        if ($("#makina :selected").index() > 0)
        {
            $.post("/Ariza/getaktifariza", { id: $("#makina :selected").val() }, function (data) {
                var item = "";
                $.each(data, function (v, k) {

                    item += '<option value="' + k._arizano + '">' + k._arizano+' '+ '-' +' '+ k._kategoriadi   + '</option>';
                })
                $("#ariza").empty();
                $("#ariza").append(item);
            })
        }

    })

    //----------------------------------------ılk sayfadan teklıf bılgılerını alıyoruz ve bır sonrakı sayfaya yonlendırıyoruz
    var url = window.location.pathname;
    var musteriid = url.substring(url.lastIndexOf('/') + 1);   //müsteri id numarası url uzerınden alınıyor
    $("#form1").click(function () {

        var _tarih = $("#tarih").val();
        var _teklifno = $("#teklifno").val();
        var _not = $("#teklifnot").val();
        var _sevkiyat = $("#sevkiyattip :selected").val();
        var _teslimat = $("#teslimattip :selected").val();
        var _teslimatnotu = $("#acıklama").val();
        
        if (_tarih == "" || _teklifno == "" || _not == "" || _sevkiyat == "" || _teslimat == "" || _teslimatnotu == "")
        {
            swal("", "Lütfen bilgileri eksiksiz giriniz", "warning");
        }
        else {
            $("#_teklifno2").text(_teklifno);
            $.post("/Customer/teklifolustur", { id: musteriid, tarih: _tarih, teklifno: _teklifno, not: _not, sevkiyat: _sevkiyat, teslimat: _teslimat, teslimatnotu: _teslimatnotu }, function (data) {

                if (data == 0) {
                    swal("Bağlantı Hatası", "", "warning");
                }
                else if (data == 1) {
                    swal("Teklif No mevcut", "Lütfen teklif numarasını değiştirin", "info");
                }
                else
                {
                    ileri($("#form1").val());
                    
                }

            })
           
        }

    });
    
    // 2 cı sayfadan makına bılgılerını alıp bır sonrakı sayfaya yonlendırıyoruz

    $("#form2").click(function () {
        var _serino = "";
        var _arizano = "";
        if ($("#makina :selected").index() > 0) {
            _serino = $("#makina :selected").val();
            _arizano = $("#ariza :selected").val();
        }

        $.post("/Customer/teklifdetay", { teklifno: $("#_teklifno2").text(), serino: _serino, arizano: _arizano, iskonto: $("#iskonto").val(), parabirimi: $("#birim :selected").val() }, function (data) {

            if (data == 0) {
                swal("işlem başarısız", "", "warning");
            }
            else {
                ileri($("#form2").val())
            }

        })

    });
    // ---------------------------------listeden urun secılıyor------------------------------------------
    $("#urunstokları tbody").on("click", "#_sec", function () {
      
        document.getElementById("basarılı").style.display = "none";
        var data = $('#urunstokları').DataTable().row($(this).parents('tr')).data();
        $("#urunid").text(data[0]);
        $("#urunadi").val(data[1]);
        $("#fiyat").val(data[3]);
        

        //--------------------------------------------secilen urun baska bır fırma ıcın daha once teklıf hazırlandıysa bılgılerını yazdırıyoruz

        $.post("/Stok/urunteklifgecmisi", { id: data[0] }, function (gelenveri) {
            if (gelenveri.length>0)
            {
                var urungecmisi = $("#eskiteklif").DataTable();
                urungecmisi
                    .clear()
                    .draw();
                $.each(gelenveri, function (v, k) {

                    urungecmisi.row.add([
                        k.firmaadi,
                        k.tarih,
                        k.fiyat,
                        k.parabirimi


                    ]).draw();

                })

                document.getElementById("urungecmisi").style.display = "";

            }
            else
            {
                document.getElementById("urungecmisi").style.display = "none";
            }

        })

    })
    // --------------------------------------ekle butonuna basınca tabloya urun eklenıyor
    $("#sepet").click(function () {

        var table = $("#sepetim").DataTable();
        var id = $("#urunid").text();
        var urunadi = $("#urunadi").val();
        var adet = Number($("#adet").val());
        var birimfiyat = Number( $("#fiyat").val());
        birimfiyat = birimfiyat.toFixed(2);
        var total = (adet * birimfiyat).toFixed(2);

        table.row.add([
            id,
            urunadi,
            adet,
            birimfiyat,
            total,
            '<button class="btn btn-warning" id="sil">' + 'Ürünü Sil' + '</button>',

        ]).draw();
        $("#urunid").val("");
        $("#urunadi").val("");
        $("#adet").val(1);
        $("#fiyat").val("");

        document.getElementById("basarılı").style.display = "";

    });
   

    $("#form3").click(function () {

        ileri($("#form3").val())

    })
    // ıstenmeyen urun tablodan sılınıyor
    $("#sepetim tbody").on("click", "#sil", function () {

        var data = $('#sepetim').DataTable().row($(this).parents('tr')).remove().draw();

    })
    // teklıfe eklenen urunlerı alıp post edıcez
    $("#kaydet").click(function () {
        var table = $('#sepetim').DataTable();
        var uzunluk = Number(table.rows().count());
        var form_data = table.rows().data();
        var item = "";
        var a = 0;
        $.each(form_data, function (key, value) {
            if (a == 0) {
                a++;
                item += value[0] + '-' + value[1] + '-' + value[2] + '-' + value[3] + '-' + value[4];
            }
            else
            {
                item += '/' + value[0] + '-' + value[1] + '-' + value[2] + '-' + value[3] + '-' + value[4];
            }
        });

        $.post("/Customer/teklifurunleri", { data: item, teklifno: $("#_teklifno2").text() }, function (data) {

            if (data != null)
            {
                pdfgoruntule(data);
            }
            else {
                swal("Uyarı !!!", "işlem başarısız", "warning");
            }

        })

        

    })
   
}
// ------------------------------------olustrurunlan teklıfı ekranda gosterme-----------------------
function pdfgoruntule(id)
{
    location.href = "/Customer/pdfgoruntule/" + id;
}

//-------------------------------------------musteriye ait teklifleri gosterme------------
function tekliflistesi(id) {
    var table = $("#tekliflistesi").DataTable();
    table
        .clear()
        .draw();
    $.post("/Customer/gettekliflistesi", { musteriid: id }, function (gelenveri) {
        
        if (gelenveri.length > 0)
        {
            if (document.getElementById("divteklifler").style.display = "none") {
                document.getElementById("divteklifler").style.display = "";
            }
            var items = "";
            $.each(gelenveri, function (k, v) {

                if (v.durum == 0) {
                    table.row.add([
                        v.firmaadi,
                        v.tarih,
                        v.acıklama,
                        'Onay Bekleniyor',
                        '<a href="../Customer/teklifduzenle/' + v.teklifid + '" class="btn btn-success">' + 'Düzenle' + '</a>' +' ' +'<a href="../Customer/pdfgoruntule/' + v.teklifid + '" class="btn btn-primary">'+'Görüntüle' + '</a>',
                       
                    ]).draw();
                }
                else {
                    table.row.add([
                        v.firmaadi,
                        v.tarih,
                        v.acıklama,
                        'Teklif Onaylandı',
                        '<a href="../Customer/teklifduzenle/' + v.teklifid + '" class="btn btn-success">' + 'Düzenle' + '</a>' + ' ' + '<a href="../Customer/pdfgoruntule/' + v.teklifid + '" class="btn btn-primary">' + 'Görüntüle' + '</a>',
                    ]).draw();
                }



                


            })
           
        }

    })
}





    


