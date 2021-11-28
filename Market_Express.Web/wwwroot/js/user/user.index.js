var frmTable = document.querySelector("#frmTable");

frmTable.addEventListener("submit", async function (e) {
    e.preventDefault();

    const params = createQueryStringParams(new FormData(frmTable));

    const url = `/Admin/Users/GetUserTable?${params}`;

    try {

        showLoading();

        const fetchResponse = await fetch(url, { method: 'GET' });
        const text = await fetchResponse.text();
   
        document.querySelector("#divUsersTable").innerHTML = text;

        bindEnableOrDisableButtonEvts();

        hideLoading();
        
    } catch (e) {
        console.error(e)
        popUp(false, "Ocurrio un error inesperado al refrescar la tabla..");
    }

    hideLoading();
});

function bindEnableOrDisableButtonEvts() {
    var lstBtn = document.querySelectorAll("button[data-id]");

    lstBtn.forEach(btn => {
        btn.addEventListener("click", async function (e) {

            let id = btn.getAttribute("data-id");
            let mode = btn.getAttribute("data-mode");
            let tdStatus = btn.parentElement
                .parentElement
                .parentElement
                .parentElement
                .querySelector('td[data-field="Status"]')

            let url = `/Admin/Users/ChangeStatus?id=${id}`;

            const body = {
                method: 'POST'
            }

            btn.disabled = true;

            if (await confirmChangeStatus(mode)) {
                fetch(url, body)
                    .then(response => response.json())
                    .then(json => {

                        popUp(json.success, json.message);

                        if (json.success) {
                            if (json.resultCode == 0) {
                                btn.classList.replace("btn-success", "btn-danger");
                                btn.setAttribute("data-mode", "E");
                                btn.setAttribute("title", "Activar");
                                tdStatus.innerHTML = '<p class="text-capitalize-first">Desactivado</p>';
                            } else if (json.resultCode == 1) {
                                btn.classList.replace("btn-danger", "btn-success");
                                btn.setAttribute("data-mode", "D");
                                btn.setAttribute("title", "Desactivar");
                                tdStatus.innerHTML = '<p class="text-capitalize-first">Activado</p>';
                            }
                        }

                    }).catch(err => popUp(false, "Hubo un error inesperado."));
            }

            btn.disabled = false;
        });
    });
}


async function confirmChangeStatus(mode) {
    let ok = false;
    let title;
    let text;
    let icon;
    let btnText;

    if (mode == "E") {
        title = "Activar Usuario";
        text = "Estás seguro que deseas activar la cuenta de usuario?";
        icon = "warning";
        btnText = "Si, Activar";
    }
    else if (mode == "D") {
        title = "Desactivar Usuario";
        text = "Estás seguro que deseas desactivar la cuenta de usuario?";
        icon = "warning";
        btnText = "Si, Desactivar";
    }

    await Swal.fire({
        title: title,
        text: text,
        icon: icon,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: btnText,
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed)
            ok = true;
    });

    return ok;
}

bindEnableOrDisableButtonEvts();