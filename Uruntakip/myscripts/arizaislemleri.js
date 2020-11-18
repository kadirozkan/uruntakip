//-----------------------------arıza formunun acılması-----------------------

function arizaformunuac() {
    $('#tblmakinalar tbody').on("click", "#ariza", function () {
        var _makinaid = $(this).val();
        document.getElementById("frmariza_tanim").value = "";
        document.getElementById("frmariza_tarih").value = "";
        $("#frmariza_makinaid").text(_makinaid);

        var gidendata = { makinaid: _makinaid };

        $.ajax({
            type: "POST",
            url: "/Ariza/_arizakategorileri",
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

// -------------------------------arıza kaydı olustur-------------------------
function arızakaydiac() {
    var _makinaid = $("#frmariza_makinaid").text();
    var _arizakategori = $("#frmariza_kategoriler :selected").val();
    var _tanim = $("#frmariza_tanim").val();
    var _tarih = $("#frmariza_tarih").val();
    if (_tanim == "" || _tarih == "") {
        swal("Uyarı!!!", "Bilgileri eksiksiz giriniz", "warning");
        return;

    }
    else {
        var gidendata = { makinaid: _makinaid, kategori: _arizakategori, tanım: _tanim, tarih: _tarih };
        $.ajax({
            type: "POST",
            url: "/Ariza/yeniarizakaydi",
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

//----------------------------------arıza bılgılerının getırılmesı-------------------

function arizabilgilerinigetir() {
    $("#tblarizagecmisi tbody").on("click", "#duzen", function () {

        var _arizano = $(this).val();
        $("#frmedit_arizano").text(_arizano);
        var gidendata = { arizano: _arizano };
        $.ajax({

            type: "POST",
            url: "/Ariza/_arizabilgileri",
            data: gidendata,
            success: function (data) {
                $("#frmedit_tarih").val(data.tarih2);
                $("#frmedit_makinaid").text(data._makinaid);
                document.getElementById("frmedit_tanim").value = data._arizatanimi;

                var dt = { makinaid: data._makinaid };

                $.ajax({

                    url: "/Ariza/_arizakategorileri",
                    data: dt,
                    type: "POST",
                    success: function (dr) {
                        var item = "";
                        $.each(dr, function (a, b) {

                            if (b.ariza_kategori_ID == data._kategoriid) {
                                item += '<option value="' + b.ariza_kategori_ID + '" selected="selected" >' + b.kategoriadi + '</option>';
                            }
                            else {
                                item += '<option value="' + b.ariza_kategori_ID + '">' + b.kategoriadi + '</option>';
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

//----------------------------------- arıza bılgılerını guncellıyoruz---------------

function arizayiguncelle() {
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
        url: "/Ariza/_arizaguncelle",
        data: gidendata,
        success: function (data) {

            if (data == "1") {
                $("#frmedit").modal("hide");
                $("#Registration")[0].reset();
                swal("İşlem başarılı", "", "success");
                var table = $('#tblarizagecmisi').DataTable();

                table
                    .clear()
                    .draw();

                var gidendata2 = { makinaid: _makinaid };
                $.ajax({
                    type: "POST",
                    url: "/Ariza/_arizagecmisi",
                    data: gidendata2,
                    success: function (data) {

                        $.each(data, function (v, k) {
                            if (k._durum == "İşleme Alındı") {
                                table.row.add([

                                    '<b>' + k._arizano + '</b>',
                                    '<b>' + k._kategoriadi + '</b>',
                                    '<b>' + k.tarih2 + '</b>',
                                    '<b>' + k._arizatanimi + '</b>',
                                    '<b style="color:green">' + k._durum + '</b>',
                                    '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>',,


                                ]).draw();
                            }
                            else if (k._durum == "Kontrol Ediliyor") {
                                table.row.add([

                                    '<b>' + k._arizano + '</b>',
                                    '<b>' + k._kategoriadi + '</b>',
                                    '<b>' + k.tarih2 + '</b>',
                                    '<b>' + k._arizatanimi + '</b>',
                                    '<b style="color:red">' + k._durum + '</b>',
                                    '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>',,


                                ]).draw();
                            }
                            else if (k._durum == "Malzeme Gönderildi") {
                                table.row.add([

                                    '<b>' + k._arizano + '</b>',
                                    '<b>' + k._kategoriadi + '</b>',
                                    '<b>' + k.tarih2 + '</b>',
                                    '<b>' + k._arizatanimi + '</b>',
                                    '<b style="color:blue">' + k._durum + '</b>',
                                    '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/'+k._arizano+'">'+'<input id="detay" type="button" class="btn btn-warning" value="Detaylar">'+'</a>',


                                ]).draw();
                            }
                            else if (k._durum == "Tamamlandı") {
                                table.row.add([

                                    '<b>' + k._arizano + '</b>',
                                    '<b>' + k._kategoriadi + '</b>',
                                    '<b>' + k.tarih2 + '</b>',
                                    '<b>' + k._arizatanimi + '</b>',
                                    '<b style="color:black">' + k._durum + '</b>',
                                    '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                                ]).draw();
                            }
                            else {
                                table.row.add([

                                    '<b>' + k._arizano + '</b>',
                                    '<b>' + k._kategoriadi + '</b>',
                                    '<b>' + k.tarih2 + '</b>',
                                    '<b>' + k._arizatanimi + '</b>',
                                    '<b style="color:yellow">' + k._durum + '</b>',
                                    '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                                ]).draw();

                            }

                        })

                    }



                });
            }
            else {
                swal("İşlem başarısız", "", "warning");
            }

        }


    })

}

//----------- Makinaya ait arızalar-----------------------

function arizalarigöster()
{
    var url = window.location.pathname;
    var id = url.substring(url.lastIndexOf('/') + 1);  
        var table = $('#tblarizagecmisi').DataTable();
           
           var gidendata = { makinaid: id };
            $.ajax({
                type: "POST",
                url: "/Ariza/_arizagecmisi",
                data: gidendata,
                success: function (data) {

                    $.each(data, function (v, k) {
                        if (k._durum == "İşleme Alındı") {
                            table.row.add([

                                '<b>' + k._arizano + '</b>',
                                '<b>' + k._kategoriadi + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:green">' + k._durum + '</b>',
                                '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', 


                            ]).draw();
                        }
                        else if (k._durum == "Kontrol Ediliyor") {
                            table.row.add([

                                '<b>' + k._arizano + '</b>',
                                '<b>' + k._kategoriadi + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:red">' + k._durum + '</b>',
                                '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                            ]).draw();
                        }
                        else if (k._durum == "Malzeme Gönderildi") {
                            table.row.add([

                                '<b>' + k._arizano + '</b>',
                                '<b>' + k._kategoriadi + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:blue">' + k._durum + '</b>',
                                '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                            ]).draw();
                        }
                        else if (k._durum == "Tamamlandı") {
                            table.row.add([

                                '<b>' + k._arizano + '</b>',
                                '<b>' + k._kategoriadi + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:black">' + k._durum + '</b>',
                                '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                            ]).draw();
                        }
                        else
                        {
                            table.row.add([

                                '<b>' + k._arizano + '</b>',
                                '<b>' + k._kategoriadi + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:yellow">' + k._durum + '</b>',
                                '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                            ]).draw();

                        }
                        

                    })

                }



            })


           
}

//--------------------------------------işlem formunun acılması
function islemformunuac()
{
    $("#tblarizalistesi tbody").on("click", "#ekle", function () {
        document.getElementById("frmislem_tanim").value = "";
        document.getElementById("frmislem_tarih").value = "";

        var arizano = $(this).val();
        $("#frmislem_arizano").text(arizano);
        $.get("/Ariza/_islemtipleri", {}, function (data) {
            var item = "";
            $.each(data, function (v, k) {

                if (k.arizasonuc_id != 1 && k.arizasonuc_id != 3) {
                    item += '<option value="' + k.arizasonuc_id + '">' + k.durum + '</option>'
                }


            });
            $("#frmislem_islemtipi").empty();
            $("#frmislem_islemtipi").append(item);

        })


        $("#frmislem").modal();

    })




}

//-------------------------------------Arızaya yeni işlem girisi-----------------------------

function islemgirisi()
{
    var _arizano = $("#frmislem_arizano").text();
    var _islem = $("#frmislem_tanim").val();
    var _tarih = $("#frmislem_tarih").val();
    var _islemtipi = $("#frmislem_islemtipi :selected").val();
    if (_islem == "" || _tarih == "")
    {
        swal("Uyarı", "Bilgileri eksiksiz giriniz", "warning")
        return;
    }

    var gidendata = { arizano: _arizano, islem: _islem, tarih: _tarih, islemtipi: _islemtipi };
    $.ajax({
        type: "POST",
        url: "/Ariza/_arizaislemleri",
        data: gidendata,

        success: function (data) {
            if (data == "1") {
                $("#frmislem").modal("hide");
                swal("İşlem Başarılı", "", "success");
                var table = $("#tblarizalistesi").DataTable();
                table
                    .clear()
                    .draw();
                $.get("/Ariza/_arizalistesinicek", {}, function (data) {

                    $.each(data, function (v, k) {
                        if (k._durum == "İşleme Alındı") {
                            table.row.add([
                                '<b>' + k._arizano + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._firmaadi + '</b>',
                                '<b>' + k._makinatipi + '</b>',
                                '<b>' + k._serino + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:green">' + k._durum + '</b>',
                                '<button id="ekle" value="' + k._arizano + '" class="btn btn-success">' + 'Ekle' + '</button>' + "   " + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + "   " + '<input type="button" value="Detay" id="detay" class="btn btn-warning" />' + " " + '</a >' + "    " + '<a href="#">' + " " + '<input type="button" value="Sevk" id="sevk" class="btn btn-info" />' + " " + '</a>'


                            ]).draw();

                        }
                        else if (k._durum == "Kontrol Ediliyor") {
                            table.row.add([
                                '<b>' + k._arizano + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._firmaadi + '</b>',
                                '<b>' + k._makinatipi + '</b>',
                                '<b>' + k._serino + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:red">' + k._durum + '</b>',
                                '<button id="ekle" value="' + k._arizano + '" class="btn btn-success">' + 'Ekle' + '</button>' + "   " + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '' + '<input type="button" value="Detay" id="detay" class="btn btn-warning" />' + '' + '</a >' + "   " + '<a href="#">' + '' + '<input type="button" value="Sevk" id="sevk" class="btn btn-info" />' + '' + '</a>'


                            ]).draw();

                        }
                        else if (k._durum == "Malzeme Gönderildi") {
                            table.row.add([
                                '<b>' + k._arizano + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._firmaadi + '</b>',
                                '<b>' + k._makinatipi + '</b>',
                                '<b>' + k._serino + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:blue">' + k._durum + '</b>',
                                '<button id="ekle" value="' + k._arizano + '" class="btn btn-success">' + 'Ekle' + '</button>' + "   " + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '' + '<input type="button" value="Detay" id="detay" class="btn btn-warning" />' + '' + '</a >' + "   " + '<a href="#">' + '' + '<input type="button" value="Sevk" id="sevk" class="btn btn-info" />' + '' + '</a>'


                            ]).draw();

                        }
                        else if (k._durum == "Tamamlandı") {
                            table.row.add([

                                '<b>' + k._arizano + '</b>',
                                '<b>' + k._kategoriadi + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:black">' + k._durum + '</b>',
                                '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                            ]).draw();
                        }
                        else {
                            table.row.add([

                                '<b>' + k._arizano + '</b>',
                                '<b>' + k._kategoriadi + '</b>',
                                '<b>' + k.tarih2 + '</b>',
                                '<b>' + k._arizatanimi + '</b>',
                                '<b style="color:yellow">' + k._durum + '</b>',
                                '<button id="duzen" value="' + k._arizano + '" class="btn btn-success">' + 'Güncelle' + '</button>' + ' ' + '<a href="/Ariza/arizadetayı/' + k._arizano + '">' + '<input id="detay" type="button" class="btn btn-warning" value="Detaylar">' + '</a>', ,


                            ]).draw();

                        }

                    })

                })
            }
            else
            {
                swal("İşlem başarısız", "", "warning");
            }

        }


    })
}