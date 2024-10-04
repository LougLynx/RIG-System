function toggleNav() {
    const sidebar = document.getElementById("mySidebar");
    const main = document.getElementById("main");
    const sidebarState = localStorage.getItem('sidebarState');

    if (sidebarState === 'open') {
        sidebar.style.width = "0";
        main.style.marginLeft = "0";
        localStorage.setItem('sidebarState', 'closed');
    } else {
        sidebar.style.width = "250px";
        main.style.marginLeft = "250px";
        localStorage.setItem('sidebarState', 'open');
    }
}

function checkSidebarState() {
    const sidebarState = localStorage.getItem('sidebarState');
    if (sidebarState === 'open') {
        document.getElementById("mySidebar").style.width = "250px";
        document.getElementById("main").style.marginLeft = "250px";
    } else {
        document.getElementById("mySidebar").style.width = "0";
        document.getElementById("main").style.marginLeft = "0";
    }
}

function highlightActiveLink() {
    const links = document.querySelectorAll('#mySidebar a.nav-link');
    const currentUrl = window.location.href;

    links.forEach(link => {
        if (link.href === currentUrl) {
            link.style.backgroundColor = 'red';
        }
    });
}

document.addEventListener('DOMContentLoaded', function () {
    checkSidebarState();
    highlightActiveLink();
});
