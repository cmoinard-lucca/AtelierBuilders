namespace AtelierBuilders.Models
{
    public class Compte
    {
        public int Numero { get; set; }
        public string Nom { get; set; }
    }

    public static class Comptes
    {
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