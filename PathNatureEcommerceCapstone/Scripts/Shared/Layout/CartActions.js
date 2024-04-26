
/* AJAX FOR ADD e REMOVE BUTTON IN INDEX */
$(document).on('click', '.addToCartButton', function (e) {
    e.preventDefault(); // Evita il comportamento predefinito del link
    var productId = $(this).data("productid");
    var url = $(this).data("url"); // Ottieni l'URL dall'attributo data

    $.ajax({
        url: url,
        type: 'POST',
        data: { ProductID: productId },
        success: function (result) {
            // Console log per verificare il valore di totalQuantity
            console.log("Total Quantity: " + result.totalQuantity);

            // Verifica se la proprietà totalQuantity è presente nella risposta JSON
            if (result.success) {
                // Incrementa il numero attuale di prodotti nel carrello di uno
                var currentQuantity = parseInt($("#navbarDropdown").text().match(/\d+/)[0]);
                var newQuantity = currentQuantity + 1;
                $("#navbarDropdown").text("Carrello (" + newQuantity + ")");
                // Mostra un messaggio di conferma per l'aggiunta al carrello
                toastr.success("Prodotto aggiunto al carrello!");
            } else {
                // Mostra un messaggio di errore per l'aggiunta al carrello
                toastr.error("Si è verificato un errore durante l'aggiunta al carrello.");
            }
        },
        error: function (xhr, status, error) {
            // Gestisci l'errore qui...
            console.log(xhr.responseText); // Visualizza la risposta completa
            console.log(status); // Visualizza lo status della richiesta
            console.log(error); // Visualizza l'errore
            // Mostra un messaggio di errore generico
            toastr.error("Si è verificato un errore durante l'aggiunta al carrello.");
        }
    });
});


/* AJAX FOR REMOVE BUTTON IN INDEX */
// Funzione per gestire la rimozione di un prodotto dal carrello
$(document).on('click', '.remove-item', function (e) {
    e.preventDefault(); // Evita il comportamento predefinito del link
    var productId = $(this).data("productid");
    var url = $(this).data("url"); // Ottieni l'URL dall'attributo data

    // Memorizza l'elemento del prodotto da rimuovere per la successiva rimozione visuale
    var productToRemove = $(this).closest('.d-flex');

    $.ajax({
        url: url,
        type: 'POST',
        data: { ProductID: productId },
        success: function (result) {
            // Gestisci la risposta qui...
            if (result.success) {
                // Aggiorna dinamicamente il numero di prodotti nel carrello
                $("#navbarDropdown").text("Carrello (" + result.totalQuantity + ")");
                // Rimuovi visivamente il prodotto dal carrello
                productToRemove.remove();
                // Mostra un messaggio di conferma per la rimozione dal carrello
                showAlertMessage(".alert-success", "Prodotto rimosso dal carrello!");
            } else {
                // Mostra un messaggio di errore per la rimozione dal carrello
                showAlertMessage(".alert-danger", result.message);
            }
        },
        error: function (xhr, status, error) {
            // Gestisci l'errore qui...
            showAlertMessage("#alertGenericError", "Si è verificato un errore durante la rimozione dal carrello.");
        }
    });
});

function showAlertMessage(selector, message) {
    // Mostra il messaggio nell'elemento specificato
    $(selector).text(message).show();
    // Nasconde il messaggio dopo 3 secondi
    setTimeout(function () {
        $(selector).hide();
    }, 3000);
}



// Funzione per gestire il checkout
// Aggiungiamo un gestore di eventi per il submit del form

$(document).ready(function () {
    // Ottieni l'URL per CheckoutID dall'attributo data
    var checkoutUrl = $('#checkoutForm').data('checkout-url');


    // Aggiungi un gestore di eventi per il submit del form
    $('#checkoutForm').submit(function (event) {
        // Preveniamo il comportamento predefinito del submit del form
        event.preventDefault();

        console.log("Submit del form avviato"); // Aggiunto un log per il debug
        console.log("URL per CheckoutID:", checkoutUrl);

        // Effettuiamo una chiamata AJAX per inviare il modulo all'azione CheckoutID
        $.ajax({
            url: checkoutUrl,
            type: 'POST',
            data: $('#checkoutForm').serialize(), // Serializziamo i dati del form
            success: function (result) {
                console.log("Risposta AJAX ricevuta:", result); // Stampa la risposta AJAX nella console per debug

                // Costruiamo manualmente l'URL di reindirizzamento
                var redirectUrl = '/Home/Checkout/' + result.orderId;

                // Reindirizziamo alla pagina CheckoutDetails
                window.location.href = redirectUrl;
            },
            error: function (xhr, status, error) {
                // Gestione degli errori
                console.error(error);
                // Possiamo visualizzare un messaggio di errore o eseguire altre azioni di gestione degli errori
            }
        });
    });
});



/* AJAX FOR ADD BUTTON IN DETAILS */
// Funzione per gestire l'aggiunta di un prodotto al carrello quando viene selezionato dalla pagina dei dettagli
$(document).on('click', '.addToCartButtonDetails', function (e) {
    e.preventDefault(); // Evita il comportamento predefinito del link
    var productId = $(this).data("productid");
    var url = $(this).data("url"); // Ottieni l'URL dall'attributo data

    $.ajax({
        url: url,
        type: 'POST',
        data: { ProductID: productId },
        success: function (result) {
            // Verifica se la proprietà totalQuantity è presente nella risposta JSON
            if (result.success && typeof result.totalQuantity !== 'undefined') {
                // Aggiorna dinamicamente il numero di prodotti nel carrello
                var totalQuantity = result.totalQuantity;
                $("#navbarDropdown").text("Carrello (" + totalQuantity + ")");
                // Mostra un messaggio di conferma per l'aggiunta al carrello
                toastr.success("Prodotto aggiunto al carrello!");
            } else {
                // Mostra un messaggio di errore per l'aggiunta al carrello
                toastr.error("Si è verificato un errore durante l'aggiunta al carrello.");
            }
        },
        error: function (xhr, status, error) {
            // Gestisci l'errore qui...
            console.log(xhr.responseText); // Visualizza la risposta completa
            console.log(status); // Visualizza lo status della richiesta
            console.log(error); // Visualizza l'errore
            // Mostra un messaggio di errore generico
            $("#alertGenericError").text("Si è verificato un errore durante l'aggiunta al carrello.").show();
            // Nasconde il messaggio dopo 3 secondi
            setTimeout(function () {
                $("#alertGenericError").hide();
            }, 3000);
        }
    });
});

//----------------------------------------------------------------------------------------------------
///* AJAX FOR decrease BUTTON IN CHEKOUT */
//// Funzione per gestire la rimozione di un prodotto dal carrello
//$(document).on('click', '.decreaseQtyButton', function (e) {
//    e.preventDefault(); // Evita il comportamento predefinito del link
//    var productId = $(this).data("productid");
//    var url = $(this).data("url"); // Ottieni l'URL dall'attributo data

//    $.ajax({
//        url: url,
//        type: 'POST',
//        data: { ProductID: productId },
//        success: function (result) {
//            // Gestisci la risposta qui...
//            if (result.success) {
//                // Aggiorna dinamicamente il numero di prodotti nel carrello
//                var totalQuantity = result.totalQuantity;
//                $("#navbarDropdown").text("Carrello (" + totalQuantity + ")");
//                // Mostra un messaggio di conferma per la diminuzione della quantità nel carrello
//                $(".alert-success").text("Quantità del prodotto diminuita nel carrello!").show();
//                // Nasconde il messaggio dopo 3 secondi
//                setTimeout(function () {
//                    $(".alert-success").hide();
//                }, 3000);
//            } else {
//                // Mostra un messaggio di errore per la diminuzione della quantità nel carrello
//                $(".alert-danger").text(result.message).show(); // Mostra il messaggio di errore restituito dal controller
//                // Nasconde il messaggio dopo 3 secondi
//                setTimeout(function () {
//                    $(".alert-danger").hide();
//                }, 3000);
//            }
//        },
//        error: function (xhr, status, error) {
//            // Gestisci l'errore qui...
//            console.log(xhr.responseText); // Visualizza la risposta completa
//            console.log(status); // Visualizza lo status della richiesta
//            console.log(error); // Visualizza l'errore
//            // Mostra un messaggio di errore generico
//            $("#alertGenericError").text("Si è verificato un errore durante la diminuzione della quantità nel carrello.").show();
//            // Nasconde il messaggio dopo 3 secondi
//            setTimeout(function () {
//                $("#alertGenericError").hide();
//            }, 3000);
//        }
//    });
//});


/* AJAX FOR REMOVE BUTTON IN CHEKOUT */
// Funzione per gestire la rimozione di un prodotto dal carrello
$(document).on('click', '.decreaseQty', function (e) {
    e.preventDefault(); // Evita il comportamento predefinito del link

    var productId = $(this).data("productid");

    $.ajax({
        url: '/Home/DecreaseQty', // URL corretto per DecreaseQty
        type: 'POST',
        data: { productId: productId },
        success: function (result) {
            if (result.success) {
                alert("Prodotto rimosso dal carrello!");
            } else {
                alert("Impossibile rimuovere il prodotto dal carrello.");
            }
        },
        error: function () {
            alert("Si è verificato un errore durante la richiesta.");
        }
    });
});


/* AJAX FOR ADD BUTTON IN CHEKOUT */
// Funzione per gestire la aggiunta di un prodotto dal carrello
$(document).on('click', '.increaseQty', function (e) {
    e.preventDefault(); // Evita il comportamento predefinito del link

    var productId = $(this).data("productid");

    $.ajax({
        url: '/Home/IncreaseQty', // URL corretto per IncreaseQty
        type: 'POST',
        data: { productId: productId },
        success: function (result) {
            if (result.success) {
                alert("Prodotto aggiunto al carrello!");
            } else {
                alert("Impossibile aggiungere il prodotto al carrello.");
            }
        },
        error: function () {
            alert("Si è verificato un errore durante la richiesta.");
        }
    });
});

/* AJAX FOR CONTINUE BUTTON IN CHEKOUT */

$(".continueButton").click(function (e) {
    e.preventDefault(); // Evita il comportamento predefinito del link

    $.ajax({
        url: '@Url.Action("CheckoutDetails", "Home")',
        type: 'GET',
        success: function (result) {
            // Reindirizza l'utente alla pagina di checkout
            window.location.href = result.url;
        },
        error: function () {
            // Gestisci eventuali errori
            alert("Si è verificato un errore durante il recupero dei dettagli del carrello.");
        }
    });
});





