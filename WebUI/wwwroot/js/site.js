const lobbycards = document.querySelectorAll(".game__card");
const historyListItems = document.querySelectorAll(".history__list__item");

const clickAudio = new Audio('../../audio/click_sound.mp3');

const observer = new IntersectionObserver(entries => {
    entries.forEach(entry => {
        entry.target.classList.toggle("show", entry.isIntersecting);
        // if(entry.isIntersecting) observer.unobserve(entry.target);
    });
}, {
    threshold: 0.45,
});

lobbycards.forEach(card => {
    observer.observe(card);
});

historyListItems.forEach(item => {
    observer.observe(item);
});

//onsubmit="return handleMove(this)" 

/*document.getElementById("reversiboardform").addEventListener("submit", (event) => {
   
    event.preventDefault();
    clickAudio.play();

    let x = parseInt(event.submitter.getAttribute('x'));
    let y = parseInt(event.submitter.getAttribute('y'));

    let data = {
        x: x,
        y: y,
        hasPassed: false
    }

    console.log("X:", x);
    console.log("Y:", y);

    connection.invoke("OnMove", data);

    *//*return false;*//*
});*/

const handleMove = (event) => {
    /*console.log(event);
    clickAudio.play();
    return false;*/
};

