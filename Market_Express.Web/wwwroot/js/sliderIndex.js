var lstBtnStatus = document.querySelectorAll("#btnChangeStatus");

lstBtnStatus.forEach(btn => {
    btn.addEventListener("click", function (e) {
        e.preventDefault();

        let id = btn.getAttribute("data-id");

        const url = `/Admin/Slider/ChangeStatus?id=${id}`;

        fetch(url, { method: 'POST' })
            .then(response => response.json())
            .then(json => {
                if (json.success) {
                    alert(json.message);
                } else {
                    alert(json.message);
                }
            });
    });
});