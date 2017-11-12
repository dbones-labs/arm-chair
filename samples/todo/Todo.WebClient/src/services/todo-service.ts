import { HttpClient, json } from 'aurelia-fetch-client';
import { Todo } from '../models/todo';
import { Priority } from '../models/priority';
import { singleton, autoinject } from 'aurelia-framework';
import environment from '../environment';
import { Mapper } from '../utils/mapper';
import { LogManager } from "aurelia-framework";
import { Logger } from "aurelia-logging";
import { Types } from '@dboneslabs/mpr/core/types';


var log = <Logger>LogManager.getLogger('todo-service');

@autoinject()
@singleton()
export class TodoService {

    todo: Todo[] = [];
    private _http: HttpClient;
    private _mapper: Mapper;

    constructor(http: HttpClient, mapper: Mapper) {
        this._http = http;
        this._mapper = mapper;

        http.configure(config => {
            config
                .withBaseUrl(environment.apiUrl);
        });

    }

    async createTodo(description: string, priority: Priority = Priority.Medium) {

        let task = new Todo();
        task.priority = priority;
        task.description = description;

        await this._http.fetch('/tasks', {
            method: 'post',
            //mode: 'no-cors',
            body: json(task)
        }).then(response => response.json())
        
        this.getTodoItems();
    }

    async getTodoItems() {

        let data = await this._http.fetch('/tasks', {
            //mode: 'no-cors',
            method: 'get'
        }).then(response => response.json());

        var items = this._mapper.map(data, Types.asArray(Todo));
        this.setItems(items);

    }

    async remove(todo: Todo) {

        await this._http.fetch(`/tasks${todo.id}`, {
            //mode: 'no-cors',
            method: 'delete'
        });

        this.getTodoItems();

    }

    private setItems(items) {
        
        this.todo.length = 0;
    
        items.forEach(item => {
            this.todo.push(item);
        });
    
    }

}