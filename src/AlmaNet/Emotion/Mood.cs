namespace AlmaNet.Emotion
{
    public readonly struct Mood
    {
        public Mood(PadMoodOctants moodType, MoodIntensity intensity)
        {
            MoodType = moodType;
            Intensity = intensity;
        }

        public PadMoodOctants MoodType { get; }
        public MoodIntensity Intensity { get; }
    }
}