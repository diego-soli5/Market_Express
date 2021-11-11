var params = '';
var currentPage = '';
var frm = document.querySelector("#frmTable");
var btnCancelRecent = document.querySelector("#btnCancelRecent");

function bindPaginationEvts() {

    document.querySelectorAll('#btnPage')?.forEach(x => {
        x.addEventListener('click', async (e) => {
            e.preventDefault();

            let pgnum = e.target.getAttribute('data-page');

            currentPage = pgnum;

            await getPaginatedPage(pgnum);
        });
    });

    document.querySelector('#btnNextPage')?.addEventListener("click", async (e) => {
        e.preventDefault();

        let pgnum = e.target.getAttribute('data-page');

        currentPage = pgnum;

        await getPaginatedPage(pgnum);
    });

    document.querySelector('#btnPrevPage')?.addEventListener("click", async (e) => {
        e.preventDefault();

        let pgnum = e.target.getAttribute('data-page');

        currentPage = pgnum;

        await getPaginatedPage(pgnum);
    });
}

async function getPaginatedPage(pgnum) {

    const url = `/Client/Order/GetMyOrdersTable?${params}&PageNumber=${pgnum}`;

    try {

        showLoading();

        const fetchResponse = await fetch(url, { method: 'GET' });
        const text = await fetchResponse.text();

        document.querySelector("#divTableAllOrders").innerHTML = text;

        bindPaginationEvts();
    } catch (e) {
        hideLoading();
        console.error(e);
        popUp(false, "Ocurrio un error inesperado al refrescar la tabla..");
    }

    hideLoading();
}

frm.addEventListener("submit", async function (e) {
    e.preventDefault();

    const formData = new FormData(frm);

    params = createQueryStringParams(formData);

    currentPage = "1";

    await getPaginatedPage("1");
});


btnCancelRecent.addEventListener("click", async function (e) {
    const url = `/Client/Order/CancelRecent`;

    try {

        if (await confirmCancelRecent()) {
            showLoading();

            const fetchResponse = await fetch(url, { method: 'POST' });
            const json = await fetchResponse.json();

            if (json.success) {
                await getPaginatedPage(currentPage);
                await refreshMostRecentOrders();
                await refreshStats();
            }

            popUp(json.success, json.message);

        }
    } catch (e) {
        console.error(e)
        hideLoading();
        popUp(false, "Ocurrio un error inesperado..");
    }

    hideLoading();
});

async function refreshStats() {
    const url = `/Client/Order/GetStats`;

    const fetchResponse = await fetch(url, { method: 'GET' });
    const text = await fetchResponse.text();

    document.querySelector("#divOrderStats").innerHTML = text;
}

async function refreshMostRecentOrders() {
    const url = `/Client/Order/GetRecentOrders`;

    const fetchResponse = await fetch(url, { method: 'GET' });
    const text = await fetchResponse.text();

    document.querySelector("#divMostRecentOrders").innerHTML = text;
}

async function confirmCancelRecent() {
    let ok = false;

    await Swal.fire({
        title: 'Cancelar más reciente',
        text: "Deseas cancelar tu pedido más reciente?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si',
        cancelButtonText: 'No'
    }).then((result) => {
        if (result.isConfirmed)
            ok = true;
    });

    return ok;
}

bindPaginationEvts();