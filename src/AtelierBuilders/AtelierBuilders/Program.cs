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
                PopulationBuilder.Declare()
                    .Profils(2)
                    .Departements(83, 0100)
                    .Build();
            var regul =
                new RegleRegulBuilder()
                    .ComptesImpactants(Comptes.Teletravail, Comptes.FormationInterne)
                    .CompteCible(Comptes.Rtt)
                    .Population(PopulationBuilder.Declare().Profils(2).Departements(101, 383))
                    .Build();
            
            Console.WriteLine("Hello World!");
        }
    }
}