using System.Collections.Generic;

namespace AtelierBuilders.Models
{
    public class RegleRegul
    {
        public int Id { get; set; }
        public CompteCibleBase CompteCible { get; set; }
        public IReadOnlyCollection<Compte> ComptesImpactants { get; set; } = new List<Compte>();
        public Population Population { get; set; } = new Population();
        public int IdReglementaire { get; set; }
        public int? Seuil { get; set; }
        public int? Plafond { get; set; }
        public bool Consecutivite { get; set; }
        public ModePeriodeSeuil ModePeriodeSeuil { get; set; }
    }
}