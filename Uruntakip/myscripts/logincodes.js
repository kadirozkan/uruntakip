
//   ---------login işlemleri-------

    function logincontrol() {

            var userName = $("#name").val();
            var password = $("#pass").val();
            
        if (userName == "" || password == "")
           {
                swal("Bilgileri eksiksiz giriniz", "", "info");
                return;
            }

            var gidendata = {username: userName, password: password};

            $.ajax({
                type: 'POST',
                url: "/Home/Login",
                data: gidendata,

                success: function (sonuc) {

                    if (sonuc == "")
                    {

                       swal("Kullanıcı Bilgilerinizi kontrol ediniz", "", "warning");


                    }
                    else
                    {
                           
                         location.href = '/Customer/Index/';
                    }


                }

            }); return false;
        };


//----------yeni kayıt-------

function hesabikayet() {
    var email = $("#userposta").val();
    var sifre = $("#usersifre").val();

    if (email == "" || sifre == "") {
        swal("Bilgileri eksiksiz giriniz", "", "info");
        return;
    }
    var gidendata = { kullaniciadi: email, kullanicisifresi: sifre };
    $.ajax({

        type: "POST",
        url: "/Home/Kayıt",
        data: gidendata,
        success: function (sonuc) {
            if (sonuc == "1") {
                swal("Kayıt işlemi başarılı", "", "success");
                $("#Registration")[0].reset();
                
            }
            else if (sonuc == "2") {
                swal("Bu e-posta sistemde zaten kayıtlıdır", "", "warning");
                $("#Registration")[0].reset();

            }
            else {
                swal("Kayıt işlemi başarısız", "", "warrning");
                $("#Registration")[0].reset();
            }
        }


    });
};


//---- sifreyi göster -----
function goster() {
   
        var pas = $("#pass");
        if (pas.attr("type") == "password") {
            pas.attr("type", "text");
        }
        else {
            pas.attr("type", "password");
        }
    
};

//----kayıt ekranını formunu cagırma

function kayitformu() {
    $("#ShowModal").modal();
}


