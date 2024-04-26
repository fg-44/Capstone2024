
/* AJAX CODE FOR RESEARCH*/
document.addEventListener('DOMContentLoaded', function () {
    const searchForm = document.getElementById('searchForm');
    const searchInput = document.getElementById('searchInput');
    const clearSearchIcon = document.getElementById('clearSearchIcon');
    const searchResults = document.getElementById('searchResults');

    // Aggiungi un listener per l'input
    searchInput.addEventListener('input', function () {
        // Controlla se l'input ha del testo
        if (this.value.length > 0) {
            // Se c'è del testo, mostra l'icona di cancellazione
            clearSearchIcon.classList.remove('d-none');
        } else {
            // Altrimenti, nascondi l'icona di cancellazione
            clearSearchIcon.classList.add('d-none');
        }
    });

    // Aggiungi un listener per cliccare sull'icona di cancellazione
    clearSearchIcon.addEventListener('click', function () {
        // Cancella il testo dall'input
        searchInput.value = '';
        // Nascondi l'icona di cancellazione
        clearSearchIcon.classList.add('d-none');
        // Cancella i risultati della ricerca
        searchResults.innerHTML = '';
    });

    // Aggiungi un listener per la sottomissione del form di ricerca
    searchForm.addEventListener('submit', function (event) {
        // Evita il comportamento predefinito del form
        event.preventDefault();
        // Esegui la ricerca
        search();
    });

    // Funzione per eseguire la ricerca
    function search() {
        // Ottieni il testo di ricerca
        const query = searchInput.value.trim();

        // Esegui una richiesta AJAX solo se la query non è vuota
        if (query !== '') {
            // Effettua una richiesta AJAX
            // Sostituisci questo con la tua logica di ricerca AJAX
            // Qui puoi inviare la query al tuo backend e ricevere i risultati
            // In questa demo, mostriamo solo la query di ricerca
            searchResults.innerHTML = `<p>Risultati della ricerca per: <strong>${query}</strong></p>`;
        } else {
            // Se la query è vuota, cancella i risultati della ricerca
            searchResults.innerHTML = '';
        }
    }
});
