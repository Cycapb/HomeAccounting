﻿function showPopupLogin() {
    $.blockUI({
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        },
        message: '<img src="/assets/img/Gear.gif" width="31" /> Пожалуйста, подождите...'
    });
};

function checkForErrorsAndRedirect(result) {
    if (result.url) {
        window.location.replace(result.url);
    } else if (hasErrors) {
        $.unblockUI();
    }
}

function hidePopup() {
    $.unblockUI();
}

function redirectToMain() {
    window.location.replace("/");
}