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
        const maxHeight = containerHeight - 200; // Leave at least 200px for bottom panel

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