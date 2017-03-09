namespace ArmChair.Linq.Transform
{
    using System;
    using System.Collections.Generic;
    using Handlers;
    using Handlers.BinaryHandlers;
    using Handlers.CollectionHandlers;
    using Handlers.InNotInHandlers;
    using Handlers.StringHandlers;

    /// <summary>
    /// regisrty of all the handlers used to convert the linq where clauses into mongo json object.
    /// </summary>
    public static class HandlerRegistry
    {
        public static readonly IDictionary<Type, List<IHandler>> Handlers = new Dictionary<Type, List<IHandler>>();

        static HandlerRegistry()
        {
            Register<AndOrVisitorHandler>();

            Register<EqualityHandler>();
            Register<NotHandler>();
            Register<LessAndGreaterThanHandler>();

            Register<StringContainsHandler>();
            Register<StringStartsWithHandler>();
            Register<StringEndsWithHandler>();

            Register<AnyHandler>();

            Register<InHandler>();
            Register<NotInHandler>();
        }

        public static void Register<T>() where T : IHandler, new()
        {
            var handler = new T();

            List<IHandler> handlers = null;
            if (!Handlers.TryGetValue(handler.HandleTypeOf, out handlers))
            {
                handlers = new List<IHandler>();
                Handlers.Add(handler.HandleTypeOf, handlers);
            }

            handlers.Add(handler);
        }
    }
}