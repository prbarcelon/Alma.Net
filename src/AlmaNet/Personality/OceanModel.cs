namespace AlmaNet.Personality
{
    public readonly struct OceanModel
    {
        public OceanModel(float openness, float conscientiousness, float extraversion, float agreeableness,
            float neuroticism)
        {
            Openness = openness;
            Conscientiousness = conscientiousness;
            Extraversion = extraversion;
            Agreeableness = agreeableness;
            Neuroticism = neuroticism;
        }

        public float Openness { get; }
        public float Conscientiousness { get; }
        public float Extraversion { get; }
        public float Agreeableness { get; }
        public float Neuroticism { get; }
    }
}