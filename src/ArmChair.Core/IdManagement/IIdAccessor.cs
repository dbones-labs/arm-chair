﻿// Copyright 2013 - 2014 dbones.co.uk (David Rundle)
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
using System;
using System.Linq.Expressions;
using System.Reflection;
using ArmChair.Utils;

namespace ArmChair.IdManagement
{
    public interface IIdAccessor
    {
        void AllowAutoScanningForId();
        void SetUpIdPattern(Func<Type, string> pattern);
        void SetUpId<T>(FieldInfo field);
        void SetUpId<T>(string fieldName);
        void SetUpId<T>(Expression<Func<T, object>> property);
        object GetId(object instance);
        void SetId(object instance, object id);
        FieldMeta GetIdField(Type type);
    }
}