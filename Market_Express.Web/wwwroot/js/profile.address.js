var frmAddress = document.querySelector("#frmAddress");
var lblTitle = document.querySelector("#lblAddressTitle");
var hdfrmAddressId = document.querySelector("#id");

//------------------INICIA DIRECCIONES------------------\\
//Hace el submit segun sea el caso, agregar o editar una direccion
frmAddress.addEventListener("submit", async function (e) {
    e.preventDefault();

    let mode = frmAddress.getAttribute("data-mode");

    const body = new FormData(frmAddress);
    let url = "";

    if (mode == "POST") {
        url = "/Account/CreateAddress";
    }
    else {
        url = "/Account/EditAddress";
    }

    try {
        showLoading();

        const fetchResponse = await fetch(url, { body: body, method: 'POST' });
        const json = await fetchResponse.json();

        popUp(json.success, json.message);

        hideLoading();

        if (json.success) {
            $("#modalAddress").modal("hide");
            txtName.value = "";
            txtDetail.value = "";

            $("#divAddressTable").load("/Account/GetAddressTable", function () {
                bindAddressEvts();
            });
        }
    } catch (e) {
        hideLoading();
        popUp(false, "Ocurrio un error inesperado..");
    }
});

function bindAddressEvts() {
    lstBtnSetInUse = document.querySelectorAll("#setInUse");
    lstBtnPutAddress = document.querySelectorAll("#putAddress");
    lstBtnPostAddress = document.querySelectorAll("#postAddress");
    txtName = document.querySelector("#name");
    txtDetail = document.querySelector("#detail");

    //Establece el evento de los botones para asignar la direccion a ser usada
    lstBtnSetInUse.forEach(btn => {
        btn.addEventListener("click", async function (e) {
            try {
                showLoading();

                let addressId = btn.getAttribute("data-id");

                let url = `/Account/SetAddressToUse?addressId=${addressId}`;

                const fetchResponse = await fetch(url, { method: 'POST' });
                const json = await fetchResponse.json();

                

                if (json.success) {
                    $("#divAddressTable").load("/Account/GetAddressTable", function () {
                        bindAddressEvts();
                    });
                } else {
                    popUp(json.success, json.message);
                }

                hideLoading();
            } catch (e) {
                console.error(e);
                hideLoading();
                popUp(false, "Ocurrio un error inesperado..");
            }
        });
    });

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
}
//------------------FIN DIRECCIONES------------------\\



bindAddressEvts();