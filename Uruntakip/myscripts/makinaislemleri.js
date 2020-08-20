
//---------------tum makinalar cekılıyor---------------------

function tummakinalar() {
    $("#tblmakinalar").dataTable({
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });
    var table = $('#tblmakinalar').DataTable();
   
    $(document).ready(function () {

        $.get("/Machine/_makinalar", {}, function (gelendata) {

            $.each(gelendata, function (i, v1) {

                table.row.add([
                    '<b>' + v1._firmaadi + '</b>',
                    '<b>' + v1._makinatipi + '</b>',
                    '<b>' + v1._makinaserino + '</b>',
                    '<button value="' + v1._makinaid + '" class= "btn btn-success" onclick="makinabilgileri()" id="guncelle">' +'Güncelle'+ '</button>' +" "+ '<button value="' + v1._makinaid + '" class= "btn btn-warning" id="ariza">' +'Arıza Aç'+ '</button>' +" "+ '<button value="' + v1._makinaid + '" class= "btn btn-info" id="gecmis">' +'Geçmiş'+ '</button>',
                ]).draw(false);


            })



        })
        
    })
   
}


//----------- Makinaya ait arızalar-----------------------

function arizalarigöster() {

    $('#tblmakinalar tbody').on("click", "#gecmis", function () {
       
        var table = $('#tblgecmis').DataTable();

        if (document.getElementById('divgecmis').style.display == "") {
            document.getElementById('divgecmis').style.display = "none";
            document.getElementById('tblgecmis').style.display = "none";
            table
                .clear()
                .draw();
            
        }
        else if (document.getElementById('divgecmis').style.display == "none")
        {
            

            var _makinaid = $(this).val();

            var gidendata = { makinaid: _makinaid };
            $.ajax({
                type: "POST",
                url: "/Machine/_arizagecmisi",
                data: gidendata,
                success: function (data)
                {

                    $.each(data, function (v, k)
                    {
                        table.row.add([

                            '<b>' + k._arizano + '</b>',
                            '<b>' + k._kategoriadi + '</b>',
                            '<b>' + k._arizatanimi + '</b>',
                            '<b style="color:green">' + k._durum + '</b>',
                            '<button id="duzen" value="' + k._arizano + '" onclick="" class="btn btn-success">' + 'Düzenle' + '</button>' + '' + '<a href="/Machine/arizadetay/' + k._arizano + '">' + '  ' + '<input class="btn btn-warning" type="button" value="Detaylar" /></a >',
                           
                        ]).draw(false);

                    })

                }



            })


            document.getElementById('divgecmis').style.display = "";

            document.getElementById('tblgecmis').style.display = "";
        }


    });
}





//------------------------Makina bilgilerini cekiyoruz---------------

function makinabilgileri()
{
    $('#tblmakinalar tbody').on("click", "#guncelle", function () {
        var _makinaid = $(this).val();
        var gidendata = { makinaid: _makinaid };
        var makinatipid = "";
     
     
        // firmaid numarasını getirdik
        $.ajax({
            type: "POST",
            url: "/Machine/_makinasahibi",
            data: gidendata,
            success: function (data) {
                $("#frmguncelle_ad").text(data.firmaid);
                $("#frmguncelle_makinaid").text(_makinaid);
            }
        });
        // makinanın seri numarasını ve tipini aldık
        $.ajax({
            type: "POST",
            url: "/Machine/_makinabul",
            data: gidendata,
            success: function (data) {
                makinatipid = data.makinatip_id
                document.getElementById("frmguncelle_serino").value = data.serino;
            }
        });
        // makinanın ait oldugu firmayı dropdown listte secılı olarak getırdık selected ozellıgı ıle
        $.get("/Customer/getcustomer", {}, function (gelendata) {
            var musteri = $("#frmguncelle_ad").text();
            var item = "";
            $.each(gelendata, function (i, v1) {
                
                if (v1.firmaid == musteri) {
                    item += '<option selected="selected" value="'+ v1.firmaid +'">' + v1.firmaadi + "</option>";
                }
                else
                {
                    item += '<option value="' + v1.firmaid + ' ">' + v1.firmaadi  +"</option>";

                }
                
                
            });
            $("#frmguncelle_firmalar").empty();
            $("#frmguncelle_firmalar").append(item);

        });
        // makina tiplerini getirdik
        $.get("/Machine/_makinatipi", {}, function (gelendata) {
            
            var items = "";
            $.each(gelendata, function (i, v1) {

                if (v1.tipid == makinatipid) {
                    items += '<option selected="selected" value="' + v1.tipid + '">'  + v1.makinatipi + "</option>";
                }
                else {
                    items += '<option value="' + v1.tipid + ' ">' + v1.makinatipi + "</option>";

                }


            });
            $("#frmguncelle_makinatipi").empty();
            $("#frmguncelle_makinatipi").append(items);

        });


    });
    $("#frmguncelle").modal();
}

//-------------------------makina bilgilerini guncelliyoruz------------------------

function makinaguncelle()
{
    var _makinaid = $("#frmguncelle_makinaid").text();
    var secilenfirma = $("#frmguncelle_firmalar :selected").val();
    var secilentip = $("#frmguncelle_makinatipi :selected").val();
    
    var _serino = $("#frmguncelle_serino").val();
    var gidendata = { makinaid: _makinaid, firmaid: secilenfirma, makinatipi: secilentip, makinaserino: _serino };
    $.ajax({

        type: "POST",
        url: "/Machine/_makinaguncelle",
        data: gidendata,
        success: function (data) {
            if (data == "1") {
               
                var table = $('#tblmakinalar').DataTable();

                table
                    .clear()
                    .draw();
                //-----------------makina bılgılerını tekrar cekıp yazdırdık . yukardakı tummakınlar formunu cagırınca datatable 2 kez cagrıldıgı ıcın hata verıyor
                $.get("/Machine/_makinalar", {}, function (gelendata) {

                    $.each(gelendata, function (i, v1) {

                        table.row.add([
                            '<b>' + v1._firmaadi + '</b>',
                            '<b>' + v1._makinatipi + '</b>',
                            '<b>' + v1._makinaserino + '</b>',
                            '<button value="' + v1._makinaid + '" class= "btn btn-success" onclick="makinabilgileri()" id="guncelle">' + 'Güncelle' + '</button>' + " " + '<button value="' + v1._makinaid + '" class= "btn btn-warning" id="ariza">' + 'Arıza Aç' + '</button>' + " " + '<button value="' + v1._makinaid + '" class= "btn btn-info" id="gecmis">' + 'Geçmiş' + '</button>',
                        ]).draw(false);


                    })



                })
                $("#frmguncelle").modal("hide");
                swal("Başarılı", "", "success");
            }
            else
            {
                $("#frmguncelle").modal("hide");
                swal("Başarısız", "", "warning");
            }
        }



    });
}


//-----------------------Arıza formunu ac-----------------------------------------------------


function arizaformunuac() {
    $('#tblmakinalar tbody').on("click", "#ariza", function () {
        var _makinaid = $(this).val();
        document.getElementById("frmariza_tanim").value = "";
        document.getElementById("frmariza_tarih").value = "";
        $("#frmariza_makinaid").text(_makinaid);

        var gidendata = { makinaid: _makinaid };

        $.ajax({
            type: "POST",
            url: "/Machine/_arizakategorileri",
            data: gidendata,
            success: function (data) {
                var items = "";
                $.each(data, function (v, k) {

                    items += '<option value="' + k.ariza_kategori_ID + ' ">' + k.kategoriadi + "</option>";
                })
                $("#frmariza_kategoriler").empty();
                $("#frmariza_kategoriler").append(items);
            }

        })
        $.ajax({
            type: "POST",
            url: "/Machine/_makinabul",
            data: gidendata,
            success: function (data) {
                $("#frmariza_serino").text(data.serino);
               
            }

        })
        
        $("#frmariza").modal();
    })
}

//-------------------------Arıza kaydı olustur-----------------------------------------

function arızakaydiac()
{
    var _makinaid = $("#frmariza_makinaid").text();
    var _arizakategori = $("#frmariza_kategoriler :selected").val();
    var _tanim = $("#frmariza_tanim").val();
    var _tarih = $("#frmariza_tarih").val();
    if (_tanim == "" || _tarih == "") {
        swal("Uyarı!!!", "Bilgileri eksiksiz giriniz", "warning");
        return ;

    }
    else
    {
        var gidendata = { makinaid: _makinaid, kategori: _arizakategori, tanım: _tanim, tarih: _tarih };
        $.ajax({
            type: "POST",
            url: "/Machine/yeniarizakaydi",
            data: gidendata,
            success: function (data) {

                if (data == 1) {
                    $("#frmariza").modal("hide");
                    $("#Registration")[0].reset();
                    swal("İşlem başarılı", "", "success");
                }
                else {
                    swal("İşlem başarısız", "", "warning");
                }

            }


        })
    }
   
}
function arizabilgilerinigetir()
{
    $("#tblgecmis tbody").on("click", "#duzen", function () {

        var _arizano = $(this).val();
        $("#frmedit_arizano").text(_arizano);
        var gidendata = { arizano: _arizano };
        $.ajax({

            type:"POST",
            url:"/Machine/_arizabilgileri",
            data:gidendata,
            success: function (data)
            {
                $("#frmedit_tarih").val(data.tarih2);
                $("#frmedit_makinaid").text(data._makinaid);
                document.getElementById("frmedit_tanim").value = data._arizatanimi;
               
                var dt = { makinaid: data._makinaid };

                $.ajax({

                    url: "/Machine/_arizakategorileri",
                    data: dt,
                    type: "POST",
                    success: function (dr) {
                        var item = "";
                        $.each(dr, function (a, b) {

                            if (b.ariza_kategori_ID == data._kategoriid) {
                                item += '<option value="' + b.ariza_kategori_ID+'" selected="selected" >' + b.kategoriadi + '</option>';
                            }
                            else {
                                item += '<option value="' + b.ariza_kategori_ID +'">' + b.kategoriadi + '</option>';
                            }

                        })
                        $("#frmedit_kategoriler").empty();
                        $("#frmedit_kategoriler").append(item);
                    }

                })
                

            }
        })
        
       
       

        $("#frmedit").modal();
    })
}

//-----------------------------ARızayı guncellıyoruz ---------------------------------------------------

function arizayiguncelle()
{
    var _arizano = $("#frmedit_arizano").text();
    var _kategoriid = $("#frmedit_kategoriler :selected").val();
    var _tarih = $("#frmedit_tarih").val();
    var _tanim = $("#frmedit_tanim").val();
    var _makinaid = $("#frmedit_makinaid").text();
    var gidendata = { arizano: _arizano, kategori: _kategoriid, tanim: _tanim, tarih: _tarih };
    if (_tanim == "" || _kategoriid == "" || _tarih == "") {
        swal("Bilgileri kontrol ediniz", "", "Warning");
        return;
    }
   
    
    $.ajax({

          type: "POST",
          url: "/Machine/_arizaguncelle",
         data: gidendata,
         success: function (data) {

             if (data == "1")
             {
                    $("#frmedit").modal("hide");
                    $("#Registration")[0].reset();
                    swal("İşlem başarılı", "", "success");
                    var table = $('#tblgecmis').DataTable();

                    table
                        .clear()
                     .draw();

                    

                    var gidendata2 = { makinaid: _makinaid };
                 $.ajax({
                     type: "POST",
                     url: "/Machine/_arizagecmisi",
                     data: gidendata2,
                     success: function (data) {

                         $.each(data, function (v, k) {
                             table.row.add([

                                 '<b>' + k._arizano + '</b>',
                                 '<b>' + k._kategoriadi + '</b>',
                                 '<b>' + k._arizatanimi + '</b>',
                                 '<b style="color:green">' + k._durum + '</b>',
                                 '<button id="duzen" value="' + k._arizano + '" onclick="" class="btn btn-success">' + 'Düzenle' + '</button>' + '' + '<a href="/Machine/arizadetay/' + k._arizano + '">' + '  ' + '<input class="btn btn-warning" type="button" value="Detaylar" /></a >',

                             ]).draw(false);

                         })

                     }



                 });
                }
             else
             {
                    swal("İşlem başarısız", "", "warning");
             }

            }


        })
    
}