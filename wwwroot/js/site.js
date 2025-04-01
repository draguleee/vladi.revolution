function toggleDarkMode() {
    document.documentElement.classList.toggle('dark-mode');
    const isDarkMode = document.documentElement.classList.contains('dark-mode');
    localStorage.setItem('darkMode', isDarkMode ? 'enabled' : 'disabled');
    updateModeIcon(isDarkMode);
}

function updateModeIcon(isDarkMode) {
    const modeIcon = document.getElementById('modeIcon');
    if (isDarkMode) {
        modeIcon.innerHTML = '<i class="fas fa-moon"></i>';
    } else {
        modeIcon.innerHTML = '<i class="bi bi-brightness-high"></i>';
    }
}

window.onload = function () {
    const darkModeToggle = document.getElementById('darkModeToggle');
    const modeIcon = document.getElementById('modeIcon');
    const darkMode = localStorage.getItem('darkMode');
    const isDarkMode = (darkMode === 'enabled');

    if (isDarkMode) {
        document.documentElement.classList.add('dark-mode');
        darkModeToggle.checked = true;
    } else {
        darkModeToggle.checked = false;
    }

    // Call updateModeIcon to sync the icon with the mode
    updateModeIcon(isDarkMode);
};