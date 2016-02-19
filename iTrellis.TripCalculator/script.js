var endpoint = "api/transactions"

$(document).ready(function () {
    getAllTransactions();
});

function printTransaction(transaction) {
    return transaction.Owner + ' - Expense: $' + transaction.Amount;
}

function getAllTransactions() {
    $.getJSON(endpoint)
        .done(function (data) {
            $.each(data, function (key, transaction) {
                $('<li>', { text: printTransaction(transaction) })
                    .appendTo($('.transactions'));
            });
        })
        .fail(function (jqXHR, textStatus, err) {
            $('<li>', { text: 'Error: ' + err })
                .appendTo($('.transactions'));
        });
}

function getSplits() {
    var endpoint = "api/calculate"
    // clear out any old data
    $('.splits').empty();
    $.getJSON(endpoint)
        .done(function (data) {
            $.each(data, function (key, split) {
                $('<li>', { text: split })
                    .appendTo($('.splits'));
            });
        })
        .fail(function (jqXHR, textStatus, err) {
            $('.transaction').text('Error: ' + err);
        });
}

function lookupById() {
    var id = $('#transactionId').val();
    $.getJSON(endpoint + '/' + id)
        .done(function (transaction) {
            $('.transaction').text(printTransaction(transaction))
        })
        .fail(function (jqXHR, textStatus, err) {
            $('.transaction').text('Error: ' + err);
        });
}

function lookupByOwner() {
    var owner = $('#transactionOwner').val();
    // clear out any old data
    $('.transactionsByOwner').empty();
    $.getJSON(endpoint + '/' + owner)
        .done(function (data) {
            $.each(data, function (key, transaction) {
                $('<li>', { text: printTransaction(transaction) })
                    .appendTo($('.transactionsByOwner'));
            });
        })
        .fail(function (jqXHR, textStatus, err) {
            $('<li>', { text: 'Error: ' + err })
                .appendTo($('.transactionsByOwner'));
        });
}
