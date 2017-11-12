import { singleton } from 'aurelia-framework';
import { Todo } from '../models/todo';
import { MapperFactory } from '@dboneslabs/mpr/mapper-factory';
import { Setup } from '@dboneslabs/mpr/initializing/setup';
import { Builder } from '@dboneslabs/mpr/initializing/builders/builder';
import { Types } from '@dboneslabs/mpr/core/types';
import { Mapper as Mpr } from '@dboneslabs/mpr/mapper';
import { TypeConverter } from '@dboneslabs/mpr/core/type-converter';
import { MappingContext } from '@dboneslabs/mpr/core/mapping-context';

export class DtoTypes {
    static todo = "Todo.Service.Dto.Resources.TodoResource, Todo.Service";
    static todoCollection = "Todo.Service.Dto.Resources.CollectionResource`1[[Todo.Service.Dto.Resources.TodoResource, Todo.Service]], Todo.Service";
}


class TodoSetup implements Setup {
    configure(builder: Builder): void {

        //add types
        builder.addType(Todo).scanForAttributes()

        builder.addType(DtoTypes.todo)
            .addProperty('id', Types.string)
            .addProperty('description', Types.string)
            .addProperty('isComplete', Types.boolean)
            .addProperty('creaated', Types.date)
            .addProperty('priority', Types.number);

        builder.addType(DtoTypes.todoCollection)
            .addProperty("data", Types.AsArray(DtoTypes.todo));

        //add maps
        builder.createMap(DtoTypes.todo, Todo);

        builder.createMap<Todo, any>(Todo, DtoTypes.todo)
            .forMember("$type", opts => opts.using(src => DtoTypes.todo));

        builder.createMap<any, Todo[]>(DtoTypes.todoCollection, Types.asArray(Todo))
            .withSource(src => src.data, opt => opt.flattern());
    }

}


@singleton()
export class Mapper {

    private _mapper: Mpr;

    constructor() {
        let factory = new MapperFactory();
        factory.addSetup(new TodoSetup());
        this._mapper = factory.createMapper();
    }

    map(source: any, targetType: string): any {
        return this._mapper.map(source, targetType);
    }

}


/*
class TodoResourceCollectionTodoModelCollectionConverter implements TypeConverter {
    sourceType: string = DtoTypes.todoCollection;
    destinationType: string = Types.AsArray((<any>Todo).$$type);
    execute(context: MappingContext): void {
        let data: any[] = context.source.data;
        context.destination = data.map(item => {
            return context.mapper.map(item, (<any>Todo).$$type);
        });
    }

}
*/