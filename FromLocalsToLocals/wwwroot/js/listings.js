

var handleSaveClick = (e) => {
    var formName = document.getElementById("editName");
    var formPrice = document.getElementById("editPrice");
    var formDesc = document.getElementById("editDesc");

    // einam per edit buttonus, randam kuris enabled, ir to id ir zinom kad reikia keist
    // jei reikes addint, tai dar atskira logika
    for (var j = 0; j < 100; j++) {
        var button = document.getElementById("editListing" + j);
        if (button === null || button === undefined) {
            continue;
        }
        if (button.disabled === false) {
            document.getElementById("name=" + j).innerHTML = formName.value;
            document.getElementById("price=" + j).innerHTML = formPrice.value;
            document.getElementById("desc=" + j).innerHTML = formDesc.value;

            break;
        }
    }
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
        changeButtonState(false, e.id);
    } else {
        e.innerText = "Cancel";
        e.className = "btn btn-danger";
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
        changeButtonState(false, e.id);
    } else {
        e.innerText = "Cancel";
        e.className = "btn btn-danger";
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