var frm = document.querySelector("#frm");
var selectType = document.querySelector("select");

//Añade la mascara inicial al input
$("#identification").mask("9-9999-9999");

document.querySelectorAll('input[name="idtype"]').forEach(radio => {
    radio.addEventListener("change", function (e) {
        let maskType = radio.value;

        if (maskType == "F") {
            $("#identification").mask("9-9999-9999");
        }
        else {
            $("#identification").mask("9-999-999999");
        }
    });
});


selectType.addEventListener("change", function (e) {
    let type = selectType.value;

    if (type == 0) {//cli
        $("#divRoles").collapse('hide');
        document.querySelectorAll('input[type=checkbox]').forEach(chk => {
            chk.checked = false;
        });
    } else if(type == 1) {
        $("#divRoles").collapse('show');
    }
});

frm.addEventListener("submit", async function (e) {

    btnSubmit.disabled = true;

    toastr.info("El botón se habilitará en 3 segundos.");

    await delayAsync(3000);

    btnSubmit.disabled = false;

});


