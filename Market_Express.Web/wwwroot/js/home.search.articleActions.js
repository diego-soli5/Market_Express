const htmlPlusMinus = (data, id) => {
    return ` <div align="center">
                    <div class="form-group col-md flex-grow-0">
                        <div class="input-group input-spinner">

                            <div class="input-group-prepend">
                                <button class="btn btn-light font-weight-bold"
                                    type="button"
                                    data-article-id="${id}"
                                    id="btnPlus">
                                    <i class="fas fa-plus text-primary"></i>
                                </button>
                            </div>

                            <input type="text"
                                   class="form-control-original font-weight-bold text-center"
                                   value="${data}"
                                   readonly style="background: #fff !important; ">

                                <div class="input-group-append">
                                    <button class="btn btn-light font-weight-bold"
                                        type="button"
                                        data-article-id="${id}"
                                        id="btnMinus">
                                        <i class="fas fa-minus text-primary"></i>
                                    </button>
                                </div>

                            </div>
                        </div>
                    </div>`;
}

const htmlAddToCart = (id) => {
    return `<button data-article-id="${id}"
                    class="btn btn-block btn-primary"
                    id="btnAdd">Añadir al carrito
            </button>`;
}

//------------------------------------------------INICIA ADD------------------------------------------------------------------
async function eventAdd(e) {
    let articleId = this.getAttribute("data-article-id");
    let fetchResponse;
    let json;

    const url = `/Client/Cart/AddDetail?articleId=${articleId}`;

    this.disabled = true;

    try {
        showLoading();

        fetchResponse = await fetch(url, { method: 'POST' });
        json = await fetchResponse.json();
        updateDetailElementsForAdd(json, this, articleId);

        hideLoading();
    } catch (e) {
        hideLoading();

        if (fetchResponse.status === 401) {
            $("#modalLogin").modal();
        } else {
            console.error(e);
            popUp(false, "Ocurrio un error inesperado..");
        }
    }

    await delayAsync(500);

    this.disabled = false;
}

function updateDetailElementsForAdd(json, btn, articleId) {
    if (!json.success) {
        popUp(false, json.message);
        return;
    }

    let div = btn.parentElement;

    div.innerHTML = htmlPlusMinus(json.data, articleId);
    div.querySelector("#btnPlus").addEventListener("click", eventPlus);
    div.querySelector("#btnMinus").addEventListener("click", eventMinus);

    updateCartCount();
}
//------------------------------------------------FIN ADD------------------------------------------------------------------




//------------------------------------------------INICIA MINUS------------------------------------------------------------------
async function eventMinus(e) {
    let articleId = this.getAttribute("data-article-id");
    let fetchResponse;
    let json;

    const url = `/Client/Cart/UpdateDetail?plus=false&fromCartView=false&articleId=${articleId}`;

    this.disabled = true;

    try {

        showLoading();

        fetchResponse = await fetch(url, { method: 'POST' });
        json = await fetchResponse.json();
        updateDetailElementsForMinus(json, this, articleId);

        hideLoading();
    } catch (e) {
        if (fetchResponse.status === 401) {
            popUp(false, "Debe iniciar sesion");
        } else {
            console.error(e);
            popUp(false, "Ocurrio un error inesperado..");
        }
    }

    await delayAsync(500);

    this.disabled = false;
}

function updateDetailElementsForMinus(json, btn, articleId) {
    if (!json.success) {
        popUp(false, json.message);
        return;
    }

    let div = btn.parentElement.parentElement.parentElement.parentElement.parentElement;

    if (json.resultCode == 0) {
        div.innerHTML = htmlAddToCart(articleId);
        div.querySelector("#btnAdd").addEventListener("click", eventAdd);
    } else if (json.resultCode == 1) {
        div.innerHTML = htmlPlusMinus(json.data, articleId);
        div.querySelector("#btnPlus").addEventListener("click", eventPlus);
        div.querySelector("#btnMinus").addEventListener("click", eventMinus);
    }

    updateCartCount();
}
//-------------------------------------------------FIN MINUS-----------------------------------------------------------------

//-------------------------------------------------INICIA PLUS-----------------------------------------------------------------
async function eventPlus(e) {
    let articleId = this.getAttribute("data-article-id");
    let fetchResponse;
    let json;

    const url = `/Client/Cart/UpdateDetail?plus=true&fromCartView=false&articleId=${articleId}`;

    this.disabled = true;

    try {

        showLoading();

        fetchResponse = await fetch(url, { method: 'POST' });
        json = await fetchResponse.json();

        updateDetailElementsForPlus(json, this, articleId);

        hideLoading();
    } catch (e) {
        if (fetchResponse.status === 401) {
            popUp(false, "Debe iniciar sesion");
        } else {
            console.error(e);
            popUp(false, "Ocurrio un error inesperado..");
        }
    }

    await delayAsync(500);

    this.disabled = false;
}

function updateDetailElementsForPlus(json, btn, articleId) {
    if (!json.success) {
        popUp(false, json.message);
        return;
    }

    let div = btn.parentElement.parentElement.parentElement.parentElement.parentElement;

    div.innerHTML = htmlPlusMinus(json.data, articleId);

    div.querySelector("#btnPlus").addEventListener("click", eventPlus);
    div.querySelector("#btnMinus").addEventListener("click", eventMinus);

    updateCartCount();
}
//----------------------------------------------FIN PLUS--------------------------------------------------------------------


function bindArticleCartButtons() {

    document.querySelectorAll("#btnPlus").forEach(btn => {
        btn.addEventListener("click", eventPlus);
    });

    document.querySelectorAll("#btnMinus").forEach(btn => {
        btn.addEventListener("click", eventMinus);
    });

    document.querySelectorAll("#btnAdd").forEach(btn => {
        btn.addEventListener("click", eventAdd);
    });
}


bindArticleCartButtons();