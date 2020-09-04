

//------musteri ekleme-----
function musteriekle()
{
    var _adi = $("#personeladi").val();
    var _soyadi = $("#personelsoyadi").val();
    var _eposta = $("#personeleposta").val();
    var _telefon = $("#personeltelefon").val();
    var _adres = $("#adres").val();
    var _firmadı = $("#firmaadi").val();
    if (_adi == "" || _soyadi == "" || _eposta == "" || _telefon == "" || _adres == "") {
        swal("Bilgileri eksiksiz girin", "", "info");
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
                $("#yenimusteri")[0].reset();
            }
            else
            {
                swal("İşlem başarılı", "", "success");
                $("#yenimusteri")[0].reset();
            }

        }

    });
}


// --------musteri listesini cekme--------
function musteriler()
{
    $('#tblmusteri').dataTable({
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    var table = $('#tblmusteri').DataTable();
   
    $(document).ready(function () {
       
        $.get("/Customer/getcustomer", {}, function (gelendata) {
            
                $.each(gelendata, function (i, v1) {
               
                table.row.add([
                    '<b>' + v1.firmaadi + '</b>',
                    '<b>' + v1.ad + '  ' + v1.soyad + '</b>',
                    '<b>' + v1.email + '</b>',
                    '<b>' + v1.telefon + '</b>',
                    '<b>' + v1.adres + '</b>',
                    '<button value="' + v1.firmaid + '" onclick="'+musteribilgileri()+'" class= "btn btn-success" id="guncelle">'+'Güncelle'+'</button>',
                   
                ]).draw(false);
            });
            
        });
       
    });
}

//-------musteri bilgilerini cekme --------------

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
                                '<button value="' + v1.firmaid + '" onclick="' + musteribilgileri() + '" class= "btn btn-success" id="guncelle">' + 'Güncelle' + '</button>',

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

//----- musterinin adresını gosterme------------------------
function adresgoster() {
    var _musteriid;
    $("#tblmusteri tbody").on("click", "#adres", function () {
        _musteriid = $(this).val();
        var _data = { id: _musteriid }
        $.ajax({
            type: "POST",
            url: "/Customer/adresgoster",
            data: _data,
            success: function (data) {
                if (data != null || data != "") {
                    swal("Adres Bilgileri",data, "info");
                }
                else {
                    swal("Adres bulunamadı", "", "warning");
                }
            }

        });

    })

}





    


