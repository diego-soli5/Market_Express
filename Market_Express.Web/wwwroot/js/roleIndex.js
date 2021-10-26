function bindRoleDeleteButtonEvts() {
    var lstBtnDelete = document.querySelectorAll('button[title="Eliminar"]');

    lstBtnDelete.forEach(btn => {
        btn.addEventListener("click", async function (e) {

            let id = btn.getAttribute("data-id");
            let url = `/Admin/Role/Delete?id=${id}`;

            const body = {
                method: 'POST'
            }

            if (await confirmDelete()) {
                fetch(url, body)
                    .then(response => response.json())
                    .then(json => {
                        popUp(json.success, json.message);

                        if (json.success) {
                            $("#divRoleTable").load('/Admin/Role/GetTable', function () {
                                bindRoleDeleteButtonEvts();
                            });
                        }

                    }).catch(err => popUp(false, "Hubo un error inesperado."));
            }

        });
    });

}

async function confirmDelete() {
    let ok = false;

    await Swal.fire({
        title: 'Estás seguro?',
        text: "El Rol no se podrá recuperar!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, Eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed)
            ok = true;
    });

    return ok;
}

bindRoleDeleteButtonEvts();