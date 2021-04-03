function readMore() {
    $("#allReviews li:hidden").slice(0, 10).show();
    if ($("#allReviews li").length == $("#allReviews li:visible").length) {
        document.getElementById("readMoreBtn").hidden = true;
    }
}

function checkInput() {
    var stars = document.querySelector(".stars").getAttribute("data-rating");
    var text = (document.getElementById("userComment").value).trim();

    if (stars == 0 || text == "") {
        document.getElementById("postComment").disabled = true;
    } else {
        document.getElementById("postComment").disabled = false;
    }
}

//initial setup
document.addEventListener("DOMContentLoaded",
    function() {
        let stars = document.querySelectorAll(".star");
        stars.forEach(function(star) {
            star.addEventListener("click", setRating);
        });

        let rating = parseInt(document.querySelector(".stars").getAttribute("data-rating"));
        let target = stars[rating - 1];
        target.dispatchEvent(new MouseEvent("click"));
    });

function setRating(ev) {
    let span = ev.currentTarget;
    let stars = document.querySelectorAll(".star");
    let match = false;
    let num = 0;
    stars.forEach(function(star, index) {
        if (match) {
            star.classList.remove("rated");
        } else {
            star.classList.add("rated");
        }
        //are we currently looking at the span that was clicked
        if (star === span) {
            match = true;
            num = index + 1;
        }
    });
    document.querySelector(".stars").setAttribute("data-rating", num);
    checkInput();
}

function sortList(arg) {
    var list, i, switching, b, shouldSwitch;
    list = document.getElementById("allReviews");
    switching = true;

    // Make a loop that will continue until no switching has been done:
    while (switching) {

        // Start by saying: no switching is done:
        switching = false;
        b = list.getElementsByTagName("LI");

        switch (arg) {

            // reviews by newest
        case 1:
            for (i = 0; i < (b.length - 1); i++) {
                shouldSwitch = false;

                var date1 = getDate(b, i);
                var date2 = getDate(b, i + 1);

                if (date1 < date2) {
                    shouldSwitch = true;
                    break;
                }
            }
            break;

        // reviews by oldest
        case 2:
            for (i = 0; i < (b.length - 1); i++) {
                shouldSwitch = false;

                var date1 = getDate(b, i);
                var date2 = getDate(b, i + 1);

                if (date1 > date2) {
                    shouldSwitch = true;
                    break;
                }
            }
            break;
        default:
            for (i = 0; i < (b.length - 1); i++) {
                shouldSwitch = false;

                rating = getStarRating(b, i);
                rating1 = getStarRating(b, i + 1);

                if (rating < rating1) {
                    shouldSwitch = true;
                    break;
                }
            }
            break;
        }

        if (shouldSwitch) {
            /* If a switch has been marked, make the switch
            and mark the switch as done: */
            b[i].parentNode.insertBefore(b[i + 1], b[i]);
            switching = true;
        }
    }
}

function getDate(arr, index) {
    var x = arr[index].getElementsByTagName("SMALL");
    return x[0].getAttribute("data-val");
}

function getStarRating(arr, index) {
    var x = arr[index].getElementsByTagName("P");
    return x[0].getAttribute("data-rating");
}