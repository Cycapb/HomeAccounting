function showSubcategoriesCheckBoxClick() {
    $('#showSubcategoriesCheckBox').change(function () {
        if (this.checked) {
            if ($('#sumInput').val() === '') {
                $('#sumInput').val('0,00');
            }
            $('#divProducts').removeAttr('hidden');
            $('#divPayingItemSum').attr('hidden', true);
        } else {
            $('#divProducts').attr('hidden', true);
            $('#divPayingItemSum').removeAttr('hidden');
        }
    });
}