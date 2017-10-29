function clickDate() {
    $('#PayingItem_Date').datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1
    });
    $('#PayingItem_Date').on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });
};



function clickDtFrom() {
    $('#DtFrom').datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1
    });
    $('#DtFrom').on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });
};

function clickDtTo() {

    $('#DtTo').datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1
    });
    $('#DtTo').on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });
};

function clickDateFrom() {
    $('#DateFrom').datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1
    });
    $('#DateFrom').on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });
};

function clickDateTo() {
    $('#DateTo').datepicker({
        format: "dd.mm.yyyy",
        weekStart: 1
    });
    $('#DateTo').on('changeDate', function (ev) {
        $(this).datepicker('hide');
    });
};