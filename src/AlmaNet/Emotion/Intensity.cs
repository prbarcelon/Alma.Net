using System;

namespace AlmaNet.Emotion
{
    public readonly struct Intensity
    {
        public Intensity(float value)
        {
            if (value > 1.0f || value < 0.0f)
                throw new ArgumentOutOfRangeException(nameof(value));
            
            Value = value;
        }

        public float Value { get; }
        
        public static implicit operator Intensity(float value)
        {
            return new Intensity(value);
        }
    }
}