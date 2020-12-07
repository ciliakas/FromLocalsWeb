function mySentMsgs() {
    document.getElementById("listOfVendors").style.display = "none";
    document.getElementById("mySentMsgs").style.display = "block";
}

function myVendorsMsgs() {
    document.getElementById("mySentMsgs").style.display = "none";
    document.getElementById("listOfVendors").style.display = "block";
}

var lSide = `<li class="left">`;
var rSide = `<li class="right">`;

function loadMessages(msg, userName, vendorTitle, fromVendors) {
    if (fromVendors) {
        document.getElementById("chattingWith").innerText = userName;
    } else {
        document.getElementById("chattingWith").innerText = vendorTitle;
    }

    document.getElementById("messagesList").innerHTML = "";

    var userImg = document.getElementById(userName + '-img');
    var vendorImg = document.getElementById(vendorTitle + '-img');

    var html, currName, currImg;


    for (var i = 0; i < msg.length; i++) {
        var msgItem = document.createElement("li")

        if ((fromVendors && !msg[i].IsUserSender) || (!fromVendors && msg[i].IsUserSender)) {
            msgItem.classList.add("right");
        } else {
            msgItem.classList.add("left");
        }


        if (msg[i].IsUserSender) {
            currName = userName;
            currImg = userImg.getAttribute("src");
        } else {
            currName = vendorTitle;
            currImg = vendorImg.getAttribute("src");
        }


        html =
            `   <span class="username">${currName}</span>
            <small class="timestamp">
                <i class="fa fa-clock-o"></i>${msg[i].Date}
            </small>
            <span class="avatar available tooltips" data-toggle="tooltip " data-placement="right" data-original-title="${currName}">
                <img src="${currImg}" alt="avatar" class="img-circle" style="width:70px;height:50px;">
            </span>
            <div class="body">
                <div class="message well well-sm" style="color:black;">
                    ${msg[i].Text}
                </div>
            </div>`

        msgItem.innerHTML = html;

        document.getElementById("messagesList").appendChild(msgItem);
    }
}
