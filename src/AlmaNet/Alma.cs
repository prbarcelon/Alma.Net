using System;
using AlmaNet.Emotion;
using AlmaNet.Personality;

namespace AlmaNet
{
    public enum PadMoodOctant
    {
        Unknown = 0,
        Exuberant,
        Dependent,
        Relaxed,
        Docile,
        Bored,
        Disdainful,
        Anxious,
        Hostile
    }

    public static class Alma
    {
        public static PadModel ToPadModel(in this OceanModel o)
        {
            var pleasure = 0.21f * o.Extroversion
                           + 0.59f * o.Agreeableness
                           + 0.19f * o.Neuroticism;

            var arousal = 0.15f * o.Openness
                          + 0.30f * o.Agreeableness
                          - 0.57f * o.Neuroticism;

            var dominance = 0.25f * o.Openness
                            + 0.17f * o.Conscientiousness
                            + 0.60f * o.Extroversion
                            - 0.32f * o.Agreeableness;

            var padModel = new PadModel(pleasure, arousal, dominance);

            return padModel;
        }

        public static PadModel[] ToPadModel(this OceanModel[] oceanModels)
        {
            var numberOfModels = oceanModels.Length;
            var padModels = new PadModel[numberOfModels];
            for (var i = 0; i < numberOfModels; i++)
                padModels[i] = oceanModels[i].ToPadModel();

            return padModels;
        }

        /// <summary>
        ///     The virtual emotion center represents a point in the PAD space and has an intensity that is the average of all
        ///     active emotions’ intensity. Its maximum intensity is 1.0. If no active emotions exist, no virtual emotion center
        ///     exists and the current mood is not influenced by active emotions.
        /// </summary>
        /// <param name="activeEmotions"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static PadModel GetVirtualEmotionCenter(this PadModel[] activeEmotions)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Converts the emotion to mood octants in PAD space.
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public static PadMoodOctant ToMood(this PadModel emotion)
        {
            var padMoodOctant = emotion switch
            {
                var m when m.Pleasure > 0 && m.Arousal > 0 && m.Dominance > 0 => PadMoodOctant.Exuberant,
                var m when m.Pleasure > 0 && m.Arousal > 0 && m.Dominance < 0 => PadMoodOctant.Dependent,
                var m when m.Pleasure > 0 && m.Arousal < 0 && m.Dominance > 0 => PadMoodOctant.Relaxed,
                var m when m.Pleasure > 0 && m.Arousal < 0 && m.Dominance < 0 => PadMoodOctant.Docile,
                
                var m when m.Pleasure < 0 && m.Arousal < 0 && m.Dominance < 0 => PadMoodOctant.Bored,
                var m when m.Pleasure < 0 && m.Arousal < 0 && m.Dominance > 0 => PadMoodOctant.Disdainful,
                var m when m.Pleasure < 0 && m.Arousal > 0 && m.Dominance < 0 => PadMoodOctant.Anxious,
                var m when m.Pleasure < 0 && m.Arousal > 0 && m.Dominance > 0 => PadMoodOctant.Hostile,
                
                _ => PadMoodOctant.Unknown
            };

            return padMoodOctant;
        }
    }
}