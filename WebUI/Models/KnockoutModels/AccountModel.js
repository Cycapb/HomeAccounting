var accountViewModel;

function Account(accountId, accountName, cash, use, userId) {
    var self = this;
    self.AccountId = ko.observable(accountId);
    self.AccountName = ko.observable(accountName);
    self.Cash = ko.observable(cash);
    self.Use = ko.observable(use);
    self.UserId = ko.observable(userId);

    self.editAccount = function() {
        alert("!!!!");
    }
}

function AccountList() {
    var self = this;

    self.accounts = ko.observableArray([]);
    
    self.getAccounts = function () {
        self.accounts.removeAll();
        $.ajax({
            url: "/Account/GetAccountsJson/",
            type: "GET",
            contentType: "application/json",
            success: function(data) {
                $.each(data,
                    function(key, value) {
                        self.accounts.push(new Account(value.AccountId,
                            value.AccountName,
                            value.Cash,
                            value.Use,
                            value.UserId));
                    });
            }
        });
    }
}

$(document).ready(function () {
    accountViewModel = {
        accountListViewModel: new AccountList()
}
    ko.applyBindings(accountViewModel);
    accountViewModel.accountListViewModel.getAccounts();
});