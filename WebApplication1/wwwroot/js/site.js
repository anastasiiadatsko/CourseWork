document.addEventListener("DOMContentLoaded", function () {
    console.log("Графіки завантажено");

    const path = window.location.pathname;
    document.querySelectorAll(".nav-link").forEach(link => {
        if (link.getAttribute("href") === path) {
            link.classList.add("fw-bold", "text-primary");
        }
    });
});
