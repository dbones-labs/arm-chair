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
namespace ArmChair.Serialization.Newton
{
    using System;
    using System.Collections.Generic;
    using EntityManagement;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// the base override supplies support for the id and revision fields
    /// </summary>
    public class BaseOverride : PropertiesOverride<object>
    {
        private readonly IIdAccessor _idAccessor;
        private readonly IRevisionAccessor _revisionAccessor;

        /// <summary>
        /// create the base override, to support the Id and Revision
        /// </summary>
        /// <param name="idAccessor">the idaccessor use to find the Id</param>
        /// <param name="revisionAccessor">the revisionaccessor is used to find the revision</param>
        public BaseOverride(IIdAccessor idAccessor, IRevisionAccessor revisionAccessor)
        {
            _idAccessor = idAccessor;
            _revisionAccessor = revisionAccessor;
        }

        public override void Set(Type actualType, IDictionary<string, JsonProperty> properties)
        {
            var idField = _idAccessor.GetIdField(actualType);
            var revField = _revisionAccessor.GetRevisionField(actualType);

            if (idField == null || revField == null)
            {
                return;
            }

            properties[ToCamelCase(idField.FriendlyName)].PropertyName = "_id";
            properties[ToCamelCase(revField.FriendlyName)].PropertyName = "_rev";
        }
    }
}