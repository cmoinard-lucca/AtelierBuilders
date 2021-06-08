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
                    .CompteCible(new CategorieCompteCible(Comptes.CongesPayes))
                    .Population(p => p.Profils(2).Departements(101, 383))
                    .AvecSeuilConsecutif(s => s
                        .Seuil(20)
                        .Plafond(90)
                        .ModePeriod(ModePeriodeSeuil.Depuis12Mois))
                    .Build();
            var regul2 =
                RegleRegulBuilder
                    .Reglementaire(3)
                    .MoinsDeCongesSiMaladie()
                    .Population(p => p.Profils(2).Departements(101, 383))
                    .SeuilConsecutif30()
                    .Build();
            
            Console.WriteLine("Hello World!");
        }
    }
}