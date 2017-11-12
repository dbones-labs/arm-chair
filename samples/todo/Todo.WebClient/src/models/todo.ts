import { Priority } from './priority';
import { mapClass } from '@dboneslabs/mpr/annotations/map-class';
import { mapProperty } from '@dboneslabs/mpr/annotations/map-property';

@mapClass("models.todo")
export class Todo {

    @mapProperty()
    id: string;

    @mapProperty()
    description: string;

    @mapProperty()
    isComplete?: boolean;

    @mapProperty()
    created?: Date;

    @mapProperty()
    priority: Priority = Priority.Medium;
 
}

