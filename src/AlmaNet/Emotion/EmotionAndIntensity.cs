namespace AlmaNet.Emotion
{
    public readonly struct EmotionAndIntensity
    {
        public EmotionAndIntensity(EmotionType emotion, Intensity intensity)
        {
            Emotion = emotion;
            Intensity = intensity;
        }

        public EmotionType Emotion { get; }
        public Intensity Intensity { get; }
    }
}