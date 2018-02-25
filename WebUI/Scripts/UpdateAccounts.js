function updateAccounts() {
    $.when(
        $.ajax({
                url: '/NavLeft/GetAccounts',
                cache: false,
                success: function (data) {
                                $('#myAccounts').empty();
                                $('#myAccounts').html(data);
                            }
            }),
            $.ajax({
                url: '/NavLeft/GetBudgets',
                cache: false,
                success: function(budgets) {
                    $('#myBudgets').empty();
                    $('#myBudgets').html(budgets);
                }
            }),
            $.ajax({
                url: '/PayingItem/ExpensiveCategories',
                cache: false,
                success: function(data) {
                    $('#expensiveCategories').empty();
                    $('#expensiveCategories').html(data);
                }
            }),
            $.ajax({
                url: '/NavRight/MenuIncoming',
                cache: false,
                success: function(data) {
                    $('#income').empty();
                    $('#income').html(data);
                }
            }),
            $.ajax({
                url: '/NavRight/MenuOutgo',
                cache: false,
                success: function (data) {
                                $('#outgo').empty();
                                $('#outgo').html(data);
                            }
            })
    )
.then(function () {
            
        });
}

function updateAfterTransfer() {
    $.when(
            $.ajax({
                url: '/NavLeft/GetAccounts',
                cache: false,
                success: function (data) {
                    var target = $('#myAccounts');
                    target.empty();
                    target.html(data);
                }
            }),
            $.ajax({
                url: '/NavLeft/GetBudgets',
                cache: false,
                success: function (budgets) {
                    var target = $('#myBudgets');
                    target.empty();
                    target.html(budgets);
                }
            })
        )
        .then(function () {
            
        });
}

function updateAfterDebt() {
    $.when(
            $.ajax({
                url: '/NavLeft/GetAccounts',
                cache: false,
                success: function(data) {
                    var target = $('#myAccounts');
                    target.empty();
                    target.html(data);
                }
            }),
            $.ajax({
                url: '/NavLeft/GetBudgets',
                cache: false,
                success: function(budgets) {
                    var target = $('#myBudgets');
                    target.empty();
                    target.html(budgets);
                }
            }),
            $.ajax({
                url: '/NavRight/MenuIncoming',
                cache: false,
                success: function(data) {
                    $('#income').empty();
                    $('#income').html(data);
                }
            }),
            $.ajax({
                url: '/NavRight/MenuOutgo',
                cache: false,
                success: function(data) {
                    $('#outgo').empty();
                    $('#outgo').html(data);
                }
            })
        )
        .then(function() {
            $.ajax({
                url: '/Debt/Index',
                cache: false,
                success: function(data) {
                    var target = $('#myDebts');
                    target.empty();
                    target.html(data);
                }
            });
        });
}