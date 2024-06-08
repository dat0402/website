document.addEventListener('DOMContentLoaded', function () {
    const range = document.getElementById('range');
    const lowerThumb = document.getElementById('lower-thumb');
    const upperThumb = document.getElementById('upper-thumb');
    const lowerValue = document.getElementById('lower-value');
    const upperValue = document.getElementById('upper-value');

    let lower = 25;
    let upper = 75;
    let activeThumb = null;

    lowerThumb.addEventListener('mousedown', () => activeThumb = 'lower');
    upperThumb.addEventListener('mousedown', () => activeThumb = 'upper');

    document.addEventListener('mouseup', () => activeThumb = null);

    document.addEventListener('mousemove', (event) => {
        if (activeThumb) {
            const rect = range.getBoundingClientRect();
            const offsetX = event.clientX - rect.left;
            const percent = (offsetX / rect.width) * 100;

            if (activeThumb === 'lower') {
                if (percent >= 0 && percent <= upper - 1) {
                    lower = percent;
                    lowerThumb.style.left = `${lower}%`;
                    lowerValue.textContent = Math.round(lower);
                }
            } else if (activeThumb === 'upper') {
                if (percent <= 100 && percent >= lower + 1) {
                    upper = percent;
                    upperThumb.style.left = `${upper}%`;
                    upperValue.textContent = Math.round(upper);
                }
            }
        }
    });
});