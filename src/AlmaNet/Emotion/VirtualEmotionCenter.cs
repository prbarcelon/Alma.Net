namespace AlmaNet.Emotion
{
    public readonly struct VirtualEmotionCenter
    {
        public VirtualEmotionCenter(PadModel center, Intensity intensity)
        {
            Center = center;
            Intensity = intensity;
        }

        public PadModel Center { get; }
        public Intensity Intensity { get; }
    }
}