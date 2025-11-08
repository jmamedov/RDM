window.focusElement = (element) => {
    if (element) {
        element.focus();
    }
};

window.trapFocus = (modalSelector) => {
    const modal = typeof modalSelector === 'string' ? document.querySelector(modalSelector) : modalSelector;
    if (!modal) return;
    const focusableSelectors = [
        'a[href]', 'button:not([disabled])', 'textarea:not([disabled])', 'input:not([disabled])',
        'select:not([disabled])', '[tabindex]:not([tabindex="-1"])'
    ];
    const focusableElements = modal.querySelectorAll(focusableSelectors.join(','));
    if (!focusableElements.length) return;
    const first = focusableElements[0];
    const last = focusableElements[focusableElements.length - 1];

    function handleTrap(e) {
        if (e.key === 'Tab') {
            if (e.shiftKey) {
                if (document.activeElement === first) {
                    e.preventDefault();
                    last.focus();
                }
            } else {
                if (document.activeElement === last) {
                    e.preventDefault();
                    first.focus();
                }
            }
        }
    }
    modal.addEventListener('keydown', handleTrap);
    // Remove event on close
    modal._trapFocusCleanup = () => modal.removeEventListener('keydown', handleTrap);
};

window.releaseTrapFocus = (modalSelector) => {
    const modal = typeof modalSelector === 'string' ? document.querySelector(modalSelector) : modalSelector;
    if (modal && modal._trapFocusCleanup) {
        modal._trapFocusCleanup();
        delete modal._trapFocusCleanup;
    }
};
