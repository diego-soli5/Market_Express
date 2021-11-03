var lstBtnStatus = document.querySelectorAll("#btnChangeStatus");

lstBtnStatus.forEach(btn => {
    btn.addEventListener("click", async function (e) {
        e.preventDefault();

        let id = btn.getAttribute("data-id");

        const url = `/Admin/Slider/ChangeStatus?id=${id}`;

        btn.disabled = true;

        try {

            showLoading();

            const fetchResponse = await fetch(url, { method: 'POST' });
            const json = await fetchResponse.json();

            if (json.success) {
                toastr.success(json.message);

                if (json.resultCode == 0) {
                    btn.classList.replace("btn-success", "btn-danger");
                } else if (json.resultCode == 1) {
                    btn.classList.replace("btn-danger", "btn-success");
                }
            }
            else {
                popUp(false, json.message);
            }

            hideLoading();

        } catch (e) {
            console.error(e)
            popUp(false, "Ocurrio un error inesperado..");
        }

        hideLoading();

        await delayAsync(3000);

        btn.disabled = false;
    });
});