
//---------------tum makinalar cekılıyor---------------------

function tummakinalar()
{
    var table = $('#tblmakinalar').DataTable({
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    $.get("/Machine/_makinalar", {}, function (gelendata) {

            $.each(gelendata, function (i, v1) {

                table.row.add([
                    '<b>' + v1._firmaadi + '</b>',
                    '<b>' + v1._makinatipi + '</b>',
                    '<b>' + v1._makinaserino + '</b>',
                    '<button value="' + v1._makinaid + '" class= "btn btn-success" onclick="makinabilgileri()" id="guncelle">' +'Güncelle'+ '</button>' +" "+ '<button value="' + v1._makinaid + '" class= "btn btn-warning" id="ariza">' +'Arıza Aç'+ '</button>' +" "+ '<button value="' + v1._makinaid + '" class= "btn btn-info" id="gecmis">' +'Geçmiş'+ '</button>',
                ]).draw(false);


            })
            document.getElementById("veri").style.display = "none";


    })
        
   
   
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


