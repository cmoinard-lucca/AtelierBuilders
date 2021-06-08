namespace AtelierBuilders.Models
{
    public class ConfigurationSeuil
    {
        private ConfigurationSeuil(int? seuil, int? plafond, bool consecutivite, ModePeriodeSeuil modePeriode)
        {
            Seuil = seuil;
            Plafond = plafond;
            Consecutivite = consecutivite;
            ModePeriode = modePeriode;
        }

        public int? Seuil { get; }
        public int? Plafond { get; }
        public bool Consecutivite { get; }
        public ModePeriodeSeuil ModePeriode { get; }

        public static IRacine Consecutif() =>
            new Builder(true);

        public static IRacine NonConsecutif() =>
            new Builder(false);

        public interface IBuild
        {
            ConfigurationSeuil Build();
        }

        public interface IRacine : ISeuil, IPlafond
        {
        }
        
        public interface ISeuil
        {
            public interface IResult : IPlafond, IModePeriod, IBuild
            {
            }

            IResult Seuil(int seuil);
        }
        
        public interface IPlafond
        {
            public interface IResult : IModePeriod, IBuild
            {
            }

            IResult Plafond(int plafond);
        }
        
        public interface IModePeriod
        {
            public interface IResult : IBuild
            {
            }

            IResult ModePeriod(ModePeriodeSeuil mode);
        }

        private class Builder :
            IRacine,
            ISeuil, ISeuil.IResult,
            IPlafond, IPlafond.IResult,
            IModePeriod, IModePeriod.IResult,
            IBuild
        {
            private readonly bool _consecutivite;
            private int? _seuil;
            private int? _plafond;
            private ModePeriodeSeuil _mode = ModePeriodeSeuil.PeriodeCourante;

            public Builder(bool consecutivite)
            {
                _consecutivite = consecutivite;
            }

            public ISeuil.IResult Seuil(int seuil)
            {
                _seuil = seuil;
                return this;
            }

            public IPlafond.IResult Plafond(int plafond)
            {
                _plafond = plafond;
                return this;
            }

            public IModePeriod.IResult ModePeriod(ModePeriodeSeuil mode)
            {
                _mode = mode;
                return this;
            }

            public ConfigurationSeuil Build() =>
                new ConfigurationSeuil(_seuil, _plafond, _consecutivite, _mode);
        }
    }
}