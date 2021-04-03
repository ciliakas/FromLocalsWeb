function subBtnClick(obj, id) {

    var sButtons = $(`.subBtn[data-id='${id}']`);;
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(m) {
            if (m.success) {
                if (obj.innerText == "Follow") {
                    for (var i = 0; i < sButtons.length; i++) {
                        sButtons[i].innerText = "Unfollow";
                        sButtons[i].className = "btn btn-outline-dark w-100 followBtn";
                    }
                } else if (obj.innerText == "Unfollow") {
                    for (var i = 0; i < sButtons.length; i++) {
                        sButtons[i].innerText = "Follow";
                        sButtons[i].className = "btn btn-dark w-100 followBtn";
                    }
                }
            } else {
                alert("Error occured when trying to " + obj.innerText + " vendor");
            }
        },
        error: function(e) {
            alert("Error occured when trying to " + obj.innerText + " vendor");
        }
    });
}