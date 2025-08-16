// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// ==================== Quantity Buttons ====================


let items = document.querySelectorAll('#recipeCarousel2 .carousel-item');
items.forEach((el) => {
    const minPerSlide = 4;
    let next = el.nextElementSibling;
    for (var i = 1; i < minPerSlide; i++) {
        if (!next) {
            // wrap carousel by using first child
            next = items[0];
        }
        let cloneChild = next.cloneNode(true);
        el.appendChild(cloneChild.children[0]);
        next = next.nextElementSibling;
    }
});

let items3 = document.querySelectorAll('#recipeCarousel3 .carousel-item');
items3.forEach((el) => {
    const minPerSlide = 4;
    let next3 = el.nextElementSibling;
    for (var i = 1; i < minPerSlide; i++) {
        if (!next3) {
            // wrap carousel by using first child
            next3 = items3[0];
        }
        let cloneChild3 = next3.cloneNode(true);
        el.appendChild(cloneChild3.children[0]);
        next3 = next3.nextElementSibling;
    }
});

// ==================== Quantity Buttons ====================

//document.querySelectorAll('.minus-btn').forEach(btn => {
//    btn.addEventListener('click', function () {
//        let countElement = this.previousElementSibling;
//        let count = parseInt(countElement.textContent);
//        if (count > 1) {
//            count--;
//            countElement.textContent = count;
//            updateTotalPrice(this, count);
//        }
//    });
//});

//document.querySelectorAll('.plus-btn').forEach(btn => {
//    console.log("adama8")
//    btn.addEventListener('click', function () {
//        let countElement = this.nextElementSibling;
//        let count = parseInt(countElement.textContent);
//        count++;
//        countElement.textContent = count;
//        updateTotalPrice(this, count);
//    });
//});

//function updateTotalPrice(btn, count) {
//    let unitPrice = 100; // Example unit price
//    let totalPriceElement = btn.closest('.card-body').querySelector('.total-price');
//    totalPriceElement.textContent = (unitPrice * count) + ' هزار تومان';
//}
$('.offcanvas-body').on("click", '.plus-btn', function () {
    const card = $(this).closest('.card')
    const productId = card.data('product-id')
    increaseOrLowOffCartDetail(productId, true, card)
})

$('.offcanvas-body').on("click", '.minus-btn', function () {
    const card = $(this).closest('.card')
    const productId = card.data('product-id')
    increaseOrLowOffCartDetail(productId, false, card)
})

$('.offcanvas-body').on("click", '.remove-btn', function () {
    const card = $(this).closest('.card')
    const productId = card.data('product-id')
    increaseOrLowOffCartDetail(productId, false, card, true)
})

function increaseOrLowOffCartDetail(productId, isIncrease, card, removeAll = false) {
    $.ajax({
        url: '/Cart/IncreaseOrLowOff',
        type: 'POST',
        data: {
            productId, isIncrease, removeAll
        },
        beforeSend: function () {
            $('.plus-btn, .minus-btn, .remove-btn').prop('disabled', true);
        },
        success: function () {
            if (false) {
                reloadTotalPriceInNavbar();
                getCardDetails();
            }
            else {
                const countOfCartDetail = parseInt($(card).find('.count').text());
                const uniquePriceOfCartDetail = parseInt($(card).find('div:eq(3) p:eq(1)')
                    .text().replace(/,/g, ''), 10);
                if ((countOfCartDetail === 1 && !isIncrease) || removeAll) {
                    $(card).remove();
                }
                else {
                    const newCount = isIncrease ? countOfCartDetail + 1 : countOfCartDetail - 1;
                    $(card).find('.count').text(newCount);
                    const newPrice = uniquePriceOfCartDetail * newCount;
                    $(card).find('div:eq(3) p:eq(3)').text(newPrice.toLocaleString('en-US'));
                }
                const cartTotalPrice = parseInt($('#cart-total-price')
                    .text().replace(/,/g, ''), 10);
                let newCartTotalPrice;
                if (isIncrease) {
                    newCartTotalPrice = (cartTotalPrice + uniquePriceOfCartDetail);
                }
                else if (removeAll) {
                    newCartTotalPrice = (cartTotalPrice - (uniquePriceOfCartDetail * countOfCartDetail));
                }
                else {
                    newCartTotalPrice = (cartTotalPrice - uniquePriceOfCartDetail);
                }
                $('#cart-total-price').text(`${newCartTotalPrice.toLocaleString('en-US')} تومان`);
                $('[data-bs-target="#offcanvasCart"] span').eq(1).text(`(${newCartTotalPrice.toLocaleString('en-US') })`)  
            }
        },
        error: function (xhr, status, error) {

        },
        complete: function () {
            $('.plus-btn, .minus-btn, .remove-btn').prop('disabled', false);
        }
    });
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
function reloadTotalPriceInNavbar() {
    $('[data-bs-target="#offcanvasCart"] span').eq(1).text('لطفا صبر کنید ...')
    $.ajax({
        url: '/Cart/GetUserCartTotalPrice',
        type: 'GET',
        success: function (data) {
            $('[data-bs-target="#offcanvasCart"] span').eq(1).text(`(${data})`)
        }
    });
}
if (isUserAuthenticated) {
    reloadTotalPriceInNavbar()
}

$("#offcanvasCart").on("show.bs.offcanvas", getCardDetails)

function getCardDetails() {
    if (isUserAuthenticated) {
        $('#cart-details-loading').removeClass('d-none');
        $.ajax({
            url: '/Cart/ShowCartDetailsPreview',
            type: 'GET',
            success: function (cartsData) {
                $('#cart-details-loading').addClass('d-none');
                $('#offcanvasCart .offcanvas-body').html(cartsData)
            }
        });
    }
}

