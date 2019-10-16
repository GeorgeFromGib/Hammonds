/* File Created: May 18, 2012 */


function RegisterMenu(menuid,menuAction,onSelectedFunction) {
    $('#' + menuid + ' a').live('click', function (event, e) {
        event.preventDefault();
        $('#' + menuid + ' li').removeClass('active');
        $(this).parent('li').addClass('active');

        var idx = $(this).attr('data-parent');

        $.post(menuAction, { id: idx });

        if (onSelectedFunction != null)
            eval(onSelectedFunction + '(' + idx + ')');

        var href = $(this).attr('href');
        if (href != '')
            $(location).attr("href", href);
    });
}


