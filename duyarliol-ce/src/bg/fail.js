//var html = '<div id="MyQuotes" class="reset-this MyQuotesform"> <div style="display:block!important" class="reset-this">www.putnotes.net</div> <br /> <div style="color:red;display:block!important" class="reset-this">' + notLogin + '</div><br/>  <a style="display:block!important" class="reset-this"  href="http://www.putnotes.net">' + extPopupLogin + '.</a> '+

//'<input type="text" /><input type="button" name="btn" value="btn" id="hakkebtn" />'


//+'</div>';

var html = '<div id="MyQuotes" class="reset-this MyQuotesform"> <div style="display:block!important" class="reset-this">Duyarlı.ol</div> <br /> <div style="color:red;display:block!important" class="reset-this">Öncelikle Siteye Giriş Yapmalısın!</div><br/><a style="display:block!important" class="reset-this" target="_blank" id="doresetbutton" href="http://duyarliol.azurewebsites.net/">Giriş İçin Tıkla</a> </div>';


$("#MyQuotes").is(function () {
    $(".MyQuotesform").remove();
});

$("html body").append(html);


$('#doresetbutton').click(function () {
    location.reload();
});