﻿// Copyright 2014 - dbones.co.uk (David Rundle)
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
namespace ArmChair.Tests.Domain
{
    public class Contributor
    {
        protected Contributor() { }

        public Contributor(string id, string name, ContributorType contribution)
        {
            ContributorId = id;
            Name = name;
            Contribution = contribution;
        }

        public Contributor(Person person, ContributorType contribution)
            : this(person.Id, person.Name, contribution)
        {

        }

        public virtual string Name { get; private set; }
        public virtual ContributorType Contribution { get; private set; }
        public virtual string ContributorId { get; private set; }
    }
}