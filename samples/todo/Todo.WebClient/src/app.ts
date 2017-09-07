import { TodoService } from './services/todo-service';
import { autoinject } from 'aurelia-framework';

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
}
