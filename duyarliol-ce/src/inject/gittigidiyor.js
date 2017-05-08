
$(document).ready(function () {




    /*  /sepetim */
    //var sepetimSatinAl = $('#gg-cart-footer > .cart-buttons > .gg-w-push-12');

    var sepetimSatinAl = $('#gg-cart-footer .gg-w-24 .cart-buttons .gg-t-push-8 .row');

    if (sepetimSatinAl !== null) {
        //$('#gg-cart-footer .gg-w-24 .cart-buttons .gg-t-push-8 .row').prepend("<span id='btnSepetimInjectionGG' class='gg-d-24 gg-ui-btn gg-ui-btn-blue btn-pay'>Alışverişi Hakke</span>")

        $('#btnSepetimInjectionGG').click(function (e) {
            console.log('fsafafsaf');
            //buraya basket gelecek
            /* basketItemTable */
            //console.log($('#basketItemTable').rows);
        });

        console.log('not null');
        sepetimSatinAl.click(function (e) {
            e.preventDefault();

            //alert('Duyarlı.ol was here!');

            if (document.getElementsByClassName('product-items-container') !== null) {
                
                var childs = $('.product-items-container').children();
                var len = childs.length;
                var wishlist = [];

                //console.log(childs);
                var uname = '', ucount = 0, uprice = 0;

                for (var i = 1; i < len ; i++) {
                    var curr = childs[i];

                    if (curr.classList.contains('selected')) {

                        //console.log(curr);
                        uname = curr.querySelector('.title-link').title;
                        uprice = curr.querySelector('.total-price-box .total-price strong').innerText.split(' ')[0];


                        if (curr.querySelector('.auction-quantity-text') !== null) {
                            ucount = curr.querySelector('.auction-quantity-text').innerText.split(' ')[0];
                        }
                        else {
                            ucount = curr.querySelector('.number-selection input[type="text"]').value;
                        }
                    }

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
            }

            chrome.runtime.sendMessage({ injector: "gittigidiyor", wishlist: wishlist }, function (response) {
                //console.log(response.farewell);
            });
        });
    }


});
