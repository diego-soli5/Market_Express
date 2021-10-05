var frmChangePass = document.querySelector("#frmChangePass");
var txtCurrentPass = document.querySelector("#currentPass");
var txtNewPass = document.querySelector("#newPass");
var txtNewPassConfirmation = document.querySelector("#newPassConfirmation");

var btnChangeAlias = document.querySelector("#btnChangeAlias");
var frmChangeAlias = document.querySelector("#frmChangeAlias");


//------------------INICIA DIRECCIONES------------------\\
function bindAddressEvts() {
    var frmAddress = document.querySelector("#frmAddress");
    var lstBtnPutAddress = document.querySelectorAll("#putAddress");
    var lstBtnPostAddress = document.querySelectorAll("#postAddress");
    var lblTitle = document.querySelector("#lblAddressTitle");
    var txtName = document.querySelector("#name");
    var txtDetail = document.querySelector("#detail");
    var hdfrmAddressId = document.querySelector("#hdfrmAddressId");

    lstBtnPostAddress.forEach(btn => {
        btn.addEventListener("click", function (e) {

            if (frmAddress.getAttribute("data-mode") == "PUT") {
                txtName.value = "";
                txtDetail.value = "";
            }

            lblTitle.innerHTML = "Agregar Dirección";
            frmAddress.setAttribute("data-mode", "POST");
        });
    });

    lstBtnPutAddress.forEach(btn => {
        btn.addEventListener("click", function (e) {

            lblTitle.innerHTML = "Editar Dirección";
            frmAddress.setAttribute("data-mode", "PUT");

            const url = "/Account/GetAddressInfo";

            fetch(url, { method: 'GET' })
                .then(response => response.json())
                .then(json => {
                    if (json.success) {
                        hdfrmAddressId.value = json.data.id;
                        txtName.value = json.data.name;
                        txtDetail.value = json.data.detail;
                    }
                    else {
                        alert("Fallo al intentar recuperar la información de dirección.");
                    }
                });
        });
    });

    frmAddress.addEventListener("submit", function (e) {
        e.preventDefault();

        if ($("#name").val().trim() == "" || $("#detail").val().trim() == "") {
            return;
        }

        let mode = frmAddress.getAttribute("data-mode");

        const url = "/Account/ChangeAlias";
        const body = new FormData(frmAddress);

        if (mode == "POST") {
            alert("POST")
        }
        else if (mode == "PUT") {
            alert("PUT")
        }
    });
}
//------------------FIN DIRECCIONES------------------\\

//------------------INICIA CONTRASEÑA------------------\\
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
//------------------FIN CONSTRASEÑA------------------\\

//------------------INICIA ALIAS------------------\\
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
//------------------FIN ALIAS------------------\\




bindAddressEvts();
