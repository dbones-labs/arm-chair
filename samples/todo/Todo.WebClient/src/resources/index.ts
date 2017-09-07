import {FrameworkConfiguration} from 'aurelia-framework';
import { TodoItem } from './elements/todo-item';

export function configure(config: FrameworkConfiguration) {
  config.globalResources('./elements/todo-item');
}

export {
  TodoItem
}