var btnChangeAlias = document.querySelector("#btnChangeAlias");
var frmChangeAlias = document.querySelector("#frmChangeAlias");


//------------------INICIA ALIAS------------------\\
btnChangeAlias.addEventListener("click", function (e) {
    const url = "/Account/GetUserAlias";

    fetch(url, { method: 'GET' })
        .then(response => response.text())
        .then(text => $("#txtAlias").val(text));
});

frmChangeAlias.addEventListener("submit", async function (e) {
    e.preventDefault();

    if ($("#txtAlias").val().trim() == "") {
        return;
    }

    const url = "/Account/ChangeAlias";
    const body = new FormData(frmChangeAlias);


    try {
        showLoading();

        const fetchResponse = await fetch(url, { body: body, method: 'POST' });
        const json = await fetchResponse.json();

        hideLoading();

        popUp(json.success, json.message);

        if (json.success) {
            $("#modalChangeAlias").modal("hide");

            document.querySelectorAll("#usrAlias").forEach(x => {
                x.innerHTML = $("#txtAlias").val();
            });
        }
    } catch (e) {
        popUp(false, "Ocurrio un error inesperado..");
    }

    hideLoading();
});
//------------------FIN ALIAS------------------\\