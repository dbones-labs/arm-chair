using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;

namespace ArmChair.Castle
{
    public class Documentify : IDocumentify
    {
        readonly ProxyGenerator _generator = new ProxyGenerator(new PersistentProxyBuilder());

        public T AggregateRoot<T>(T instance = default(T)) where T : class
        {
            return instance == null
                ? (T) _generator.CreateClassProxy(typeof (T), new []{ typeof(IDocument) }, new DocumentInterceptor())
                : (T) _generator.CreateClassProxyWithTarget(typeof(T), new [] { typeof(IDocument) }, instance, new DocumentInterceptor());
        }
    }
}
