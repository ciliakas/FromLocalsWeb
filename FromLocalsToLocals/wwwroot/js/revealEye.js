function checkShowPasswordVisibility() {
    var $revealEye = $(this).parent().find(".reveal-eye");
    if (this.value)
    {
        $revealEye.addClass("is-visible");
    }
    else
    {
        $revealEye.removeClass("is-visible");
    }
    }

    $(document).ready(function () {
        var txtPassword = document.getElementById('Password');
        var $revealEye = $('<span class="reveal-eye"></span>')

        $(txtPassword).parent().append($revealEye);
        $(txtPassword).on("keyup", checkShowPasswordVisibility)



        $revealEye.on({
        mousedown: function () {txtPassword.setAttribute("type", "text")},
            mouseup: function () {txtPassword.setAttribute("type", "password")},
            mouseout: function () {txtPassword.setAttribute("type", "password")},

        });
    })

    $(document).ready(function () {
        var txtPassword = document.getElementById('ConfirmPassword');
        var $revealEye = $('<span class="reveal-eye"></span>')

        $(txtPassword).parent().append($revealEye);
        $(txtPassword).on("keyup", checkShowPasswordVisibility)



        $revealEye.on({
        mousedown: function () {txtPassword.setAttribute("type", "text")},
            mouseup: function () {txtPassword.setAttribute("type", "password")},
            mouseout: function () {txtPassword.setAttribute("type", "password")},

        });


    })


    $(document).ready(function () {
        var txtPassword = document.getElementById('NewPassword');
        var $revealEye = $('<span class="reveal-eye"></span>')

        $(txtPassword).parent().append($revealEye);
        $(txtPassword).on("keyup", checkShowPasswordVisibility)



        $revealEye.on({
        mousedown: function () {txtPassword.setAttribute("type", "text")},
            mouseup: function () {txtPassword.setAttribute("type", "password")},
            mouseout: function () {txtPassword.setAttribute("type", "password")},

        });


    })

