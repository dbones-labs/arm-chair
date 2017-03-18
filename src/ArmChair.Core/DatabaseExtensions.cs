// Copyright 2014 - dbones.co.uk (David Rundle)
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
    using System.Collections.Generic;
    using EntityManagement;
    using EntityManagement.Config;

    public static class DatabaseExtensions
    {
        /// <summary>
        /// <see cref="ITypeManager.Register"/>
        /// </summary>
        public static void Register(this Database database, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                database.Settings.TypeManager.Register(type);
            }
        }

        /// <summary>
        /// <see cref="ITypeManager.Register"/>, 
        /// if you are using default conventions, then use the <see cref="Register(ArmChair.Settings,System.Collections.Generic.IEnumerable{System.Type})"/> instead.
        /// note when using the class maps you can provide additional information about a class.
        /// </summary>
        public static void Register(this Database database, IEnumerable<ClassMap> classMaps)
        {
            foreach (var map in classMaps)
            {
                database.Settings.TypeManager.Register(map.Type);

                //register class level info
                if (map.IdField != null) database.Settings.IdAccessor.SetUpId(map.Type, map.IdField);
                if (map.RevisionField != null) database.Settings.RevisionAccessor.SetUpRevision(map.Type, map.RevisionField);

                //register indexes.
                foreach (var index in map.Indexes)
                {
                    database.Index.Create(index);
                }
            }
        }

    }
}