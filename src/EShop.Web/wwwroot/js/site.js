// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelectorAll('.minus-btn').forEach(btn => {
	btn.addEventListener('click', function () {
		let countElement = this.previousElementSibling;
		let count = parseInt(countElement.textContent);
		if (count > 1) {
			count--;
			countElement.textContent = count;
			updateTotalPrice(this, count);
		}
	})
})

document.querySelectorAll('.plus-btn').forEach(btn => {
	btn.addEventListener('click', function () {
		let countElement = this.nextElementSibling;
		let count = parseInt(countElement.textContent);
		count++;
		countElement.textContent = count;
		updateTotalPrice(this, count);
	})
})

function updateTotalPrice(btn, count) {
	let unitPrice = 100; // Example unit price
	let totalPriceElement = btn.closest('.card-body').querySelector('.total-price');
	totalPriceElement.textContent = (unitPrice * count) + ' هزار تومان';
}

function onBegin() {
	$("#registerForm button[type='submit']").addClass("disabled");
	$("#registerForm .alert-success").addClass("d-none ");
	$("#validationErrors  ul").empty();
}

function onComplete() {
	$("#registerForm button[type='submit']").removeClass("disabled");
}

function onfailure (data, status, xhr)  {
	console.log("onerror Called")
	console.log(status)
	console.log("xhr")
	console.log(xhr)
	console.log("data")
	console.log(data.responseText)
	console.log(data.status)
	let errors = data.responseJSON 
	$.each(errors, function (i, error) {
		$("#validationErrors ul").append("<li>" + error + "</li>");
	});
}
function onsuccess  (data, status, xhr)  {
	console.log("onsuccess Called")
	console.log(status)
	console.log("xhr")
	console.log(xhr)
	console.log("xhr")
	console.log(data)
	if (data === "Success") {
		$("#registerForm input").val("");
		$("#registerForm .alert-success").removeClass("d-none ");
    }
}
