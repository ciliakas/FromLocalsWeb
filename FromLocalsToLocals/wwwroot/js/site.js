$(window).resize( () => navbarModificationsOnResize());

function navbarModificationsOnResize() {
    var width = $(window).width();

    var msgItem = document.getElementById("tabChatIcon");
    var notiItem = document.getElementById("noti_Container");

    if (msgItem != null && notiItem != null) {

        var msgItemClone = msgItem;
        var notiItemClone = notiItem;

        msgItem.remove();
        notiItem.remove();

        if (width <= 990) {
            var navHeader = document.getElementById("navHeader");
            navHeader.appendChild(msgItemClone);
            navHeader.appendChild(notiItemClone);
        } else {
            var navBody = document.getElementById("iconsNavBar");
            navBody.prepend(msgItemClone);
            navBody.prepend(notiItemClone);
        }
    }
}
