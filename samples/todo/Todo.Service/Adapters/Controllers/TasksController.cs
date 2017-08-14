namespace Todo.Service.Adapters.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using AutoMapper;
    using Dto.Resources;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using TodoTask = Models.Task;
    using TodoResource = Todo.Service.Dto.Resources.TodoResource;

    [Route("api/v1/[controller]")]
    public class TasksController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TasksController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public virtual async Task<CollectionResource<TodoResource>> Get([FromQuery] AllQueryString queryString)
        {
            if (queryString == null) throw new ArgumentNullException(nameof(queryString));

            var query = _mapper.Map<Ports.Queries.AllTasks>(queryString);
            var tasks = await _mediator.Send(query);

            var result = _mapper.Map<CollectionResource<TodoResource>>(tasks);
            return result;
        }

        [Route("active")]
        [HttpGet]
        public virtual async Task<CollectionResource<TodoResource>> Get([FromQuery] ByActiveQueryString queryString)
        {
            if (queryString == null) throw new ArgumentNullException(nameof(queryString));

            var query = _mapper.Map<Ports.Queries.TasksByActive>(queryString);
            var tasks = await _mediator.Send(query);

            var result = _mapper.Map<CollectionResource<TodoResource>>(tasks);
            return result;
        }


        [Route("priority")]
        [HttpGet]
        public virtual async Task<CollectionResource<TodoResource>> Get([FromQuery] ByPriorityQueryString queryString)
        {
            if (queryString == null) throw new ArgumentNullException(nameof(queryString));
            var query = _mapper.Map<Ports.Queries.TasksByPriority>(queryString);
            var tasks = await _mediator.Send(query);

            var result = _mapper.Map<CollectionResource<TodoResource>>(tasks);
            return result;
        }

        [HttpPost]
        public virtual async Task<TodoResource> Create([FromBody]TodoResource todoResource)
        {
            if (todoResource == null) throw new ArgumentNullException(nameof(todoResource));

            var taskCommand = _mapper.Map<Ports.Commands.CreateUpdateTask>(todoResource);
            var task = await _mediator.Send(taskCommand);

            return _mapper.Map<TodoResource>(task);
        }

        [Route("{id}")]
        [HttpPut]
        public virtual async Task<TodoResource> Update(string id, [FromBody]TodoResource todoResource)
        {
            if (todoResource == null) throw new ArgumentNullException(nameof(todoResource));
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Value cannot be null or empty.", nameof(id));
            if (id != todoResource.Id) throw new ValidationException();
            
            var taskCommand = _mapper.Map<Ports.Commands.CreateUpdateTask>(todoResource);
            var task = await _mediator.Send(taskCommand);

            return _mapper.Map<TodoResource>(task);
        }
        
        [Route("{id}/complete")]
        [HttpPut]
        public virtual async Task<TodoResource> Complete([FromBody]TodoResource todoResource)
        {
            if (todoResource == null) throw new ArgumentNullException(nameof(todoResource));

            var taskCommand = _mapper.Map<Ports.Commands.MarkTaskAsComplete>(todoResource);
            var task = await _mediator.Send(taskCommand);

            return _mapper.Map<TodoResource>(task);
        }

        [HttpDelete("{id}")]
        public virtual async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Value cannot be null or empty.", nameof(id));
            await _mediator.Send(new Ports.Commands.RemoveTask() { Id = id });
        }
        
        [HttpPost("prune")]
        public virtual async Task Prune()
        {
            await _mediator.Send(new Ports.Commands.PuneStaleTasks());
        }

    }


    public class AllQueryString : FromQueryStringBase
    {
        public string OrderBy { get; set; } = "date";
    }

    public class ByActiveQueryString : FromQueryStringBase
    {
        public string OrderBy { get; set; } = "date";
    }

    public class ByPriorityQueryString : FromQueryStringBase
    {
        public string Priority { get; set; } = "High";
    }

}