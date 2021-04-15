

var handleSaveClick = (e) => {
    var formName = document.getElementById("editName");
    var formPrice = document.getElementById("editPrice");
    var formDesc = document.getElementById("editDesc");

    // einam per edit buttonus, randam kuris enabled, ir to id ir zinom kad reikia keist
    // jei reikes addint, tai dar atskira logika

    document.getElementById("editPanel").className = "collapse mt-2";
    e.disabled = true;

    for (var j = 0; j < 100; j++) {
        var button = document.getElementById("editListing" + j);
        if (button === null || button === undefined) {
            continue;
        }
        if (button.disabled === false) {
            document.getElementById("name=" + j).innerHTML = formName.value;
            document.getElementById("price=" + j).innerHTML = formPrice.value;
            document.getElementById("desc=" + j).innerHTML = formDesc.value;

            if (document.getElementById("desc=" + j).innerHTML === "") {
                document.getElementById("desc=" + j).innerHTML = "No description provided";
            }

            button.innerText = "Edit";
            button.className = "btn btn-primary";
            changeButtonState(false, "editListing" + j);

            return;
        }
    }
    document.getElementById("add").innerText = "Another one";
    document.getElementById("add").className = "btn btn-success";
    document.getElementById("save").disabled = true;

    //id = "name=@item.VendorID"
    var listings = document.getElementById("listings");
    var newId = getLastId();
    var newDiv = document.getElementById("div=" + (newId - 1)).cloneNode(true);
    newDiv.id = "div=" + newId;

    var divToChange = newDiv.childNodes[3].childNodes;
    console.log(divToChange);
    console.log(divToChange[1].childNodes[3].childNodes);
    //name
    divToChange[1].childNodes[1].childNodes[1].id = "name=" + newId;
    divToChange[1].childNodes[1].childNodes[1].textContent = formName.value;
    //price
    divToChange[1].childNodes[3].childNodes[1].id = "price=" + newId;
    divToChange[1].childNodes[3].childNodes[1].textContent = formPrice.value;
    //button
    divToChange[1].childNodes[3].childNodes[5].id = "editListing" + newId;
    //desc
    divToChange[3].innerHTML = formDesc.value;
    divToChange[3].id = "desc=" + newId;

    listings.appendChild(newDiv);
    changeButtonState(false, e.id);
}

var getLastId = () => {
    var i = 0;
    for (var j = 0; j < 100; j++) {
        var button = document.getElementById("editListing" + j);
        if (button === null || button === undefined) {
            continue;
        }
        i = j;
    }
    return i + 1;
}


var handleAddClick = (e) => {
    var formName = document.getElementById("editName");
    var formPrice = document.getElementById("editPrice");
    var formDesc = document.getElementById("editDesc");
    formName.value = "";
    formPrice.value = "";
    formDesc.value = "";

    if (document.getElementById("add").innerText === "Cancel") {
        e.innerText = "Another one";
        e.className = "btn btn-success";
        document.getElementById("save").disabled = true;
        changeButtonState(false, e.id);
    } else {
        e.innerText = "Cancel";
        e.className = "btn btn-danger";
        document.getElementById("save").disabled = false;
        changeButtonState(true, e.id);
    }
}

var handleEditClick = (e, id) => {
    var formName = document.getElementById("editName");
    var formPrice = document.getElementById("editPrice");
    var formDesc = document.getElementById("editDesc");

    //get id, create the form, show the form with given info?
    console.log(id);
    formName.value = document.getElementById("name=" + id).innerText;
    formPrice.value = document.getElementById("price=" + id).innerText;
    formDesc.value = document.getElementById("desc=" + id).innerText;
    if (formDesc.value === "No description provided") {
        formDesc.value = "";
    }
    //console.log(e);
    //console.log(e.className);
    if (document.getElementById("add").disabled === true) {
        e.innerText = "Edit";
        e.className = "btn btn-primary";
        document.getElementById("save").disabled = true;
        changeButtonState(false, e.id);
    } else {
        e.innerText = "Cancel";
        e.className = "btn btn-danger";
        document.getElementById("save").disabled = false;
        changeButtonState(true, e.id);
    }
}

var changeButtonState = (state, buttonToExclude) => {
    for (var j = 0; j < 100; j++) {
        if (buttonToExclude === "editListing" + j) {
            continue;
        }
        var button = document.getElementById("editListing" + j);
        if (button === null || button === undefined) {
            continue;
        }
        button.disabled = state;
    }
    if (buttonToExclude !== "add") {
        document.getElementById("add").disabled = state;
    }
}