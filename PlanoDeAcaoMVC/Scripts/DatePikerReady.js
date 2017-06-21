var enviar = {};
var start = moment();
var end = moment();

var FullCallendar = {

    objFull: {
        startDate: start,
        endDate: end,
        opens: "left",
        alwaysShowCalendars: true,
        showDropdowns: true,
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        }
    },

    cb: function (start, end, element) {
        $('#reportrange span').html(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
    },

    IntiFullCallendarById: function (idElement) {
        $('#' + idElement).daterangepicker(FullCallendar.objFull, FullCallendar.cb);
        FullCallendar.cb(start, end, $('#' + idElement));
    },

    IntiFullCallendarByClass: function (nameElement) {

        $('.' + nameElement).daterangepicker(FullCallendar.objFull, FullCallendar.cb);
        FullCallendar.cb(start, end, $('.' + nameElement));
    },

};

var SimpleCallendar = {
    
    objSimple: {
        autoUpdateInput: false,
        autoApply: true,
        showDropdowns: true,
        startDate: start,
        opens: "left",
        endDate: end,
        locale: {
            cancelLabel: 'Clear'
        }
    },

    IntiFullCallendarByClass: function (className) {
    
        $('.' + className).daterangepicker(SimpleCallendar.objSimple, function (start, end, label) {
            console.log("New date range selected: ' + start.format('MM/DD/YYYY') + ' to ' + end.format('MM/DD/YYYY') + ' (predefined range: ' + label + ')");
        });

        $('.' + className).on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY') + ' - ' + picker.endDate.format('MM/DD/YYYY'));
        });
        $('.' + className).on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
            enviar['startDate'] = "";
            enviar['endDate'] = "";
        });
    },
}


var SingleCallendar = {

    objSingle: {
        autoUpdateInput: false,
        singleDatePicker: true,
        autoApply: true,
        showDropdowns: true,
        startDate: start,
        opens: "left",
        locale: {
            cancelLabel: 'Clear'
        }
    },

    IntiFullCallendarByClass: function (className) {

        $('.' + className).daterangepicker(SingleCallendar.objSingle, function (start, end, label) { });

        $('.' + className).on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY'));
        });
        $('.' + className).on('cancel.daterangepicker', function (ev, picker) {
            $(this).val('');
        });
    },
}