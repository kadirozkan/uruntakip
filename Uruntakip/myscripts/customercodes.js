

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
        $("#ShowModal").modal();
        
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
                if (data == "1") {
                    $("#ShowModal").modal("hide");
                    swal("İşlem başarılı", "", "success");
                    // ıslem basarılı olduktan sonra tablo ıcı bosaltılıp musterıler fonksıyonu ıle tekrar yenı bılgıler dolduruyor.
                    var table = $('#tblmusteri').DataTable();

                    table
                        .clear()
                        .draw();
                    $.get("/Customer/getcustomer", {}, function (gelendata) {

                        $.each(gelendata, function (i, v1) {

                            table.row.add([
                                '<b>' + v1.firmaadi + '</b>',
                                '<b>' + v1.ad + '  ' + v1.soyad + '</b>',
                                '<b>' + v1.email + '</b>',
                                '<b>' + v1.telefon + '</b>',
                                '<b>' + v1.adres + '</b>',
                                '<b>' + '<button value="' + v1.firmaid + '" onclick="' + musteribilgileri() + '" class= "btn btn-success" id="guncelle">' + 'Güncelle' + '</button>' + '</b>' + ' ' + '<button id="teklif" onclick="teklifformu('+v1.firmaid+')" class="btn btn-warning">' + 'Teklif' + '</button>' + ' ' + '<button id="sevk" value="'+v1.firmaid+'" class="btn btn-info">'+'Sevk'+'</button>',

                                ]).draw(false);
                                });

                                });
                                }
                else {

                    swal("İşlem başarısız", "", "warning");
                }

            }


        });


}

//---------------------------------------teklif olusturma sayfasına yönlendırme 

function teklifformu(id)
{
    location.href = '/Customer/yeniteklif/'+id;
}
function teklifhazırla()
{
    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);   //müsteri id numarası url uzerınden alınıyor
    $("#form1").click(function () {

        var deger = $("#email").val();
        var ad = $("#adı").val();
        var soy = $("#soyadı").val();
        if (deger == "" || ad == "" || soy == "") {
            alert("eksık bılgı var");
        }
        else {

            ileri($("#form1").val());
        }

    });
    $("#form2").click(function () {

        ileri($("#form2").val())

    })
    $("#form3").click(function () {

        ileri($("#form3").val())

    })

}






    


