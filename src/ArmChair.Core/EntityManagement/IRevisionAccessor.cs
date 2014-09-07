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
namespace ArmChair.EntityManagement
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Utils;

    public interface IRevisionAccessor
    {
        void AllowAutoScanningForRevision();
        void DisableAutoScanningForRevision();
        void SetUpRevisionPattern(Func<Type, string> pattern);
        void SetUpRevision<T>(FieldInfo field);
        void SetUpRevision<T>(string fieldName);
        void SetUpRevision<T>(Expression<Func<T, object>> property);
        object GetRevision(object instance);
        void SetRevision(object instance, object id);
        FieldMeta GetRevisionField(Type type);
    }
}