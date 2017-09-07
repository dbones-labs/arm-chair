import { HttpClient, json } from 'aurelia-fetch-client';
import { Todo } from '../models/todo';
import { Priority } from '../models/priority';
import { singleton, autoinject } from 'aurelia-framework';
import environment  from '../environment';
import { Mapper } from '../utils/mapper';
import { LogManager  } from "aurelia-framework";
import { Logger  } from "aurelia-logging";


var log = <Logger>LogManager.getLogger('todo-service');
 
@autoinject()
@singleton()
export class TodoService {

    todo : Todo[] = [];
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

    createTodo(description: string, priority: Priority= Priority.Medium) {
        let task = new Todo();
        task.priority = priority;
        task.description = description;

        this._http.fetch('/tasks', {
            method: 'post',
            //mode: 'no-cors',
            body: json(task)
        })
        .then(response => response.json())
        .then(data=> {
            log.debug(data);
            return data;
        })
        .then(data => {
            debugger;
            
            this.getTodoItems();
            //var items = this._mapper.map('todoResources', 'todos', data);
        });

    }

    getTodoItems(){
        this._http.fetch('/tasks', {
            //mode: 'no-cors',
            method: 'get'
            
        })
        .then(response => response.json())
        .then(data=> {
            log.debug(data);
            return data;
        })
        .then(data => {
            debugger;
            var items = this._mapper.map('todoResources', 'todos', data);
            this.setItems(items);
        });
    }


    private setItems(items) {
        this.todo.length = 0;
        items.forEach(item => {
            this.todo.push(item);
        });
    }


}