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
                    .CategorieCompteCible(Comptes.CongesPayes)
                    .Population(p => p.Profils(2).Departements(101, 383))
                    .AvecSeuilConsecutif()
                    .Seuil(30)
                    .Plafond(90)
                    .Mode(ModePeriodeSeuil.Depuis12Mois)
                    .Build();
            
            Console.WriteLine("Hello World!");
        }
    }
}