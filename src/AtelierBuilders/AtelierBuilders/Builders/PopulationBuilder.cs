using System.Collections.Generic;
using System.Linq;
using AtelierBuilders.Models;

namespace AtelierBuilders.Builders
{
    public class PopulationBuilder :
        PopulationBuilder.IRacine,
        PopulationBuilder.IProfils, PopulationBuilder.IProfils.IResult,
        PopulationBuilder.IEntitesLegales, PopulationBuilder.IEntitesLegales.IResult,
        PopulationBuilder.IDepartements, PopulationBuilder.IDepartements.IResult,
        PopulationBuilder.IBuild
    {
        private IReadOnlyCollection<int> _idsProfils = new List<int>();
        private IReadOnlyCollection<int> _idsEntitesLegales = new List<int>();
        private IReadOnlyCollection<int> _idsDepartements;

        private PopulationBuilder()
        {
        }

        public static IRacine Declare()
        {
            return new PopulationBuilder();
        }

        public interface IRacine : IProfils
        {
        }

        public interface IProfils
        {
            public interface IResult : IEntitesLegales, IDepartements, IBuild
            {
            }

            IResult Profils(int id1, params int[] autresIds);
        }
        
        public interface IEntitesLegales
        {
            public interface IResult : IDepartements, IBuild
            {
            }
            
            IResult EntitesLegales(int id1, params int[] autresIds);
        }
        
        public interface IDepartements
        {
            public interface IResult : IBuild
            {
            }
            
            IResult Departements(int id1, params int[] autresIds);
        }

        public interface IBuild
        {
            Population Build();
        }

        public IProfils.IResult Profils(int id1, params int[] autresIds)
        {
            _idsProfils = new[] {id1}.Concat(autresIds).ToList();
            return this;
        }

        public IEntitesLegales.IResult EntitesLegales(int id1, params int[] autresIds)
        {
            _idsEntitesLegales = new[] {id1}.Concat(autresIds).ToList();
            return this;
        }

        public IDepartements.IResult Departements(int id1, params int[] autresIds)
        {
            _idsDepartements = new[] {id1}.Concat(autresIds).ToList();
            return this;
        }

        public Population Build() =>
            new Population
            {
                IdsProfils = _idsProfils,
                IdsEntiteLegales = _idsEntitesLegales,
                IdsDepartements = _idsDepartements
            };
    }
}