var jsContactId = -1;

$(document).ready(() => {
    var msgH = document.getElementById("msg_history");
    if (msgH.innerHTML.trim().length != 0 || newContactId != -1) {
        setViewToBottom();

        jsContactId = newContactId;
        if (jsContactId != -1) {
            var cDiv = document.getElementById(jsContactId);
            cDiv.classList.add("active_chat");
        }
    }
});

//Loads all the messages
function loadMessages(obj) {

    $("#msg_history").empty();
    $.ajax({
        type: "GET",
        url: `/Chat/GetChatComponent`,
        data: { contactId: obj.id, isUserTab: userTab, componentName:"Messages"},
        success: function (result) {
            $("#msg_history").html(result);
            jsContactId = parseInt(obj.id);
            setViewToBottom();
            clearTextField();
            $(".active_chat").removeClass("active_chat");
            obj.classList.add("active_chat");

            var active = document.getElementsByClassName("active_chat")[0].querySelector("h5").innerHTML;
            var chatW = document.getElementById("chatWith"); 
            chatW.innerHTML = active.substr(0, active.indexOf('<span')); 

            readMessage();
        },
    });
   
}

//Post new message to db
function postMessage() {
    var date = new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '');
    var input = document.getElementById("postMsgText").value;
    
    if (input == "" || jsContactId==-1) {
        alert("Cannot send the message");
    } else {

        var mData = {Message: input, IsUserTab: userTab, ContactId: jsContactId};

        $.ajax({
            type: "POST",
            url: `/api/Chat/CreateMessage`,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(mData),
            datatype: 'json',
            success: function (result) {
                loadNewMyMsg(input, date);
                setViewToBottom();
                clearTextField();

                updateContact(jsContactId, input, date);
            },
        });
    }
}

//Load new message writter by user to himself
function loadNewMyMsg(msg,date) {
    var newMsg = document.createElement("div");
    newMsg.classList.add("outgoing_msg");
    newMsg.innerHTML = `
            <div class="sent_msg">
                 <p>
                     ${msg}
                 </p>
                 <span class="time_date">${date}</span>
             </div>`;
    $("#msg_history").append(newMsg);
}

function clearTextField() {
    document.getElementById("postMsgText").value = "";
}

function setViewToBottom() {
    var scrollBar = document.getElementById('msg_history');
    scrollBar.scrollTop = scrollBar.scrollHeight - scrollBar.offsetHeight;
}


//------------------------signalR-------------------------------------------

var connectionToMsg = new signalR.HubConnectionBuilder().withUrl("/msgHub").build();

connectionToMsg.on("sendNewMessage", (obj) => {
    loadNewIncomingMsg(obj);
    setViewToBottom();
});

connectionToMsg.start();

//Load new message to vendor
function loadNewIncomingMsg(obj) {
    var date = new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '');
    var newObj = JSON.parse(obj);

    if (parseInt(newObj.ContactID) == jsContactId) {
        readMessage();

        var img = `<img class="img-circle" src="/Assets/localSeller.png" alt="avatar" />`;
        if (newObj.Image != null) {
            img = `<img src="data:image;base64,${newObj.Image}" alt="avatar" class="img-circle" />`
        } else if (newObj.IsUserTab) {
            img = `<img class="img-circle" src="/Assets/profile.png" alt="avatar" />`;
        }

        var newMsg = document.createElement("div");
        newMsg.classList.add("incoming_msg");
        newMsg.innerHTML = `
            <div class="incoming_msg_img">
            ${img}
            </div>
            <div class="received_msg">
                 <div class="received_withd_msg">
                     <p>
                         ${newObj.Message}
                     </p>
                     <span class="time_date">${date}</span>
                 </div>
             </div>`;
        $("#msg_history").append(newMsg);

        updateContact(parseInt(newObj.ContactID), newObj.Message, date);
    } else {
        var contactBody = document.getElementById(parseInt(newObj.ContactID));
        if (contactBody == null) {
            updateContact(parseInt(newObj.ContactID), newObj.Message, date, newObj.VendorTitle,false);
        } else {
            contactBody.classList.add("unread_chat");
            updateContact(parseInt(newObj.ContactID), newObj.Message, date);
        }
    }
}


//Sets UserRead and ReceiverRead to true
function readMessage() {
    var contactBody = document.getElementById(jsContactId);
    contactBody.classList.remove("unread_chat");

    $.ajax({
        type: "POST",
        url: `/api/Chat/ReadMessage`,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ ContactId: jsContactId}),
        datatype: 'json'
    });
}

//Update message in Contact list
function updateContact(ucontactId, text, date, title, tab) {

    var contactsBody = document.getElementById(ucontactId);

    if (contactsBody == null) {
        var vendorDiv = document.getElementById(title);
        var mData = { contactId: ucontactId, isUserTab: tab, componentName:"ContactBody" };

        $.ajax({
            type: "POST",
            url: `/Chat/GetChatComponent`,
            data: mData,
            success: function (result) {
                var ulDiv = vendorDiv.querySelector("ul");
                ulDiv.innerHTML = '<li>' + result + '</li>' + ulDiv.innerHTML;
                var div = document.getElementById(ucontactId);
                div.classList.add("unread_chat");
            },
        });
    } else {
        var contactDate = contactsBody.querySelector('.chat_date');
        contactDate.innerHTML = `<i class="fa fa-clock-o"></i>${date}`;

        var contactText = contactsBody.querySelector('p');
        contactText.innerHTML = text;
    }
}
