

function urunarama()
{
    var isim = $("#arama").val();
    if (isim == "") {
        $.get("../Stok/_urunlericek", {},function (data) {
            var liste = "";
            $("#urun").empty();
            $.each(data, function (v, k) {


                liste += '<option value="' + k.uruid + '">' + '<b>' + k.urunadi + '' + '</b>' + '</option>';


            })
            $("#urun").append(liste);


        })
    }
    $.post("../Stok/_urunarama", { name: isim }, function (data) {
        var liste = "";
        $("#urun").empty();
        $.each(data, function (v, k) {
           

            liste += '<option value="' + k.uruid + '">'+'<b>' + k.urunadi +''+'</b>'+'</option>';


        })
        $("#urun").append(liste);


    })

}

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
        if (code < 48 || code > 57)
            return false;
    });
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