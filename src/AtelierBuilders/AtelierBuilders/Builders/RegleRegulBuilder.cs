using System;
using System.Collections.Generic;
using System.Linq;
using AtelierBuilders.Models;

namespace AtelierBuilders.Builders
{
    public class RegleRegulBuilder :
        RegleRegulBuilder.IRacine,
        RegleRegulBuilder.IComptesImpactants, RegleRegulBuilder.IComptesImpactants.IResult, 
        RegleRegulBuilder.ICompteCible, RegleRegulBuilder.ICompteCible.IResult,
        RegleRegulBuilder.IPopulation, RegleRegulBuilder.IPopulation.IResult,
        RegleRegulBuilder.IConsecutivite, RegleRegulBuilder.IConsecutivite.IResult,
        RegleRegulBuilder.ISeuil, RegleRegulBuilder.ISeuil.IResult,
        RegleRegulBuilder.IPlafond, RegleRegulBuilder.IPlafond.IResult,
        RegleRegulBuilder.IModePeriodeSeuil, RegleRegulBuilder.IModePeriodeSeuil.IResult,
        RegleRegulBuilder.IBuild
    {
        public interface IRacine : IComptesImpactants, ICompteCible, IPopulation, IBuild
        {
        }
        
        public interface IComptesImpactants
        {
            public interface IResult : ICompteCible, IPopulation, IConsecutivite, IBuild
            {
            }

            IResult ComptesImpactants(Compte compte1, params Compte[] autresComptes);
        }
        
        public interface ICompteCible
        {
            public interface IResult : IPopulation, IConsecutivite, IBuild
            {
            }

            IResult CompteCible(Compte compte);
        }
        
        public interface IPopulation
        {
            public interface IResult : IConsecutivite, IBuild
            {
            }

            IResult Population(Func<PopulationBuilder.IRacine, PopulationBuilder.IBuild> configureBuilder);
        }
        
        public interface IConsecutivite
        {
            public interface IResult : ISeuil, IPlafond
            {
            }

            IResult AvecSeuilConsecutif();
            IResult AvecSeuilNonConsecutif();
        }
        
        public interface ISeuil
        {
            public interface IResult : IPlafond, IModePeriodeSeuil, IBuild
            {
            }

            IResult Seuil(int seuil);
        }
        
        public interface IPlafond
        {
            public interface IResult : IModePeriodeSeuil, IBuild
            {
            }

            IResult Plafond(int plafond);
        }
        
        public interface IModePeriodeSeuil
        {
            public interface IResult : IBuild
            {
            }

            IResult Mode(ModePeriodeSeuil mode);
        }
        
        public interface IBuild
        {
            RegleRegul Build();
        }
        
        private Compte _compteCible = Comptes.Cp2020;
        private IReadOnlyCollection<Compte> _comptesImpactants = new[] {Comptes.Maladie};
        private readonly int _idReglementaire;
        private Func<PopulationBuilder.IRacine, PopulationBuilder.IBuild> _configurePopulationBuilder;
        private bool _consecutivite = true;
        private int? _seuil;
        private int? _plafond;
        private ModePeriodeSeuil _modeSeuil = ModePeriodeSeuil.PeriodeCourante;

        private RegleRegulBuilder(int idReglementaire)
        {
            _idReglementaire = idReglementaire;
        }

        public static IRacine Reglementaire(int idReglementaire) => 
            new RegleRegulBuilder(idReglementaire);
        
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

        public IPopulation.IResult Population(Func<PopulationBuilder.IRacine, PopulationBuilder.IBuild> configureBuilder)
        {
            _configurePopulationBuilder = configureBuilder;
            return this;
        }

        public IConsecutivite.IResult AvecSeuilConsecutif()
        {
            _consecutivite = true;
            return this;
        }

        public IConsecutivite.IResult AvecSeuilNonConsecutif()
        {
            _consecutivite = false;
            return this;
        }

        public ISeuil.IResult Seuil(int seuil)
        {
            _seuil = seuil;
            return this;
        }

        public IPlafond.IResult Plafond(int plafond)
        {
            _plafond = plafond;
            return this;
        }

        public IModePeriodeSeuil.IResult Mode(ModePeriodeSeuil mode)
        {
            _modeSeuil = mode;
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
                IdReglementaire = _idReglementaire,
                Population = 
                    _configurePopulationBuilder != null
                        ? _configurePopulationBuilder(PopulationBuilder.Reglementaire(_idReglementaire)).Build()
                        : defaultPopulation,
                CompteCible = _compteCible,
                ComptesImpactants = _comptesImpactants,
                Consecutivite = _consecutivite,
                Seuil = _seuil,
                Plafond = _plafond,
                ModePeriodeSeuil = _modeSeuil
            };
        }
    }
}