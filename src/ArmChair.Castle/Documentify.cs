// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
//  
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
