﻿function toggleNav() {
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
            link.classList.add('active');
        }
    });
}

function showLoading() {
    document.getElementById('loadingOverlay').style.display = 'flex';
}

function hideLoading() {
    document.getElementById('loadingOverlay').style.display = 'none';
}

function updateClock() {
    const now = new Date();
    const options = { 
        weekday: 'long', 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    };
    document.getElementById('currentTime').textContent = now.toLocaleDateString('en-US', options);
}

document.addEventListener('DOMContentLoaded', function () {
    checkSidebarState();
    highlightActiveLink();
    
    // Initialize and update clock
    updateClock();
    setInterval(updateClock, 1000);

    showLoading();

    setTimeout(function () {
        hideLoading();
    }, 800); // Adjust the timeout duration as needed

    // Show loading overlay on navigation, excluding dropdown links
    document.querySelectorAll('a.nav-link').forEach(link => {
        link.addEventListener('click', function (event) {
            if (!link.classList.contains('dropdown-toggle')) {
                showLoading();
            }
        });
    });

    // Adjust layout when dropdown is toggled
    document.querySelectorAll('.dropdown-toggle').forEach(dropdown => {
        dropdown.addEventListener('click', function (event) {
            event.preventDefault(); // Prevent default link behavior
            const dropdownMenu = this.nextElementSibling;
            if (dropdownMenu.classList.contains('show')) {
                dropdownMenu.classList.remove('show');
            } else {
                document.querySelectorAll('.dropdown-menu').forEach(menu => menu.classList.remove('show'));
                dropdownMenu.classList.add('show');
            }
        });
    });

    // Close dropdowns when clicking outside
    document.addEventListener('click', function(event) {
        if (!event.target.matches('.dropdown-toggle')) {
            document.querySelectorAll('.dropdown-menu.show').forEach(menu => {
                menu.classList.remove('show');
            });
        }
    });
});

