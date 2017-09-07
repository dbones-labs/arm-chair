import { singleton } from 'aurelia-framework';
import { Todo } from '../models/todo';
import 'automapper-ts';

//import * as automapper from 'automapper-ts';

@singleton()
export class Mapper {

    constructor() 
    {
        automapper.initialize((config) => {
            config.addProfile(new MappingProfile());
        });
        new TodoSetup().setup(automapper);       
    }

    map(sourceType: string, targetType: string, source: any): any {
        return automapper.map(sourceType, targetType, source);
    }

}


class TodoSetup  {
    
    setup(mapper) {

        mapper.createMap('todoResource', 'todo')
            .convertToType(Todo)
            .withProfile('default');
        
        mapper.createMap('todo', 'todoResource')
 //           .forMember('destination', (opts: AutoMapperJs.IMemberConfigurationOptions) => { opts.mapFrom('destination'); })
 //           .forMember('isComplete', (opts: AutoMapperJs.IMemberConfigurationOptions) => { opts.mapFrom('isComplete'); })
 //           .forMember('created', (opts: AutoMapperJs.IMemberConfigurationOptions) => { opts.mapFrom('created'); })
 //           .forMember('priority', (opts: AutoMapperJs.IMemberConfigurationOptions) => { opts.mapFrom('priority'); })
            ;

        mapper.createMap('todoResources', 'todos')
            .convertUsing(ctx => {
                let src = <any[]>(ctx.sourceValue.data);
                let dest = [];
        
                src.forEach(item=> {
                    let destIem = automapper.map('todoResource', 'todo', src);
                    dest.push(destIem);
                });
        
                return dest;
            });

    }

}

class TodoResourceCollection  {
    public convert(ctx: any): any {

        let src = <any[]>(ctx.sourceValue.data);
        let dest = [];

        src.forEach(item=> {
            let destIem = automapper.map('todoResource', 'todo', src);
            dest.push(destIem);
        });

        return dest;
    }
}

class MappingProfile  {
    public sourceMemberNamingConvention = new CamelCaseNamingConvention();
    public destinationMemberNamingConvention = new CamelCaseNamingConvention();

    public profileName = 'default';

    public configure(): void {
        this.sourceMemberNamingConvention = new CamelCaseNamingConvention();
        this.destinationMemberNamingConvention = new CamelCaseNamingConvention();
    }
}

export class CamelCaseNamingConvention  {
    public splittingExpression = /(^[a-z]+(?=$|[A-Z]{1}[a-z0-9]+)|[A-Z]?[a-z0-9]+)/;
    public separatorCharacter = '';

    public transformPropertyName(sourcePropertyNameParts: string[]): string {
        // Transform the splitted parts.
        var result: string = '';

        for (var index = 0, length = sourcePropertyNameParts.length; index < length; index++) {
             if (index === 0) {
                result += sourcePropertyNameParts[index].charAt(0).toLowerCase() +
                        sourcePropertyNameParts[index].substr(1);
            } else {
            result += sourcePropertyNameParts[index].charAt(0).toUpperCase() +
                      sourcePropertyNameParts[index].substr(1);
            }
        }

        return result;
    }
}