var todoCreateViewModel;

function UserSaveRequest(data) {
    var self = this;
    self.Name = data.Name;
    self.Comment = data.Comment;
    self.GroupId = data.GroupId;
    self.StatusId = data.StatusId;
    self.GroupName = data.GroupName;
    self.UserId = data.UserId;
    self.StatusId = false;
}

function TodoModel(todo) {
    var self = this;
    self.NoteId = ko.observable(todo.NoteId);
    self.Name = ko.observable(todo.Name);
    self.Comment = ko.observable(todo.Comment);
    self.GroupId = ko.observable(todo.groupId);
    self.StatusId = ko.observable(todo.statusId);
    self.GroupName = ko.observable(todo.groupName);
    self.UserId = ko.observable(todo.userId);
}

function Group(groupId, name, userId) {
    var self = this;
    self.GroupId = ko.observable(groupId);
    self.Name = ko.observable(name);
    self.UserId = ko.observable(userId);

    self.addGroup = function () {
        this.UserId = currentUserId;
        var dataObject = ko.toJSON(this);
        $.ajax({
            url: "/api/group/",
            data: dataObject,
            type: "POST",
            contentType: "application/json",
            success: function() {
                todoCreateViewModel.addTodoViewModel.getGroups();
                todoCreateViewModel.groupListViewModel.getGroups();
                self.Name("");
            }
    });
    }
}

function GroupList() {
    var self = this;
    self.allGroups = ko.observableArray([]);

    self.getGroups = function () {
        self.allGroups.removeAll();
        $.getJSON("/api/group/" + currentUserId, function (data) {
            $.each(data, function (key, value) {
                                    self.allGroups.push(new Group(value.GroupId, value.Name, value.UserId));                
            });
        });
    }

    self.deleteGroup = function(group) {
        $.ajax({
            url: "/api/group/" + group.GroupId(),
            type: "delete",
            contentType: "application/json",
            success: function () {
                self.allGroups.remove(group);
                todoCreateViewModel.addTodoViewModel.getGroups();
            }
        });
    }

}

function Todo(noteId, name, comment, statusId, groupId, groupName, userId) {
    var self = this;
    self.NoteId = ko.observable(noteId);
    self.Name = ko.observable(name);
    self.Comment = ko.observable(comment);
    self.GroupId = ko.observable(groupId);
    self.StatusId = ko.observable(statusId);
    self.GroupName = ko.observable(groupName);
    self.UserId = ko.observable(userId);

    self.groups = ko.observableArray([]);

    self.addTodo = function () {
        this.UserId = currentUserId;
        if (this.GroupId() != null) {
            this.GroupName = this.GroupId().Name;
        }
        var queryObject = new UserSaveRequest(this);
        if (this.GroupId() != null) {
            queryObject.GroupId = this.GroupId().GroupId;
        }
            var dataObject = ko.toJSON(queryObject);

            $.ajax({
                url: "/api/todo/",
                type: "POST",
                data: dataObject,
                contentType: "application/json",
                success: function(data) {
                    todoCreateViewModel.todoListViewModel.getTodos();
                    self.Name("");
                    self.Comment("");
                    self.generalErrors.removeAll();
                },
                    statusCode: {
                        400: function(data) {
                            if (typeof data.responseJSON.ModelState !== "undefined") {
                                $.each(data.responseJSON.ModelState,
                                    function(key, errors) {
                                        $.each(errors,
                                            function(index, error) {
                                                switch (key) {
                                                    case "item.Name":
                                                        self.generalErrors.push(error);
                                                    break;
                                                case "item.UserId":
                                                    self.generalErrors.push(error);
                                                    break;
                                                default:
                                                    self.generalErrors.push(error);
                                                    break;
                                                };
                                            });
                                    });
                            } else {
                                self.generalErrors.push(data.responseJSON.Message);
                            };
                        },
                        500: function(data) {
                            self.generalErrors.push(data.statusText + ". Пожалуйста, попробуйте еще раз.");
                        }
                    }
            });
        };

    self.getGroups = function() {
        self.groups.removeAll();
        $.getJSON("/api/group/" + currentUserId, function (data) {
            $.each(data, function (key, value) {
                                    self.groups.push(new Group(value.GroupId, value.Name, value.UserId));                
            });
        });
    }

    self.generalErrors = ko.observableArray([]);
}

function TodoList() {
    var self = this;
    self.todos = ko.observableArray([]);

    self.getTodos = function () {
        self.todos.removeAll();
        $.getJSON("/api/todo/" + currentUserId, function (data) {
            $.each(data, function(key,value) {
                if (value.StatusId == false) {
                    self.todos.push(new Todo(value.NoteId, value.Name, value.Comment, value.StatusId, value.GroupId, value.GroupName, value.UserId));
                }
            });
        });
    }

    self.getAllTodos = function () {
    self.todos.removeAll();
        
    $.getJSON("/api/todo/", function (data) {
        $.each(data, function (key, value) {
            if (value.UserId == currentUserId) {
                self.todos.push(new Todo(value.NoteId,
                    value.Name,
                    value.Comment,
                    value.StatusId,
                    value.GroupId,
                    value.GroupName,
                    value.UserId));
            }
        });
        });
    };
    
    self.removeTodo = function (todo) {
        $.ajax({
            url: "/api/todo/" + todo.NoteId(),
            type: "delete",
            contentType: "application/json",
            success: function () {
                self.todos.remove(todo);
            }
        });
    };

    self.updateTodos = function () {
        var items = ko.utils.arrayFilter(self.todos(), function (todo) {
            delete todo.groups;
            return todo;
        });
        var dataObject = ko.toJSON(items);
        $.ajax({
            url: "/api/todo/",
            type: "put",
            data: dataObject,
            contentType: "application/json",
            success: function () {
                self.getTodos();
            }
        });
    }

    self.todo = ko.observable();

    self.editTodo = function (todo) {
        self.todo(new TodoModel(todo));
    }

    self.updateTodo = function () {
        var items = ko.utils.arrayFilter(self.todos(), function (todo) {
            delete todo.groups;
            return todo;
        });
        var dataObject = ko.toJSON(items);
        $.ajax({
            url: "/api/todo/",
            type: "put",
            data: dataObject,
            contentType: "application/json"
        });
        self.todo(null);
    }
}

todoCreateViewModel = {
    addTodoViewModel: new Todo(),
    todoListViewModel: new TodoList(),
    addGroupViewModel: new Group(),
    groupListViewModel: new GroupList()
};


$(document).ready(function () {
    var knockoutValidationSettings = {
        insertMessages: false,
        decorateElement: true,
        errorElementClass: "error-element",
        errorClass: "error-element",
        errorsAsTitle: true,
        parseInputAttributes: false,
        messagesOnModified: true,
        decorateElementOnModified: true,
        decorateInputElement: true
    };
    ko.validation.init(knockoutValidationSettings, true);
    
    ko.applyBindings(todoCreateViewModel);
    
    todoCreateViewModel.todoListViewModel.getTodos();
    todoCreateViewModel.addTodoViewModel.getGroups();
    todoCreateViewModel.groupListViewModel.getGroups();

});

