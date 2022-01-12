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


const handleMove = (event) => {
    console.log(clickAudio);
    clickAudio.play();
    return false;
};

