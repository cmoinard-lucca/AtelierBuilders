using System.Collections.Generic;
using System.Linq;
using AtelierBuilders.Models;

namespace AtelierBuilders.Builders
{
    public class RegleRegulBuilder :
        RegleRegulBuilder.IComptesImpactants, RegleRegulBuilder.IComptesImpactants.IResult, 
        RegleRegulBuilder.ICompteCible, RegleRegulBuilder.ICompteCible.IResult,
        RegleRegulBuilder.IPopulation, RegleRegulBuilder.IPopulation.IResult,
        RegleRegulBuilder.IBuild
    {
        public interface IComptesImpactants : IBuild
        {
            public interface IResult : ICompteCible, IBuild
            {
            }

            IResult ComptesImpactants(Compte compte1, params Compte[] autresComptes);
        }
        
        public interface ICompteCible
        {
            public interface IResult : IPopulation, IBuild
            {
            }

            IResult CompteCible(Compte compte);
        }
        
        public interface IPopulation
        {
            public interface IResult : IBuild
            {
            }

            IResult Population(PopulationBuilder.IBuild builder);
        }
        
        public interface IBuild
        {
            RegleRegul Build();
        }
        
        
        private Compte _compteCible = Comptes.Cp2020;
        private IReadOnlyCollection<Compte> _comptesImpactants = new[] {Comptes.Maladie};

        private PopulationBuilder.IBuild _populationBuilder;

        public ICompteCible.IResult CompteCible(Compte compte)
        {
            _compteCible = compte;
            return this;
        }

        public IComptesImpactants.IResult ComptesImpactants(Compte compte1, params Compte[] autresComptes)
        {
            _comptesImpactants =
                new[] {compte1}
                    .Concat(autresComptes)
                    .ToList();

            return this;
        }

        public IPopulation.IResult Population(PopulationBuilder.IBuild builder)
        {
            _populationBuilder = builder;
            return this;
        }

        public RegleRegul Build()
        {
            var defaultPopulation = new Population
            {
                IdsProfils = new[] {1}
            };
            
            return new RegleRegul
            {
                Id = 1,
                Population = _populationBuilder?.Build() ?? defaultPopulation,
                CompteCible = _compteCible,
                ComptesImpactants = _comptesImpactants
            };
        }
    }
}