function confirmDelete(unique, fromDeleteButton) {
    var deleteSpan = 'deleteSpan_' + unique;
    var confirmDeleteSpan = 'deleteConfirmSpan_' + unique;
    console.log(confirmDeleteSpan);
    console.log(deleteSpan);
    if (fromDeleteButton) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}