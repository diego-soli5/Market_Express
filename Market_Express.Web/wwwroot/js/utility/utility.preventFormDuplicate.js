$(document).on('invalid-form.validate', 'form', function () {
    var button = document.querySelector('#btnSubmit');
    setTimeout(function () {
        button.disabled = false;
    }, 1);
});

$(document).on('submit', 'form', function () {
    var button = document.querySelector('#btnSubmit');
    setTimeout(function () {
        button.disabled = true;
    }, 0);
});
