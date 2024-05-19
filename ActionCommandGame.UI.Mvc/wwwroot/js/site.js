document.addEventListener('DOMContentLoaded', () => {
    const rows = document.querySelectorAll('tr');

    rows.forEach((row, index) => {
        if (index === 0) return; // Skip the first row (header)

        // Set data-rank starting from 1 for the second row (index 1)
        row.dataset.rank = index;

        row.addEventListener('mouseover', () => {
            rows.forEach(r => r.style.backgroundColor = ''); // Reset all rows
            if (index === 1) {
                row.style.backgroundColor = 'gold';
            } else if (index === 2) {
                row.style.backgroundColor = 'silver';
            } else if (index === 3) {
                row.style.backgroundColor = 'bronze';
            }
        });

        row.addEventListener('mouseout', () => {
            row.style.backgroundColor = ''; // Reset the color when mouse leaves
        });
    });
});
