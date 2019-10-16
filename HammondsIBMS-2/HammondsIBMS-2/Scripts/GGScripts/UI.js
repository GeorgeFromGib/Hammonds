/* File Created: May 21, 2012 */

/* Function to open ActionWindow windows on click */
jQuery(document).ready(function () {
    $('.actionWindow').live('click', function (e) {
        var wdw = $('#' + this.id + 'Window').data("tWindow");
        var controller = $(wdw.element).attr('data-action');
        var data = $(wdw.element).attr('data-values');
        var cont = $('.window-contents', wdw.element);
        $('.window-loading').show();
        cont.hide();
        wdw.center().open();
        cont.load(controller, data, function () {
            $('.window-loading').hide();
            cont.show(20);
        });
        _ggCurrentWindowId = this.id + 'Window';
    });

    $.fn.extend({
        UpdateRate: function (newRate) {
            if ($(this).is('.foreignCurrency')) {
                $(this).attr('data-exhrate', newRate);
                var id = $(this).attr('id');
                var hObj = $('input#' + id.replace('_CTR', ''));
                var val = $(hObj).val();
                $(this).html('(£' + parseFloat(val / newRate).toFixed(2) + ')');
            }
        }
    });

    //Setup buttons with this class to call CloseIfInWindow Function
    $('.CloseIfInWindow').live('click', function (e) {
        CloseIfInWindow(e);
    });

    $('.focus :input').focus();
});

function CloseIfInWindow(e) {
    var wdw = $('#' + _ggCurrentWindowId).data("tWindow");
    if (wdw != null)
        wdw.close();

}

var _ggCurrentWindowId;
var _ggScriptUFCTimeOut;
var _ggScriptUFCTimeOutOldValue;

function UpdateForeignCurrency(e) {
    var val = e.newValue;
    if (_ggScriptUFCTimeOutOldValue == null) _ggScriptUFCTimeOutOldValue = e.oldValue;
    var divEl = $('#' + this.id + "_CTR");
    var ctl = this;
    var exRte = divEl.attr('data-exhrate');
    divEl.html('(£' + parseFloat(val / exRte).toFixed(2) + ')');

    clearTimeout(_ggScriptUFCTimeOut);
    _ggScriptUFCTimeOut = setTimeout(function () {
        $(divEl).trigger('change', { oldValue: _ggScriptUFCTimeOutOldValue, newValue: e.newValue });
        $(ctl).trigger('change', { oldValue: _ggScriptUFCTimeOutOldValue, Value: e.newValue });
        _ggScriptUFCTimeOutOldValue = null;
    },
        500);

}

function UpdateForeignCurrencyExchangeRate(id, newRate) {
    debugger;
    var divEl = $('#' + id);
    divEl.attr('data-exhrate', newRate);
    divEl.html('(£' + parseFloat(val / newRate).toFixed(2) + ')');
}

var _ggScriptMUTimeOut;
var _ggScriptMUOldValue;

function UpdateNumerical(e) {
    if (_ggScriptMUOldValue == null) _ggScriptMUOldValue = e.oldValue;
    var divEl = $('#' + this.id + "_CTR");
    var ctl = this;
    clearTimeout(_ggScriptMUTimeOut);
    _ggScriptMUTimeOut = setTimeout(function () {
        $(divEl).trigger('change', { oldValue: _ggScriptMUOldValue, newValue: e.newValue });
        $(ctl).trigger('change', { oldValue: _ggScriptUFCTimeOutOldValue, Value: e.newValue });
        _ggScriptMUOldValue = null;
    },
        500);
}

function UpdateInteger(e) {
    if (_ggScriptMUOldValue == null) _ggScriptMUOldValue = e.oldValue;
    var ctl = this;
    clearTimeout(_ggScriptMUTimeOut);
    _ggScriptMUTimeOut = setTimeout(function () {
        $(ctl).trigger('change', { oldValue: _ggScriptMUOldValue, value: e.newValue });
        _ggScriptMUOldValue = null;
    },
        500);
}

function DropdownChange(e) {
    $(this).trigger('change', { value: e.value });
}

function UpdateDate(e) {
    $(this).trigger('change', { value: e.value });
}

function AnimHide(el) {
    $(el).slideUp('fast');
}

function AnimShow(el) {
    $(el).slideDown('fast');
}

function ChangeImage(imgEl, path) {
    var de = $(imgEl);

    de.fadeOut('fast', function () {
        de.attr("src", path);
    });
    de.fadeIn('fast');
}

function ClientButtonFunction(caller) {
    if ($(caller).attr('disabled')!='disabled') {
        var funcToCall = $(caller).attr('data-funcToCall');
        eval(funcToCall);
    }
}

function EnableButton(id, enable) {
    id = hashFix(id);
    if (enable)
        $(id).removeAttr('disabled');
    else {
        $(id).attr('disabled', 'disabled');
    }

}

function hashFix(id) {
    if (id.substr(0, 1) != '#')
        id = '#' + id;
    return id;
}

function ClearModalWindowSize() {
    var winContent = $('.t-window-content');
    winContent.css("width", "");
    winContent.css("height", "");
}

function UpdateProgressBar(id, value) {
    id = hashFix(id);
    var valStr = 'width:' + value + '%';
    $(id).attr('style', valStr);
}

function ChangeBoolTextToIcon() {
    var classId = '.bool-text-to-icon';
    $(classId+":contains('true')").html('<i class="icon-check"></i>').attr('style', 'text-align:center');
    $(classId+":contains('false')").html('').attr('style', 'text-align:center');
}

jQuery.fn.htmlChange = function (string) {
    var o = $(this[0]); // It's your element
    $(o).html(string);
    $(o).effect("highlight");
    $(o).trigger('change', string);
};

function CheckBoxValue(element) {
    return GetCheckBoxValue(element);
}

function GetCheckBoxValue(element) {
    return $(element + ':checked').val();
}



