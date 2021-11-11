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
    }
}