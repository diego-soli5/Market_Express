var chkAllCategory = document.querySelector("#chkAllCategory");
var lstChk = document.querySelectorAll('input[name="category"]');

chkAllCategory.addEventListener("change", function (e) {
    if (chkAllCategory.checked == true) {
        lstChk.forEach(chk => chk.checked = true);
    }
    else {
        lstChk.forEach(chk => chk.checked = false);
    }
});

lstChk.forEach(chk => {
    chk.addEventListener("change", function (e) {
        if (Array.prototype.slice.call(lstChk).every((c) => {
            return c.checked;
        })) {
            chkAllCategory.checked = true;
        }
        else {
            chkAllCategory.checked = false;
        }
    });
});