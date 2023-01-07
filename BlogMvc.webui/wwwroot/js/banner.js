// Slide-edit
var slider_main = document.getElementById("slider-main-id");
var slider_edit = document.getElementById("slide-edit-id");
slider_main.addEventListener("mouseover",slidermainover);
slider_main.addEventListener("mouseout",slidermainout);
function slidermainout()
{
    slider_edit.style.display="none";
}
function slidermainover()
{
    slider_edit.style.display="block";
}

// Slide-banner 

let slideIndex = 0;
showSlides();

function showSlides() {
  let i;
  let slides = document.getElementsByClassName("slide");
  let dots = document.getElementsByClassName("slide-count");
  // slides[sliderların sayısı kadar i'nin tektek style.display=none yap];
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";  
  }
  //slideIndex dışarıdan 0 olarak gelir ve 1 arttırılır.
  slideIndex++;
  // eğer slideIndex değişkeni slides.length'den büyük ise slideIndex = 1'e eşitle
  // bu şekilde slider tekrar başa sarabilir...
  if (slideIndex > slides.length) 
  {slideIndex = 1}    
  // dots[i değişkenini dots.length kadar tek tek arttır.]
  // artarken className'sine "active" classını ekle.
  for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" countcolor", "");
  }
  // slides[slideIndex-1] = bir önceki nesnenin
  slides[slideIndex-1].style.display = "block";  
  dots[slideIndex-1].className += " countcolor";
  setTimeout(showSlides, 3500); // Change image every 2 seconds
}





// var models = [
//     {
//         header:'BLOCKCHAİN VE NFT',
//         text:' Solidity - Trade - NFT - Blokchain ',
//         image:'/images/web_bg4.jpg'
//     },
//     {
//         header:'WEB TASARIM VE GELİŞTİRME',
//         text:'HTML - CSS - JavaScript - jQuery - Asp.Net - Entity Framework - MVC',
//         image:'/images/web_bg.jpg'
//     },
//     {
//         header:'BİLGİSAYAR PROGRAMCILIĞI',
//         text:'C# - C++ - .Net Core - MySQL - MsSQL - React Native - Unity',
//         image:'/images/web_bg2.jpg'
//     },
//     {
//         header:'MİMARİ VE STATİK YAPI PROJELERİ',
//         text:'Autocad - SAP2000 - Sta4Cad - Sketchup - Blender - Adobe Photoshop',
//         image:'/images/web_bg3.jpg'
//     },
    
// ];
// var slide_circle = document.querySelectorAll(".fa-solid");
// var index=0;
// var SlaytCount = models.length;
// var interval;
// var settings=
// {
//     duration : '3000',
//     random : false
// };

// init(settings);


// document.querySelector('.fa-chevron-left').addEventListener('click',function () {
//     index--;
//     slide_range(index);
//     showSlide(index);
// });

// document.querySelector(".fa-chevron-right").addEventListener('click',function () {
//     index++;
//     slide_range(index);
//     showSlide(index);
// });

// document.querySelectorAll('.slide-content').forEach(function (item){
//     item.addEventListener('mouseenter',function (){
//         clearInterval(interval);
//     })
// });
// document.querySelectorAll('.slide-content').forEach(function (item){
//     item.addEventListener('mouseleave',function (){
//         init(settings);
//     })
// });

// function init(settings) {
//     var prev;
//     interval=setInterval(function(){

//         if (settings.random)
//         {
//              //random index
//             do{
//                 index = Math.floor(Math.random()*SlaytCount);
//             } while (index == prev);
//             prev=index;
//         }
//         else
//         {
//             // artan index
//             if(SlaytCount == index+1)
//             {
//                 index=-1;
//             }
//             showSlide(index);
//             index++;
//         }
//         showSlide(index);
//     },settings.duration)
// }

// function showSlide(i) {

//     index = i;

//     if (i<0){
//         index = SlaytCount -1;
//     }
//     if (i >= SlaytCount){
//         index = 0;
//     }
//     document.querySelector("#headerid").innerHTML=models[index].header;
//     document.querySelector("#textid").innerHTML=models[index].text;
//     document.querySelector(".slide-img").setAttribute('src',models[index].image);
// }

// function slide_range(i){
//     index=i;
    
//     slide_circle.forEach(function(slide,i){
//         slide.style.opacity=`${1}`;
//     });
//     showSlide(index);
// }

