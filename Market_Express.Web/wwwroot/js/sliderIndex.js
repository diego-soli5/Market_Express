﻿var lstBtnStatus = document.querySelectorAll("#btnChangeStatus");

lstBtnStatus.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();

        let id = btn.getAttribute("data-id");

        const url = `/Admin/Slider/ChangeStatus?id=${id}`;

        fetch(url, { method: 'POST' })
            .then(response => response.json())
            .then(json => {
                if (json.success) {
                    toastr.success(json.message);

                    if (json.resultCode == 0) {
                        btn.classList.replace("btn-success", "btn-danger");
                    } else if (json.resultCode == 1) {
                        btn.classList.replace("btn-danger", "btn-success");
                    }
                }
                else {
                    popUp(false, "No se pudo cambiar el estado.");
                } 
            });
    });
});