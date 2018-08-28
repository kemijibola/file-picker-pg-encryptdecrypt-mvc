function Generic() {

}


Generic.prototype.ajaxCall = function (url, verb, _data, callback)
{
    $.ajax({
        cache: false,
        type: verb,
        url: url,
        data: _data,
        processData: false,
        contentType: false,
        success: function (response) {
            callback(null, response);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(thrownError)
            callback("Failed to process request", null);
        }
    });
}