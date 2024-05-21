// Ensure the section is displayed initially based on localStorage
document.addEventListener('DOMContentLoaded', function () {
    var section = document.getElementById('howToPlaySection');
    var button = document.getElementById('toggleButton');
    var sectionState = localStorage.getItem('howToPlaySectionState');

    if (sectionState === 'hidden') {
        section.style.display = 'none';
        button.innerHTML = '&#9660;'; // Down arrow
    } else {
        section.style.display = 'block';
        button.innerHTML = '&#9650;'; // Up arrow
    }
});

function toggleSection() {
    var section = document.getElementById('howToPlaySection');
    var button = document.getElementById('toggleButton');

    if (section.style.display === 'block') {
        section.style.display = 'none';
        button.innerHTML = '&#9660;'; // Down arrow
        localStorage.setItem('howToPlaySectionState', 'hidden');
    } else {
        section.style.display = 'block';
        button.innerHTML = '&#9650;'; // Up arrow
        localStorage.setItem('howToPlaySectionState', 'visible');
    }
}