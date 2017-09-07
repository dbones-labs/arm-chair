
class Person {
    name: string;
    age: number;

    $type: 'model.person';
}


class test {

    testm() {

        let mapperFactor = new MapperFactor();
        mapperFactor
            .addSetup(new MapSetup())
            .setConfiguration(cfg => {

            });

        let mapper = mapperFactor.createMapper();

        var anonPerson = {
            name: "test",
            _type: "personResource"
        }


        let person = mapper.map(anonPerson, 'model.Person');

    }


}

class MapSetup implements Setup {
    configure(builder: Builder) {

        builder.addType('model.person', Person);
        builder.addType('personResource');

        builder.createMap('model.person', 'personResource')
            .forMemberSimple('name', opts => opts.mapFrom('name'))
            .forMemberSimple('age', opts => opts.using(() => 5))
            .forMemberSimple('test', opt => opt.ignore())
            .forMember('model.person', 'name', opt => opt.mapFrom(s => s.hello, 'hello'))
            .forSource('meh', opt => opt.ignore());


    }
}



//--------------------------------------------------------


export interface Mapper {
    map(source: any, destinationType: string);
    mapTo(source: any, destination: any);
}


export class JsMapper implements Mapper {

    private _config: Configuration;
    private _maps: Dictionary<TypeMap> = new Dictionary<TypeMap>();
    private _typeMetas: Dictionary<TypeMeta> = new Dictionary<TypeMeta>();

    constructor(config: Configuration, maps: TypeMap[], typeMetas: TypeMeta[]) {

        this._config = config;

        maps.forEach(map => {

            let key = map.source + '=>' + map.target;
            this._maps.set(key, map);

        });

        typeMetas.forEach(meta => {
            this._typeMetas.set(meta.name, meta);
        });
    }

    map(source: any, destinationType: string): any {
        return this.execute(source, null, destinationType);
    }
    mapTo(source: any, destination: any): any {
        return this.execute(source, destination, null);
    }

    private execute(source: any, destination: any, destinationType: string) {

        if (destination == null && destinationType == null) throw new Error('destination type and instance is null, supply one of them');
        if (source == null) return null; //this is valid;


        let destinationProvided = destination != null;

        //confirm if we have a collection or an actual map
        let srcIsAnArray = Array.isArray(source);
        let destinationIsAnArray = (destinationProvided && Array.isArray(destination)) || destinationType.includes('[]');

        if (srcIsAnArray && destinationIsAnArray) {
            return this.mapArray(source, destination, destinationType);
        }

        //find types
        let sourceTypeProperty = this._config.typeStrategy.getTypeProperty(source);
        let sourceType = source[sourceTypeProperty];

        if (destinationType == null) {
            let destinationTypeProperty = this._config.typeStrategy.getTypeProperty(destination);
            destinationType = destination[destinationTypeProperty];
        }

        //find maps
        var key = sourceType + '=>' + destinationType;
        let map = this._maps.get(key);


        //ensure destination instance.
        var destinationTypeMeta = this._typeMetas.get(destinationType);
        var sourceTypeMeta = this._typeMetas.get(sourceType);
        //TODO: null


        let mapped = this.mapSingle(<MapSingle>{

            typeMap: map,

            source: source,
            sourceType: sourceType,
            sourceTypeMeta: sourceTypeMeta,

            destination: destination,
            destinationType: destinationType,
            destinationTypeMeta: sourceTypeMeta,

        });

        return mapped; //which should be the same as destination if passed in
    }

    private mapArray(source: any, destination: any, destinationType: string) {

        if (source == null) throw new Error('source is null');
        if ((destination == null || destination.length == 0) && destinationType == null)
            throw new Error('destination type and instance is null, supply one of them');

        if (source.length == 0)
            return [];

        //find types
        let sourceTypeProperty = this._config.typeStrategy.getTypeProperty(source[0]);
        let sourceType = source[sourceTypeProperty];


        if (destinationType == null) {
            destinationType = this._config.typeStrategy.getTypeProperty(destination[0]);
        }
        else {
            destinationType = destinationType.replace('[]', '');
        }

        //find maps
        var key = sourceType + '=>' + destinationType;
        let map = this._maps.get(key);

        //get some info
        var destinationTypeMeta = this._typeMetas.get(destinationType);
        var sourceTypeMeta = this._typeMetas.get(sourceType);
        //TODO: null

        //new
        if (destination == null)
            destination = [];

        let temp = new Dictionary<any>();

        //map id's where possible.
        destination.forEach(item => {
            let id = item[destinationTypeMeta.id];
            temp.set(id.toString(), item);
        });

        //empty the items for now.
        destination.length = 0;

        source.forEach(srcItem => {

            let id = srcItem[sourceTypeMeta.id];
            let dest = temp.get(id);

            let mapped = this.mapSingle(<MapSingle>{

                typeMap: map,

                source: srcItem,
                sourceType: sourceType,
                sourceTypeMeta: sourceTypeMeta,

                destination: dest,
                destinationType: destinationType,
                destinationTypeMeta: sourceTypeMeta,

            });

            destination.push(mapped);

        });

        return destination;
    }


    private mapSingle(ctx: MapSingle): any {

        let src = ctx.source;
        let dest = ctx.destination;

        if (dest == null) {
            dest = ctx.destinationTypeMeta.ctor.createInstance();
        }

        let sourcePropeties = ctx.sourceTypeMeta.properties;
        let destPropeties = ctx.destinationTypeMeta.properties;
        let propMaps = ctx.typeMap.propertyMaps;


        //figure out if we need to setup some automated mappings.

        //the first 2 are to see if we know all that there is needed about the destination types
        //as they will DRIVE the mapping in most cases

        //anon type
        let anonNotSetup =
            destPropeties.keys.length == 0  //no properties, indicates missing setup.
        //    && ctx.destinationTypeMeta.isAnon //is annon, as for known we would know the number of properties.
            && ctx.typeMap.sourcePropertyMaps.length != sourcePropeties.keys.length; //if we had ignored source props it may account for the lack of known properties in this case


        //crap we acutally do not know much information about known
        //as typescript does not create the properties unless they are assign.
        //known type
        //let typeNotSetup =
        //    !ctx.destinationTypeMeta.isAnon
        //    && ctx.destinationTypeMeta.allPropertiesKnown; //for known types we set a field.

        //now we see if mappngs have beed setup for this
        //there should be mappings for each destination property        
        let possibleMissingMaps = destPropeties.keys.length != propMaps.length;


        //find all the properties
        //well as much as we can for an Anon type.
        if ((anonNotSetup)) { // || typeNotSetup)) {
            //let properties;
            //if(ctx.destinationTypeMeta.isAnon) {
                let newValues = this._config.anonPropertyScanner.listProperties(
                    ctx.destinationTypeMeta, 
                    ctx.typeMap,
                    ctx.sourceTypeMeta);

                newValues.forEach(property => {
                    ctx.destinationTypeMeta.addProperty(property);
                });
            //}
            //else {
            //    this._config.propertyScanner.listProperties()
            //}
        }

        if(possibleMissingMaps) {

            let tempMaps = new Dictionary<PropertyMap>();
            ctx.typeMap.propertyMaps.forEach(map => {
                tempMaps.set(map.destinationName, map);
            });

            

            ctx.destinationTypeMeta.properties.keys.forEach(property => {
                if(tempMaps.get(property)!= null) return;
             
                let propertyMap = new PropertyMap();
                let src = ctx.sourceTypeMeta.properties.get(property);
                if(src == null) {
                    propertyMap.ignoreDestination = true; 
                }
                else{
                    
                    propertyMap.sourceName = property;
                    propertyMap.sourceGetter = (instance) => instance[src];
                }
        
                ctx.typeMap.propertyMaps.push(propertyMap);
            
            });

        }

        //map each property.
        ctx.typeMap.propertyMaps.forEach(map => {
            
            //IM here Dave

            let src = map.sourceGetter(ctx.source);
            if(src == null) continue;

            if(map.sourceType == null) {

                let sType = typeof ctx.source;
                let isObject = sType === 'object'
                let isArray = Array.isArray(src);

                if(isArray) {
                    map.destinationType = 
                }
                
            }



        });











    }

}

interface MappingCollection {
    source: any;
    destination: any;
}

interface MapSingle {

    typeMap: TypeMap;

    source: any;
    sourceType: string;
    sourceTypeMeta: TypeMeta;

    destination: any;
    destinationType: string;
    destinationTypeMeta: TypeMeta;

}

export class MapperFactor {

    private _builder: Builder = new Builder();
    private _config: Configuration = new Configuration();

    addSetup(setup: Setup) {
        setup.configure(this._builder);
        return this;
    }

    setConfiguration(setupConfig: (config: Configuration) => void) {
        setupConfig(this._config);
    }


    createMapper(): Mapper {
        return new JsMapper(this._config, this._builder.mappings, this._builder.typeMetas);
    }
}

export class Configuration {

    idStrategy: IdStrategy = new DefaultIdStrategy();
    typeStrategy: TypeStrategy = new DefaultTypeStrategy();
    anonPropertyScanner: AnonPropertyScanner = new DefaultAnonPropertyScanner();
    propertyScanner: PropertyScanner = new DefaultPropertyScanner();

}


export class Builder {

    mappings: TypeMap[] = [];
    typeMetas: TypeMeta[] = [];

    createMap<TSrc, TDest>(sourceType: string, destinationType: string): FluentClassMapping<TSrc, TDest> {
        let classMap = new TypeMap(sourceType, destinationType);
        let config = new FluentClassMapping(classMap);
        this.mappings.push(classMap);
        return config;
    };

    addType(typeName: string, type: Function = null) {
        let meta = new TypeMeta(typeName);

        if (type != null) {
            meta.setType(type);
        }

        this.typeMetas.push(meta);
    }

}


export class FluentClassMapping<TSrc, TDest> {

    private _classMapping: TypeMap;

    constructor(mapping: TypeMap) {
        this._classMapping = mapping;
        this._classMapping
    }

    /**
     * provide explict mapping instunction for a simple propty ie value type or string.
     * @param destinationProperty the name of the property
     * @param opts mapping instuctions.
     */
    forMemberSimple(destinationProperty: string, opts: MapFromOpts<TSrc>): FluentClassMapping<TSrc, TDest> {
        let result = this.forMember(null, destinationProperty, opts);
        return result;
    }

    /**
     * provide explicit mapping instuctions for a destination member (how is is maped from source)
     * @param destinationType the property's type (please tell us, as there is no reflection)
     * @param destinationProperty the name of the property
     * @param opts the mapping options to apply
     */
    forMember(destinationType: string|PropertyType, destinationProperty: string, opts: MapFromOpts<TSrc>): FluentClassMapping<TSrc, TDest> {
        let propertyMap = new PropertyMap();

        propertyMap.destinationName = destinationProperty;

        if(typeof destinationType === 'string') {
            let dt = <string>destinationType;
            if(dt.endsWith('[]')){
                propertyMap.destinationType = PropertyType.array;
                propertyMap.destinationTypeName = dt.replace('[]','');
            }
            else {
                propertyMap.destinationTypeName = dt;
                propertyMap.destinationType = PropertyType.object;
            }
        }
        else {
            propertyMap.destinationType = PropertyType.value;
            propertyMap.destinationTypeName = 'valueType';
        }
        

        propertyMap.destinationSetter = (instance, value) => {
            instance[destinationProperty] = value;
        };

        let options = new MapFromOptions(propertyMap);
        opts(options);
        this._classMapping.propertyMaps.push(propertyMap);
        return this;
    }

    forSource(member: string, opts: MapFromOpts<TSrc>): FluentClassMapping<TSrc, TDest> {
        let propertyMap = new PropertyMap();
        let options = new MapFromOptions(propertyMap);
        opts(options);
        this._classMapping.sourcePropertyMaps.push(propertyMap);
        return this;
    }

    /*
    convertToClass(type: Function) {
        this._classMapping.ctorStrategy = new TypeCtor(type);
        return this;
    }
    */

}

/**
 * store information about the mapping
 */
class TypeMap {

    target: string; //the type name
    source: string;
    propertyMaps: PropertyMap[] = [];
    sourcePropertyMaps: PropertyMap[] = [];
    

    constructor(source: string, target: string) {
        this.source = source;
        this.target = target;
    }

}


/**
 * each class map should have seval property maps
 */
class PropertyMap {

    isCreatedByMapper: boolean = false; //if this was created by this mapper

    sourceType: PropertyType;
    sourceTypeName: string;
    sourceName: string;
    sourceGetter: (instance: any) => any;
    ignoreSource: boolean = false;

    destinationType: PropertyType;
    destinationTypeName: string
    destinationName: string;
    destinationSetter: (instance: any, value: any) => void;
    ignoreDestination: boolean = false;

}

enum PropertyType {
    value,
    object,
    array
}


export interface MapFromOpts<TSrc> {
    (opt: MapFromOptions<TSrc>): void;
}

export interface MapSourceOpts<TSrc, TDest> {
    (opt: MapSourceOptions<TSrc>): void;
}

export class MapFromOptions<TSrc> {

    private _propertyMap: PropertyMap;

    constructor(propertyMap: PropertyMap) {
        this._propertyMap = propertyMap;
    }

    /**
     * mapping options
     * @param source the source where we can source the value from.
     * @param sourceName the name of the source, this is only need if you passed a delegae in for the source and we a mappping to an Anon type.
     */
    mapFrom(source: string | ((srcInstnace: TSrc | any) => any), sourceName: string = null) {

        if (typeof source === 'string') {
            this._propertyMap.sourceGetter = (instance: any) => {
                return instance[source];
            }
            this._propertyMap.sourceName = source;
        }
        else {
            this._propertyMap.sourceGetter = source;
            this._propertyMap.sourceName = sourceName;
        }

    }

    /**
     * a function which will provide the value.
     * @param func a func which will result in a value you want.
     */
    using(func: () => any): void {
        this._propertyMap.sourceGetter = (instance: any) => {
            return func();
        };
    }

    /**
     * this map will IGNORE this destinaiton value
     */
    ignore() {
        this._propertyMap.ignoreDestination = true;
    }
}


export class MapSourceOptions<TSrc> {

    private _propertyMap: PropertyMap;

    constructor(propertyMap: PropertyMap) {
        this._propertyMap = propertyMap;
    }


    /**
     * this source value will be ignored
     */
    ignore() {
        this._propertyMap.ignoreSource = true;
    }
}


interface Setup {
    configure(builder: Builder): void;
}


export interface CtorStrategy {
    createInstance(): any;
}

class AnonCtor implements CtorStrategy {
    createInstance(): any {
        return {};
    }
}

class TypeCtor implements CtorStrategy {
    _ctor: Function;

    constructor(ctor: Function) {
        this._ctor = ctor;
    }

    createInstance(): any {
        return this._ctor();
    }

}





//engine
export interface TypeStrategy {
    getTypeProperty(instance: any): string;
}

class DefaultTypeStrategy implements TypeStrategy {
    getTypeProperty(instance: any): string {
        let typeName = null;
        typeName = instance['$type'];
        if (typeName != null) return typeName;

        typeName = instance['_type'];
        if (typeName != null) return typeName;

    }
}


interface NamingConvention {
    convertToTarget(name: string): string;
    convertToCommon(name: string): string;
}




class CamelCaseNamingConvention implements NamingConvention {
    
    convertToTarget(name: string): string {
        return this.camelize(name);
    }
    
    convertToCommon(name: string): string {
        return this.camelize(name);
    }

    camelize(str) {
        return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (letter, index) {
            return index == 0 ? letter.toLowerCase() : letter.toUpperCase();
        });//.replace(/\s+/g, '');
    }


}


interface PropertyScanner {
    listProperties(instance: any): string[];
}

class DefaultPropertyScanner implements PropertyScanner {

    listProperties(instance: any): string[] {
        let properties = Object.keys(instance);
        let result = [];
        properties.forEach(property => {
            //not interested in functions
            if (typeof property === "function") return;

            if (property.indexOf('_') === 0) return;
            if (property.indexOf('$') === 0) return;

            result.push(property);
        });

        return result;
    }

}


/**
 * this is used to figure our what properies the anon
 * destination object should have.
 */
interface AnonPropertyScanner {
    listProperties(destionationMeta: TypeMeta, typeMap: TypeMap, sourceTypeMeta: TypeMeta): string[];
}


class DefaultAnonPropertyScanner implements AnonPropertyScanner {

    listProperties(destionationTypeMeta: TypeMeta, typeMap: TypeMap, sourceTypeMeta: TypeMeta): string[] {

        let result = [];

        let known = new Dictionary<string>();
        destionationTypeMeta.properties.keys.forEach(knownItem => {
            known.set(knownItem, knownItem);
        });

        let source = new Dictionary<string>();
        sourceTypeMeta.properties.keys.forEach(sourceItem => {
            source.set(sourceItem, sourceItem);
        });


        let destinationMapped = new Dictionary<PropertyMap>();
        typeMap.propertyMaps.forEach(map => {

            let key = map.destinationName;
            if (!map.ignoreDestination && known.get(key) == null) {
                //found a new property 
                result.push(known.get(key));
                source.remove(map.sourceName);
            }
        });

        typeMap.sourcePropertyMaps.forEach(map => {
            let key = map.sourceName;
            if (map.ignoreSource && source.get(key) != null)
                source.remove(key);
        });

        //anything left will need to be mapped over.
        source.keys.forEach(item => {
            result.push(source.get(item));
        });

        return result;
    }

}



interface IdStrategy {
    getIdProperty(typeName: string, instance: any, properties: string[]): string;
}

class DefaultIdStrategy {
    getIdProperty(typeName: string, instance: any, properties: string[]) {
        let parts = typeName.split('.') // 'hello.Person';
        let tName = parts[parts.length - 1].toLowerCase(); //person

        let idName = null;
        properties.forEach(property => {
            let p = property.toLowerCase();

            if (idName != null) return;

            if (p == 'id') {
                idName = property;
                return;
            }

            if (p.startsWith(tName) && p.endsWith('id')) {
                idName = property;
                return;
            }

        });

        return idName;
    }
}


/**
 * information about each type/class
 */
class TypeMeta {

    constructor(typeName: string) {
        this.name = name;
    }

    name: string;
    id: string; //property name, not the instance value.

    properties: Dictionary<string> = new Dictionary<string>();
    allPropertiesKnown: boolean = false; //assume a declared type will have a fixed number of properties.


    namingConvention: NamingConvention = new CamelCaseNamingConvention();

    isAnon: boolean = false;
    ctor: CtorStrategy = new AnonCtor();
    actualType: Function;

    setType(type: Function) {
        this.isAnon = false;
        this.ctor = new TypeCtor(type);
        this.actualType = type;
    }

    addProperty(name: string) {
        let value = this.namingConvention.convertToTarget(name);
        this.properties.set(name, value);
    }
}


//misk
class Dictionary<TValue> {
    state = {};

    get keys(): Array<string> {
        return Object.keys(this.state);
    }

    set(key: string, value: TValue): void {
        this.state[key] = value;
    }

    get(key: string): TValue {
        let ret = this.state[key];
        return <TValue>ret;
    }

    remove(key: string): void {
        delete this.state[key];
    }
}
