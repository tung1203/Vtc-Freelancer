﻿var slideIndex = 1;
showSlides(slideIndex);

// Write your JavaScript code.
function plusSlides(n) {
  showSlides((slideIndex += n));
}

function currentSlide(n) {
  showSlides((slideIndex = n));
}
setInterval(() => {
  let a = 1;
  plusSlides(a);
  a += 1;
}, 2000);

function showSlides(n) {
  var i;
  var slides = document.getElementsByClassName("mySlides");
  var dots = document.getElementsByClassName("dot");
  if (n > slides.length) {
    slideIndex = 1;
  }
  if (n < 1) {
    slideIndex = slides.length;
  }
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" active", "");
  }
  slides[slideIndex - 1].style.display = "block";
  dots[slideIndex - 1].className += " active";
}

const btn = document.getElementsByClassName("btn-footer-link");
for (let i = 0; i < btn.length; i++) {
  btn[i].addEventListener("click", function() {
    var content = this.nextElementSibling;
    if (content.style.display === "block") {
      content.style.display = "none";
    } else {
      content.style.display = "block";
    }
  });
}

function Checkpass() {
  var a = document.getElementById("p1").value;
  var b = document.getElementById("p2").value;
  if (a !== b) {
    document.getElementById("error").innerHTML = "Invalid password";
    document.getElementById("but").disabled = true;
  } else {
    document.getElementById("error").innerHTML = "Valid Password";
    document.getElementById("but").disabled = false;
  }
}
window.addEventListener("file", function() {
  document
    .querySelector('input[type="file"]')
    .addEventListener("change", function() {
      if (this.files && this.files[0]) {
        var img = document.querySelector("img"); // $('img')[0]
        img.src = URL.createObjectURL(this.files[0]); // set src to blob url
        img.onload = imageIsLoaded;
      }
    });
});
