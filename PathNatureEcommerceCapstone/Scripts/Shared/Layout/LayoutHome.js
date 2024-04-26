window.addEventListener('DOMContentLoaded', event => {

    // Toggle the side navigation
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        // Uncomment Below to persist sidebar toggle between refreshes
        // if (localStorage.getItem('sb|sidebar-toggle') === 'true') {
        //     document.body.classList.toggle('sb-sidenav-toggled');
        // }
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sb-sidenav-toggled');
            localStorage.setItem('sb|sidebar-toggle', document.body.classList.contains('sb-sidenav-toggled'));
        });
    }

});

window.addEventListener('DOMContentLoaded', () => {
    // Ottieni il dropdown del carrello
    const dropdownCart = document.getElementById('dropdownCart');

    // Verifica se il dropdown del carrello esiste
    if (dropdownCart) {
        // Aggiungi un gestore di eventi per il clic sul dropdown
        dropdownCart.addEventListener('click', (event) => {
            // Evita il comportamento predefinito del link
            event.preventDefault();
            // Apri o chiudi il dropdown del carrello
            dropdownCart.nextElementSibling.classList.toggle('show');
        });
    }
});



