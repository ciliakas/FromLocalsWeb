﻿
var loading = false;

var query = window.location.search;
var params = new URLSearchParams(query);
var activeT = params.get("ActiveTab");


function imageUpload(obj) {
    var fileName = obj.value;
    var fileNameArr = fileName.split("\\");
    console.log(fileNameArr);
    if (fileNameArr.length > 1) {
        $("#selectedPhotoName").html(fileNameArr[fileNameArr.length - 1]);
        $("#selectPhotoBtnText").html("Change Photo");
    } else {
        $("#selectedPhotoName").html("");
        $("#selectPhotoBtnText").html("Add Photo");
    }
};

$(document).ready(() => {
    loadPageData();
});

var feedScroll = document.getElementById("content1");
var pageCount = 8;

function scrollFeedDetails() {
    if (feedScroll.scrollTop + feedScroll.offsetHeight + 150 >= feedScroll.scrollHeight && !loading && details) {
        loadPageData();
    }
};

$(window).scroll(function() {
    if ($(window).scrollTop() + $(window).height() + 250 >= $(document).height() && !loading && !details) {
        loadPageData();
    }
});

function loadPageData() {
    loading = true;

    var alreadyShowing = document.getElementsByClassName("postsL").length;

    var mUrl = "/Feed/GetAllPosts";
    var mdata = { skip: alreadyShowing, itemsCount: pageCount };
    if (details) {
        mUrl = "/Feed/GetVendorPosts";
        mdata = { vendorId: vendorID, skip: alreadyShowing, itemsCount: pageCount };
    } else if (activeT == "MyFeed") {
        mUrl = "/Feed/GetFollowingPosts";
        mdata = { userId: userID, skip: alreadyShowing, itemsCount: pageCount };
    }


    $.ajax({
        url: mUrl,
        method: "get",
        dataType: "json",
        data: mdata,
        beforeSend: function() {

            var lelem = document.getElementsByClassName("lastPost");
            if (lelem.length == 0) {


                var div = document.createElement("li");
                div.id = "feedLoader";
                div.innerHTML = `<li>
                                     <div class="timeline-body" style="height:150px;background:transparent;">
                                         <div class="loader"></div>
                                     </div>
                                 </li>`;


                document.getElementById("postsUL").appendChild(div);

            }
        },
        success: function(data) {

            var feedLoader = document.getElementById("feedLoader");

            if (feedLoader != null) {
                feedLoader.remove();
            }


            var lelem = document.getElementsByClassName("lastPost");
            if (lelem.length != 0) {
                lelem[0].remove();
            }

            for (var i = 0; i < data.value.length; i++) {
                var id = data.value[i].vendorID;
                var date = data.value[i].date;
                var text = data.value[i].text;
                var image = data.value[i].image;
                var vendorImage = data.value[i].vendor.image;
                var vendorTitle = data.value[i].vendor.title;
                var owner = false;
                for (var j = 0; j < allUserVendors.length; j++) {
                    if (allUserVendors[j].Title == vendorTitle) {
                        owner = true;
                        break;
                    }
                }

                date = date.substring(0, date.length - 3);


                addPostItem(id, date, text, image, vendorImage, vendorTitle, owner);

            }

            if (data.value.length < pageCount && document.getElementsByClassName("lastPost").length == 0) {
                if (details) {
                    addLastItem(vendorTitleImp, vendorDate, details);
                } else {
                    addLastItem(vendorTitle, "", details);
                }
            }

            loading = false;
        },
        error: function(err) {
            alert("Sorry something went wrong..");
            loading = false;
        }
    });
}


function addPostItem(id, date, text, image, vendorImage, vendorTitle, owner) {
    var vendorImageLine;
    var postImageLine = "";

    if (vendorImage == null) {
        vendorImageLine = `<span class="userimage"><img src="/Assets/localSeller.png" alt=""></span>`;
    } else {
        vendorImageLine = `<span class="userimage"><img src="data:image;base64,${vendorImage}" alt=""></span>`;
    }
    if (image != null) {
        postImageLine = `<div class="row">
                                    <img src="data:image;base64,${image}" style="width: 100%; max-height: 250px;" />
                               </div>`;
    }

    var li = document.createElement("li");
    li.classList.add("postsL");

    li.innerHTML = `
            <div class="timeline-icon">
                <a href="javascript:;">&nbsp;</a>
            </div>
            <div class="timeline-body">
                <div class="timeline-header">
                    ${vendorImageLine}
                        <span class="username"><a href="/Vendors/Details/${id}">${vendorTitle
        }</a> <small></small></span>
                        <span class="time">${date}</span>
                </div>
                <div class="timeline-content">
                    <p>
                        ${text}
                    </p>
                    ${postImageLine}
                </div>
            </div>`;

    document.getElementById("postsUL").appendChild(li);
}

function addLastItem(vendorTitle, dateCreated, details) {

    var displayDateCreated = ``;
    var lastItemP;

    if (details) {
        displayDateCreated = `<div class="timeline-time">
                                            <span style="float:right" class="time"></span>
                                       </div>`;

        lastItemP = `<p>${vendorTitle} was created</p>`;


    } else {
        lastItemP = `<p>No more posts...</p>`;
    };

    var li = document.createElement("li");
    li.classList.add("lastPost");
    li.innerHTML = `<li> ${displayDateCreated}
                        <div class="timeline-icon">
                            <a href="javascript:;">&nbsp;</a>
                        </div>
                        <div class="timeline-body">
                            ${dateCreated} ${lastItemP}
                        </div>
                     </li>`;

    document.getElementById("postsUL").appendChild(li);
}