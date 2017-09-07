import { bindable, bindingMode } from 'aurelia-framework';

const ENTER_KEY = 13;
const ESC_KEY = 27;

export class TodoItem {

    completed: boolean;
    @bindable description: string;

    

}