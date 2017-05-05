/*
chrome.extension.sendMessage({}, function(response) {
	var readyStateCheckInterval = setInterval(function() {
	if (document.readyState === "complete") {
		clearInterval(readyStateCheckInterval);

		// ----------------------------------------------------------
		// This part of the script triggers when page is done loading
		console.log("Hello. This message was sent from scripts/inject.js");
		// ----------------------------------------------------------

	}
	}, 10);
});


console.log('hakke ready');

var html = document.createElement('div');
    html.innerHTML = '<input id="clickMe" type="button" value="Click Me" style="width: 200px; height:300px; background: black; color:white;"/>';
document.body.appendChild(html);

document.getElementById('clickMe').addEventListener('click', function() {
	console.log('click you');
   // do stuff
});
*/

$(document).ready(function(){

  
   

    /*  /sepetim */
	var sepetimSatinAl = $('#stickyCartTotal > div > #buyButton');
	
	if(sepetimSatinAl !== null)
	{
		$('#stickyCartTotal > div').prepend("<span id='btnSepetimInjectionN11' class='button green big inicon'>Satın Al<span>")
		
		$('#btnSepetimInjectionN11').click(function(e){
		 
            //buraya basket gelecek
		    /* basketItemTable */
		    //console.log($('#basketItemTable').rows);
		    if (document.getElementById('basketItemTable') !== null) {
		        var basket = document.getElementById('basketItemTable');
		        var len = basket.rows.length;

		        var wishlist = [];

		        for (var i = 1; i < len ; i++) {
		            if ((i % 3) == 0) {
		                //console.log(basket.rows[i]);
		                var uname = '', ucount = 0, uprice = 0;
		                for (var j = 1; j < basket.rows[i].cells.length; j++) {
		                    var currCell = basket.rows[i].cells[j];
		                    if (j == 1) {
		                        //console.log(currCell.querySelector('.productDetail a'));
		                        //console.log(currCell.querySelector('.productDetail a h4').innerText);
		                        uname = currCell.querySelector('.productDetail a h4').innerText;
		                    }
		                    if (j == 3) {
		                        //console.log(currCell.querySelector('div input[name=cardItemQuantity]'));
		                        ucount = currCell.children[0].childNodes[3].value;
		                        //ucount = currCell.querySelector('input[type=hidden]').value;
		                    }
		                    if (j == 4) {
		                        uprice = currCell.querySelector('input[type=hidden]').value;
		                        //console.log(urunFiyati);
		                    }

		                    //console.log(currCell);
		                    //console.log(currCell.children);
		                    if (uname !== '' && ucount != 0 && uprice != 0) {
		                        var currItem = {
		                            name: uname,
		                            count: ucount,
		                            price: uprice
		                        };
		                        //console.log(currItem);
		                        wishlist.push(currItem);

		                        uname = '';
		                        ucount = 0;
		                        uprice = 0;
		                    }

		                }
		                //basket.rows[i].find('td').each(function () {
		                //    console.log($(this));
		                //});
		            }
		        }
		    }

		    chrome.runtime.sendMessage({ injector: "n11", wishlist: wishlist }, function (response) {
			   //console.log(response.farewell);
			});
		});
		
		console.log('not null');
		sepetimSatinAl.click(function(e){
			e.preventDefault();
						
			alert('Duyarlı.ol was here!');

		});
	}
	
	
});




/* 
localstorage => manifest.json permission add "storage"

chrome.storage.sync.set({'mykey':'myvalue'}, function(){
	alert('success');
});


window.onload = function(){
	document.getElementById('get').onclick = function(){
		chrome.storage.sync.get('mykey', function(data){
			alert(data.myLine); // return 'myvalue'
		});
	}
	
}


*/

/* create tab */
/*
	chrome.tabs.create({'url': 'http://duyarliol2.azurewebsites.net'}, callback);
	
	function callback(data){
		console.log(data.url);
	}
	

	
	manifest.json 
	"browser_action": {
		"default-title" : "Hallo!"
	}
	//background.js
	chrome.browserAction.onClicked.addListener(function(){
		chrome.tabs.create({'url': 'http://google.com'});
		//options.html i açmak
		//chrome.tabs.create({'url': chrome.extension.getURL('options.html')});
	
	});
	
*/



/*
var evt = document.createEvent('Event');
evt.initEvent('yourEventName', true, true);
var some_element = document.getElementById('clickMe');
some_element.dispatchEvent(evt);

document.addEventListener('yourEventName', function(e){
   //send message to ext
   console.log('yourEventName');
   var someInformation = "Click mi ye tıkladı. from n11.js"
   chrome.extension.sendMessage(someInformation, function(response) {
      //callback
   });
}, false);
*/