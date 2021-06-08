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
        RegleRegulBuilder.IConfigurationSeuil, RegleRegulBuilder.IConfigurationSeuil.IResult,
        RegleRegulBuilder.IBuild
    {
        public interface IRacine : IComptesImpactants, ICompteCible, IPopulation, IBuild
        {
        }

        public interface IComptesImpactants
        {
            public interface IResult : ICompteCible, IPopulation, IConfigurationSeuil, IBuild
            {
            }

            IResult ComptesImpactants(Compte compte1, params Compte[] autresComptes);
        }

        public interface ICompteCible
        {
            public interface IResult : IPopulation, IConfigurationSeuil, IBuild
            {
            }

            IResult CompteCible(CompteCibleBase compteCible);
        }

        public interface IPopulation
        {
            public interface IResult : IConfigurationSeuil, IBuild
            {
            }

            IResult Population(Func<PopulationBuilder.IRacine, PopulationBuilder.IBuild> configureBuilder);
        }
        
        public interface IConfigurationSeuil
        {
            public interface IResult : IBuild
            {
            }

            IResult AvecSeuilConsecutif(Func<ConfigurationSeuil.IRacine, ConfigurationSeuil.IBuild> configureBuilder);
            IResult AvecSeuilNonConsecutif(Func<ConfigurationSeuil.IRacine, ConfigurationSeuil.IBuild> configureBuilder);
        }
        
        public interface IBuild
        {
            RegleRegul Build();
        }

        private CompteCibleBase _compteCible = new CompteCible(Comptes.Cp2020);
        private IReadOnlyCollection<Compte> _comptesImpactants = new[] {Comptes.Maladie};
        private readonly int _idReglementaire;
        private Func<PopulationBuilder.IRacine, PopulationBuilder.IBuild>? _configurePopulationBuilder;
        private ConfigurationSeuil.IBuild? _configureSeuilBuilder;

        private RegleRegulBuilder(int idReglementaire)
        {
            _idReglementaire = idReglementaire;
        }

        public static IRacine Reglementaire(int idReglementaire) =>
            new RegleRegulBuilder(idReglementaire);

        public ICompteCible.IResult CompteCible(CompteCibleBase compteCible)
        {
            _compteCible = compteCible;
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

        public IPopulation.IResult Population(
            Func<PopulationBuilder.IRacine, PopulationBuilder.IBuild> configureBuilder)
        {
            _configurePopulationBuilder = configureBuilder;
            return this;
        }

        public IConfigurationSeuil.IResult AvecSeuilConsecutif(Func<ConfigurationSeuil.IRacine, ConfigurationSeuil.IBuild> configureBuilder)
        {
            _configureSeuilBuilder = configureBuilder(ConfigurationSeuil.Consecutif());
            return this;
        }

        public IConfigurationSeuil.IResult AvecSeuilNonConsecutif(Func<ConfigurationSeuil.IRacine, ConfigurationSeuil.IBuild> configureBuilder)
        {
            _configureSeuilBuilder = configureBuilder(ConfigurationSeuil.NonConsecutif());
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
                ConfigurationSeuil = _configureSeuilBuilder?.Build()
            };
        }
    }
}