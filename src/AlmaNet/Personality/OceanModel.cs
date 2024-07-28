namespace AlmaNet.Personality
{
    public readonly struct OceanModel
    {
        public OceanModel(
            FloatNegativeOneToPositiveOne openness,
            FloatNegativeOneToPositiveOne conscientiousness,
            FloatNegativeOneToPositiveOne extroversion,
            FloatNegativeOneToPositiveOne agreeableness,
            FloatNegativeOneToPositiveOne neuroticism
        )
        {
            Openness = openness;
            Conscientiousness = conscientiousness;
            Extroversion = extroversion;
            Agreeableness = agreeableness;
            Neuroticism = neuroticism;
        }

        public FloatNegativeOneToPositiveOne Openness { get; }
        public FloatNegativeOneToPositiveOne Conscientiousness { get; }
        public FloatNegativeOneToPositiveOne Extroversion { get; }
        public FloatNegativeOneToPositiveOne Agreeableness { get; }
        public FloatNegativeOneToPositiveOne Neuroticism { get; }

        public static OceanModel operator *(OceanModel left, float right)
        {
            return new OceanModel(
                Clamp(left.Openness * right),
                Clamp(left.Conscientiousness * right),
                Clamp(left.Extroversion * right),
                Clamp(left.Agreeableness * right),
                Clamp(left.Neuroticism * right)
            );
        }

        public static OceanModel operator +(OceanModel left, OceanModel right)
        {
            return new OceanModel(
                Clamp(left.Openness + right.Openness),
                Clamp(left.Conscientiousness + right.Conscientiousness),
                Clamp(left.Extroversion + right.Extroversion),
                Clamp(left.Agreeableness + right.Agreeableness),
                Clamp(left.Neuroticism + right.Neuroticism)
            );
        }

        public override string ToString()
        {
            return
                $"O:{Openness}, C:{Conscientiousness}, E:{Extroversion}, A:{Agreeableness}, N:{Neuroticism}";
        }

        private static FloatNegativeOneToPositiveOne Clamp(float value)
        {
            return new FloatNegativeOneToPositiveOne(value, true);
        }
    }
}