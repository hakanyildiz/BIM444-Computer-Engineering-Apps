

$(document).ready(function(){
	/*  /sepetim */
	var sepetimSatinAl = $('#BasketFooter > .BasketButtons > #Continue');
	
	if(sepetimSatinAl !== null)
	{
		$('#BasketFooter > .BasketButtons > .gg-m-24').prepend("<span class='button_blue1 button_size3 gg-btn' id='btnSepetimInjectionGG'>Hakke Al<span>")
		
		
		$('#btnSepetimInjectionGG').click(function(e){
			chrome.runtime.sendMessage({injector: "gittigidiyor"}, function(response) {
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


