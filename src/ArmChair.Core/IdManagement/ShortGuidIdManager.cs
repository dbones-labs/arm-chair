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
namespace ArmChair.IdManagement
{
    using System;


    /// <summary>
    /// uses the Short Guid as the ID for entities
    /// </summary>
    public class ShortGuidIdManager : IIdManager
    {
        private readonly IIdentityGenerator _identityGenerator;

        public ShortGuidIdManager()
        {
            _identityGenerator = new ShortGuidGenerator();
        }

        public Key GenerateId(Type type)
        {
            return new ShortGuidKey((string)_identityGenerator.GenerateId());
        }

        public Key GetFromId(Type type, object id)
        {
            return id == null ? null : new ShortGuidKey((string)id);
        }

        public Key GetFromCouchDbId(Type type, string id)
        {
            return new ShortGuidKey(id);
        }
    }
}