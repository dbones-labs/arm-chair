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
namespace ArmChair
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public interface ISession : IDisposable
    {
        void Add<T>(T instance) where T : class;
        void Remove<T>(T instance) where T : class;
        void Attach<T>(T instance) where T : class;
        
        IEnumerable<T> GetByIds<T>(IEnumerable ids) where T : class;
        T GetById<T>(object id) where T : class;

        void Commit();
    }
}