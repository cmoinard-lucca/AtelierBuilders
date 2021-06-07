using System.Collections.Generic;

namespace AtelierBuilders.Models
{
    public class RegleRegul
    {
        public int Id { get; set; }
        public Compte CompteCible { get; set; }
        public IReadOnlyCollection<Compte> ComptesImpactants { get; set; }
        public Population Population { get; set; }
        public int IdReglementaire { get; set; }
        public int? Seuil { get; set; }
        public int? Plafond { get; set; }
        public bool Consecutivite { get; set; }
        public ModePeriodeSeuil ModePeriodeSeuil { get; set; }
    }
}