﻿var endpoint = "api/transactions"

$(document).ready(function () {
    getAllTransactions();
});

function printTransaction(transaction) {
    return transaction.Owner + ' - Expense: $' + transaction.Amount;
}

function deleteTransaction() {
    var id = $('#deleteId').val();
    $.ajax({
        type: "DELETE",
        url: endpoint + '/' + id,
        success: function (data) {
            $('.deleteResult').text("Successfully deleted Transaction: " + printTransaction(data))
        },
        dataType: 'json'
    }).fail(function (jqXHR, textStatus, err) {
        $('.deleteResult').text('Error: ' + err);
    });
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

function updateTransaction() {
    var id = $('#updateId').val();
    var amount = $('#amountUpdate').val();
    var owner = $('#ownerUpdate').val();
    var updateTransaction = {
        "Id": id,
        "Amount": amount,
        "Owner": owner,
    };
    $.ajax({
        type: "PUT",
        url: endpoint + '/' + id,
        data: updateTransaction,
        success: function () {
            $('.updateResult').text("Successfully updated ID: " + id)
        },
        dataType: 'json'
    }).fail(function (jqXHR, textStatus, err) {
        $('.updateResult').text('Error: ' + err);
    });
}

function addTransaction() {
    var amount = $('#amountAdd').val();
    var owner = $('#ownerAdd').val();
    var transaction = {
        "Amount": amount,
        "Owner": owner,
    };
    $.post(endpoint, transaction)
        .done(function (data) {
            $('.addResult').text(printTransaction(data))
        })
        .fail(function (jqXHR, textStatus, err) {
            $('.addResult').text('Error: ' + err);
        });
}
