window.rdmShortcuts = {
 dotNetRef: null,
 init: function(dotNetRef) {
 this.dotNetRef = dotNetRef;
 window.addEventListener('keydown', this.handleKeyDown);
 },
 dispose: function() {
 window.removeEventListener('keydown', this.handleKeyDown);
 this.dotNetRef = null;
 },
 handleKeyDown: function(e) {
 if (e.altKey && !e.shiftKey && !e.ctrlKey && !e.metaKey) {
 // Don't trigger if a modal is open (optional: check for modal overlays)
 if (document.querySelector('.modal-overlay')) return;
 if (e.code === 'KeyN') {
 e.preventDefault();
 window.rdmShortcuts.dotNetRef && window.rdmShortcuts.dotNetRef.invokeMethodAsync('ShowNodeDialogFromShortcut');
 }
 if (e.code === 'KeyC') {
 e.preventDefault();
 window.rdmShortcuts.dotNetRef && window.rdmShortcuts.dotNetRef.invokeMethodAsync('ShowBomDialogFromShortcut');
 }
 }
 }
};
