// Creating Popup Navbar
let navbar = document.querySelector('.navbar');


document.addEventListener('click', function (e) {
    if (!menu.contains(e.target) && !navbar.contains(e.target)) {
        menu.classList.remove('fa-times');
        navbar.classList.remove('active');
    };

    if (!searchIcon.contains(e.target) && !searchForm.contains(e.target)) {
        searchIcon.classList.remove('fa-times');
        searchForm.classList.remove('active');
    };
});

let section = document.querySelectorAll('section');
let navLinks = document.querySelectorAll('header .navbar a');

window.onscroll = () => {
    
    menu.classList.remove('fa-times');
    navbar.classList.remove('active');
    
    section.forEach(sec =>{

        let top = window.scrollY;
        let height = sec.offsetHeight;
        let offset = sec.offsetTop - 150;
        let id = sec.getAttribute('id');

        if (top >= offset && top < offset + height) {
            navLinks.forEach(links =>{
                links.classList.remove('active');
                document.querySelector('header .navbar a[href*='+id+']').classList.add('active');
            });
        };
        
    });
  };

var swiper = new Swiper(".home-slider", {
    spaceBetween: 30,
    centeredSlides: true,
    autoplay: {
      delay: 2500,
      disableOnInteraction: false,
    },
    loop:true
  });

var swiper = new Swiper(".review-slider", {
    spaceBetween: 30,
    centeredSlides: true,
    autoplay: {
      delay: 2500,
      disableOnInteraction: false,
    },
    loop:true,
    breakpoints: {
        0: {
            slidesPerView: 1,
        },
        640: {
            slidesPerView: 2,
        },
        768: {
            slidesPerView: 2,
        },
        1024: {
            slidesPerView: 3,
        },
    }
  });