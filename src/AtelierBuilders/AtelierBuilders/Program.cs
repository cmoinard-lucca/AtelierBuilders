using System;
using AtelierBuilders.Builders;
using AtelierBuilders.Models;

namespace AtelierBuilders
{
    class Program
    {
        static void Main(string[] args)
        {
            var pop =
                PopulationBuilder
                    .Reglementaire(3)
                    .Profils(2)
                    .Departements(83, 0100)
                    .Build();
            var regul =
                RegleRegulBuilder
                    .Reglementaire(3)
                    .ComptesImpactants(Comptes.Teletravail, Comptes.FormationInterne)
                    .CompteCible(Comptes.Rtt)
                    .Population(p => p.Profils(2).Departements(101, 383))
                    .Build();
            
            Console.WriteLine("Hello World!");
        }
    }
}