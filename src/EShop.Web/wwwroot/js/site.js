// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// ==================== Quantity Buttons ====================

document.querySelectorAll('.minus-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        let countElement = this.previousElementSibling;
        let count = parseInt(countElement.textContent);
        if (count > 1) {
            count--;
            countElement.textContent = count;
            updateTotalPrice(this, count);
        }
    });
});

document.querySelectorAll('.plus-btn').forEach(btn => {
    btn.addEventListener('click', function () {
        let countElement = this.nextElementSibling;
        let count = parseInt(countElement.textContent);
        count++;
        countElement.textContent = count;
        updateTotalPrice(this, count);
    });
});

function updateTotalPrice(btn, count) {
    let unitPrice = 100; // Example unit price
    let totalPriceElement = btn.closest('.card-body').querySelector('.total-price');
    totalPriceElement.textContent = (unitPrice * count) + ' هزار تومان';
}

// ==================== Register Form ====================

function onBeginRegister() {
    $("#registerForm button[type='submit']").addClass("disabled");
    $("#registerForm .alert-success").addClass("d-none");
    $("#validationErrorsRegister ul").empty();
}

function onCompleteRegister() {
    $("#registerForm button[type='submit']").removeClass("disabled");
}

function onfailureRegister(data, status, xhr) {
    console.log("onerror Called");
    console.log(status);
    console.log(xhr);
    console.log(data.responseText);
    console.log(data.status);
    let errors = data.responseJSON;
    $.each(errors, function (i, error) {
        $("#validationErrorsRegister ul").append("<li>" + error + "</li>");
    });
}

function onsuccessRegister(data, status, xhr) {
    console.log("onsuccess Called");
    console.log(status);
    console.log(xhr);
    console.log(data);
    if (data === "Success") {
        $("#registerForm div").remove();
        $("#registerForm .alert-success").removeClass("d-none");
    }
}

// ==================== Login Form ====================

function onBeginLogin() {
    $("#loginrForm button[type='submit']").addClass("disabled");
    $("#loginrForm .alert-success").addClass("d-none");
    $("#validationErrorsLogin ul").empty();
}

function onCompleteLogin() {
    $("#loginrForm button[type='submit']").removeClass("disabled");
}

function onfailureLogin(data, status, xhr) {
    console.log("onerror Called");
    console.log(status);
    console.log(xhr);
    console.log(data.responseText);
    console.log(data.status);
    let errors = data.responseJSON;
       $.each(errors, function (i, error) {
        $("#validationErrorsLogin ul").append("<li>" + error + "</li>");
       });
    if (errors[0] == 'شما قبلا وارد سیستم شده اید')
        location.reload();
}

function onsuccessLogin(data, status, xhr) {
    console.log("onsuccess Called");
    console.log(status);
    console.log(xhr);
    console.log(data);
    if (data === "Success") {
        $("#loginrForm input").val("");
        $("#loginrForm .alert-success").removeClass("d-none");
    }
    if (window.location.pathname == "/Account/ConfirmationAccount") {
        window.location.href = "/";
    } else {
        location.reload();
    }
}

// ==================== Modal Events ====================

const myModalEl = document.getElementById('loginModal');
if (myModalEl) {
    myModalEl.addEventListener('shown.bs.modal', event => {
        $.ajax({
            url: '/Account/LoadLoginPartial',
            type: 'GET',
            success: function (data) {
                $("#login-tab-pane").html(data);
                $.validator.unobtrusive.parse($('#loginModal'));
                var currentpath = window.location.pathname
                $("form#external-login-form").attr("action", `/Account/ExternalLogin?returnUrl=${currentpath}`) 
            }
        });
    });
}

$("#signup-tab").on("click", function () {
    $.ajax({
        url: '/Account/LoadRegisterPartial',
        type: 'GET',
        success: function (data) {
            $("#signup-tab-pane").html(data);
            $.validator.unobtrusive.parse($('#loginModal'));
        }
    });
});
