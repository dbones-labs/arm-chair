import { TodoService } from './services/todo-service';
import { autoinject } from 'aurelia-framework';
import { Todo } from './models/index';

@autoinject()
export class App {

    private _todoService: TodoService;
    todoItems: any[];

    constructor(todoService: TodoService) {
        this._todoService = todoService;
        this.todoItems = this._todoService.todo; //get a reference.
    }

    created() {
        this._todoService.getTodoItems();
    }

    addItem(description: string) {
        this._todoService.createTodo(description);
    }

    removeItem(todo: Todo) {
        this._todoService.remove(todo);
    }


}
