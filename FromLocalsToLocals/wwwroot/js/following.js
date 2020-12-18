function followBtnClick(obj, id) {
    var fButton = $(`.followBtn[data-id='${id}']`);
    $.ajax({
        type: "POST",
        url: '/Follower/' + obj.innerText + '/' + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (m) {
            if (m.success) {
                if (obj.innerText == "Follow") {
                    fButton.innerText = "Unfollow";
                    fButton.className = "btn btn-outline-primary w-100 followBtn";
                }
                else if (obj.innerText == "Sekti") {
                    fButton.innerText = "Nebesekti";
                    fButton.className = "btn btn-outline-primary w-100 followBtn";
                }
                else if (obj.innerText == "Unfollow") {
                    fButton.innerText = "Follow";
                    fButton.className = "btn btn-primary w-100 followBtn";
                }
                else if (obj.innerText == "Nebesekti") {
                    fButton.innerText = "Sekti";
                    fButton.className = "btn btn-primary w-100 followBtn";
                }
            } else {
                alert("Error occured when trying to " + obj.innerText + " vendor");
            }
        },
        error: function (e) {
            alert("Error occured when trying to " + obj.innerText + " vendor");
        }
    });
}