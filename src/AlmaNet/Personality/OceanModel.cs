namespace AlmaNet.Personality
{
    public readonly struct OceanModel
    {
        public OceanModel(FloatNegativeOneToPositiveOne openness, FloatNegativeOneToPositiveOne conscientiousness,
            FloatNegativeOneToPositiveOne extroversion, FloatNegativeOneToPositiveOne agreeableness,
            FloatNegativeOneToPositiveOne neuroticism)
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

        public override string ToString()
        {
            return $"O:{Openness}, C:{Conscientiousness}, E:{Extroversion}, A:{Agreeableness}, N:{Neuroticism}";
        }
    }
}