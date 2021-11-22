var params = '';
var currentPage = '';
var frm = document.querySelector("#frmTable");

const setTdStatus = (id, val, text, number, textClass) => {
    return `<p class="${textClass}"
                       data-val="${val}"
                       data-toggle="modal"
                       data-id="${id}"
                       data-ordernumber="${number}"
                       data-target="#modalChangeStatus">
                      ${text}
                    </p>`;
}

$('#modalChangeStatus').on('show.bs.modal', function (e) {
    let id = $(e.relatedTarget).data().id;
    let orderNumber = $(e.relatedTarget).data().ordernumber;
    let val = $(e.relatedTarget).data().val;
    $(e.currentTarget).find('#hdId').val(id);
    $(e.currentTarget).find('#modalOrderNumber').html(orderNumber);
    $(e.currentTarget).find('#slStatus').val(val);
});

document.querySelector("#frmChangeStatus").addEventListener("submit", async function (e) {
    e.preventDefault();

    const body = new FormData(this);
    const url = `/Admin/Order/ChangeStatus`;

    let id = document.querySelector("#hdId").value;

    let tdOrder = document.querySelector(`td[data-id=ord-${id}][data-field="status"]`);

    try {

        showLoading();

        const fetchResponse = await fetch(url, { body: body, method: 'POST' });
        const json = await fetchResponse.json();

        if (json.success) {
            toastr.success(json.message);

            let textClass = "text-warning";

            if (json.data.val == 1)
                textClass = "text-success";
            else if (json.data.val == 2)
                textClass = "text-danger";

            tdOrder.innerHTML = setTdStatus(json.data.id,
                json.data.val,
                json.data.text,
                json.data.number,
                textClass);

            $("#divOrderStats").load("/Admin/Order/GetStats");
            $("#divMostRecentPendingOrders").load("/Admin/Order/GetPendingdidos ");

            $('#modalChangeStatus').modal('hide');
        } else {
            popUp(false, json.message);
        }

    } catch (e) {
        console.error(e);
        popUp(false, "Ocurrio un error inesperado..");
    }

    hideLoading();
});

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