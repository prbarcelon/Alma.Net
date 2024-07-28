using System;

namespace AlmaNet
{
    public readonly struct FloatNegativeOneToPositiveOne
    {
        public FloatNegativeOneToPositiveOne(float value, bool clamp = false)
        {
            if (clamp)
            {
                if (value < -1.0f)
                    value = -1.0f;
                else if (value > 1.0f)
                    value = 1.0f;
            }
            else if (value < -1.0f || value > 1.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            Value = value;
        }

        public float Value { get; }

        public static implicit operator FloatNegativeOneToPositiveOne(
            float value
        )
        {
            return new FloatNegativeOneToPositiveOne(value);
        }

        public static implicit operator float(
            FloatNegativeOneToPositiveOne value
        )
        {
            return value.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}