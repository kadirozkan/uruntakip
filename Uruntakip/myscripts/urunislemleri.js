
 //---------------------------------------------girilen bilgilere göre ürün bilgileri getiriliyor--------------------
function urunarama()  
{
    var isim = $("#arama").val();
    
    $.post("../Stok/_urunarama", { name: isim }, function (data) {
        var liste = "";
       
        $.each(data, function (v, k) {
           

            liste += '<option value="' + k.uruid + '">'+  k.urunadi +''+'</option>';


        })
        $("#urun").empty();
        $("#urun").append(liste);


    })

}
// -----------------------------------------adet ve fiyat bölümüne sadece rakam girilmesini sagliyor--------------------------
function onlynumber() {
    $("#adet").bind('keypress', function (e) {
        if (e.keyCode == '9' || e.keyCode == '16') {
            return;
        }
        var code;
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;
        if (e.which == 46)
            return false;
        if (code == 8 || code == 46)
            return true;
        if (code < 48 || code > 57 )
            return false;
    });

    //Disable paste
    $("#adet").bind("paste", function (e) {
        e.preventDefault();
    });

    $("#adet").bind('mouseenter', function (e) {
        var val = $(this).val();
        if (val != '0') {
            val = val.replace(/[^0-9]+/g, "")
            $(this).val(val);
        }
    });
}
function fiyatverikontrol() {
    $("#fiyat").bind('keypress', function (e) {
        if (e.keyCode == '9' || e.keyCode == '16') {
            return;
        }
        var code;
        if (e.keyCode) code = e.keyCode;
        else if (e.which) code = e.which;
        if (e.which == 46)
            return false;
        if (code == 8 || code == 46||code==44)
            return true;
        if (code < 48 || code > 57)
            return false;
    });

    //Disable paste
    $("#fiyat").bind("paste", function (e) {
        e.preventDefault();
    });

    
}
//--------------------------------------------tedarikcilerin listesi getiriliyor---------------
function tedarikciler() {
    var table = $("#tbltedarikci").DataTable();
    table
        .clear()
        .draw();
    $.get("../Stok/_tedarikcilerigetir", {}, function (data) {
        $.each(data, function (v, k) {
            table.row.add([
                '<b>' + k._firmaadi + '</b >',
                '<b>' + k._personelad + '  ' + k._personelsoyad + '</b>',
                '<b>' + k._adres + '</b>',
                '<b>' + k._telefon + '</b>',
                '<b>' + k._email + '</b>',
                '<b>' + k._urunkategori + '</b>',
                '<b>' + '<button id="musteri" class="btn btn-success" value="'+k._musteriid+'">' + 'Düzenle' + '</button>' + '</b>',


            ]).draw();

        })

    })
}
//----------------------------------------------tedarikcinin urun kategorisi----------------------------------------------------------------------------
function tedarikcikategorisi()
{
    $(document).ready(function () {

        $.post("../Stok/_firmaurunkategorisi", { id: $("#tedarikci :selected").val() }, function (data) {
            var item = "";
            $.each(data, function (v, k) {

                item += '<option value="' + k._urunkatid + '">' + k._urunkategori + '</option>';

            })
            $("#kategori").empty();
            $("#kategori").append(item);

        }) 

    })
   
    
}

//---------------------------------------------tedarikci bilgileri getiriliyor

function tedarikcibilgileri(_id) {
    $.post("../Stok/_tedarickibilgileri", { id: _id }, function (data) {
        $.each(data, function (v, k) {
            $("#frmtedarikci_id").text(_id);
            document.getElementById("frmtedarikci_adi").value = k._personelad;
            document.getElementById("frmtedarikci_soyadi").value = k._personelsoyad;
            document.getElementById("frmtedarikci_firmaadi").value = k._firmaadi;
            document.getElementById("frmtedarikci_adres").value = k._adres;
            document.getElementById("frmtedarikci_telefon").value = k._telefon;
            document.getElementById("frmtedarikci_email").value = k._email;
            $.get("../Stok/_urunkategorileri", {}, function (data2) {

                var item = "";
                $.each(data2, function (i, j) {

                    if (k._urunkatid == j.kategoriID) {
                        item += '<option value="' + j.kategoriID + '" selected="selected">' + j.kategoriadi + '</option>';
                    }
                    else {
                        item += '<option value="' + j.kategoriID + '">' + j.kategoriadi + '</option>';
                    }

                })
                $("#frmtedarikci_kategori").empty();
                $("#frmtedarikci_kategori").append(item);
            })
            

        })


    })
}

//-----------------------------------------------tedarikci bilgilerini guncelle------------------

function tedarikciguncelle()
{
   
    $.post("../Stok/_tedarikci_bilgilerini_guncelle", { musteriid: $("#frmtedarikci_id").text(), firmaadi: $("#frmtedarikci_firmaadi").val(), personelad: $("#frmtedarikci_adi").val(), personelsoyad: $("#frmtedarikci_soyadi").val(), adres: $("#frmtedarikci_adres").val(), email: $("#frmtedarikci_email").val(), telefon: $("#frmtedarikci_telefon").val(), kategori: $("#frmtedarikci_kategori :selected").val() }, function (data) {
        if (data == 0) {
            swal("İşlem Başarısız", "", "warning");
        }
        else {
            $("#frmtedarikci").modal("hide");
            tedarikciler();
            swal("İşlem Başarılı", "", "success");
        }

    })
}