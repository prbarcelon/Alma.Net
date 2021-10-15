namespace AlmaNet.Personality
{
    public readonly struct OceanModel
    {
        public OceanModel(float openness, float conscientiousness, float extroversion, float agreeableness,
            float neuroticism)
        {
            Openness = openness;
            Conscientiousness = conscientiousness;
            Extroversion = extroversion;
            Agreeableness = agreeableness;
            Neuroticism = neuroticism;
        }

        public float Openness { get; }
        public float Conscientiousness { get; }
        public float Extroversion { get; }
        public float Agreeableness { get; }
        public float Neuroticism { get; }
    }
}