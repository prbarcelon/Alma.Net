using AlmaNet.Emotion;
using MathNet.Numerics.LinearAlgebra;

namespace AlmaNet
{
    public static class Helpers
    {
        public static float Clamp(this float value, float max = 1.0f, float min = -1.0f)
        {
            return value > max
                ? max
                : value < min
                    ? min
                    : value;
        }

        public static Vector<float> ToVector(this PadModel padModel)
        {
            var vector = Vector<float>.Build.Dense(new[]
            {
                padModel.Pleasure,
                padModel.Arousal,
                padModel.Dominance,
            });

            return vector;
        }

        public static PadModel ToPadModel(this Vector<float> vector)
        {
            return new PadModel(vector[0], vector[1], vector[2]);
        }
    }
}