namespace AlmaNet.Emotion
{
    public readonly struct PadModel
    {
        public PadModel(float pleasure, float arousal, float dominance)
        {
            Pleasure = pleasure;
            Arousal = arousal;
            Dominance = dominance;
        }

        public float Pleasure { get; }
        public float Arousal { get; }
        public float Dominance { get; }

        public override string ToString()
        {
            return $"P:{Pleasure}, A:{Arousal}, D:{Dominance}";
        }
    }
}