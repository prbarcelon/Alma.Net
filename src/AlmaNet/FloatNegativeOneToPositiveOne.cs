using System;

namespace AlmaNet
{
    public readonly struct FloatNegativeOneToPositiveOne
    {
        public FloatNegativeOneToPositiveOne(float value)
        {
            if (value < -1.0f || value > 1.0f)
                throw new ArgumentOutOfRangeException(nameof(value));

            Value = value;
        }

        public float Value { get; }

        public static implicit operator FloatNegativeOneToPositiveOne(float value)
        {
            return new FloatNegativeOneToPositiveOne(value);
        }

        public static implicit operator float(FloatNegativeOneToPositiveOne value)
        {
            return value.Value;
        }
    }
}