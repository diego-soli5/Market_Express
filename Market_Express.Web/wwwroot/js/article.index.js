var params = '';
var frm = document.querySelector("#frmTable");

function bindPaginationEvts() {

    document.querySelectorAll('#btnPage')?.forEach(x => {
        x.addEventListener('click', async (e) => {
            e.preventDefault();

            let pgnum = e.target.getAttribute('data-page');

            await getPaginatedPage(pgnum);
        });
    });

    document.querySelector('#btnNextPage')?.addEventListener("click", async (e) => {
        e.preventDefault();

        let pgnum = e.target.getAttribute('data-page');

        await getPaginatedPage(pgnum);
    });

    document.querySelector('#btnPrevPage')?.addEventListener("click", async (e) => {
        e.preventDefault();

        let pgnum = e.target.getAttribute('data-page');

        await getPaginatedPage(pgnum);
    });
}

async function getPaginatedPage(pgnum) {

    const url = `/Admin/Article/GetArticleTable?${params}&PageNumber=${pgnum}`;

    try {

        showLoading();

        const fetchResponse = await fetch(url, { method: 'GET' });
        const text = await fetchResponse.text();

        document.querySelector("#divArticlesTable").innerHTML = text;

        bindPaginationEvts();
        bindEnableOrDisableButtonEvts();

        hideLoading();

    } catch (e) {
        console.error(e)
        popUp(false, "Ocurrio un error inesperado al refrescar la tabla..");
    }

    hideLoading();
}

frm.addEventListener("submit", async function (e) {
    e.preventDefault();

    const formData = new FormData(frm);

    params = createQueryStringParams(formData);

    await getPaginatedPage("1");
});


function bindEnableOrDisableButtonEvts() {
    var lstBtn = document.querySelectorAll("button[data-id]");

    lstBtn.forEach(btn => {
        btn.addEventListener("click", async function (e) {

            let id = btn.getAttribute("data-id");
            let tdStatus = btn.parentElement
                .parentElement
                .parentElement
                .parentElement
                .querySelector('td[data-field="Status"]');

            let url = `/Admin/Article/ChangeStatus?id=${id}`;

            btn.disabled = true;

            try {
                showLoading();

                const fetchResponse = await fetch(url, { method: 'POST' });
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

            } catch (e) {
                popUp(false, "Ocurrio un error inesperado..");
            }

            hideLoading();

            await delayAsync(3000);

            btn.disabled = false;
        });
    });
}

bindEnableOrDisableButtonEvts();
bindPaginationEvts();