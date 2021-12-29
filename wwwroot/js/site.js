const lobbycards = document.querySelectorAll(".game__card");
const historyListItems = document.querySelectorAll(".history__list__item");

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

