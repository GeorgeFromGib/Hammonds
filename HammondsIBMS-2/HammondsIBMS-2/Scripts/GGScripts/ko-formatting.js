ko.bindingHandlers.date = {
    update: function (element, valueAccessor, allBindingsAccessor) {
        var value = valueAccessor(), allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);

        var d = "";
        if (valueUnwrapped) {
            var m = /Date\([\d+-]+\)/gi.exec(valueUnwrapped);
            if (m) {
                d = String.format("{0:dd/MM/yyyy}", eval("new " + m[0]));
            }
        }
        $(element).text(d);
    }
};
//ko.bindingHandlers.money = {
//    update: function (element, valueAccessor, allBindingsAccessor) {
//        var value = valueAccessor(), allBindings = allBindingsAccessor();
//        var valueUnwrapped = ko.utils.unwrapObservable(value);
//        var m = accounting.formatMoney(0);
//        if (valueUnwrapped) {
//            m = parseInt(valueUnwrapped);
//            if (m) {
//                m = accounting.formatMoney(m);
//            } else {
//                m = accounting.formatMoney(0);
//            }
//        }

//        $(element).text(m);
//    }
//};

//ko.bindingHandlers.money = {
//    init: function (element, valueAccessor) {
//        var observable = valueAccessor(),
//            formatted = ko.computed({
//                read: function (key) {
//                    debugger;
//                    //return '$' + (+observable()).toFixed(2);
//                    return accounting.formatMoney(+observable());
//                },
//                write: function (value) {
//                    debugger;
//                    value = parseFloat(value.replace(/[^\.\d]/g, ""));
//                    observable(isNaN(value) ? null : value); // Write to underlying storage 
//                },
//                disposeWhenNodeIsRemoved: element
//            });

//        //apply the actual value binding with our new computed
//        ko.applyBindingsToNode(element, { value: formatted });
//    }
//};

ko.bindingHandlers.money = {
    init: function (elm, valueAccessor) {
        $(elm).change(function () {
            valueAccessor()(parseCurrency(elm.value));
        }).addClass('money');
    },
    update: function (elm, valueAccessor, allBindingsAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        $elm = $(elm),
        method = $elm.is(":input") ? "val" : "html";

        $elm[method](accounting.formatMoney(value)).toggleClass('negative', value < 0);
    }
};