var jsContactId = -1;

//Loads all the messages
function loadMessages(obj) {

    $("#msg_history").empty();
    $.ajax({
        type: "GET",
        url: `/Chat/GetMessagesComponent`,
        data: { contactId: obj.id , isUserTab : userTab },
        success: function (result) {
            $("#msg_history").html(result);
            $(".msg_send_btn").removeAttr('disabled');
            jsContactId = parseInt(obj.id);
            setViewToBottom();
            clearTextField();
            $(".active_chat").removeClass("active_chat");
            obj.classList.add("active_chat");

            readMessage();
        },
    });
   
}

//Post new message to db
function postMessage() {
    var input = document.getElementById("postMsgText").value;

    if (input == "") {
        alert("Cannot send an empty message");
    } else {
        $.ajax({
            type: "POST",
            url: `/Chat/CreateMessage`,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ Message: input, IsUserTab: userTab, ContactId: jsContactId }),
            datatype: 'json',
            success: function (result) {
                loadNewMyMsg(input);
                setViewToBottom();
                clearTextField();
            },
        });
    }
}

//Load new message writter by user
function loadNewMyMsg(msg) {
    var date = new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '');
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

    updateContact(jsContactId, msg , date);
}

function clearTextField() {
    document.getElementById("postMsgText").value = "";
}

function setViewToBottom() {
    var scrollBar = document.getElementById('msg_history');
    scrollBar.scrollTop = scrollBar.scrollHeight - scrollBar.offsetHeight;
}


//signalR
var connectionToMsg = new signalR.HubConnectionBuilder().withUrl("/msgHub").build();

connectionToMsg.on("sendNewMessage", (obj) => {
    loadNewIncomingMsg(obj);
    setViewToBottom();
});

connectionToMsg.start();

//Loads new message written by other person
function loadNewIncomingMsg(obj) {
    var date = new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '');
    var newObj = JSON.parse(obj);
    console.log(newObj.ContactID);
    
    if (parseInt(newObj.ContactID) == jsContactId) {
        readMessage();

        var img = `<img class="img-circle" src="/Assets/profile.png" alt="avatar" />`;
        if (newObj.Image != null) {
            img = `<img src="data:image;base64,${newObj.Image}" alt="avatar" class="img-circle" />`
        } else if (obj.IsUserTab) {
            img = `<img class="img-circle" src="/Assets/localSeller.png" alt="avatar" />`;
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
                         ${newObj.Text}
                     </p>
                     <span class="time_date">${date}</span>
                 </div>
             </div>`;
        $("#msg_history").append(newMsg);
    } else {
        var contactBody = document.getElementById(parseInt(newObj.ContactID));
        contactBody.classList.add("unread_chat");
    }

    updateContact(parseInt(newObj.ContactID) , newObj.Text , date);
}


//Sets UserRead and ReceiverRead to true
function readMessage() {
    $.ajax({
        type: "POST",
        url: `/Chat/ReadMessage`,
        data: { contactId: jsContactId},
        datatype: 'json'    
    });

    var contactBody = document.getElementById(jsContactId);
    contactBody.classList.remove("unread_chat");
}

//Update message in Contact list
function updateContact(contactId, text, date) {
    var contactsBody = document.getElementById(contactId);

    var contactDate = contactsBody.querySelector('.chat_date');
    contactDate.innerHTML = `<i class="fa fa-clock-o"></i>${date}`;

    var contactText = contactsBody.querySelector('p');
    contactText.innerHTML = text;

    console.log(date);

}
