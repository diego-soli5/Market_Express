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

    //Restablece el modal para agregar una nueva direccion
    lstBtnPostAddress.forEach(btn => {
        btn.addEventListener("click", function (e) {

            if (frmAddress.getAttribute("data-mode") == "PUT") {
                txtName.value = "";
                txtDetail.value = "";
            }

            hdfrmAddressId.value = "";

            lblTitle.innerHTML = "Agregar Dirección";
            frmAddress.setAttribute("data-mode", "POST");
        });
    });

    //Carga el modal con info de la direccion seleccionada
    lstBtnPutAddress.forEach(btn => {
        btn.addEventListener("click", function (e) {

            lblTitle.innerHTML = "Editar Dirección";
            frmAddress.setAttribute("data-mode", "PUT");

            const addressId = btn.parentElement.querySelector("#hdAddressId").value;
            const url = `/Account/GetAddressInfo?addressId=${addressId}`;

            fetch(url, { method: 'GET' })
                .then(response => response.json())
                .then(json => {
                    if (json.success) {
                        hdfrmAddressId.value = json.data.id;
                        txtName.value = json.data.name;
                        txtDetail.value = json.data.detail;
                    }
                    else {
                        popUp(false, "No pudimos cargar la información de la dirección");
                    }
                }).catch(err => popUp(false, "No pudimos cargar la información de la dirección"));
        });
    });

    //Hace el submit segun sea el caso, agregar o editar una direccion
    frmAddress.addEventListener("submit", function (e) {
        e.preventDefault();

        if (txtName.value.trim() == "" || txtDetail.value.trim() == "") {
            return;
        }

        let mode = frmAddress.getAttribute("data-mode");

        const body = new FormData(frmAddress);
        let url = "";

        if (mode == "POST") {
            url = "/Account/CreateAddress";
        }
        else {
            url = "/Account/EditAddress";
        }

        fetch(url, { body:body, method: 'POST' })
            .then(response => response.json())
            .then(json => {
                popUp(json.success, json.message);

                if (json.success) {
                    $("#modalAddress").modal("hide");
                    txtName.value = "";
                    txtDetail.value = "";

                    $("#addressContainer").load("/Account/AddressManager")
                }

            }).catch(err => popUp(false, "Hubo un error desconocido."));
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
