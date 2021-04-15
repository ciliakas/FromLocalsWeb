
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
        changeButtonState(false, e.id);
    } else {
        e.innerText = "Cancel";
        e.className = "btn btn-danger";
        changeButtonState(true, e.id);
    }
}

var handleEditClick = (e, name, price, desc) => {
    var formName = document.getElementById("editName");
    var formPrice = document.getElementById("editPrice");
    var formDesc = document.getElementById("editDesc");
    //get id, create the form, show the form with given info?
    formName.value = name;
    formPrice.value = price;
    formDesc.value = desc;
    //console.log(e);
    //console.log(e.className);
    if (document.getElementById("add").disabled === true) {
        e.innerText = "Edit";
        e.className = "btn btn-primary";
        changeButtonState(false, e.id);
    } else {
        e.innerText = "Cancel";
        e.className = "btn btn-danger";
        changeButtonState(true, e.id);
    }
}

var changeButtonState = (state, buttonToExclude) => {
    var i = 0;
    while (true) {
        i++;
        // cia reiketu gauti man listingu id's
        if (buttonToExclude === "editListing" + i) {
            continue;
        }
        var button = document.getElementById("editListing" + i);
        if (button === null || button === undefined) {
            break;
        }
        button.disabled = state;
    }
    if (buttonToExclude !== "add") {
        document.getElementById("add").disabled = state;
    }
}
