var frmChangePass = document.querySelector("#frmChangePass");
var txtCurrentPass = document.querySelector("#currentPass");
var txtNewPass = document.querySelector("#newPass");
var txtNewPassConfirmation = document.querySelector("#newPassConfirmation");

var btnChangeAlias = document.querySelector("#btnChangeAlias");
var frmChangeAlias = document.querySelector("#frmChangeAlias");

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

    fetch(url, { method: 'GET' })
        .then(response => response.text())
        .then(text => $("#txtAlias").val(text));
});

frmChangeAlias.addEventListener("submit", function (e) {
    e.preventDefault();

    if ($("#txtAlias").val().trim() == "") {
        return;
    }

    const url = "/Account/ChangeAlias";
    const body = new FormData(frmChangeAlias);

    fetch(url, { body: body, method: 'POST' })
        .then(response => response.json())
        .then(json => {
            popUp(json.success, json.message);

            if (json.success) {
                $("#modalChangeAlias").modal("hide");

                document.querySelectorAll("#usrAlias").forEach(x => {
                    x.innerHTML = $("#txtAlias").val();
                });
            }
        });
});