// Grid Resize Functionality
window.initializeResizer = function () {
    const resizeHandle = document.getElementById('resizeHandle');
    const nodesPanel = document.getElementById('nodesPanel');
    const bomPanel = document.getElementById('bomPanel');

    if (!resizeHandle || !nodesPanel || !bomPanel) {
        // Retry after a short delay if elements aren't ready
        setTimeout(window.initializeResizer, 100);
        return;
    }

    let isResizing = false;
    let startY = 0;
    let startNodesPanelHeight = 0;

    resizeHandle.addEventListener('mousedown', function (e) {
        isResizing = true;
        startY = e.clientY;
        startNodesPanelHeight = nodesPanel.offsetHeight;

        document.body.style.cursor = 'ns-resize';
        document.body.style.userSelect = 'none';

        e.preventDefault();
    });

    document.addEventListener('mousemove', function (e) {
        if (!isResizing) return;

        const deltaY = e.clientY - startY;
        const newHeight = startNodesPanelHeight + deltaY;
        const containerHeight = nodesPanel.parentElement.offsetHeight;
        const minHeight = 200;
        const maxHeight = containerHeight - 200;

        if (newHeight >= minHeight && newHeight <= maxHeight) {
            nodesPanel.style.height = newHeight + 'px';
            nodesPanel.style.flex = 'none';
        }
    });

    document.addEventListener('mouseup', function () {
        if (isResizing) {
            isResizing = false;
            document.body.style.cursor = '';
            document.body.style.userSelect = '';
        }
    });
};

// Scroll row into view for keyboard navigation
window.scrollRowIntoView = function (tableElement, rowIndex) {
    try {
        const tbody = tableElement.querySelector('tbody');
        if (!tbody) return;

        const rows = tbody.querySelectorAll('tr[data-index]');
        if (rowIndex < 0 || rowIndex >= rows.length) return;

        const row = rows[rowIndex];
        if (!row) return;

        row.scrollIntoView({
            behavior: 'smooth',
            block: 'nearest',
            inline: 'nearest'
        });
    } catch (error) {
        console.error('Error scrolling row into view:', error);
    }
};