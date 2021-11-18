var params = '';
var currentPage = '';
var frm = document.querySelector("#frmTable");

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

    const url = `/Admin/Order/GetTable?${params}&PageNumber=${pgnum}`;

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

async function refreshStats() {
    const url = `/Admin/Order/GetStats`;

    const fetchResponse = await fetch(url, { method: 'GET' });
    const text = await fetchResponse.text();

    document.querySelector("#divOrderStats").innerHTML = text;
}

async function refreshMostRecentOrders() {
    const url = `/Admin/Order/GetPending`;

    const fetchResponse = await fetch(url, { method: 'GET' });
    const text = await fetchResponse.text();

    document.querySelector("#divMostRecentPendingOrders").innerHTML = text;
}

bindPaginationEvts();


$(function () {
    $('input[name="clientName"]').autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/Admin/Order/SearchClient",
                dataType: "json",
                data: { query: $('input[name="clientName"]').val() },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.name, value: item.name };
                    }));
                },
                error: function (xhr, status, error) {
                    alert("Error");
                }
            });
        }
    });

});