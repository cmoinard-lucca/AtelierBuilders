using System.Collections.Generic;
using System.Linq;
using AtelierBuilders.Models;

namespace AtelierBuilders.Builders
{
    public class RegleRegulBuilder
    {
        private Compte _compteCible = Comptes.Cp2020;
        private IReadOnlyCollection<Compte> _comptesImpactants = new[] {Comptes.Maladie};

        private Population _population = new Population
        {
            IdsProfils = new[] {1}
        };

        public RegleRegulBuilder CompteCible(Compte compte)
        {
            _compteCible = compte;
            return this;
        }

        public RegleRegulBuilder ComptesImpactants(Compte compte1, params Compte[] autresComptes)
        {
            _comptesImpactants =
                new[] {compte1}
                    .Concat(autresComptes)
                    .ToList();

            return this;
        }

        public RegleRegul Build() =>
            new RegleRegul
            {
                Id = 1,
                Population = _population,
                CompteCible = _compteCible,
                ComptesImpactants = _comptesImpactants
            };
    }
}