function bindRoleDeleteButtonEvts() {
    var lstBtnChangeStatus = document.querySelectorAll('button[data-mode]');

    lstBtnChangeStatus.forEach(btn => {
        btn.addEventListener("click", async function (e) {

            let id = btn.getAttribute("data-id");
            let url = `/Admin/Role/ChangeStatus?id=${id}`;
            let tdStatus = btn.parentElement
                .parentElement
                .parentElement
                .parentElement
                .querySelector('td[data-field="Status"]');

            debugger

            const body = {
                method: 'POST'
            }

            btn.disabled = true;

            try {
                showLoading();

                const fetchResponse = await fetch(url, body);
                const json = await fetchResponse.json();

                if (json.success) {
                    toastr.success(json.message);

                    if (json.resultCode == 0) {
                        btn.classList.replace("btn-success", "btn-danger");
                        btn.setAttribute("title", "Activar");
                        tdStatus.innerHTML = '<p class="text-capitalize-first">Desactivado</p>';
                    } else if (json.resultCode == 1) {
                        btn.classList.replace("btn-danger", "btn-success");
                        btn.setAttribute("title", "Desactivar");
                        tdStatus.innerHTML = '<p class="text-capitalize-first">Activado</p>';
                    }
                } else {
                    popUp(false, json.message);
                }

                hideLoading();

            } catch (e) {
                console.error(e)
                hideLoading();
                popUp(false, "Hubo un error inesperado.");
            }

            await delayAsync(3000);

            btn.disabled = false;
        });
    });

}

async function confirmChangeStatus() {
    let ok = false;

    await Swal.fire({
        title: 'Desactivar Rol',
        text: "Estás seguro de desactivar el Rol?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, Desactivar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed)
            ok = true;
    });

    return ok;
}

bindRoleDeleteButtonEvts();