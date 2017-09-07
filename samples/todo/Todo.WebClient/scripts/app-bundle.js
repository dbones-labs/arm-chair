var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('app',["require", "exports", "./services/todo-service", "aurelia-framework"], function (require, exports, todo_service_1, aurelia_framework_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var App = (function () {
        function App(todoService) {
            this._todoService = todoService;
            this.todoItems = this._todoService.todo;
        }
        App.prototype.created = function () {
            this._todoService.getTodoItems();
        };
        App = __decorate([
            aurelia_framework_1.autoinject(),
            __metadata("design:paramtypes", [todo_service_1.TodoService])
        ], App);
        return App;
    }());
    exports.App = App;
});

define('environment',["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.default = {
        debug: true,
        testing: true,
        apiUrl: 'http://localhost:5000/api/v1'
    };
});

define('main',["require", "exports", "./environment"], function (require, exports, environment_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    Promise.config({
        warnings: {
            wForgottenReturn: false
        }
    });
    function configure(aurelia) {
        aurelia.use
            .standardConfiguration()
            .feature('resources');
        if (environment_1.default.debug) {
            aurelia.use.developmentLogging();
        }
        if (environment_1.default.testing) {
            aurelia.use.plugin('aurelia-testing');
        }
        aurelia.start().then(function () { return aurelia.setRoot("shell"); });
    }
    exports.configure = configure;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
define('shell',["require", "exports", "aurelia-framework", "aurelia-logging"], function (require, exports, aurelia_framework_1, LogManager) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var log = LogManager.getLogger('shell');
    var Shell = (function () {
        function Shell() {
        }
        Shell.prototype.configureRouter = function (config, router) {
            this._router = router;
            config.title = 'ArmChair - Todo';
            config.map([
                { route: ['', 'active', 'all'], name: 'app', moduleId: 'app' }
            ]);
        };
        Shell = __decorate([
            aurelia_framework_1.autoinject()
        ], Shell);
        return Shell;
    }());
    exports.Shell = Shell;
});

define('models/index',["require", "exports", "./priority", "./todo"], function (require, exports, priority_1, todo_1) {
    "use strict";
    function __export(m) {
        for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
    }
    Object.defineProperty(exports, "__esModule", { value: true });
    __export(priority_1);
    __export(todo_1);
});

define('models/priority',["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Priority;
    (function (Priority) {
        Priority[Priority["High"] = 3] = "High";
        Priority[Priority["Medium"] = 2] = "Medium";
        Priority[Priority["Low"] = 1] = "Low";
    })(Priority = exports.Priority || (exports.Priority = {}));
});

define('models/todo',["require", "exports", "./priority"], function (require, exports, priority_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Todo = (function () {
        function Todo() {
            this.priority = priority_1.Priority.Medium;
        }
        Todo.type = "todo";
        return Todo;
    }());
    exports.Todo = Todo;
});

define('resources/index',["require", "exports", "./elements/todo-item"], function (require, exports, todo_item_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.TodoItem = todo_item_1.TodoItem;
    function configure(config) {
        config.globalResources('./elements/todo-item');
    }
    exports.configure = configure;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('services/todo-service',["require", "exports", "aurelia-fetch-client", "../models/todo", "../models/priority", "aurelia-framework", "../environment", "../utils/mapper", "aurelia-framework"], function (require, exports, aurelia_fetch_client_1, todo_1, priority_1, aurelia_framework_1, environment_1, mapper_1, aurelia_framework_2) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var log = aurelia_framework_2.LogManager.getLogger('todo-service');
    var TodoService = (function () {
        function TodoService(http, mapper) {
            this.todo = [];
            this._http = http;
            this._mapper = mapper;
            http.configure(function (config) {
                config
                    .withBaseUrl(environment_1.default.apiUrl);
            });
        }
        TodoService.prototype.createTodo = function (description, priority) {
            var _this = this;
            if (priority === void 0) { priority = priority_1.Priority.Medium; }
            var task = new todo_1.Todo();
            task.priority = priority;
            task.description = description;
            this._http.fetch('/tasks', {
                method: 'post',
                body: aurelia_fetch_client_1.json(task)
            })
                .then(function (response) { return response.json(); })
                .then(function (data) {
                log.debug(data);
                return data;
            })
                .then(function (data) {
                debugger;
                _this.getTodoItems();
            });
        };
        TodoService.prototype.getTodoItems = function () {
            var _this = this;
            this._http.fetch('/tasks', {
                method: 'get'
            })
                .then(function (response) { return response.json(); })
                .then(function (data) {
                log.debug(data);
                return data;
            })
                .then(function (data) {
                debugger;
                var items = _this._mapper.map('todoResources', 'todos', data);
                _this.setItems(items);
            });
        };
        TodoService.prototype.setItems = function (items) {
            var _this = this;
            this.todo.length = 0;
            items.forEach(function (item) {
                _this.todo.push(item);
            });
        };
        TodoService = __decorate([
            aurelia_framework_1.autoinject(),
            aurelia_framework_1.singleton(),
            __metadata("design:paramtypes", [aurelia_fetch_client_1.HttpClient, mapper_1.Mapper])
        ], TodoService);
        return TodoService;
    }());
    exports.TodoService = TodoService;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('utils/mapper',["require", "exports", "aurelia-framework", "../models/todo", "automapper-ts"], function (require, exports, aurelia_framework_1, todo_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Mapper = (function () {
        function Mapper() {
            automapper.initialize(function (config) {
                config.addProfile(new MappingProfile());
            });
            new TodoSetup().setup(automapper);
        }
        Mapper.prototype.map = function (sourceType, targetType, source) {
            return automapper.map(sourceType, targetType, source);
        };
        Mapper = __decorate([
            aurelia_framework_1.singleton(),
            __metadata("design:paramtypes", [])
        ], Mapper);
        return Mapper;
    }());
    exports.Mapper = Mapper;
    var TodoSetup = (function () {
        function TodoSetup() {
        }
        TodoSetup.prototype.setup = function (mapper) {
            mapper.createMap('todoResource', 'todo')
                .convertToType(todo_1.Todo)
                .withProfile('default');
            mapper.createMap('todo', 'todoResource');
            mapper.createMap('todoResources', 'todos')
                .convertUsing(function (ctx) {
                var src = (ctx.sourceValue.data);
                var dest = [];
                src.forEach(function (item) {
                    var destIem = automapper.map('todoResource', 'todo', src);
                    dest.push(destIem);
                });
                return dest;
            });
        };
        return TodoSetup;
    }());
    var TodoResourceCollection = (function () {
        function TodoResourceCollection() {
        }
        TodoResourceCollection.prototype.convert = function (ctx) {
            var src = (ctx.sourceValue.data);
            var dest = [];
            src.forEach(function (item) {
                var destIem = automapper.map('todoResource', 'todo', src);
                dest.push(destIem);
            });
            return dest;
        };
        return TodoResourceCollection;
    }());
    var MappingProfile = (function () {
        function MappingProfile() {
            this.sourceMemberNamingConvention = new CamelCaseNamingConvention();
            this.destinationMemberNamingConvention = new CamelCaseNamingConvention();
            this.profileName = 'default';
        }
        MappingProfile.prototype.configure = function () {
            this.sourceMemberNamingConvention = new CamelCaseNamingConvention();
            this.destinationMemberNamingConvention = new CamelCaseNamingConvention();
        };
        return MappingProfile;
    }());
    var CamelCaseNamingConvention = (function () {
        function CamelCaseNamingConvention() {
            this.splittingExpression = /(^[a-z]+(?=$|[A-Z]{1}[a-z0-9]+)|[A-Z]?[a-z0-9]+)/;
            this.separatorCharacter = '';
        }
        CamelCaseNamingConvention.prototype.transformPropertyName = function (sourcePropertyNameParts) {
            var result = '';
            for (var index = 0, length = sourcePropertyNameParts.length; index < length; index++) {
                if (index === 0) {
                    result += sourcePropertyNameParts[index].charAt(0).toLowerCase() +
                        sourcePropertyNameParts[index].substr(1);
                }
                else {
                    result += sourcePropertyNameParts[index].charAt(0).toUpperCase() +
                        sourcePropertyNameParts[index].substr(1);
                }
            }
            return result;
        };
        return CamelCaseNamingConvention;
    }());
    exports.CamelCaseNamingConvention = CamelCaseNamingConvention;
});

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('resources/elements/todo-item',["require", "exports", "aurelia-framework"], function (require, exports, aurelia_framework_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var ENTER_KEY = 13;
    var ESC_KEY = 27;
    var TodoItem = (function () {
        function TodoItem() {
        }
        __decorate([
            aurelia_framework_1.bindable,
            __metadata("design:type", String)
        ], TodoItem.prototype, "description", void 0);
        return TodoItem;
    }());
    exports.TodoItem = TodoItem;
});



define("utils/automapper/automapper", [],function(){});

define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Person = (function () {
        function Person() {
        }
        return Person;
    }());
    var test = (function () {
        function test() {
        }
        test.prototype.testm = function () {
            var mapperFactor = new MapperFactor();
            mapperFactor
                .addSetup(new MapSetup())
                .setConfiguration(function (cfg) {
            });
            var mapper = mapperFactor.createMapper();
            var anonPerson = {
                name: "test",
                _type: "personResource"
            };
            var person = mapper.map(anonPerson, 'model.Person');
        };
        return test;
    }());
    var MapSetup = (function () {
        function MapSetup() {
        }
        MapSetup.prototype.configure = function (builder) {
            builder.addType('model.person', Person);
            builder.addType('personResource');
            builder.createMap('model.person', 'personResource')
                .forMemberSimple('name', function (opts) { return opts.mapFrom('name'); })
                .forMemberSimple('age', function (opts) { return opts.using(function () { return 5; }); })
                .forMemberSimple('test', function (opt) { return opt.ignore(); })
                .forMember('model.person', 'name', function (opt) { return opt.mapFrom(function (s) { return s.hello; }, 'hello'); })
                .forSource('meh', function (opt) { return opt.ignore(); });
        };
        return MapSetup;
    }());
    var JsMapper = (function () {
        function JsMapper(config, maps, typeMetas) {
            var _this = this;
            this._maps = new Dictionary();
            this._typeMetas = new Dictionary();
            this._config = config;
            maps.forEach(function (map) {
                var key = map.source + '=>' + map.target;
                _this._maps.set(key, map);
            });
            typeMetas.forEach(function (meta) {
                _this._typeMetas.set(meta.name, meta);
            });
        }
        JsMapper.prototype.map = function (source, destinationType) {
            return this.execute(source, null, destinationType);
        };
        JsMapper.prototype.mapTo = function (source, destination) {
            return this.execute(source, destination, null);
        };
        JsMapper.prototype.execute = function (source, destination, destinationType) {
            if (destination == null && destinationType == null)
                throw new Error('destination type and instance is null, supply one of them');
            if (source == null)
                return null;
            var destinationProvided = destination != null;
            var srcIsAnArray = Array.isArray(source);
            var destinationIsAnArray = (destinationProvided && Array.isArray(destination)) || destinationType.includes('[]');
            if (srcIsAnArray && destinationIsAnArray) {
                return this.mapArray(source, destination, destinationType);
            }
            var sourceTypeProperty = this._config.typeStrategy.getTypeProperty(source);
            var sourceType = source[sourceTypeProperty];
            if (destinationType == null) {
                var destinationTypeProperty = this._config.typeStrategy.getTypeProperty(destination);
                destinationType = destination[destinationTypeProperty];
            }
            var key = sourceType + '=>' + destinationType;
            var map = this._maps.get(key);
            var destinationTypeMeta = this._typeMetas.get(destinationType);
            var sourceTypeMeta = this._typeMetas.get(sourceType);
            var mapped = this.mapSingle({
                typeMap: map,
                source: source,
                sourceType: sourceType,
                sourceTypeMeta: sourceTypeMeta,
                destination: destination,
                destinationType: destinationType,
                destinationTypeMeta: sourceTypeMeta,
            });
            return mapped;
        };
        JsMapper.prototype.mapArray = function (source, destination, destinationType) {
            var _this = this;
            if (source == null)
                throw new Error('source is null');
            if ((destination == null || destination.length == 0) && destinationType == null)
                throw new Error('destination type and instance is null, supply one of them');
            if (source.length == 0)
                return [];
            var sourceTypeProperty = this._config.typeStrategy.getTypeProperty(source[0]);
            var sourceType = source[sourceTypeProperty];
            if (destinationType == null) {
                destinationType = this._config.typeStrategy.getTypeProperty(destination[0]);
            }
            else {
                destinationType = destinationType.replace('[]', '');
            }
            var key = sourceType + '=>' + destinationType;
            var map = this._maps.get(key);
            var destinationTypeMeta = this._typeMetas.get(destinationType);
            var sourceTypeMeta = this._typeMetas.get(sourceType);
            if (destination == null)
                destination = [];
            var temp = new Dictionary();
            destination.forEach(function (item) {
                var id = item[destinationTypeMeta.id];
                temp.set(id.toString(), item);
            });
            destination.length = 0;
            source.forEach(function (srcItem) {
                var id = srcItem[sourceTypeMeta.id];
                var dest = temp.get(id);
                var mapped = _this.mapSingle({
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
        };
        JsMapper.prototype.mapSingle = function (ctx) {
            var src = ctx.source;
            var dest = ctx.destination;
            if (dest == null) {
                dest = ctx.destinationTypeMeta.ctor.createInstance();
            }
            var sourcePropeties = ctx.sourceTypeMeta.properties;
            var destPropeties = ctx.destinationTypeMeta.properties;
            var propMaps = ctx.typeMap.propertyMaps;
            var anonNotSetup = destPropeties.keys.length == 0
                && ctx.typeMap.sourcePropertyMaps.length != sourcePropeties.keys.length;
            var possibleMissingMaps = destPropeties.keys.length != propMaps.length;
            if ((anonNotSetup)) {
                var newValues = this._config.anonPropertyScanner.listProperties(ctx.destinationTypeMeta, ctx.typeMap, ctx.sourceTypeMeta);
                newValues.forEach(function (property) {
                    ctx.destinationTypeMeta.addProperty(property);
                });
            }
            if (possibleMissingMaps) {
                var tempMaps_1 = new Dictionary();
                ctx.typeMap.propertyMaps.forEach(function (map) {
                    tempMaps_1.set(map.destinationName, map);
                });
                ctx.destinationTypeMeta.properties.keys.forEach(function (property) {
                    if (tempMaps_1.get(property) != null)
                        return;
                    var propertyMap = new PropertyMap();
                    var src = ctx.sourceTypeMeta.properties.get(property);
                    if (src == null) {
                        propertyMap.ignoreDestination = true;
                    }
                    else {
                        propertyMap.sourceName = property;
                        propertyMap.sourceGetter = function (instance) { return instance[src]; };
                    }
                    ctx.typeMap.propertyMaps.push(propertyMap);
                });
            }
            ctx.typeMap.propertyMaps.forEach(function (map) {
                var src = map.sourceGetter(ctx.source);
                if (src == null)
                    continue;
                if (map.sourceType == null) {
                    var sType = typeof ctx.source;
                    var isObject = sType === 'object';
                    var isArray = Array.isArray(src);
                    if (isArray) {
                        map.destinationType =
                        ;
                    }
                }
            });
        };
        return JsMapper;
    }());
    exports.JsMapper = JsMapper;
    var MapperFactor = (function () {
        function MapperFactor() {
            this._builder = new Builder();
            this._config = new Configuration();
        }
        MapperFactor.prototype.addSetup = function (setup) {
            setup.configure(this._builder);
            return this;
        };
        MapperFactor.prototype.setConfiguration = function (setupConfig) {
            setupConfig(this._config);
        };
        MapperFactor.prototype.createMapper = function () {
            return new JsMapper(this._config, this._builder.mappings, this._builder.typeMetas);
        };
        return MapperFactor;
    }());
    exports.MapperFactor = MapperFactor;
    var Configuration = (function () {
        function Configuration() {
            this.idStrategy = new DefaultIdStrategy();
            this.typeStrategy = new DefaultTypeStrategy();
            this.anonPropertyScanner = new DefaultAnonPropertyScanner();
            this.propertyScanner = new DefaultPropertyScanner();
        }
        return Configuration;
    }());
    exports.Configuration = Configuration;
    var Builder = (function () {
        function Builder() {
            this.mappings = [];
            this.typeMetas = [];
        }
        Builder.prototype.createMap = function (sourceType, destinationType) {
            var classMap = new TypeMap(sourceType, destinationType);
            var config = new FluentClassMapping(classMap);
            this.mappings.push(classMap);
            return config;
        };
        ;
        Builder.prototype.addType = function (typeName, type) {
            if (type === void 0) { type = null; }
            var meta = new TypeMeta(typeName);
            if (type != null) {
                meta.setType(type);
            }
            this.typeMetas.push(meta);
        };
        return Builder;
    }());
    exports.Builder = Builder;
    var FluentClassMapping = (function () {
        function FluentClassMapping(mapping) {
            this._classMapping = mapping;
            this._classMapping;
        }
        FluentClassMapping.prototype.forMemberSimple = function (destinationProperty, opts) {
            var result = this.forMember(null, destinationProperty, opts);
            return result;
        };
        FluentClassMapping.prototype.forMember = function (destinationType, destinationProperty, opts) {
            var propertyMap = new PropertyMap();
            propertyMap.destinationName = destinationProperty;
            if (typeof destinationType === 'string') {
                var dt = destinationType;
                if (dt.endsWith('[]')) {
                    propertyMap.destinationType = PropertyType.array;
                    propertyMap.destinationTypeName = dt.replace('[]', '');
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
            propertyMap.destinationSetter = function (instance, value) {
                instance[destinationProperty] = value;
            };
            var options = new MapFromOptions(propertyMap);
            opts(options);
            this._classMapping.propertyMaps.push(propertyMap);
            return this;
        };
        FluentClassMapping.prototype.forSource = function (member, opts) {
            var propertyMap = new PropertyMap();
            var options = new MapFromOptions(propertyMap);
            opts(options);
            this._classMapping.sourcePropertyMaps.push(propertyMap);
            return this;
        };
        return FluentClassMapping;
    }());
    exports.FluentClassMapping = FluentClassMapping;
    var TypeMap = (function () {
        function TypeMap(source, target) {
            this.propertyMaps = [];
            this.sourcePropertyMaps = [];
            this.source = source;
            this.target = target;
        }
        return TypeMap;
    }());
    var PropertyMap = (function () {
        function PropertyMap() {
            this.isCreatedByMapper = false;
            this.ignoreSource = false;
            this.ignoreDestination = false;
        }
        return PropertyMap;
    }());
    var PropertyType;
    (function (PropertyType) {
        PropertyType[PropertyType["value"] = 0] = "value";
        PropertyType[PropertyType["object"] = 1] = "object";
        PropertyType[PropertyType["array"] = 2] = "array";
    })(PropertyType || (PropertyType = {}));
    var MapFromOptions = (function () {
        function MapFromOptions(propertyMap) {
            this._propertyMap = propertyMap;
        }
        MapFromOptions.prototype.mapFrom = function (source, sourceName) {
            if (sourceName === void 0) { sourceName = null; }
            if (typeof source === 'string') {
                this._propertyMap.sourceGetter = function (instance) {
                    return instance[source];
                };
                this._propertyMap.sourceName = source;
            }
            else {
                this._propertyMap.sourceGetter = source;
                this._propertyMap.sourceName = sourceName;
            }
        };
        MapFromOptions.prototype.using = function (func) {
            this._propertyMap.sourceGetter = function (instance) {
                return func();
            };
        };
        MapFromOptions.prototype.ignore = function () {
            this._propertyMap.ignoreDestination = true;
        };
        return MapFromOptions;
    }());
    exports.MapFromOptions = MapFromOptions;
    var MapSourceOptions = (function () {
        function MapSourceOptions(propertyMap) {
            this._propertyMap = propertyMap;
        }
        MapSourceOptions.prototype.ignore = function () {
            this._propertyMap.ignoreSource = true;
        };
        return MapSourceOptions;
    }());
    exports.MapSourceOptions = MapSourceOptions;
    var AnonCtor = (function () {
        function AnonCtor() {
        }
        AnonCtor.prototype.createInstance = function () {
            return {};
        };
        return AnonCtor;
    }());
    var TypeCtor = (function () {
        function TypeCtor(ctor) {
            this._ctor = ctor;
        }
        TypeCtor.prototype.createInstance = function () {
            return this._ctor();
        };
        return TypeCtor;
    }());
    var DefaultTypeStrategy = (function () {
        function DefaultTypeStrategy() {
        }
        DefaultTypeStrategy.prototype.getTypeProperty = function (instance) {
            var typeName = null;
            typeName = instance['$type'];
            if (typeName != null)
                return typeName;
            typeName = instance['_type'];
            if (typeName != null)
                return typeName;
        };
        return DefaultTypeStrategy;
    }());
    var CamelCaseNamingConvention = (function () {
        function CamelCaseNamingConvention() {
        }
        CamelCaseNamingConvention.prototype.convertToTarget = function (name) {
            return this.camelize(name);
        };
        CamelCaseNamingConvention.prototype.convertToCommon = function (name) {
            return this.camelize(name);
        };
        CamelCaseNamingConvention.prototype.camelize = function (str) {
            return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (letter, index) {
                return index == 0 ? letter.toLowerCase() : letter.toUpperCase();
            });
        };
        return CamelCaseNamingConvention;
    }());
    var DefaultPropertyScanner = (function () {
        function DefaultPropertyScanner() {
        }
        DefaultPropertyScanner.prototype.listProperties = function (instance) {
            var properties = Object.keys(instance);
            var result = [];
            properties.forEach(function (property) {
                if (typeof property === "function")
                    return;
                if (property.indexOf('_') === 0)
                    return;
                if (property.indexOf('$') === 0)
                    return;
                result.push(property);
            });
            return result;
        };
        return DefaultPropertyScanner;
    }());
    var DefaultAnonPropertyScanner = (function () {
        function DefaultAnonPropertyScanner() {
        }
        DefaultAnonPropertyScanner.prototype.listProperties = function (destionationTypeMeta, typeMap, sourceTypeMeta) {
            var result = [];
            var known = new Dictionary();
            destionationTypeMeta.properties.keys.forEach(function (knownItem) {
                known.set(knownItem, knownItem);
            });
            var source = new Dictionary();
            sourceTypeMeta.properties.keys.forEach(function (sourceItem) {
                source.set(sourceItem, sourceItem);
            });
            var destinationMapped = new Dictionary();
            typeMap.propertyMaps.forEach(function (map) {
                var key = map.destinationName;
                if (!map.ignoreDestination && known.get(key) == null) {
                    result.push(known.get(key));
                    source.remove(map.sourceName);
                }
            });
            typeMap.sourcePropertyMaps.forEach(function (map) {
                var key = map.sourceName;
                if (map.ignoreSource && source.get(key) != null)
                    source.remove(key);
            });
            source.keys.forEach(function (item) {
                result.push(source.get(item));
            });
            return result;
        };
        return DefaultAnonPropertyScanner;
    }());
    var DefaultIdStrategy = (function () {
        function DefaultIdStrategy() {
        }
        DefaultIdStrategy.prototype.getIdProperty = function (typeName, instance, properties) {
            var parts = typeName.split('.');
            var tName = parts[parts.length - 1].toLowerCase();
            var idName = null;
            properties.forEach(function (property) {
                var p = property.toLowerCase();
                if (idName != null)
                    return;
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
        };
        return DefaultIdStrategy;
    }());
    var TypeMeta = (function () {
        function TypeMeta(typeName) {
            this.properties = new Dictionary();
            this.allPropertiesKnown = false;
            this.namingConvention = new CamelCaseNamingConvention();
            this.isAnon = false;
            this.ctor = new AnonCtor();
            this.name = name;
        }
        TypeMeta.prototype.setType = function (type) {
            this.isAnon = false;
            this.ctor = new TypeCtor(type);
            this.actualType = type;
        };
        TypeMeta.prototype.addProperty = function (name) {
            var value = this.namingConvention.convertToTarget(name);
            this.properties.set(name, value);
        };
        return TypeMeta;
    }());
    var Dictionary = (function () {
        function Dictionary() {
            this.state = {};
        }
        Object.defineProperty(Dictionary.prototype, "keys", {
            get: function () {
                return Object.keys(this.state);
            },
            enumerable: true,
            configurable: true
        });
        Dictionary.prototype.set = function (key, value) {
            this.state[key] = value;
        };
        Dictionary.prototype.get = function (key) {
            var ret = this.state[key];
            return ret;
        };
        Dictionary.prototype.remove = function (key) {
            delete this.state[key];
        };
        return Dictionary;
    }());
});

define('text!app.html', ['module'], function(module) { module.exports = "<template>\n  <section class=\"todoapp\">\n    <header class=\"header\">\n      <h1>todos</h1>\n      <input class=\"new-todo\" placeholder=\"What needs to be done?\" autofocus>\n    </header>\n    <!-- This section should be hidden by default and shown when there are todos -->\n    <section class=\"main\">\n      <input id=\"toggle-all\" class=\"toggle-all\" type=\"checkbox\">\n      <label for=\"toggle-all\">Mark all as complete</label>\n      <ul class=\"todo-list\">\n        <!-- These are here just to show the structure of the list items -->\n        <!-- List items should get the class `editing` when editing and `completed` when marked as completed -->\n        <li class=\"completed\">\n          <div class=\"view\">\n            <input class=\"toggle\" type=\"checkbox\" checked>\n            <label>Taste JavaScript</label>\n            <button class=\"destroy\"></button>\n          </div>\n          <input class=\"edit\" value=\"Create a TodoMVC template\">\n        </li>\n        <li>\n          <todo-item></todo-item>\n        </li>\n\n      </ul>\n    </section>\n    <!-- This footer should hidden by default and shown when there are todos -->\n    <footer class=\"footer\">\n      <!-- This should be `0 items left` by default -->\n      <span class=\"todo-count\"><strong>0</strong> item left</span>\n      <!-- Remove this if you don't implement routing -->\n      <ul class=\"filters\">\n        <li>\n          <a class=\"selected\" href=\"#/\">All</a>\n        </li>\n        <li>\n          <a href=\"#/active\">Active</a>\n        </li>\n        <li>\n          <a href=\"#/completed\">Completed</a>\n        </li>\n      </ul>\n      <!-- Hidden if no completed items are left ↓ -->\n      <button class=\"clear-completed\">Clear completed</button>\n    </footer>\n  </section>\n  <footer class=\"info\">\n    <p>Using ArmChair</p>\n    <!-- Remove the below line ↓ -->\n    <p>Source: <a href=\"https://bitbucket.org/dboneslabs/arm-chair\">Git Repo on BitBucket</a></p>\n    <p>Package: <a href=\"https://www.nuget.org/packages/ArmChair.Core\">.NET Core Package on nuget</a></p>\n    <p>Created by: <a href=\"http://dbones.co.uk\">dbones</a></p>\n  </footer>\n</template>"; });
define('text!shell.html', ['module'], function(module) { module.exports = "<template>\r\n    <require from=\"todomvc-common/base.css\"></require>\r\n    <require from=\"todomvc-app-css/index.css\"></require>\r\n\r\n    <router-view></router-view>\r\n</template>"; });
define('text!resources/elements/todo-item.html', ['module'], function(module) { module.exports = "<template>\r\n    <div class=\"view\">\r\n        <input class=\"toggle\" type=\"checkbox\">\r\n        <label>Buy a unicorn asdsa</label>\r\n        <button class=\"destroy\"></button>\r\n    </div>\r\n    <input class=\"edit\" value=\"Rule the web\">\r\n</template>"; });
//# sourceMappingURL=app-bundle.js.map