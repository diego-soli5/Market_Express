var frmChangePass = document.querySelector("#frmChangePass");
var txtCurrentPass = document.querySelector("#currentPass");
var txtNewPass = document.querySelector("#newPass");
var txtNewPassConfirmation = document.querySelector("#newPassConfirmation");

//------------------INICIA CONTRASEÑA------------------\\
frmChangePass.addEventListener("submit", async function (e) {
    e.preventDefault();

    if (!(txtCurrentPass.value !== "" && txtNewPass.value !== "" && txtNewPassConfirmation.value !== "" && txtNewPass.value === txtNewPassConfirmation.value)) {
        return;
    }

    const url = "/Account/ChangePassword";
    const body = new FormData(frmChangePass);

    try {

        showLoading();

        const fetchResponse = await fetch(url, { body: body, method: 'POST' });
        const json = await fetchResponse.json();

        hideLoading();

        popUp(json.success, json.message);

        if (json.success) {
            $("#modalChangePassword").modal("hide");
            txtCurrentPass.value = "";
            txtNewPass.value = "";
            txtNewPassConfirmation.value = "";
        }

    } catch (e) {
        popUp(false, "Ocurrio un error inesperado..");
    }

    hideLoading();
});
//------------------FIN CONSTRASEÑA------------------\\






