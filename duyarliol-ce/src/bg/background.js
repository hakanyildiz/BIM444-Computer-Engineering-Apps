// if you checked "fancy-settings" in extensionizr.com, uncomment this lines

// var settings = new Store("settings", {
//     "sample_setting": "This is how you use Store.js to remember values"
// });


//example of using a message handler from the inject scripts
/*
chrome.extension.onMessage.addListener(
  function(request, sender, sendResponse) {
  	//chrome.pageAction.show(sender.tab.id);
    //sendResponse();
	
	console.log(request);
	
	
});
 */
 
 
var cookie = {
    url: "http://duyarliol.azurewebsites.net/",
    name: "duyarliol"
};


function Success(profilId, wishlist,sitename,redirecturl) {

    chrome.tabs.insertCSS(null, { file: "src/bg/popup.css" }, function () {
        console.log("popup.css load");
    });

    chrome.tabs.insertCSS(null, { file: "js/jquery-ui/jquery-ui.min.css" }, function () {
        console.log("ui.css load");
    });

    chrome.tabs.executeScript(null, { file: "js/jquery/jquery.js" }, function () {
        console.log("jquery.js load")
    });

    chrome.tabs.executeScript(null, { file: "js/jquery-ui/jquery-ui.min.js" }, function () {
        console.log("jquery-ui.js load")
    });

    var sendingdata = 'var userid="' + profilId + '";var sitename = "' + sitename + '";var redirecturl = "'+redirecturl+'";var wisharray = [';

    console.log('sending data');
    console.log(sendingdata);

    for (var ll = 0; ll < wishlist.length; ll++) {
        sendingdata += '{ name: "' + wishlist[ll].name + '", count: "' + wishlist[ll].count + '", price: "' + wishlist[ll].price + '" },';
    }
    sendingdata += ']';

    chrome.tabs.executeScript(null, { 
        code: sendingdata
    }, function () { 

        chrome.tabs.executeScript(null, { file: "src/bg/popup.js", runAt: "document_start", }, function () {
            console.log("popup.js laod");

            //var veri = "'";
            ////veri += info.selectionText.replace(/['"]+/g, '');
            //veri += 'ürünün detayları'
            //veri += "'";


            //var id = "'" + profilId + "'";
            //var info = "";
            //var url = "'" + info.pageUrl + "'";

            //var code = 'var text = document.getElementById("myQuotesText"); profilId=' + id + ';' + "url=" + url + ";";

            //code += "text.value=" + veri;
            //console.log(code);
            //chrome.tabs.executeScript({}, function () {
            //    chrome.tabs.executeScript(null, { file: "src/bg/auto.js", runAt: "document_start", }, function () { });
            //});
        });

    });
}

function Fail() {
    console.log("GнRно YAPILMADI")
    chrome.tabs.insertCSS(null, { file: "src/bg/popup.css" }, function () {
        console.log("popup.css load");
    });
    chrome.tabs.executeScript(null, { file: "js/jquery/jquery.js" }, function () {
        console.log("jquery.js load")
    });
    chrome.tabs.executeScript(null, { file: "src/bg/fail.js" }, function () {
        console.log("fail.js load");
    });
}

function deneme(){
	console.log('deneme');
}

chrome.runtime.onMessage.addListener(
  function(request, sender, sendResponse) {
    if (request.injector == "n11"){
		console.log('n11den istek geldi');
		console.log(request.wishlist); //istek listesi okundu
		creation(request.wishlist, 'n11', '');
	    //sendResponse({farewell: "goodbye"});
    }

    if (request.injector == "gittigidiyor") {
        console.log('gittigidiyor istek geldi');
        console.log(request.wishlist); 
        creation(request.wishlist, 'gittigidiyor', '');
    }

    if (request.injector == "mediamarkt") {
        console.log('mediamarkt istek geldi');
        console.log(request.wishlist);
        creation(request.wishlist, 'mediamarkt', request.redirecturl);
    }
  });
  
function creation(wishlist, sitename, redirecturl) {
		 chrome.cookies.get(cookie, function (c) {
                    console.log(c);
                    if (c != null) {
                        var id = c.value.split("=")[1];
                        if (id != null) {
                            Success(id,wishlist,sitename,redirecturl);
                        }
                        else {
                            Fail();
                        }
                    }
                    else {
                        Fail();
                    }
                });
  
  }
  
  chrome.browserAction.onClicked.addListener(function(){
	  var information = "12345";
	  
	  chrome.tabs.executeScript(null, {  //null olan kısm TABID, null yaparsan default acılan sayfada çalışır sadece..
		  code : 'var information = "'+ information +'";'
	  }, function(){ //callback function 
		 chrome.tabs.executeScript(null, {file: 'src/bg/executedeneme.js'}); 
	  });
	  
  });
  
  
  
  
  