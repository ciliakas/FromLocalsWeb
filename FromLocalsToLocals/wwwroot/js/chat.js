var jsContactId = -1;

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
        },
    });
   
}

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
                loadNewMsg(input);
                setViewToBottom();
            },
        });
    }
}

function loadNewMsg(msg) {
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
}

function setViewToBottom() {
    var scrollBar = document.getElementById('msg_history');
    scrollBar.scrollTop = scrollBar.scrollHeight - scrollBar.offsetHeight;
    document.getElementById("postMsgText").value = "";
}