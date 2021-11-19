var params = '';
var frm = document.querySelector("#frmTable");
var btnReport = document.querySelector("#btnGenerateReport");

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

    const url = `/Admin/Report/GetArticleTable?${params}&PageNumber=${pgnum}`;

    try {

        showLoading();

        const fetchResponse = await fetch(url, { method: 'GET' });
        const text = await fetchResponse.text();

        document.querySelector("#divArticleTable").innerHTML = text;

        bindPaginationEvts();

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

btnReport.addEventListener("click", function (e) {
    window.open(`/Admin/Report/ArticleReport?${params}`, '_blank');
});

bindPaginationEvts();

