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

function showLoading() {
    document.getElementById('loadingOverlay').style.display = 'flex';
}

function hideLoading() {
    document.getElementById('loadingOverlay').style.display = 'none';
}

document.addEventListener('DOMContentLoaded', function () {
    checkSidebarState();
    highlightActiveLink();

    showLoading();

    setTimeout(function () {
        hideLoading();
    }, 800); // Adjust the timeout duration as needed

    // Show loading overlay on navigation
    document.querySelectorAll('a.nav-link').forEach(link => {
        link.addEventListener('click', function () {
            showLoading();
        });
    });
});
