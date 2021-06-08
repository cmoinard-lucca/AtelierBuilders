using AtelierBuilders.Builders;
using AtelierBuilders.Models;

namespace AtelierBuilders
{
    public static class BuilderExtensions
    {
        public static RegleRegulBuilder.ICompteCible.IResult MoinsDeCongesSiMaladie(this RegleRegulBuilder.IRacine builder) =>
            builder
                .ComptesImpactants(Comptes.Maladie)
                .CompteCible(new CategorieCompteCible(Comptes.CongesPayes));

        public static RegleRegulBuilder.IConfigurationSeuil.IResult SeuilConsecutif30(
            this RegleRegulBuilder.IConfigurationSeuil builder) =>
            builder
                .AvecSeuilConsecutif(
                    c => c.Seuil(30));
    }
}