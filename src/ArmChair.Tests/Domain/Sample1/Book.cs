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
namespace ArmChair.Tests.Domain.Sample1
{
    using System.Collections.Generic;
    using System.Linq;

    public class Book : EntityRoot
    {
        private readonly IList<Contributor> _contributors = new List<Contributor>();
        private readonly IList<Edition> _editions = new List<Edition>();

        public Book() { }

        public Book(string title, Person author)
        {
            Title = title;
            _contributors.Add(new Contributor(author, ContributorType.Author));
        }

        public virtual void AddContributor(Person contributor, ContributorType contributorType)
        {
            if (_contributors.Any(x => x.ContributorId == contributor.Id))
                return;

            _contributors.Add(new Contributor(contributor, contributorType));
        }

        public virtual void AddEdition(Edition edition)
        {
            if (_editions.Contains(edition))
            {
                _editions.Remove(edition);
            }
            _editions.Add(edition);
        }

        public virtual string Title { get; private set; }
        public virtual IEnumerable<Contributor> Contributors { get { return _contributors; } }
        public virtual IEnumerable<Edition> Editions { get { return _editions; } }
    }
}