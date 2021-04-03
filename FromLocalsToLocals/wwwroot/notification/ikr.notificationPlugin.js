(function($) {
    $.fn.ikrNotificationSetup = function(options) {
        var defaultSettings = $.extend({
                BeforeSeenColor: "#2E467C",
                AfterSeenColor: "#ccc"
            },
            options);
        $(".ikrNoti_Button").css({
            "background": defaultSettings.BeforeSeenColor
        });
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId).append("<div class='ikrNoti_Counter'></div>" +
                "<div class='ikrNoti_Button'><img src='/Assets/bell.png' style='width:inherit; height:inherit' /></div>" +
                "<div class='ikrNotifications'>" +
                "<h3>Notifications (<span class='notiCounterOnHead'>0</span>)</h3>" +
                "<div class='ikrNotificationItems'>" +
                "</div>" +
                "</div>");

            $("#" + parentId + " .ikrNoti_Counter")
                .css({ opacity: 0 })
                .text(0)
                .css({ top: "-10px" })
                .animate({ top: "-2px", opacity: 1 }, 500);

            $("#" + parentId + " .ikrNoti_Button").click(function() {
                $("#" + parentId + " .ikrNotifications").fadeToggle("fast",
                    "linear",
                    function() {
                        if ($("#" + parentId + " .ikrNotifications").is(":hidden")) {
                        }
                    });
                $("#" + parentId + " .ikrNoti_Counter").fadeOut("slow");
                return false;
            });
            $(document).click(function() {
                $("#" + parentId + " .ikrNotifications").hide();
                if ($("#" + parentId + " .ikrNoti_Counter").is(":hidden")) {
                }
            });
            $("#" + parentId + " .ikrNotifications").click(function() {
                return false;
            });

            $("#" + parentId).css({
                position: "relative"
            });
        }
    };
    $.fn.ikrNotificationCount = function(options) {
        var defaultSettings = $.extend({
                NotificationList: [],
                NotiFromPropName: "",
                ListTitlePropName: "",
                ListBodyPropName: "",
                ControllerName: "Notifications",
                ActionName: "AllNotifications"
            },
            options);
        var parentId = $(this).attr("id");
        if ($.trim(parentId) != "" && parentId.length > 0) {
            $("#" + parentId + " .ikrNotifications .ikrSeeAll").click(function() {
                window.open("../" + defaultSettings.ControllerName + "/" + defaultSettings.ActionName + "", "_blank");
            });

            var totalUnReadNoti = defaultSettings.NotificationList.filter(x => !x.isRead).length;
            $("#" + parentId + " .ikrNoti_Counter").text(totalUnReadNoti);
            $("#" + parentId + " .notiCounterOnHead").text(totalUnReadNoti);
            if (defaultSettings.NotificationList.length > 0) {
                $.map(defaultSettings.NotificationList,
                    function(item) {

                        var className = item.isRead ? "" : " ikrSingleNotiDivUnReadColor";
                        var sNotiFromPropName = $.trim(defaultSettings.NotiFromPropName) == ""
                            ? ""
                            : item[ikrLowerFirstLetter(defaultSettings.NotiFromPropName)];
                        $("#" + parentId + " .ikrNotificationItems").append("<div class='ikrSingleNotiDiv" +
                            className +
                            "' notiId=" +
                            item.NotiId +
                            ">" +
                            "<h4 class='ikrNotiFromPropName'>" +
                            sNotiFromPropName +
                            "</h4><a href='#'><span onclick='hide(this); return false' class='close'>X</span></a>" +
                            "<div class='ikrNotificationBody'>" +
                            item.NotiBody +
                            "</div>" +
                            "<div class='ikrNofiCreatedDate'>" +
                            formatDate(item.CreatedDate) +
                            "</div>" +
                            "</div>");
                        $(".ikrNotificationBody").click(function() {
                            if ($.trim(item.Url) != "") {
                                hide(this);
                                window.location.href = item.Url;
                            }
                        });
                    });
            }
        }
    };
}(jQuery));


function hide(e) {

    e.parentElement.parentElement.style.display = "none";
    var notiId;
    if (e instanceof HTMLDivElement) {
        notiId = e.parentElement.getAttribute("notiId");
    } else {
        notiId = e.parentElement.parentElement.getAttribute("notiId");
    }

    $.ajax({
        type: "POST",
        url: "/Notification/DeleteItem/" + notiId,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function(msg) {
            var elem = document.getElementById("noti_Container");
            elem.innerHTML = "";
            $("#noti_Container").ikrNotificationSetup();
            getNotifications();
        },
        error: function(e) {
            alert("Couldn't delete notification");
        }
    });

};

function formatDate(date) {
    var d = new Date(date),
        month = "" + (d.getMonth() + 1),
        day = "" + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2)
        month = "0" + month;
    if (day.length < 2)
        day = "0" + day;

    return [year, month, day].join("-");
}

function ikrLowerFirstLetter(value) {
    return value.charAt(0).toLowerCase() + value.slice(1);
}