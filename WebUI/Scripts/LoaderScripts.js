function showPopupLogin() {
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
        message: '<img src="../Content/assets/img/gear.gif" width="31" /> Авторизация. Пожалуйста, подождите...'
    });
};