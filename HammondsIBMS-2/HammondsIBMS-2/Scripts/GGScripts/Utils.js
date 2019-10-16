/* File Created: May 19, 2012 */

function AjaxUpdate(urlAction, targetIdToUpdate) {
    
    $.get(urlAction, function (data) {
        $(fixId(targetIdToUpdate)).html(data);
    });
}

var fpFix = function (n) {
    return Math.round(n * 100) / 100;
};

Date.parseJSON = function (jsonDate) {
    var dte = eval(jsonDate.replace(/\/Date\(([-*\d]+)(?:-\d+)?\)\//i, "new Date($1)"));

    return dte;

};

function fixId(id) {
    var fid;
    if (id == null || id == '')
        return '';
    
    if (id.substr(0, 1) != '#')
        fid = '#' + id;
    else 
        fid = id;

    return fid;
}

function RebindTelerikGrid(gridName) {
    var grid = $(fixId(gridName)).data('tGrid');
    grid.ajaxRequest();
}

function GridCheckBoxRepositoryHandler(divRepositoryId, gridId, listField, fieldId,onChangeFunction) {

    var selectedIds = [];


    var selRepository = $(fixId(divRepositoryId));
    var gridCheckBox = $(fixId(gridId));


    $(gridCheckBox).off();
    $(gridCheckBox).on('change',':checkbox', function () { onCheckBoxChanged(this); });

    function onCheckBoxChanged(that) {
        var $check = $(that);
        var id = $check.val();
        if ($check.is(':checked')) {
            //add id to selectedIds. 
            selectedIds.push(id);
            _OnChange(id, true);
        } else {
            //remove id from selectedIds. 
            _OnChange(id, false);
            selectedIds = $.grep(selectedIds, function (item, index) {
                return item != id;
            });
        }
        populateSelectRepository();
    }
   
    function _OnChange(id, status) {
        if (onChangeFunction != null) {
            onChangeFunction(id, status);
        }
    }

    this.updateGridOnDataBound = function() {
        //restore selected checkboxes. 
        var chks = $(fixId(gridId + ' :checkbox'));
        $(chks).each(function() {
            //set checked based on if current checkbox's value is in selectedIds. 
            $(this).prop('checked', jQuery.inArray($(this).val(), selectedIds) > -1);
        });
    };

    function populateSelectRepository() {
        $(selRepository).empty();
        for (var i = 0, len = selectedIds.length; i < len; i++) {
            $(selRepository).append('<input type="hidden" id="' + listField + '[' + i + '].' + fieldId + '" name="' + listField + '[' + i + '].' + fieldId + '" value="' + selectedIds[i] + '"/>');
        }
    }
}

function getTelerikNumericTextBox(textBoxName) {
    var id = hashFix(textBoxName);
    var ctl = $(id).data("tTextBox");
    return ctl;
}

function getTelerikComboBox(comboBoxName) {
    var id = hashFix(comboBoxName);
    var ctl = $(id).data("tComboBox");
    return ctl;
}

function getTelerikDropDownList(dropDownListName) {
    var id = hashFix(dropDownListName);
    var ctl = $(id).data("tDropDownList");
    return ctl;
}


function DisplayJsonErrorInSummary(xhr) {
    var error = $.parseJSON(xhr.responseText);
    ClearErrorSummary();
    $('div.validation-summary-errors:first').append('<ul><li>' + error.error + '</li></ul>');
    //$('form').prepend('<ul class="validation-summary-errors"><li>' + error.error + '</li></ul>');
}

function ClearErrorSummary() {
    $('div.validation-summary-errors').empty();
    //$('.validation-summary-errors').remove();
}

function parseCurrency(value) {
    var num = parseFloat(value.replace(/[^\d.]+/g, ""));
    return isNaN(num) ? 0 : num;
}