


var handleClick = (e, name, price, desc) => {
    var formName = document.getElementById("editName");
    var formPrice = document.getElementById("editPrice");
    var formDesc = document.getElementById("editDesc");
    //get id, create the form, show the form with given info?
    // disable all other edit buttons
    //e.disabled = true;
    //console.log(e.id);
    formName.value = name;
    formPrice.value = price;
    formDesc.value = desc;
    if (document.getElementById("add").disabled === true) {
        e.innerText = "Edit";
        changeButtonState(false, e.id);
    } else {
        e.innerText = "Cancel";
        changeButtonState(true, e.id);
    }
}

var changeButtonState = (state, buttonToExclude) => {
    var i = 0;
    while (true) {
        i++;
        // cia reiketu gauti man listingu id's
        console.log("editListing" + i);
        if (buttonToExclude === "editListing" + i) {
            continue;
        }
        var button = document.getElementById("editListing" + i);
        if (button === null || button === undefined) {
            break;
        }
        //var state = button.disabled;
        button.disabled = state;
    }
    document.getElementById("add").disabled = state;
}

var disableButtons = () => {

}