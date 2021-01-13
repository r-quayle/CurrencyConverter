window.onload = function () {
    $("#convertbutton").click(convertamount);
};

function convertamount() {
    var from = $("#FromCurrency").val();
    var to = $("#ToCurrency").val();
    var amount  = $("#amounttoconvert").val();
    var request = $.ajax({
        url: "home/index/",
        method: "POST",
        data: {
            fromCurrency: from,
            toCurrency: to,
            amount: amount
        }
    });
    request.done(function (data) {
        $("#convertedamount").html(data);
    });

    request.fail(function (jqXHR, textStatus) {
        alert("Request failed: " + textStatus);
    });
}