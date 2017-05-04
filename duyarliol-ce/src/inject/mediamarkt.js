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


*/

$(document).ready(function(){
	

	/*  /sepetim */
	var sepetimSatinAl = $('#js-cart-app > .cobuttons > .cogo-checkout');
	if(sepetimSatinAl !== null)
	{	
		console.log(sepetimSatinAl);
		
		$('#js-cart-app > .cobuttons > .cogo-checkout > .cocheckout-actions').prepend("<a id='btnSepetimInjectionMM' class='cobutton cobutton-next'><span>Hakke</span><a>")
		
		
		$('#btnSepetimInjectionMM').click(function(e){
			chrome.runtime.sendMessage({injector: "hepsiburada"}, function(response) {
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