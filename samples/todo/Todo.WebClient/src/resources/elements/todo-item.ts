import { bindable, bindingMode } from 'aurelia-framework';
import { Todo } from '../../models/index';

const ENTER_KEY = 13;
const ESC_KEY = 27;

export class TodoItem {

   @bindable todo: Todo;

   

    

}