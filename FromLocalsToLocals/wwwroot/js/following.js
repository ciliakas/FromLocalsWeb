function followBtnClick(obj, id) {
    var fButton = document.getElementById("followButn");
    var followerCount = document.getElementById("followerCount");

    $.ajax({
        type: "POST",
        url: '/Follower/' + obj.innerText + '/' + id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (m) {
            if (m.success) {
                if (obj.innerText == "Follow") {
                    fButton.innerHTML = "Unfollow";
                    fButton.className = "btn btn-outline-primary w-100 followBtn";
                    followerCount.innerHTML = parseInt(followerCount.innerHTML, 10) + 1;
                }

                else if (obj.innerText == "Sekti") {
                    fButton.innerHTML = "Nebesekti";
                    fButton.className = "btn btn-outline-primary w-100 followBtn";
                    followerCount.innerHTML = parseInt(followerCount.innerHTML, 10) + 1;
                }
                else if (obj.innerText == "Unfollow") {
                    fButton.innerHTML = "Follow";
                    fButton.className = "btn btn-primary w-100 followBtn";
                    followerCount.innerHTML = parseInt(followerCount.innerHTML, 10) - 1;
                }
                else if (obj.innerText == "Nebesekti") {
                    fButton.innerHTML = "Sekti";
                    fButton.className = "btn btn-primary w-100 followBtn";
                    followerCount.innerHTML = parseInt(followerCount.innerHTML, 10) - 1;
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