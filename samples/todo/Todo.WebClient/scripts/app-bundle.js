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
        App.prototype.addItem = function (description) {
            this._todoService.createTodo(description);
        };
        App.prototype.removeItem = function (todo) {
            this._todoService.remove(todo);
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

var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
define('models/todo',["require", "exports", "./priority", "@dboneslabs/mpr/annotations/map-class", "@dboneslabs/mpr/annotations/map-property"], function (require, exports, priority_1, map_class_1, map_property_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Todo = (function () {
        function Todo() {
            this.priority = priority_1.Priority.Medium;
        }
        __decorate([
            map_property_1.mapProperty(),
            __metadata("design:type", String)
        ], Todo.prototype, "id", void 0);
        __decorate([
            map_property_1.mapProperty(),
            __metadata("design:type", String)
        ], Todo.prototype, "description", void 0);
        __decorate([
            map_property_1.mapProperty(),
            __metadata("design:type", Boolean)
        ], Todo.prototype, "isComplete", void 0);
        __decorate([
            map_property_1.mapProperty(),
            __metadata("design:type", Date)
        ], Todo.prototype, "created", void 0);
        __decorate([
            map_property_1.mapProperty(),
            __metadata("design:type", Number)
        ], Todo.prototype, "priority", void 0);
        Todo = __decorate([
            map_class_1.mapClass("models.todo")
        ], Todo);
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
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = y[op[0] & 2 ? "return" : op[0] ? "throw" : "next"]) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [0, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
define('services/todo-service',["require", "exports", "aurelia-fetch-client", "../models/todo", "../models/priority", "aurelia-framework", "../environment", "../utils/mapper", "aurelia-framework", "@dboneslabs/mpr/core/types"], function (require, exports, aurelia_fetch_client_1, todo_1, priority_1, aurelia_framework_1, environment_1, mapper_1, aurelia_framework_2, types_1) {
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
            if (priority === void 0) { priority = priority_1.Priority.Medium; }
            return __awaiter(this, void 0, void 0, function () {
                var task;
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0:
                            task = new todo_1.Todo();
                            task.priority = priority;
                            task.description = description;
                            return [4, this._http.fetch('/tasks', {
                                    method: 'post',
                                    body: aurelia_fetch_client_1.json(task)
                                }).then(function (response) { return response.json(); })];
                        case 1:
                            _a.sent();
                            this.getTodoItems();
                            return [2];
                    }
                });
            });
        };
        TodoService.prototype.getTodoItems = function () {
            return __awaiter(this, void 0, void 0, function () {
                var data, items;
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0: return [4, this._http.fetch('/tasks', {
                                method: 'get'
                            }).then(function (response) { return response.json(); })];
                        case 1:
                            data = _a.sent();
                            items = this._mapper.map(data, types_1.Types.asArray(todo_1.Todo));
                            this.setItems(items);
                            return [2];
                    }
                });
            });
        };
        TodoService.prototype.remove = function (todo) {
            return __awaiter(this, void 0, void 0, function () {
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0: return [4, this._http.fetch("/tasks" + todo.id, {
                                method: 'delete'
                            })];
                        case 1:
                            _a.sent();
                            this.getTodoItems();
                            return [2];
                    }
                });
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
define('utils/mapper',["require", "exports", "aurelia-framework", "../models/todo", "@dboneslabs/mpr/mapper-factory", "@dboneslabs/mpr/core/types"], function (require, exports, aurelia_framework_1, todo_1, mapper_factory_1, types_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var DtoTypes = (function () {
        function DtoTypes() {
        }
        DtoTypes.todo = "Todo.Service.Dto.Resources.TodoResource, Todo.Service";
        DtoTypes.todoCollection = "Todo.Service.Dto.Resources.CollectionResource`1[[Todo.Service.Dto.Resources.TodoResource, Todo.Service]], Todo.Service";
        return DtoTypes;
    }());
    exports.DtoTypes = DtoTypes;
    var TodoSetup = (function () {
        function TodoSetup() {
        }
        TodoSetup.prototype.configure = function (builder) {
            builder.addType(todo_1.Todo).scanForAttributes();
            builder.addType(DtoTypes.todo)
                .addProperty('id', types_1.Types.string)
                .addProperty('description', types_1.Types.string)
                .addProperty('isComplete', types_1.Types.boolean)
                .addProperty('creaated', types_1.Types.date)
                .addProperty('priority', types_1.Types.number);
            builder.addType(DtoTypes.todoCollection)
                .addProperty("data", types_1.Types.AsArray(DtoTypes.todo));
            builder.createMap(DtoTypes.todo, todo_1.Todo);
            builder.createMap(todo_1.Todo, DtoTypes.todo)
                .forMember("$type", function (opts) { return opts.using(function (src) { return DtoTypes.todo; }); });
            builder.createMap(DtoTypes.todoCollection, types_1.Types.asArray(todo_1.Todo))
                .withSource(function (src) { return src.data; }, function (opt) { return opt.flattern(); });
        };
        return TodoSetup;
    }());
    var Mapper = (function () {
        function Mapper() {
            var factory = new mapper_factory_1.MapperFactory();
            factory.addSetup(new TodoSetup());
            this._mapper = factory.createMapper();
        }
        Mapper.prototype.map = function (source, targetType) {
            return this._mapper.map(source, targetType);
        };
        Mapper = __decorate([
            aurelia_framework_1.singleton(),
            __metadata("design:paramtypes", [])
        ], Mapper);
        return Mapper;
    }());
    exports.Mapper = Mapper;
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
define('resources/elements/todo-item',["require", "exports", "aurelia-framework", "../../models/index"], function (require, exports, aurelia_framework_1, index_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var ENTER_KEY = 13;
    var ESC_KEY = 27;
    var TodoItem = (function () {
        function TodoItem() {
        }
        __decorate([
            aurelia_framework_1.bindable,
            __metadata("design:type", index_1.Todo)
        ], TodoItem.prototype, "todo", void 0);
        return TodoItem;
    }());
    exports.TodoItem = TodoItem;
});

define('@dboneslabs/mpr/annotations/map-class',['require','exports','module','./reflect'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const reflect_1 = require("./reflect");
function mapClass(typeName) {
    return (target) => {
        let type = reflect_1.ReflectMetadata.getTypeData(target);
        let t2 = classDecorator(target, typeName);
        type = type
            || reflect_1.ReflectMetadata.getTypeData(t2)
            || {};
        type.type = target;
        type.proxiedType = t2;
        type.name = typeName;
        reflect_1.ReflectMetadata.setTypeData(target, type);
        reflect_1.ReflectMetadata.setTypeData(t2, type);
        return t2;
    };
}
exports.mapClass = mapClass;
function classDecorator(constructor, typeName) {
    return _a = class extends constructor {
            constructor() {
                super(...arguments);
                this.$type = typeName;
            }
        },
        _a.$$type = typeName,
        _a;
    var _a;
}

});

define('@dboneslabs/mpr/annotations/reflect',['require','exports','module','reflect-metadata','./annotation-keys'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("reflect-metadata");
const annotation_keys_1 = require("./annotation-keys");

const getMetaData = Reflect.getMetadata || Reflect.getOwnMetadata;

class ReflectMetadata {
    static setTypeData(type, value) {
        Reflect.defineMetadata(annotation_keys_1.AnnotationKeys.mapAnnotation, value, type);
    }
    static getTypeData(type) {
        return getMetaData(annotation_keys_1.AnnotationKeys.mapAnnotation, type);
    }
    static getPropertyData(type, key) {
        return getMetaData("design:type", type, key);
    }
}
exports.ReflectMetadata = ReflectMetadata;

});

define('@dboneslabs/mpr/annotations/annotation-keys',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class AnnotationKeys {
}
AnnotationKeys.mapAnnotation = "mpr.Annotation";
exports.AnnotationKeys = AnnotationKeys;

});

define('@dboneslabs/mpr/annotations/map-property',['require','exports','module','./reflect'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const reflect_1 = require("./reflect");
function mapProperty(type) {
    return (target, key) => {
        let t = reflect_1.ReflectMetadata.getTypeData(target.constructor)
            || {
                properties: {}
            };
        let metaData = reflect_1.ReflectMetadata.getPropertyData(target, key);
        let metaType = metaData.name.toLowerCase();
        let typeName = type == null || typeof type == "string" ? type : type.$$type;
        typeName = typeName != null ? typeName : metaData.$$type;
        if (metaType == "array") {
            typeName = typeName == null ? "object[]" : `${typeName}[]`;
        }
        if (typeName == null) {
            typeName = metaType;
        }
        t.properties[key] = {
            metaData: metaData,
            metaType: metaType,
            suppliedType: type,
            typeName: typeName,
            name: key
        };
        return reflect_1.ReflectMetadata.setTypeData(target.constructor, t);
    };
}
exports.mapProperty = mapProperty;

});

define('@dboneslabs/mpr/mapper-factory',['require','exports','module','./js-mapper','./configuration','./strategies/map-compiler','./initializing/builders/builder'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const js_mapper_1 = require("./js-mapper");
const configuration_1 = require("./configuration");
const map_compiler_1 = require("./strategies/map-compiler");
const builder_1 = require("./initializing/builders/builder");
class MapperFactory {
    constructor() {
        this._config = new configuration_1.Configuration();
        this.mapCompiler = new map_compiler_1.DefaultMapCompiler();
        this._builder = new builder_1.Builder(this._config);
    }
    addSetup(setup) {
        setup.configure(this._builder);
        return this;
    }
    setConfiguration(setupConfig) {
        setupConfig(this._config);
        return this;
    }
    createMapper() {
        let converters = this._builder.mappings.map(mapping => {
            return this.mapCompiler.Build(mapping, this._builder.typeMetas, this._config);
        });
        this._config.typeConverters.forEach(converter => {
            converters.push(converter);
        });
        return new js_mapper_1.JsMapper(this._config, converters);
    }
}
exports.MapperFactory = MapperFactory;

});

define('@dboneslabs/mpr/js-mapper',['require','exports','module','./core/mapping-context','./strategies/type-reflection'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const mapping_context_1 = require("./core/mapping-context");
const type_reflection_1 = require("./strategies/type-reflection");
class JsMapper {
    constructor(configuration, typeConverters) {
        this._configuration = configuration;
        this._typeConverterLocator = configuration.typeConverterLocator;
        this._typeReflection = new type_reflection_1.DefaultTypeReflection();
        typeConverters.forEach(converter => {
            this._typeConverterLocator.Add(converter);
        });
    }
    map(source, destinationType) {
        if (source == null)
            return source;
        if (destinationType == null)
            throw new Error("destinationType is null");
        if (typeof destinationType != "string") {
            destinationType = destinationType.$$type;
        }
        let sourceType = this._typeReflection.getType(source, this._configuration.typeStrategy);
        let mapLookup = this._typeConverterLocator.GetMapLookup(sourceType, destinationType);
        let ctx = this.createContext(source, null, mapLookup);
        this.mapIt(ctx);
        return ctx.destination;
    }
    mapTo(source, destination) {
        if (source == null)
            return;
        if (destination == null)
            throw new Error("destination is null");
        let sourceType = this._typeReflection.getType(source, this._configuration.typeStrategy);
        let destinationType = this._typeReflection.getType(source, this._configuration.typeStrategy);
        let mapLookup = this._typeConverterLocator.GetMapLookup(sourceType, destinationType);
        let ctx = this.createContext(source, destination, mapLookup);
        this.mapIt(ctx);
    }
    mapIt(context) {
        let mapLookup = context.mapInformation;
        let converter = this._typeConverterLocator.GetConverter(mapLookup);
        if (converter == null)
            throw new Error(`mapping not supported, ${mapLookup.source}->${mapLookup.destination}`);
        converter.execute(context);
        return context.destination;
    }
    createContext(source, destination, mapInformation) {
        let ctx = new mapping_context_1.MappingContext();
        ctx.destination = destination;
        ctx.source = source;
        ctx.mapper = this;
        ctx.mapInformation = mapInformation;
        return ctx;
    }
}
exports.JsMapper = JsMapper;

});

define('@dboneslabs/mpr/core/mapping-context',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class MappingContext {
}
exports.MappingContext = MappingContext;

});

define('@dboneslabs/mpr/strategies/type-reflection',['require','exports','module','../dictionary','../core/types'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const dictionary_1 = require("../dictionary");
const types_1 = require("../core/types");
class DefaultTypeReflection {
    constructor() {
        this._types = new dictionary_1.Dictionary();
        this._types.set("[object Date]", types_1.Types.date);
        this._types.set("[object String]", types_1.Types.string);
        this._types.set("[object Number]", types_1.Types.number);
        this._types.set("[object Boolean]", types_1.Types.boolean);
        this._types.set("[object Object]", types_1.Types.object);
        this._types.set("[object Array]", types_1.Types.objectArray);
    }
    getType(instance, typeStrategy) {
        let typeName = Object.prototype.toString.call(instance);
        let candiateType = this._types.get(typeName);
        if (candiateType != types_1.Types.object)
            return candiateType;
        let strongType = typeStrategy.getTypeFromTypeProperty(instance);
        return strongType == null ? candiateType : strongType;
    }
}
exports.DefaultTypeReflection = DefaultTypeReflection;

});

define('@dboneslabs/mpr/dictionary',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Dictionary {
    constructor() {
        this.state = {};
    }
    get keys() {
        return Object.keys(this.state);
    }
    set(key, value) {
        this.state[key] = value;
    }
    get(key) {
        let ret = this.state[key];
        return ret;
    }
    remove(key) {
        delete this.state[key];
    }
}
exports.Dictionary = Dictionary;

});

define('@dboneslabs/mpr/core/types',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Types {
    static asArray(type) {
        return typeof type == "string" ? `${type}[]` : `${type.$$type}[]`;
    }
    static AsArray(type) {
        return this.asArray(type);
    }
}
Types.string = "string";
Types.boolean = "boolean";
Types.number = "number";
Types.date = "date";
Types.objectArray = "object[]";
Types.object = "object";
Types.value = "value";
exports.Types = Types;

});

define('@dboneslabs/mpr/configuration',['require','exports','module','./core/type-converter-locator','./strategies/type-strategy','./strategies/naming-convention','./core/converters','./annotations/extract-metadata'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const type_converter_locator_1 = require("./core/type-converter-locator");
const type_strategy_1 = require("./strategies/type-strategy");
const naming_convention_1 = require("./strategies/naming-convention");
const converters_1 = require("./core/converters");
const extract_metadata_1 = require("./annotations/extract-metadata");
class Configuration {
    constructor() {
        this.typeStrategy = new type_strategy_1.DefaultTypeStrategy();
        this.namingConvention = new naming_convention_1.CamelCaseNamingConvention();
        this.typeConverterLocator = new type_converter_locator_1.DefaultTypeConverterLocator();
        this.typeConverters = new converters_1.Converts().getConverters();
        this.extractMetadata = new extract_metadata_1.ExtractMetadata();
    }
}
exports.Configuration = Configuration;

});

define('@dboneslabs/mpr/core/type-converter-locator',['require','exports','module','./map-information','../dictionary','./types'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const map_information_1 = require("./map-information");
const dictionary_1 = require("../dictionary");
const types_1 = require("./types");
class DefaultTypeConverterLocator {
    constructor() {
        this.nameExp = new RegExp("([\\.A-Za-z0-9\\-\\*]*)(\\[\\]){0,1}");
        this._mapsBySrcToDest = new dictionary_1.Dictionary();
    }
    Add(typeConverter) {
        let key = this.createKey(typeConverter.sourceType, typeConverter.destinationType);
        this._mapsBySrcToDest.set(key, typeConverter);
    }
    GetConverter(lookup) {
        let key = this.createKey(lookup.source.getName(), lookup.destination.getName());
        let converter = this._mapsBySrcToDest.get(key);
        if (converter != null)
            return converter;
        if (lookup.source.isArray == true && lookup.destination.isArray == true) {
            key = this.createKey(types_1.Types.objectArray, types_1.Types.objectArray);
            let converter = this._mapsBySrcToDest.get(key);
            if (converter != null)
                return converter;
        }
        throw new Error(`sorry key not supported ${key}`);
    }
    GetMapLookup(sourceType, destinationType) {
        let source = this.getMapComponent(sourceType);
        let destination = this.getMapComponent(destinationType);
        return new map_information_1.MapInformation(source, destination);
    }
    createKey(source, destination) {
        return `${source}->${destination}`;
    }
    getMapComponent(type) {
        let captures = this.nameExp.exec(type);
        let map = new map_information_1.MapComponent();
        map.type = captures[1];
        map.isArray = captures[2] != null;
        return map;
    }
}
exports.DefaultTypeConverterLocator = DefaultTypeConverterLocator;

});

define('@dboneslabs/mpr/core/map-information',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class MapInformation {
    constructor(src, dest) {
        this.source = src;
        this.destination = dest;
    }
}
exports.MapInformation = MapInformation;
class MapComponent {
    constructor() {
        this.isArray = false;
    }
    getName() {
        return (this.isArray)
            ? `${this.type}[]`
            : this.type;
    }
}
exports.MapComponent = MapComponent;

});

define('@dboneslabs/mpr/strategies/type-strategy',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class DefaultTypeStrategy {
    getTypeFromTypeProperty(instance) {
        let typeName = null;
        typeName = instance['$type'];
        if (typeName != null)
            return typeName;
        typeName = instance['_type'];
        if (typeName != null)
            return typeName;
        return null;
    }
}
exports.DefaultTypeStrategy = DefaultTypeStrategy;
class DollarTypeStrategy {
    getTypeFromTypeProperty(instance) {
        let typeName = instance['$type'];
        return typeName;
    }
}
exports.DollarTypeStrategy = DollarTypeStrategy;
class UnderscoreTypeStrategy {
    getTypeFromTypeProperty(instance) {
        let typeName = instance['_type'];
        return typeName;
    }
}
exports.UnderscoreTypeStrategy = UnderscoreTypeStrategy;

});

define('@dboneslabs/mpr/strategies/naming-convention',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class CamelCaseNamingConvention {
    convert(name) {
        return this.camelize(name);
    }
    camelize(str) {
        return str.replace(/(?:^\w|[A-Z]|\b\w)/g, function (letter, index) {
            return index == 0 ? letter.toLowerCase() : letter.toUpperCase();
        });
    }
}
exports.CamelCaseNamingConvention = CamelCaseNamingConvention;

});

define('@dboneslabs/mpr/core/converters',['require','exports','module','./types'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const types_1 = require("./types");
class Converts {
    getConverters() {
        return [
            new ArrayConverter(),
            new ValueArrayConverter(),
            new StringToStringConverter(),
            new NumberToNumberConverter(),
            new NumberToStringConverter(),
            new StringToNumberConverter(),
            new DateToDateConverter(),
            new StringToDateConverter(),
            new DateToStringConverter(),
            new BooleanToBooleanConverter(),
            new BooleanToStringConverter(),
            new StringToBooleanConverter(),
            new BooleanToNumberConverter(),
            new NumberToBooleanConverter(),
            new ValueToValueConverter()
        ];
    }
}
exports.Converts = Converts;
class ArrayConverter {
    constructor() {
        this.sourceType = types_1.Types.objectArray;
        this.destinationType = types_1.Types.objectArray;
    }
    execute(ctx) {
        if (ctx.destination == null)
            ctx.destination = [];
        ctx.source.forEach(item => {
            let value = ctx.mapper.map(item, ctx.mapInformation.destination.type);
            ctx.destination.push(value);
        });
        return ctx.destination;
    }
}
class ValueArrayConverter {
    constructor() {
        this.sourceType = 'value[]';
        this.destinationType = 'value[]';
    }
    execute(ctx) {
        if (ctx.destination == null)
            ctx.destination = [];
        ctx.source.forEach(item => {
            let value = ctx.mapper.map(item, types_1.Types.value);
            ctx.destination.push(value);
        });
        return ctx.destination;
    }
}
class ValueToValueConverter {
    constructor() {
        this.sourceType = types_1.Types.value;
        this.destinationType = types_1.Types.value;
    }
    execute(context) {
        context.destination = context.source;
    }
}
class StringToStringConverter {
    constructor() {
        this.sourceType = types_1.Types.string;
        this.destinationType = types_1.Types.string;
    }
    execute(ctx) {
        ctx.destination = ctx.source;
    }
}
class NumberToNumberConverter {
    constructor() {
        this.sourceType = types_1.Types.number;
        this.destinationType = types_1.Types.number;
    }
    execute(ctx) {
        ctx.destination = ctx.source;
    }
}
class StringToNumberConverter {
    constructor() {
        this.sourceType = types_1.Types.string;
        this.destinationType = types_1.Types.number;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = parseInt(ctx.source);
    }
}
class NumberToStringConverter {
    constructor() {
        this.sourceType = types_1.Types.number;
        this.destinationType = types_1.Types.string;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = ctx.source.toString();
    }
}
class DateToDateConverter {
    constructor() {
        this.sourceType = types_1.Types.date;
        this.destinationType = types_1.Types.date;
    }
    execute(ctx) {
        ctx.destination = new Date((ctx.source).getTime());
    }
}
class DateToStringConverter {
    constructor() {
        this.sourceType = types_1.Types.date;
        this.destinationType = types_1.Types.string;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = (ctx.source).toISOString();
    }
}
class StringToDateConverter {
    constructor() {
        this.sourceType = types_1.Types.string;
        this.destinationType = types_1.Types.date;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = new Date(ctx.source);
    }
}
class BooleanToBooleanConverter {
    constructor() {
        this.sourceType = types_1.Types.boolean;
        this.destinationType = types_1.Types.boolean;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = ctx.source;
    }
}
class StringToBooleanConverter {
    constructor() {
        this.sourceType = types_1.Types.string;
        this.destinationType = types_1.Types.boolean;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = ctx.source == "true";
    }
}
class BooleanToStringConverter {
    constructor() {
        this.sourceType = types_1.Types.boolean;
        this.destinationType = types_1.Types.string;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = ctx.source ? "true" : "false";
    }
}
class BooleanToNumberConverter {
    constructor() {
        this.sourceType = types_1.Types.boolean;
        this.destinationType = types_1.Types.number;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = ctx.source ? 1 : 0;
    }
}
class NumberToBooleanConverter {
    constructor() {
        this.sourceType = types_1.Types.number;
        this.destinationType = types_1.Types.boolean;
    }
    execute(ctx) {
        if (ctx.source == null)
            ctx.destination = null;
        else
            ctx.destination = ctx.source == 1;
    }
}

});

define('@dboneslabs/mpr/annotations/extract-metadata',['require','exports','module','./reflect'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const reflect_1 = require("./reflect");
class ExtractMetadata {
    getProperties(type) {
        let results = [];
        let t = reflect_1.ReflectMetadata.getTypeData(type);
        Object.keys(t.properties).forEach(key => {
            results.push(t.properties[key]);
        });
        return results;
    }
}
exports.ExtractMetadata = ExtractMetadata;

});

define('@dboneslabs/mpr/strategies/map-compiler',['require','exports','module','./type-reflection','../core/default-type-converter','../core/mapping-context','../core/map-information','./ctor-strategy'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const type_reflection_1 = require("./type-reflection");
const default_type_converter_1 = require("../core/default-type-converter");
const mapping_context_1 = require("../core/mapping-context");
const map_information_1 = require("../core/map-information");
const ctor_strategy_1 = require("./ctor-strategy");
class DefaultMapCompiler {
    Build(map, typeMetas, config) {
        if (map.converter != null)
            return map.converter;
        let info = config.typeConverterLocator.GetMapLookup(map.source || "", map.destination || "");
        let destinationMeta = typeMetas.get(map.destination);
        let sourceMeta = typeMetas.get(map.source);
        let setters = {};
        if (!info.source.isArray && !info.destination.isArray) {
            this.automapProperties(sourceMeta, destinationMeta, setters);
        }
        map.propertyMaps.forEach(map => {
            if (map.destinationName == null)
                return;
            let destinationName = map.destinationName;
            if (map.ignoreDestination) {
                delete setters[destinationName];
                return;
            }
            let destPropertyMeta = destinationMeta.properties.get(destinationName);
            setters[destinationName] = ((ctx) => {
                let source = map.sourceGetter(ctx.source);
                let result = ctx.mapper.map(source, destPropertyMeta.mapComponent.getName());
                map.destinationSetter(ctx.destination, result);
            });
        });
        let settersArray = Object.keys(setters).map(property => {
            return setters[property];
        });
        map.sourcePropertyMaps.forEach(map => {
            if (map.destinationName != null)
                return;
            if (!map.flatternSourceToDestination)
                return;
            let typeReflector = new type_reflection_1.DefaultTypeReflection();
            let setter = ((ctx) => {
                let source = map.sourceGetter(ctx.source);
                let sourceType = typeReflector.getType(source, config.typeStrategy);
                let childInfo = config.typeConverterLocator.GetMapLookup(sourceType, map.destinationType);
                let context = new mapping_context_1.MappingContext();
                context.source = source;
                context.mapInformation = childInfo;
                context.destination = ctx.destination;
                context.mapper = ctx.mapper;
                ctx.mapper.mapIt(context);
                ctx.destination = context.destination;
            });
            settersArray.push(setter);
        });
        let ctor = info.destination.isArray ? new ctor_strategy_1.ArrayCtor() : destinationMeta.ctor;
        return new default_type_converter_1.DefaultTypeConverter(info.source.getName(), info.destination.getName(), ctor, settersArray);
    }
    automapProperties(sourceMeta, destinationMeta, setters) {
        destinationMeta.propertiesKeyedOnCamelCase.keys.forEach(propertyName => {
            let destProperty = destinationMeta.propertiesKeyedOnCamelCase.get(propertyName);
            let srcProperty = sourceMeta.propertiesKeyedOnCamelCase.get(propertyName);
            if (srcProperty == null)
                return;
            setters[destProperty.name] = ((ctx) => {
                var info = new map_information_1.MapInformation(srcProperty.mapComponent, destProperty.mapComponent);
                var innerCtx = new mapping_context_1.MappingContext();
                innerCtx.source = ctx.source[srcProperty.name];
                innerCtx.destination = ctx.destination == null ? null : ctx.destination[destProperty.name];
                innerCtx.mapInformation = info;
                innerCtx.mapper = ctx.mapper;
                if (innerCtx.source === undefined)
                    return;
                ctx.mapper.mapIt(innerCtx);
                ctx.destination[destProperty.name] = innerCtx.destination;
            });
        });
    }
}
exports.DefaultMapCompiler = DefaultMapCompiler;

});

define('@dboneslabs/mpr/core/default-type-converter',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class DefaultTypeConverter {
    constructor(sourceType, destinationType, ctor, setters) {
        this.sourceType = sourceType;
        this.destinationType = destinationType;
        this.setters = setters;
        this.ctor = ctor;
    }
    execute(context) {
        if (context.destination == null)
            context.destination = this.ctor.createInstance();
        this.setters.forEach(setter => {
            let ctx = context;
            try {
                setter(ctx);
            }
            catch (error) {
                throw new Error(`failed to map ${this.sourceType} => ${this.destinationType}, innerException: ${error}`);
            }
        });
    }
}
exports.DefaultTypeConverter = DefaultTypeConverter;

});

define('@dboneslabs/mpr/strategies/ctor-strategy',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class AnonCtor {
    createInstance() {
        return {};
    }
}
exports.AnonCtor = AnonCtor;
class ArrayCtor {
    createInstance() {
        return [];
    }
}
exports.ArrayCtor = ArrayCtor;
class TypeCtor {
    constructor(ctor) {
        this._ctor = ctor;
    }
    createInstance() {
        return new this._ctor();
    }
}
exports.TypeCtor = TypeCtor;

});

define('@dboneslabs/mpr/initializing/builders/builder',['require','exports','module','../fluent-type-mapping','../mappings/type-map','../../dictionary','../metas/type-meta','../builders/property-builder'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const fluent_type_mapping_1 = require("../fluent-type-mapping");
const type_map_1 = require("../mappings/type-map");
const dictionary_1 = require("../../dictionary");
const type_meta_1 = require("../metas/type-meta");
const property_builder_1 = require("../builders/property-builder");
class Builder {
    constructor(configuration) {
        this.mappings = [];
        this.typeMetas = new dictionary_1.Dictionary();
        this._configuration = configuration;
    }
    addType(typeName, type = null) {
        if (typeof typeName != "string") {
            type = typeName;
            typeName = typeName.$$type;
        }
        let meta = new type_meta_1.TypeMeta(typeName);
        if (type != null) {
            meta.setType(type);
        }
        this.typeMetas.set(meta.name, meta);
        this.createMap(meta.name, meta.name);
        return new property_builder_1.PropertyBuilder(meta, this._configuration);
    }
    createMap(sourceType, destinationType) {
        if (typeof sourceType != "string") {
            sourceType = sourceType.$$type;
        }
        if (typeof destinationType != "string") {
            destinationType = destinationType.$$type;
        }
        let map = new type_map_1.TypeMap(sourceType, destinationType);
        let config = new fluent_type_mapping_1.FluentTypeMapping(map);
        this.mappings.push(map);
        return config;
    }
}
exports.Builder = Builder;

});

define('@dboneslabs/mpr/initializing/fluent-type-mapping',['require','exports','module','./mappings/property-map','./map-from-options','./map-source-options'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const property_map_1 = require("./mappings/property-map");
const map_from_options_1 = require("./map-from-options");
const map_source_options_1 = require("./map-source-options");
class FluentTypeMapping {
    constructor(mapping) {
        this._typeMapping = mapping;
    }
    forMember(destinationProperty, opts) {
        let propertyMap = new property_map_1.PropertyMap();
        propertyMap.destinationName = destinationProperty;
        propertyMap.destinationSetter = (instance, value) => instance[propertyMap.destinationName] = value;
        let options = new map_from_options_1.MapFromOptions(propertyMap);
        opts(options);
        this._typeMapping.propertyMaps.push(propertyMap);
        return this;
    }
    withSource(sourceProperty, opts) {
        let propertyMap = new property_map_1.PropertyMap();
        propertyMap.sourceGetter = (typeof sourceProperty == "string")
            ? (instance) => instance[sourceProperty]
            : sourceProperty;
        propertyMap.destinationType = this._typeMapping.destination;
        let options = new map_source_options_1.MapSourceOptions(propertyMap);
        opts(options);
        this._typeMapping.sourcePropertyMaps.push(propertyMap);
        return this;
    }
    using(typeConverter) {
        this._typeMapping.converter = typeConverter;
    }
}
exports.FluentTypeMapping = FluentTypeMapping;

});

define('@dboneslabs/mpr/initializing/mappings/property-map',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class PropertyMap {
    constructor() {
        this.flatternSourceToDestination = false;
        this.ignoreDestination = false;
    }
}
exports.PropertyMap = PropertyMap;

});

define('@dboneslabs/mpr/initializing/map-from-options',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class MapFromOptions {
    constructor(propertyMap) {
        this._propertyMap = propertyMap;
    }
    mapFrom(source, sourceName = null) {
        if (typeof source === 'string') {
            this._propertyMap.sourceGetter = (instance) => {
                return instance[source];
            };
            this._propertyMap.sourceName = source;
        }
        else {
            this._propertyMap.sourceGetter = source;
            this._propertyMap.sourceName = sourceName;
        }
    }
    using(func) {
        this._propertyMap.sourceGetter = (instance) => {
            return func(instance);
        };
    }
    ignore() {
        this._propertyMap.ignoreDestination = true;
    }
}
exports.MapFromOptions = MapFromOptions;

});

define('@dboneslabs/mpr/initializing/map-source-options',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class MapSourceOptions {
    constructor(propertyMap) {
        this._propertyMap = propertyMap;
    }
    flattern() {
        this._propertyMap.flatternSourceToDestination = true;
    }
}
exports.MapSourceOptions = MapSourceOptions;

});

define('@dboneslabs/mpr/initializing/mappings/type-map',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class TypeMap {
    constructor(source, destination) {
        this.propertyMaps = [];
        this.sourcePropertyMaps = [];
        if (source == null || source == '')
            throw new Error('source must be provided');
        if (destination == null || destination == '')
            throw new Error('destination must be provided');
        this.source = source;
        this.destination = destination;
    }
}
exports.TypeMap = TypeMap;

});

define('@dboneslabs/mpr/initializing/metas/type-meta',['require','exports','module','../../dictionary','./property-meta','../../strategies/ctor-strategy','../../core/map-information','../../core/types'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const dictionary_1 = require("../../dictionary");
const property_meta_1 = require("./property-meta");
const ctor_strategy_1 = require("../../strategies/ctor-strategy");
const map_information_1 = require("../../core/map-information");
const types_1 = require("../../core/types");
class TypeMeta {
    constructor(typeName) {
        this.isAnon = true;
        this.properties = new dictionary_1.Dictionary();
        this.propertiesKeyedOnCamelCase = new dictionary_1.Dictionary();
        this.ctor = new ctor_strategy_1.AnonCtor();
        this.name = typeName;
    }
    get hasId() {
        return this.id != null;
    }
    setType(type) {
        this.isAnon = false;
        this.ctor = new ctor_strategy_1.TypeCtor(type);
        this.actualType = type;
    }
    addProperty(name, processedName, type = types_1.Types.value) {
        let property = new property_meta_1.PropertyMeta();
        property.name = name;
        property.type = type;
        property.processedName = processedName;
        let mapComponent = new map_information_1.MapComponent();
        if (type.indexOf('[]') > -1) {
            type = type.replace('[]', '');
            mapComponent.isArray = true;
        }
        mapComponent.type = type;
        property.mapComponent = mapComponent;
        this.properties.set(name, property);
        this.propertiesKeyedOnCamelCase.set(processedName, property);
    }
}
exports.TypeMeta = TypeMeta;

});

define('@dboneslabs/mpr/initializing/metas/property-meta',['require','exports','module'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class PropertyMeta {
}
exports.PropertyMeta = PropertyMeta;

});

define('@dboneslabs/mpr/initializing/builders/property-builder',['require','exports','module','../../core/types'],function (require, exports, module) {"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const types_1 = require("../../core/types");
class PropertyBuilder {
    constructor(typeMeta, configuration) {
        this._typeMeta = typeMeta;
        this._configuration = configuration;
    }
    addProperty(name, type = types_1.Types.value) {
        var processedName = this._configuration.namingConvention.convert(name);
        this._typeMeta.addProperty(name, processedName, type);
        return this;
    }
    scanForAttributes() {
        let type = this._typeMeta.actualType;
        if (type == null)
            throw new Error("you need to set a type to scan");
        this._configuration.extractMetadata.getProperties(type).forEach(property => {
            this.addProperty(property.name, property.typeName);
        });
        return this;
    }
}
exports.PropertyBuilder = PropertyBuilder;

});

define('text!app.html', ['module'], function(module) { module.exports = "<template>\n  <section class=\"todoapp\">\n    <header class=\"header\">\n      <h1>todos</h1>\n      <input class=\"new-todo\" placeholder=\"What needs to be done?\" autofocus>\n    </header>\n    <!-- This section should be hidden by default and shown when there are todos -->\n    <section class=\"main\">\n      <input id=\"toggle-all\" class=\"toggle-all\" type=\"checkbox\">\n      <label for=\"toggle-all\">Mark all as complete</label>\n      <ul class=\"todo-list\">\n        <!-- These are here just to show the structure of the list items -->\n        <!-- List items should get the class `editing` when editing and `completed` when marked as completed -->\n        <li class=\"completed\">\n          <div class=\"view\">\n            <input class=\"toggle\" type=\"checkbox\" checked>\n            <label>Taste JavaScript</label>\n            <button class=\"destroy\"></button>\n          </div>\n          <input class=\"edit\" value=\"Create a TodoMVC template\">\n        </li>\n        <li repeat.for=\"todo of todoItems\">\n          <div class=\"view\">\n            <input class=\"toggle\" type=\"checkbox\">\n            <label>${todo.description}</label>\n            <button class=\"destroy\"></button>\n        </div>\n          <input class=\"edit\" value=\"Rule the web\">\n        </li>\n\n      </ul>\n    </section>\n    <!-- This footer should hidden by default and shown when there are todos -->\n    <footer class=\"footer\">\n      <!-- This should be `0 items left` by default -->\n      <span class=\"todo-count\"><strong>0</strong> item left</span>\n      <!-- Remove this if you don't implement routing -->\n      <ul class=\"filters\">\n        <li>\n          <a class=\"selected\" href=\"#/\">All</a>\n        </li>\n        <li>\n          <a href=\"#/active\">Active</a>\n        </li>\n        <li>\n          <a href=\"#/completed\">Completed</a>\n        </li>\n      </ul>\n      <!-- Hidden if no completed items are left ↓ -->\n      <button class=\"clear-completed\">Clear completed</button>\n    </footer>\n  </section>\n  <footer class=\"info\">\n    <p>Using ArmChair</p>\n    <!-- Remove the below line ↓ -->\n    <p>Source: <a href=\"https://bitbucket.org/dboneslabs/arm-chair\">Git Repo on BitBucket</a></p>\n    <p>Package: <a href=\"https://www.nuget.org/packages/ArmChair.Core\">.NET Core Package on nuget</a></p>\n    <p>Created by: <a href=\"http://dbones.co.uk\">dbones</a></p>\n  </footer>\n</template>"; });
define('text!shell.html', ['module'], function(module) { module.exports = "<template>\r\n    <require from=\"todomvc-common/base.css\"></require>\r\n    <require from=\"todomvc-app-css/index.css\"></require>\r\n\r\n    <router-view></router-view>\r\n</template>"; });
define('text!resources/elements/todo-item.html', ['module'], function(module) { module.exports = "<template>\r\n    <div class=\"view\">\r\n        <input class=\"toggle\" type=\"checkbox\">\r\n        <label>Buy a unicorn asdsa</label>\r\n        <button class=\"destroy\"></button>\r\n    </div>\r\n    <input class=\"edit\" value=\"Rule the web\">\r\n</template>"; });
//# sourceMappingURL=app-bundle.js.map