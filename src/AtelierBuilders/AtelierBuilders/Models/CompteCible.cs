namespace AtelierBuilders.Models
{
    public abstract class CompteCibleBase
    {
    }
    
    public class CompteCible : CompteCibleBase
    {
        public CompteCible(Compte compte)
        {
            Compte = compte;
        }

        public Compte Compte { get; }
    }
    
    public class CategorieCompteCible : CompteCibleBase
    {
        public CategorieCompteCible(CategorieCompte categorieCompte)
        {
            CategorieCompte = categorieCompte;
        }

        public CategorieCompte CategorieCompte { get; }
    }
}