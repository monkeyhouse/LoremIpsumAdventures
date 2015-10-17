/* these notice work with the $.ajax function eg.

    $.ajax({
        type: 'POST',
        url: "@Url.Action("ModifyFavorite")",
        data: { genreId: genreId, actionId: action },
        beforeSend: function (jqXHR, settings) {
            jqXHR.url = settings.url;
        }
    })
    .error(function (jqXHR, exception) {
        // show error notice
        Up.ErrorNotice(jqXHR, exception);

        //revert state
        el.prop('checked', !isChecked);
    })
    .success(Up.SuccessNotice);

*/

//global setting for before send
$(function () {
    $.ajaxSetup({
        beforeSend: function (jqXHR, settings) {
            jqXHR.url = settings.url;
        }
    });
})


/* mini-lib Up to work with toatr */
Up = (function(){
    
     var s = function (response) {
        var message = response.Message;
        toastr.success(message, 'Success');
    }

    var e = function(jqXHR) {
        var title = jqXHR.statusText;
        var message = jqXHR.statusText;

        if (jqXHR.status == 404) {
            message = "Page or Url Not found";
            message += "<br>" + jqXHR.url;
        } else {

            try {
                //try this
                var obj = jqXHR.responseJSON;
                message = obj.Message;
            }
            catch (e) {

                //todo : handle unable to connect case
            }
        }
        toastr.error(message, 'Error');
    }

    return {
        SuccessNotice : s,
        ErrorNotice : e
    };
})();


//Prototype polyfills
if (!String.prototype.includes) {
    String.prototype.includes = function () {
        'use strict';
        return String.prototype.indexOf.apply(this, arguments) !== -1;
    };
}

//includes case insensitive
String.prototype.includesI = function () {
    'use strict';
    var z = arguments[0].toUpperCase();
    var a = this.toUpperCase();
    return a.indexOf(z) !== -1;
};