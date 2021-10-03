var frmChangePass = document.querySelector("#frmChangePass");
var txtCurrentPass = document.querySelector("#currentPass");
var txtNewPass = document.querySelector("#newPass");
var txtNewPassConfirmation = document.querySelector("#newPassConfirmation");

var btnChangeAlias = document.querySelector("#btnChangeAlias");

frmChangePass.addEventListener("submit", function (e) {
    e.preventDefault();

    if (!(txtCurrentPass.value !== "" && txtNewPass.value !== "" && txtNewPassConfirmation.value !== "" && txtNewPass.value === txtNewPassConfirmation.value)) {
        return;
    }

    const url = "/Account/ChangePassword";
    const body = new FormData(frmChangePass);

    fetch(url, { body: body, method: 'POST' })
        .then(response => response.json())
        .then(json => {
            popUp(json.success, json.message);

            if (json.success) {
                $("#modalChangePassword").modal("hide");
                txtCurrentPass.value = "";
                txtNewPass.value = "";
                txtNewPassConfirmation.value = "";
            }
        });
});

btnChangeAlias.addEventListener("click", function (e) {
    const url = "/Account/GetUserAlias";
    const body = new FormData(frmChangePass);

    fetch(url, { body: body, method: 'GET' })
        .then(response => response.text())
        .then(text => $("#txtAlias").val(text));
});
