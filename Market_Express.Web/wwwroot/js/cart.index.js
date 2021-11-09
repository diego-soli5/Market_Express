var divCartDetails = document.querySelector("#divCartDetails");

//------------------------------------------------INICIA DELETE------------------------------------------------------------------
async function eventDelete(e) {
    let articleId = this.getAttribute("data-article-id");
    let fetchResponse;

    const url = `/Client/Cart/DeleteDetail?fromCartView=true&articleId=${articleId}`;

    try {

        showLoading();

        fetchResponse = await fetch(url, { method: 'POST' });

        await manageResponse(fetchResponse);

        hideLoading();
    } catch (e) {
        hideLoading();

        if (fetchResponse.status === 401) {
            popUp(false, "Debe iniciar sesión.");
        } else {
            console.error(e);
            popUp(false, "Ocurrio un error inesperado..");
        }
    }
}
//-------------------------------------------------FIN DELETE-----------------------------------------------------------------


//------------------------------------------------INICIA MINUS------------------------------------------------------------------
async function eventMinus(e) {
    let articleId = this.getAttribute("data-article-id");
    let fetchResponse;

    const url = `/Client/Cart/UpdateDetail?plus=false&fromCartView=true&articleId=${articleId}`;

    try {

        showLoading();

        fetchResponse = await fetch(url, { method: 'POST' });

        await manageResponse(fetchResponse);

        hideLoading();
    } catch (e) {
        hideLoading();

        if (fetchResponse.status === 401) {
            popUp(false, "Debe iniciar sesión.");
        } else {
            console.error(e);
            popUp(false, "Ocurrio un error inesperado..");
        }
    }
}

//-------------------------------------------------FIN MINUS-----------------------------------------------------------------

//-------------------------------------------------INICIA PLUS-----------------------------------------------------------------
async function eventPlus(e) {
    let articleId = this.getAttribute("data-article-id");
    let fetchResponse;

    const url = `/Client/Cart/UpdateDetail?plus=true&fromCartView=true&articleId=${articleId}`;

    try {

        showLoading();

        fetchResponse = await fetch(url, { method: 'POST' });

        await manageResponse(fetchResponse);

        hideLoading();
    } catch (e) {
        hideLoading();

        if (fetchResponse.status === 401) {
            popUp(false, "Debe iniciar sesión.");
        } else {
            console.error(e);
            popUp(false, "Ocurrio un error inesperado..");
        }
    }
}
//----------------------------------------------FIN PLUS--------------------------------------------------------------------

async function manageResponse(fetchResponse) {
    const contentType = fetchResponse.headers.get("content-type");

    if (contentType && contentType.indexOf("application/json") !== -1) {
        let json = await fetchResponse.json();

        popUp(json.success, json.message);
    } else {
        let text = await fetchResponse.text();

        divCartDetails.innerHTML = text;

        bindArticleCartButtons();
    }

    updateCartCount();
}

function bindArticleCartButtons() {

    document.querySelectorAll("#btnPlus").forEach(btn => {
        btn.addEventListener("click", eventPlus);
    });

    document.querySelectorAll("#btnMinus").forEach(btn => {
        btn.addEventListener("click", eventMinus);
    });

    document.querySelectorAll("#btnDelete").forEach(btn => {
        btn.addEventListener("click", eventDelete);
    });
}


bindArticleCartButtons();