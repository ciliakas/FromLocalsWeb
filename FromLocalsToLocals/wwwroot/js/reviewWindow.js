function handlePost() {
    document.getElementById("starCount").value = document.querySelector('.stars').getAttribute('data-rating');

    if ((document.getElementById("starCount").value == 0) || (isEmptyOrSpaces(document.getElementById("userComment").value))) {
        document.getElementById("postComment").disabled = true;
    }
    else {
         document.getElementById("postComment").disabled = false;
    }
}
function toggle() {
    document.getElementById("postComment").disabled = false;
}

function isEmptyOrSpaces(str) {
    return str === null || str.match(/^\s*$/) !== null || srt.match(/\r\n |\r |\n/) !== null;
}


//initial setup
document.addEventListener('DOMContentLoaded', function () {
    let stars = document.querySelectorAll('.star');
    stars.forEach(function (star) {
        star.addEventListener('click', setRating);
    });

    let rating = parseInt(document.querySelector('.stars').getAttribute('data-rating'));
    let target = stars[rating - 1];
    target.dispatchEvent(new MouseEvent('click'));
});

function setRating(ev) {
    let span = ev.currentTarget;
    let stars = document.querySelectorAll('.star');
    let match = false;
    let num = 0;
    stars.forEach(function (star, index) {
        if (match) {
            star.classList.remove('rated');
        } else {
            star.classList.add('rated');
        }
        //are we currently looking at the span that was clicked
        if (star === span) {
            match = true;
            num = index + 1;
        }
    });
    document.querySelector('.stars').setAttribute('data-rating', num);
}