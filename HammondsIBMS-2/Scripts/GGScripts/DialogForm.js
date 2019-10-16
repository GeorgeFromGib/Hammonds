
jQuery(document).ready(function () {

    // Don't allow browser caching of forms
    $.ajaxSetup({ cache: false });

    // Wire up the click event of any current or future dialog links
    $('.dialogLink').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var updadeUrlIndex = element.attr('data-update-url-index');
        var onSuccessClientFunction = element.attr('data-onSuccessfunction');
        var action = element.attr('data-action');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000);
        var dialogDiv = "<div id='" + dialogId + "' class='dialog-content' ></div>"; //class='dialog-content' ><div style='width:300px;height:300px'><div class='window-loading t-loading' style='position:relative;top:150px;left:150px;width:16px;height:16px;'> </div></div></div>";
        var windowName = dialogId + "_window";

        var windowElement = $.telerik.window.create({
            title: dialogTitle,
            html: dialogDiv,
            contentUrl: '',
            modal: true,
            resizable: false,
            draggable: true,
            scrollable: false,
            visible: false,
            onActivate: function () {
                 ClearModalWindowSize(); },
            onClose: function () { /*ClearWindowContent(); */}

        });
        windowElement.attr('id', windowName);

        var diagW = $('#' + windowName).data('tWindow');

        ClearWindowContent();

        $(document).ajaxStart($.blockUI({timeDelay:200})).ajaxStop($.unblockUI);
        // Load the form into the dialog div
        $('#' + dialogId).load(this.href, function () {
            diagW.center().open();



            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            $('a.form-cancel').click(function () {
                diagW.close();
                //ClearWindowContent();
            });
            
            // Called Function must return true or false
            $('a.form-close').click(function (e) {
                debugger;
                var func = $(this).attr('data-func');
                if (func != null || func == "") {
                    if (new Function(func))
                        diagW.close();
                    return;
                }
                diagW.close();
            });

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl, updadeUrlIndex, diagW, onSuccessClientFunction, action);
        });
        return false;
    });
});

function ClearWindowContent() {

    $('div.dialog-content').html(""); //<div style='width:300px;height:300px'><div class='window-loading t-loading' style='position:relative;top:150px;left:150px;width:16px;height:16px;'> </div></div>
}

function wireUpForm(dialog, updateTargetId, updateUrl, updadeUrlIndex, window, onSuccessClientFunction,urlAction) {
    $('form', dialog).submit(function () {

        // Do not submit if the form
        // does not pass client side validation
        if (!$(this).valid())
            return false;

        // Client side validation passed, submit the form
        // using the jQuery.ajax form
        var dta = $(this).serialize();
        $.ajax({
            url: this.action,
            type: this.method,
            data: dta,
            success: function (result) {
                // Check whether the post was successful
                if (result.success) {
                    // Close the dialog 
                    window.close();
                    //ClearWindowContent();
                    if (updadeUrlIndex != "" && updadeUrlIndex != null) {
                        if (result.idx == null)
                            alert("Index Value not supplied for " + updadeUrlIndex + ". Use Json(new {success=true, idx=<value>})");
                        updateUrl = updateUrl + '/' + result.idx;
                    }

                    if (urlAction != "") {
                        var compAction = urlAction;
                        if (updadeUrlIndex != "")
                            compAction = compAction + '/' + result.idx;
                        $(location).attr('href', compAction);
                        return;
                    }

                    // Reload the updated data in the target div
                    $(updateTargetId).load(updateUrl, function () {
                        //RunFormating();
                        $(this).trigger('formDialogSuccess');

                    });

                    if (onSuccessClientFunction != null)
                        eval(onSuccessClientFunction + "('" + dta + "')");

                } else {
                    // Reload the dialog to show model errors  
                    $('div.dialog-content').html(' ');
                    $('div.dialog-content').html(result);
                    //$(dialog).html(result);

                    // Enable client side validation
                    $.validator.unobtrusive.parse(dialog);

                    $('a.form-cancel').click(function () {
                        window.close();
                        //ClearWindowContent();
                    });

                    // Called Function must return true or false
                    $('a.form-close').click(function(e) {
                        var func = $(e).attr('data-func');
                        if(func!=null || func=="") {
                            if (eval(func))
                                window.close();
                            return;
                        }
                        window.close();
                    });
                    // Setup the ajax submit logic
                    ClearModalWindowSize();
                    wireUpForm(dialog, updateTargetId, updateUrl, updadeUrlIndex, window, onSuccessClientFunction, urlAction);
                }
            }
        });
        return false;
    });
}
