$(document)
    .ready(clickDate)
    .ready(clickDtTo)
    .ready(clickDtTo)
    .ready(clickDateFrom)
    .ready(clickDateTo)
    .ready(clickOrderDate);
;

function clickDate() {
    $('#PayingItem_Date').datepicker({
        language: 'ru'
    });
};

function clickDtFrom() {
    $('#DtFrom').datepicker({
            language: 'ru'
        });
    };

function clickDtTo() {

    $('#DtTo').datepicker({
        language: 'ru'
        });
    };

function clickDateFrom() {
    $('#DateFrom').datepicker({
        language: 'ru'
    });
};

function clickDateTo() {

    $('#DateTo').datepicker({
        language: 'ru'
    });
};

function clickOrderDate() {
    $('#OrderDate').datepicker({
        language: 'ru'
    });
};