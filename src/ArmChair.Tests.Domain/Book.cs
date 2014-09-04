using System.Collections.Generic;
using System.Linq;

namespace ArmChair.Tests.Domain
{
    public class Book : EntityRoot
    {
        private readonly IList<Contributor> _contributors = new List<Contributor>();
        private readonly IList<Edition> _editions = new List<Edition>();

        protected Book()
        {

        }

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