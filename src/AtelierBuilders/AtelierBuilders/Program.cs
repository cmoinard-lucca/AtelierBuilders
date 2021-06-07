using System;
using AtelierBuilders.Builders;
using AtelierBuilders.Models;

namespace AtelierBuilders
{
    class Program
    {
        static void Main(string[] args)
        {
            var regul =
                new RegleRegulBuilder()
                    .ComptesImpactants(Comptes.Teletravail, Comptes.FormationInterne)
                    .CompteCible(Comptes.Rtt)
                    .Build();
            
            Console.WriteLine("Hello World!");
        }
    }
}