using System.Collections.Generic;

namespace AtelierBuilders.Models
{
    public class Compte
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
    }

    public class CategorieCompte
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public IReadOnlyCollection<Compte> Comptes { get; set; }
    }

    public static class Comptes
    {
        public static CategorieCompte CongesPayes { get; } =
            new CategorieCompte
            {
                Id = 3,
                Nom = "Congés payés",
                Comptes = new[]
                {
                    new Compte
                    {
                        Numero = 1020,
                        Nom = "CP 2020/2021"
                    },
                    new Compte
                    {
                        Numero = 1021,
                        Nom = "CP 2021/2022"
                    },
                    new Compte
                    {
                        Numero = 1022,
                        Nom = "CP 2021/2023"
                    }
                }
            };
        
        public static Compte Maladie { get; } =
            new Compte
            {
                Numero = 33,
                Nom = "Maladie"
            };

        public static Compte Cp2020 { get; } =
            new Compte
            {
                Numero = 1020,
                Nom = "CP 2020"
            };

        public static Compte Rtt { get; } =
            new Compte
            {
                Numero = 3030,
                Nom = "RTT"
            };

        public static Compte Teletravail { get; } =
            new Compte
            {
                Numero = 11,
                Nom = "Télétravail"
            };

        public static Compte FormationInterne { get; } =
            new Compte
            {
                Numero = 20,
                Nom = "Formation interne"
            };
    }
}