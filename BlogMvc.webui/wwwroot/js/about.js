// About
const fh5co_about_main = document.querySelectorAll("#fh5co-about-main");
const edit_delete_item = document.querySelectorAll("#edit-delete-cont-id");

    for (let i = 0; i < fh5co_about_main.length; i++) {
        fh5co_about_main[i].addEventListener("mousemove",function aboutmainmove(){
            edit_delete_item[i].style.visibility="visible";
        });
        fh5co_about_main[i].addEventListener("mouseleave",function aboutmainleave(){
            edit_delete_item[i].style.visibility="hidden";
        });
    }
// School
const school_main = document.getElementById("school-main-id");
const school_edit = document.getElementById("school-edit-id");
school_main.addEventListener("mouseover",aboutschoolover);
school_main.addEventListener("mouseout",aboutschoolout);
function aboutschoolout()
{
    school_edit.style.display="none";
}
function aboutschoolover()
{
    school_edit.style.display="block";
}



