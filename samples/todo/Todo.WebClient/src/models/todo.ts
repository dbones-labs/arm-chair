import { Priority } from './priority';

export class Todo {

    description: string;
    isComplete?: boolean;
    created?: Date;
    priority: Priority = Priority.Medium;

    static type = "todo";

 }

