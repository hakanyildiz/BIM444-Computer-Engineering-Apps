console.log("autodan gelddi:" + profilId);
$("#MyQuotesOwner").autocomplete({
    source: azureUrl + "/api/quotes/GetTagList/" + profilId,
    minLength: 1,
    search: function (event, ui) {
        $('#MyQuotesOwnerLoading').show();
        console.log("searching");
    },
    open: function (event, ui) {
        $('#MyQuotesOwnerLoading').hide();
        console.log("open");
    },
    close: function (event, ui) {
        console.log("close");
        $('#MyQuotesOwnerLoading').hide();
    },
    response: function (event, ui) {
        console.log(ui);
        $('#MyQuotesOwnerLoading').hide();
    }
});