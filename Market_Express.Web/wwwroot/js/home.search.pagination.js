var params = '';
var frm = document.querySelector("#frmFilters");

function bindSelectPageSizeEvt() {
    document.querySelector("#pageSizeSL")
        .addEventListener("change", async function (e) {

            let size = this.value;

            let arrParams = params.split("&");

            let index = arrParams.findIndex(x => x.includes("pageSize="));

            if (index == -1) {
                params += `&pageSize=${size}`;
            } else {
                arrParams.splice(index, 1);
                arrParams.push(`pageSize=${size}`);
                params = arrParams.join("&");
            }

            await getPaginatedPage("1");
        });
}

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

frm.addEventListener("submit", async function (e) {
    e.preventDefault();

    const formData = new FormData(frm);

    params = createQueryStringParams(formData);

    await getPaginatedPage("1");
});

async function getPaginatedPage(pgnum) {

    const url = `/Home/Search?${params}&PageNumber=${pgnum}`;

    try {

        showLoading();

        const fetchResponse = await fetch(url, { method: 'GET' });
        const text = await fetchResponse.text();

        document.querySelector("#divSearchResult").innerHTML = text;

        bindPaginationEvts();
        bindSelectPageSizeEvt();

        hideLoading();

    } catch (e) {
        console.error(e);
        hideLoading();
        popUp(false, "Ocurrio un error inesperado..");
    }
}


bindPaginationEvts();
bindSelectPageSizeEvt();