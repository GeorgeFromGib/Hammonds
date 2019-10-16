/* File Created: May 22, 2012 */



$(function () {
    $('.form-filter').submit(function (event) {
        event.preventDefault();
        var func = $(this).attr('data-onchangefunction');
        var dta = $(this).serialize();
        $.ajax({
            url: this.action,
            type: this.method,
            data: dta,
            success: function (result) {
                if (func != null && func != "")
                    eval(func + '()');
            }
        });
        _FilterOn(this);
    });

    $('.filter-reset').click(function (event) {
        event.preventDefault();
        var action = $(this).attr('data-resetAction');
        var idToUpdate = $(this).attr('data-idToUpdate');
        var func = $(this).attr('data-onchangefunction');
        $.get(action, function (data) {
            $('#' + idToUpdate).html(data);
            if (func != null && func !="")
                eval(func + '()');
            _FilterOff();
        });

    });

    if( $('.form-filter').find('#IsFiltered').val()=='True') {
        _FilterOn($('.form-filter'));
    }
    

});


function _FilterOn(element) {
    $(element).find(':submit').addClass('btn-warning');
}

function _FilterOff(element) {
    $(':submit.btn-warning').removeClass('btn-warning');
}
