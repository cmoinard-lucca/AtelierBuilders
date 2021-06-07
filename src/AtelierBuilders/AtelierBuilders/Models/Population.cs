using System.Collections.Generic;

namespace AtelierBuilders.Models
{
    public class Population
    {
        public IReadOnlyCollection<int> IdsProfils { get; set; }
        public IReadOnlyCollection<int> IdsEntiteLegales { get; set; }
        public IReadOnlyCollection<int> IdsDepartements { get; set; }
    }
}