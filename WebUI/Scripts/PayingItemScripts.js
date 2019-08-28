function showSubcategoriesCheckBoxClick() {
    $('#showSubcategoriesCheckBox').change(function () {
        if (this.checked) {
            $('#divProducts').removeAttr('hidden');
            $('#divPayingItemSum').attr('hidden', true);
        } else {
            $('#divProducts').attr('hidden', true);
            $('#divPayingItemSum').removeAttr('hidden');
        }
    });
}