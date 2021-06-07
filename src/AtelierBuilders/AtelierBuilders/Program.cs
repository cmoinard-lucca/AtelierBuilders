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
                    .CompteCible(Comptes.Rtt)
                    .ComptesImpactants(Comptes.Teletravail, Comptes.FormationInterne)
                    .Build();
            
            Console.WriteLine("Hello World!");
        }
    }
}