//var html = '<div id="MyQuotes" class="reset-this MyQuotesform"> <div style="display:block!important" class="reset-this">www.putnotes.net</div> <br /> <div style="color:red;display:block!important" class="reset-this">' + notLogin + '</div><br/>  <a style="display:block!important" class="reset-this"  href="http://www.putnotes.net">' + extPopupLogin + '.</a> '+

//'<input type="text" /><input type="button" name="btn" value="btn" id="hakkebtn" />'


//+'</div>';

var html = '<div id="MyQuotes" class="reset-this MyQuotesformFail"><img style="width:64px;" src="' + chrome.extension.getURL('icons/icon128.png') + '" /><div style="color:red;display:block!important;font-size: 20px;" class="reset-this">Sitemizden Çıkış Yapmışsınız!</div><br/><a style="display:block!important;background-color: #4286f4;color: white;max-width: 250px;border-radius: 5px;padding: 4px 5px;" class="reset-this" target="_blank" id="doresetbutton" href="http://duyarliol.azurewebsites.net/">İşlemlere Devam Edebilmek İçin Giriş Yapınız</a> </div>';


$("#MyQuotes").is(function () {
    $(".MyQuotesform").remove();
});

$("html body").append(html);


$('#doresetbutton').click(function () {
    location.reload();
});