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
namespace ArmChair.Tests.Domain
{
    using System;

    public class Edition
    {
        protected int _hash;

        protected Edition() { }

        public Edition(string name, EditionType editionType)
        {
            Name = name;
            Type = editionType;
            _hash = Name.GetHashCode() + Type.GetHashCode();
        }
        
        public virtual DateTime ReleaseDate { get; set; }
        public virtual string Name { get; private set; }
        public virtual EditionType Type { get; private set; }

        public override bool Equals(object obj)
        {
            var other = obj as Edition;
            if (other == null)
            {
                return false;
            }

            return other.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return _hash;
        }
    }
}