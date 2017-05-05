var header = "Seçilen yazıyı not al";
var save = "Kaydet";
var cancel = "İptal";


var placeholder = "not için etiket belirle";
var icon = chrome.extension.getURL('icons/icon48.png');
$("#Quotesheader").attr("src", icon);

var oldhtml = '<div id="MyQuotes" class="reset-this MyQuotesform"><div do-toolbar><span>Duyarlı Ol</span><span id="hakkebtn"></span></div>  <div id="usercreditcards"></div> <div id="userwishlist"></div>  <div id="do-questions"></div>  <div class="reset_this"><img src="' + icon + '" id="QuotesheaderIcon" class="reset-this"/><h3 class="reset-this">' + header + '</h3></div>  <form class="reset-this login-form">  <div class="reset-this"> <textarea  autocomplete="off" autocorrect="off" autocapitalize="off" spellcheck="false" class="reset-this" id="myQuotesText"></textarea></div> <div class="reset-this" id="MyQuotesOwnerWrapper"> <img class="reset-this" id="MyQuotesOwnerLoading" /> <input class="reset-this" placeHolder="' + placeholder + '" type="text" id="MyQuotesOwner" /> </div> <div class="reset-this"> <button class="reset-this" id="MyQuotesSave" type="button">' + save + '</button> <button id="MyQuotesCancel" class="reset-this" type="button">' + cancel + '</button> </div> <p class="reset-this MyQuotesmessage"></p> </form> </div>';

var html = '<div id="MyQuotes" class="reset-this MyQuotesform"><div do-toolbar><span>Duyarlı Ol</span><span id="hakkebtn"></span></div>  <div id="usercreditcards"></div> <div id="userwishlist"></div>  <div id="do-questions"></div> ' +
    '<form class="reset-this login-form"><div class="reset-this"> <button class="reset-this" id="btnDoRun" type="button">Duyarlı Ol</button> <button id="btnDoCancel" class="reset-this" type="button">Iptal</button></div><p class="reset-this MyQuotesmessage"></p> </form> </div>';
var navbuttons = '<div class="reset-this"> <button class="reset-this" id="btnDoRun" type="button">Duyarlı Ol</button> <button id="btnDoCancel" class="reset-this" type="button">Iptal</button></div><p class="reset-this MyQuotesmessage"></p>';

//var questions = '<div id="do-questions"></div>';
var q1 = '<div id="do-q1" class="do-question"><p question>Online alışverişi çok fazla tercih ediyor musunuz?</p><div class="do-radiobuttons"><div><input type="radio" name="q1" value="evet" /> Evet </div><div> <input type="radio" name="q1" value="hayır" /> Hayır </div> </div></div>';
var q2 = '<div id="do-q2" class="do-question"><p question>Online alışveriş dışında günlük hayatta kredi  kartınızı sıklıkla kullanır mısınız?</p><div class="do-radiobuttons"><div><input type="radio" name="q2" value="evet" /> Evet </div><div> <input type="radio" name="q2" value="hayır" /> Hayır </div> </div></div>';
var q3 = '<div id="do-q3" class="do-question"><p question>Hey ay düzenli olarak kredi kartı borcunuzu (en az asgari tutar) ödüyor musunuz?</p><div class="do-radiobuttons"><div><input type="radio" name="q3" value="evet" /> Evet </div><div> <input type="radio" name="q3" value="hayır" /> Hayır </div> </div></div>';
var q4 = '<div id="do-q4" class="do-question"><p question>Elinizde olan bir ürün/ürünleri tekrar almayı tercih eder misiniz?</p><div class="do-radiobuttons"><div><input type="radio" name="q4" value="evet" /> Evet </div><div> <input type="radio" name="q4" value="hayır" /> Hayır </div> </div></div>';
var q5 = '<div id="do-q5" class="do-question"><p question>Almakta olduğunuz ürün/ürünlere gerçekten ihtiyacınız olduğunu düşünüyor musunuz?</p><div class="do-radiobuttons"><div><input type="radio" name="q5" value="evet" /> Evet </div><div> <input type="radio" name="q5" value="hayır" /> Hayır </div> </div></div>';
var q6 = '<div id="do-q6" class="do-question"><p question>Bu kampanyaya karşı dürüst olduğunuzu düşünüyor musunuz?</p><div class="do-radiobuttons"><div><input type="radio" name="q6" value="evet" /> Evet </div><div> <input type="radio" name="q6" value="hayır" /> Hayır </div> </div></div>';



var firstloader = '<div style="float:left !important;margin:5px;"><img style="width:32px;" src="' + chrome.extension.getURL('content/loading2.gif') + '" /></div> <div style="float:left !important;margin:10px 0px 0px 0px !important"><p class="reset-this">Duyarlı.ol Çalıştırılıyor</p></div> <i style="clear:both"></i>';
var load = '<div style="float:left !important;margin:5px;"><img style="width:32px;" src="' + chrome.extension.getURL('content/loading2.gif') + '" /></div> <div style="float:left !important;margin:10px 0px 0px 0px !important"><p class="reset-this">Duyarlılık Çalıştırılıyor..</p></div> <i style="clear:both"></i>';
var complete = '<div style="float:left !important;margin:5px;"><img style="width:32px;" src="' + chrome.extension.getURL('content/ok.png') + '" /></div> <div style="float:left !important;margin:10px 0px 0px 0px !important"><p id="doSuccessMessage" class="reset-this" ></p></div> <i style="clear:both"></i>';
var error = '<div style="float:left !important;margin:5px;"><img style="width:32px;" src="' + chrome.extension.getURL('content/error.png') + '" /></div> <div style="float:left !important;margin:10px 0px 0px 0px !important"><p id="doFailMessage" class="reset-this" ></p></div> <i style="clear:both"></i>';

$("#MyQuotes").is(function () {
    $(".MyQuotesform").remove();
});
//$("html body").append(html);

var loading = chrome.extension.getURL('content/loading.gif')
var loading2 = chrome.extension.getURL('content/loading2.gif');

$("#MyQuotesOwnerLoading").hide().attr("src", loading)

var splash = '<div id="splashscreen" class="reset-this splashform">' + firstloader + '</div>';
$('html body').append(html);
$('html body').append(splash);

//function getCookie(cname) {
//    var name = cname + "=";
//    var ca = document.cookie.split(';');
//    for (var i = 0; i < ca.length; i++) {
//        var c = ca[i];
//        while (c.charAt(0) == ' ') {
//            c = c.substring(1);
//        }
//        if (c.indexOf(name) == 0) {
//            return c.substring(name.length, c.length);
//        }
//    }
//    return "";
//}


var siteUrl = "http://localhost:64481";
var OnlineSiteUrl = "http://www.putnotes.net";
var azureUrl = "http://duyarliol.azurewebsites.net";
var handlerClause = "/handlers/main.ashx";

var answerNeedAssign = true;

$(function () {
   
    //get user info
    $.ajax({
        url: azureUrl + handlerClause + '?fm=get-user-info&id=' + userid,
        dataType: 'json',
        success: function (userdata) {
            //console.log('success');
            //alert(donen_veri);
            //console.log(donen_veri);
            

            var username = userdata.username;
            console.log(username);
            $('#hakkebtn').text('Hoşgeldin,' + username);
        },
        error: function (err) {
            console.log('error');
            console.log(err);
        },
        complete: function () {
            console.log('getuserinfo COMPLETE');
            setTimeout(function () {
                $('#splashscreen').remove();
                $("#MyQuotes").attr('style', 'display:inline !important');
            },3333);
            
            /* adding wishlist to html */
            if (wisharray.length > 0) {

                var table = '<p>Sipariş Listen</p><table>';
                table += '<tr id="tableheader"><th>Ürün Adı</th><th>Adedi</th><th>Fiyatı</th></tr>';

                var total = 0;
                $.each(wisharray, function (key, wish) {
                    wish.price = wish.price.replace(".", "");

                    table += '<tr><th wishthtitle>' + wish.name + '</th><th>' + wish.count + ' </th><th>' + wish.price + 'TL </th></tr>';
                    console.log(parseFloat(wish.price));

                    total += parseFloat(wish.price);
                });


                table += "<tr><th totalprice>Toplam:" + total + " TL</th><th></th><th></th></tr>";
                table += '</table>';

                $('#userwishlist').html(table);
                //$('#MyQuotes form').append(navbuttons);
            }
            questions();
            }
    });


    function questions() {

        $.ajax({
            url: azureUrl + handlerClause + '?fm=check-answer-list-ce&id=' + userid,
            dataType: 'json',
            success: function (totalcount) {
                console.log('check answer list');
                //console.log(data);

                if (parseInt(totalcount) == 0) {
                    $("#MyQuotes #do-questions").append(q1);
                    $("#MyQuotes #do-questions").append(q2);
                    $("#MyQuotes #do-questions").append(q3);
                    $("#MyQuotes #do-questions").append(q4);
                    $("#MyQuotes #do-questions").append(q5);
                    $("#MyQuotes #do-questions").append(q6);
                }
                else {
                    answerNeedAssign = false;

                    var ddd = '<p>Soruları Daha Önce Cevaplamışsınız!</p>';
                    ddd += '<a href="' + azureUrl + '" target="_blank">Cevapları Güncellemek İçin Tıklayınız..</a></th>';
                    $("#MyQuotes #do-questions").append(ddd);
                }

            },
            error: function (err) {
                console.log(err);
            }
        });

    }


    //get user credit cards
    $.ajax({
        url: azureUrl + handlerClause + '?fm=get-credit-cards-ce&id=' + userid,
        dataType: 'json',
        success: function (creditcards) {
            //console.log('creditcards');
            
            if (creditcards.length == 0) {
                var table = '<p>Kredi Kartı Bilgilerin Bulunamadı!</p>';
                table += '<a href="' + azureUrl + '" target="_blank">Hemen Kart Eklemek İçin Tıklayın</a></th>';
            }
            else {
                var table = '<p>Kredi Kartı Bilgilerin</p><table>';
                table += '<tr id="tableheader"><th>Banka Adı</th><th>Kart Limiti</th><th>Kart Borcu</th><th>Son Güncelleme Tarihi</th><tr>';

                $.each(creditcards, function (key, card) {
                    var currentdate = ConvertJsonDateString(card.updatedate);
                    table += '<tr><th>' + card.bankname + '</th><th>' + card.cardlimit + ' TL </th><th>' + card.carddebt + 'TL </th><th>' + currentdate + '<a href="' + azureUrl + '" target="_blank">Güncelle</a></th>';
                });

                table += '</table>';
            }
            $('#usercreditcards').html(table);
        },
        error: function (err) {
            console.log(err);
        }

    });

});

function ConvertJsonDateString(jsonDate) {  
    var shortDate = null;    
    if (jsonDate) {  
        var regex = /-?\d+/;  
        var matches = regex.exec(jsonDate);  
        var dt = new Date(parseInt(matches[0]));  
        var month = dt.getMonth() + 1;  
        var monthString = month > 9 ? month : '0' + month;  
        var day = dt.getDate();  
        var dayString = day > 9 ? day : '0' + day;  
        var year = dt.getFullYear();  
        shortDate = dayString + '/' + monthString + '/' + year;
    }  
    return shortDate;  
};


$("#btnDoCancel").click(function () {
    console.log("popup remove");
    $(".MyQuotesform").remove();
});

$("#btnDoRun").click(function () {
    console.log('#btnDoRun');
    var answ1, answ2, answ3, answ4, answ5, answ6, answList = [];

    if (answerNeedAssign) {
        var rb1 = document.querySelector('input[name="q1"]:checked');
        var rb2 = document.querySelector('input[name="q2"]:checked');
        var rb3 = document.querySelector('input[name="q3"]:checked');
        var rb4 = document.querySelector('input[name="q4"]:checked');
        var rb5 = document.querySelector('input[name="q5"]:checked');
        var rb6 = document.querySelector('input[name="q6"]:checked');

        //var rb3 = $('input[name="q3"]:checked').val();
        var validation = false;
        if (rb1 != null &&
            rb2 != null &&
            rb3 != null &&
            rb4 != null &&
            rb5 != null &&
            rb6 != null
            ) {
            answ1 = rb1.value;
            answ2 = rb2.value;
            answ3 = rb3.value;
            answ4 = rb4.value;
            answ5 = rb5.value;
            answ6 = rb6.value;
            validation = true;
        }

        answList.push({ answer: answ1 });
        answList.push({ answer: answ2 });
        answList.push({ answer: answ3 });
        answList.push({ answer: answ4 });
        answList.push({ answer: answ5 });
        answList.push({ answer: answ6 });
    }
    else {
        //already assigned. if wanna change. go website
        answList.push({ answer: '' });
        answList.push({ answer: '' });
        answList.push({ answer: '' });
        answList.push({ answer: '' });
        answList.push({ answer: '' });
        answList.push({ answer: '' });

        validation = true;
    }

    if (validation) {
        $("#MyQuotes").css("cssText", "width: 160px !important;").html(load);

        var dataToServer = {
            userid: userid,
            dataType: 'json',
            wishlist: JSON.stringify(wisharray),
            answerlist: JSON.stringify(answList),
        };

        console.log(dataToServer);

        $.ajax({
            url: azureUrl + handlerClause + '?fm=run-chrome-extension-control',
            data: dataToServer,
            success: function (res) {
                //console.log('success');
                //console.log(res.success);
               
                var imageIcon = chrome.extension.getURL('content/error.png');
                if (res.success) {
                    imageIcon = chrome.extension.getURL('content/ok.png');
                }
                var displaypart = '<div style="float:left !important;margin:5px; top: 230px;"><img style="width:60px;" src="' + imageIcon + '" /></div> <div style="float:left !important;margin:10px 0px 0px 0px !important; top: 260px;"><p class="reset-this" style="font-size: 20px;">' + res.message + '</p></div> <i style="clear:both"></i>';
                $("#MyQuotes").html(displaypart);
				
				
            },
            error: function (err) {
                console.log('error');
                $('#doFailMessage').text('Sistemde Hata Oluştu. Birazdan Tekrar Deneyiniz');
                $("#MyQuotes").html(error);
            },
            complete: function () {
                setTimeout(function () {
                    $("#MyQuotes").hide();
                    $(".MyQuotesform").remove();
					
					window.location = 'https://www.n11.com/sepetim/odeme-onayi';
                }, 4000);
				
				
            }
        });

        //$.ajax({
        //    url: azureUrl + handlerClause + '?fm=run-chrome-extension-control',
        //    data: dataToServer,
        //    method: 'POST',
        //    success: function (res) {
        //        console.log(res);
        //        console.log("api ajax success");
        //        $("#MyQuotes").html(complete);
        //    },
        //    error: function () {
        //        $("#MyQuotes").html(error);
        //    },
        //    complete: function () {
        //        setTimeout(function () {
        //            $("#MyQuotes").hide();
        //            $(".MyQuotesform").remove();
        //        }, 2000)
        //    }

        //});

        //$.ajax({
        //    url: azureUrl + "/api/quotes",
        //    data: quites,
        //    method: "POST",
        //    success: function () {
        //        console.log("api ajax success");
        //        $("#MyQuotes").html(complete);
        //    },
        //    error: function () {
        //        $("#MyQuotes").html(error);
        //    },
        //    complete: function () {
        //        setTimeout(function () {
        //            $("#MyQuotes").hide();
        //            $(".MyQuotesform").remove();
        //        }, 2000)
        //    }
        //})
    }
    else {
        $(".MyQuotesmessage").show().text("Lütfen Soruların Hepsini Cevaplayınız!").fadeOut(3000);
    };

});
