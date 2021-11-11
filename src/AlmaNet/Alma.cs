using System;
using AlmaNet.Emotion;
using AlmaNet.Personality;
using MathNet.Numerics.LinearAlgebra;

namespace AlmaNet
{
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
        public static VirtualEmotionCenter GetVirtualEmotionCenter(this EmotionAndIntensity[] activeEmotions)
        {
            float p, a, d, sumIntensity;
            p = a = d = sumIntensity = 0.0f;

            foreach (var activeEmotion in activeEmotions)
            {
                var pad = activeEmotion.Emotion.MapToPadSpace();
                p += pad.Pleasure;
                a += pad.Arousal;
                d += pad.Dominance;
                sumIntensity += activeEmotion.Intensity.Value;
            }

            var averageIntensity = sumIntensity / activeEmotions.Length;
            var clampedIntensity = new Intensity(averageIntensity.Clamp(1.0f, 0.0f));
            var center = new PadModel(p.Clamp(), a.Clamp(), d.Clamp());
            return new VirtualEmotionCenter(center, clampedIntensity);
        }

        /// <summary>
        ///     Converts the emotion to mood octants in PAD space.
        /// </summary>
        /// <param name="emotion"></param>
        /// <returns></returns>
        public static PadMoodOctants ToMood(this PadModel emotion)
        {
            var padMoodOctant = emotion switch
            {
                var m when m.Pleasure > 0 && m.Arousal > 0 && m.Dominance > 0 => PadMoodOctants.Exuberant,
                var m when m.Pleasure > 0 && m.Arousal > 0 && m.Dominance < 0 => PadMoodOctants.Dependent,
                var m when m.Pleasure > 0 && m.Arousal < 0 && m.Dominance > 0 => PadMoodOctants.Relaxed,
                var m when m.Pleasure > 0 && m.Arousal < 0 && m.Dominance < 0 => PadMoodOctants.Docile,

                var m when m.Pleasure < 0 && m.Arousal < 0 && m.Dominance < 0 => PadMoodOctants.Bored,
                var m when m.Pleasure < 0 && m.Arousal < 0 && m.Dominance > 0 => PadMoodOctants.Disdainful,
                var m when m.Pleasure < 0 && m.Arousal > 0 && m.Dominance < 0 => PadMoodOctants.Anxious,
                var m when m.Pleasure < 0 && m.Arousal > 0 && m.Dominance > 0 => PadMoodOctants.Hostile,

                _ => PadMoodOctants.Unknown
            };

            return padMoodOctant;
        }

        public static PadModel MapToPadSpace(this EmotionType emotion)
        {
            return emotion switch
            {
                EmotionType.None => new PadModel(),
                EmotionType.Admiration => new PadModel(0.5f, 0.3f, -0.2f),
                EmotionType.Anger => new PadModel(-0.51f, 0.59f, 0.25f),
                EmotionType.Disliking => new PadModel(-0.4f, 0.2f, 0.1f),
                EmotionType.Disappointment => new PadModel(-0.3f, 0.1f, -0.4f),
                EmotionType.Distress => new PadModel(-0.4f, -0.2f, -0.5f),
                EmotionType.Fear => new PadModel(-0.64f, 0.60f, -0.43f),
                EmotionType.FearsConfirmed => new PadModel(-0.5f, -0.3f, -0.7f),
                EmotionType.Gloating => new PadModel(0.3f, -0.3f, -0.1f),
                EmotionType.Gratification => new PadModel(0.6f, 0.5f, 0.4f),
                EmotionType.Gratitude => new PadModel(0.4f, 0.2f, -0.3f),
                EmotionType.HappyFor => new PadModel(0.4f, 0.2f, 0.2f),
                EmotionType.Hate => new PadModel(-0.6f, 0.6f, 0.3f),
                EmotionType.Hope => new PadModel(0.2f, 0.2f, -0.1f),
                EmotionType.Joy => new PadModel(0.4f, 0.2f, 0.1f),
                EmotionType.Liking => new PadModel(0.4f, 0.16f, -0.24f),
                EmotionType.Love => new PadModel(0.3f, 0.1f, 0.2f),
                EmotionType.Pity => new PadModel(-0.4f, -0.2f, -0.5f),
                EmotionType.Pride => new PadModel(0.4f, 0.3f, 0.3f),
                EmotionType.Relief => new PadModel(0.2f, -0.3f, 0.4f),
                EmotionType.Remorse => new PadModel(-0.3f, 0.1f, -0.6f),
                EmotionType.Reproach => new PadModel(-0.3f, -0.1f, 0.4f),
                EmotionType.Resentment => new PadModel(-0.2f, -0.3f, -0.2f),
                EmotionType.Satisfaction => new PadModel(0.3f, -0.2f, 0.4f),
                EmotionType.Shame => new PadModel(-0.3f, 0.1f, -0.6f),
                _ => throw new ArgumentOutOfRangeException(nameof(emotion), emotion, null)
            };
        }

        public static PadModel ApplyPushPull(this PadModel mood, VirtualEmotionCenter virtualEmotionCenter,
            int secondsElapsed = 1)
        {
            // "This defines the amount of time the pull and push mood change function needs to move a current
            // mood from one mood octant center to another one, presumably that a suitable virtual emotion
            // center exists that long, what is usually not the case. Our character’s usual mood change time
            // is 10 minutes."
            //
            // Gebhard, Patrick. (2005). ALMA: a layered model of affect. 29-36. 
            const float speedOfMoodChange = 1.0f / (10 * 60); // 0.00167 units/second

            var emotionCenter = virtualEmotionCenter.Center.ToVector();
            var currentMood = mood.ToVector();

            if (IsPullPhase(emotionCenter, currentMood))
            {
                // TODO - handle case where secondsElapsed is too high and pull overshoots center.
                var pullDirection = emotionCenter.Subtract(currentMood).Normalize(2);
                var pullAmount = speedOfMoodChange * secondsElapsed;
                var pullVector = pullDirection.Multiply(pullAmount);
                var nextMood = currentMood.Add(pullVector).ToPadModel();
                return nextMood;
            }
            else
            {
                var pushDirection = emotionCenter.Normalize(2);
                var pushAmount = speedOfMoodChange * secondsElapsed;
                var pushVector = pushDirection.Multiply(pushAmount);
                var nextMood = currentMood.Add(pushVector).ToPadModel();
                return nextMood;
            }
        }

        private static bool IsPullPhase(Vector<float> emotionCenter, Vector<float> currentMood)
        {
            var emotionCenterNorm = (float) emotionCenter.L2Norm();
            var projectionOfCurrentMoodOntoEmotionCenter =
                emotionCenter.Multiply(emotionCenter.DotProduct(currentMood) / (emotionCenterNorm * emotionCenterNorm));
            var moodProjectionNorm = (float) projectionOfCurrentMoodOntoEmotionCenter.L2Norm();
            var isCurrentMoodBetweenZeroPointAndVirtualEmotionCenter = moodProjectionNorm < emotionCenterNorm;
            return isCurrentMoodBetweenZeroPointAndVirtualEmotionCenter;
        }

        public static PadModel ApplyReturnToDefaultMood(this PadModel mood, PadModel defaultMood,
            int secondsElapsed = 1)
        {
            // "Another aspect of our mood simulation is that the current mood has a tendency to slowly move back to
            // the default mood. Generally, the return time depends how far the current mood is away from the default
            // mood. We take the longest distance of a mood octant (√3) for defining the mood return time. Currently
            // this is 20 minutes.
            //
            // Gebhard, Patrick. (2005). ALMA: a layered model of affect. 29-36.
            const float sqrt3 = 1.7320508075688772935274463415059f;
            const float speedOfMoodChange = sqrt3 / (20 * 60); // 0.00083 units/second

            var targetMood = defaultMood.ToVector();
            var currentMood = mood.ToVector();

            // TODO - handle case where secondsElapsed is too high and pull overshoots.
            var pullDirection = targetMood.Subtract(currentMood).Normalize(2);
            var pullAmount = speedOfMoodChange * secondsElapsed;
            var pullVector = pullDirection.Multiply(pullAmount);
            var nextMood = currentMood.Add(pullVector).ToPadModel();
            return nextMood;
        }
    }
}