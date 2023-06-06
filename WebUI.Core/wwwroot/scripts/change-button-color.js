    function changeButtonColor(id) {
        $("#" + id).addClass("btn btn-primary").siblings().removeClass("btn-primary");
        return false;
    }