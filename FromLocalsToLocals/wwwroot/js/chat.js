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

var isUserSending;

function loadMessages(msg, userName, vendorTitle, fromVendors) {
    var sendBtn = document.getElementById("sendMsg");
    var atru = document.createAttribute("data-username");
    atru.value = userName;
    var atrv = document.createAttribute("data-vendortitle");
    atrv.value = vendorTitle;
    sendBtn.setAttributeNode(atru);
    sendBtn.setAttributeNode(atrv);
    sendBtn.style.display = "block";

    isUserSending = !fromVendors;

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
            `   <span class="username" style="color:black;">${currName}</span>
            <small class="timestamp">
                <i class="fa fa-clock-o"></i>${msg[i].Date}
            </small>
            <span class="avatar available tooltips" data-toggle="tooltip " data-placement="right" data-original-title="${currName}">
                <img src="${currImg}" alt="avatar" class="img-circle" style="width:70px;height:50px;">
            </span>
            <div class="body">
                <div class="message well well-sm" style="font-size:18px;line-height:inherit;max-width:80%;word-wrap: break-word;margin-top:10px;margin-right: 2px;color:black;">
                    ${msg[i].Text}
                </div>
            </div>`

        msgItem.innerHTML = html;

        document.getElementById("messagesList").appendChild(msgItem);
    }

    setViewToBottom();

}

function checkInput() {
    console.log("AA");
    var text = (document.getElementById("newMsg").value).trim();

    if (text == '') {
        document.getElementById("postMsg").disabled = true;
    }
    else {
        document.getElementById("postMsg").disabled = false;
    }
}

function sendMessage() {
    var newmsg = document.getElementById("newMsg");
    if ((newmsg.value).trim() != '') {

        var mData = {};
        var sendBtn = document.getElementById("sendMsg");


        mData = {
            userName : sendBtn.getAttribute("data-username"),
            vendorTitle : sendBtn.getAttribute("data-vendortitle"),
            message: (newmsg.value).trim(),
            isUserSender: isUserSending
        };

        console.log(mData);

        $.ajax({
            type: 'POST',
            url: '/Chat/CreateMessage',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(mData),
            success: function (msg) {
                putNewMsg(mData);
                newmsg.value = "";
            },
            error: function (e) {
                alert("Something went wrong");
            }
        });
    }
}

function setViewToBottom() {
    var a = document.getElementsByClassName("pre-scrollable");
    a[0].scrollTop = a[0].scrollHeight - a[0].offsetHeight
}

function putNewMsg(data) {
    console.log(data);
    var userImg = document.getElementById(data.userName + '-img');
    var vendorImg = document.getElementById(data.vendorTitle + '-img');

    var html, currName, currImg;


    var msgItem = document.createElement("li")

    msgItem.classList.add("right");
    
    if (data.IsUserSender) {
        currName = data.userName;
        currImg = userImg.getAttribute("src");
    } else {
        currName = data.vendorTitle;
        currImg = vendorImg.getAttribute("src");
    }

    var d = new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '')

    html =
        `   <span class="username" style="color:black;">${currName}</span>
        <small class="timestamp">
            <i class="fa fa-clock-o"></i>${d}
        </small>
        <span class="avatar available tooltips" data-toggle="tooltip " data-placement="right" data-original-title="${currName}">
            <img src="${currImg}" alt="avatar" class="img-circle" style="width:70px;height:50px;">
        </span>
        <div class="body">
            <div class="message well well-sm" style="font-size:18px;line-height:inherit;max-width:80%;word-wrap: break-word;margin-top:10px;margin-right: 2px;color:black;">
                ${data.message}
            </div>
        </div>`

    msgItem.innerHTML = html;

    document.getElementById("messagesList").appendChild(msgItem);
    

    setViewToBottom();

}