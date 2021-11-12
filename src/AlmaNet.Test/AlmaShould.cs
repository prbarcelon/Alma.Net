using AlmaNet.Emotion;
using AlmaNet.Personality;
using FluentAssertions;
using Xunit;

namespace AlmaNet.Test
{
    public class AlmaShould
    {
        /// <summary>
        /// <para>
        /// This test illustrates how to use AlmaNet.
        /// </para>
        ///
        /// <para>
        /// Generate the OCEAN personality for an NPC, then converted the personality to PAD space.
        /// </para>
        ///
        /// <para>
        /// Generate emotional reactions and intensity from in-game events (see GAMYGDALA at
        /// https://github.com/prbarcelon/Gamygdala.Net) and calculate the Virtual Emotion Center (VEC). The VEC is a
        /// point in PAD space that pushes/pulls the current NPC's emotional state.
        /// </para>
        ///
        /// <para>
        /// In actual use, the current mood calculations would happen within a game loop instead of like the for loop
        /// used below. The emotional state would be updated by the VEC then the emotional state would decay towards the
        /// NPC's default mood (defined by the NPC's personality).
        /// </para>
        ///
        /// <para>
        /// At any time during the loop, the NPC's mood in PAD space can be converted into an intensity and PAD mood
        /// octant (e.g. slightly relaxed) for use in other game systems such as dialog generation or NPC AI.
        /// </para>
        /// 
        /// <para>
        /// NOTE: The VEC is not being modified in this example. In actual use, the emotions feeding into the VEC
        /// calculation would themselves decay, which would affect the VEC values used in the push/pull calculations.
        /// Both the ALMA and GAMYGDALA papers cover emotion generation and decay, with GAMYGDALA factoring in the NPC's
        /// goals and beliefs of potential outcomes/futures.
        /// </para>
        /// </summary>
        [Fact]
        public void ReturnExpectedResult_EndToEndTest()
        {
            const int simulatedSeconds = 20 * 60; // 20 minutes
            const int timeStepSeconds = 10; // evaluate new mood every 10 seconds

            // Arrange
            var personality = new OceanModel(0.4f, 0.8f, 0.6f, 0.3f, 0.4f);
            var defaultMood = personality.ToPadModel();
            var emotions = new[]
            {
                new EmotionAndIntensity(EmotionType.Hope, 0.5f),
                new EmotionAndIntensity(EmotionType.Admiration, 1.0f)
            };
            var virtualEmotionCenter = emotions.GetVirtualEmotionCenter();

            // Act
            var currentMoodInPadSpace = defaultMood;
            for (var i = 0; i < simulatedSeconds; i += timeStepSeconds)
            {
                currentMoodInPadSpace = currentMoodInPadSpace
                    .ApplyPushPull(virtualEmotionCenter, timeStepSeconds)
                    .ApplyReturnToDefaultMood(defaultMood, timeStepSeconds);
            }

            var finalMood = currentMoodInPadSpace.ToMood();

            // Assert
            finalMood.Intensity.Should().Be(MoodIntensity.Moderately);
            finalMood.MoodType.Should().Be(PadMoodOctants.Relaxed);
        }

        [Theory]
        [InlineData(0.4f, 0.8f, 0.6f, 0.3f, 0.4f, 0.38f, -0.08f, 0.50f)]
        public void ReturnExpectedPadModel_WhenGivenPersonality(float openness, float conscientiousness,
            float extroversion, float agreeableness, float neuroticism, float expectedPleasure, float expectedArousal,
            float expectedDominance)
        {
            // Arrange
            var personality = new OceanModel(openness, conscientiousness, extroversion, agreeableness, neuroticism);

            // Act
            var padModelPersonality = personality.ToPadModel();

            // Assert
            padModelPersonality.Pleasure.Should().BeApproximately(expectedPleasure, 1e-2f);
            padModelPersonality.Arousal.Should().BeApproximately(expectedArousal, 1e-2f);
            padModelPersonality.Dominance.Should().BeApproximately(expectedDominance, 1e-2f);
        }

        [Theory]
        [InlineData(EmotionType.None, 0.0f, 0.0f, 0.0f)]
        [InlineData(EmotionType.Admiration, 0.5f, 0.3f, -0.2f)]
        [InlineData(EmotionType.Anger, -0.51f, 0.59f, 0.25f)]
        [InlineData(EmotionType.Disliking, -0.4f, 0.2f, 0.1f)]
        [InlineData(EmotionType.Disappointment, -0.3f, 0.1f, -0.4f)]
        [InlineData(EmotionType.Distress, -0.4f, -0.2f, -0.5f)]
        [InlineData(EmotionType.Fear, -0.64f, 0.6f, -0.43f)]
        [InlineData(EmotionType.FearsConfirmed, -0.5f, -0.3f, -0.7f)]
        [InlineData(EmotionType.Gloating, 0.3f, -0.3f, -0.1f)]
        [InlineData(EmotionType.Gratification, 0.6f, 0.5f, 0.4f)]
        [InlineData(EmotionType.Gratitude, 0.4f, 0.2f, -0.3f)]
        [InlineData(EmotionType.HappyFor, 0.4f, 0.2f, 0.2f)]
        [InlineData(EmotionType.Hate, -0.6f, 0.6f, 0.3f)]
        [InlineData(EmotionType.Hope, 0.2f, 0.2f, -0.1f)]
        [InlineData(EmotionType.Joy, 0.4f, 0.2f, 0.1f)]
        [InlineData(EmotionType.Liking, 0.4f, 0.16f, -0.24f)]
        [InlineData(EmotionType.Love, 0.3f, 0.1f, 0.2f)]
        [InlineData(EmotionType.Pity, -0.4f, -0.2f, -0.5f)]
        [InlineData(EmotionType.Pride, 0.4f, 0.3f, 0.3f)]
        [InlineData(EmotionType.Relief, 0.2f, -0.3f, 0.4f)]
        [InlineData(EmotionType.Remorse, -0.3f, 0.1f, -0.6f)]
        [InlineData(EmotionType.Reproach, -0.3f, -0.1f, 0.4f)]
        [InlineData(EmotionType.Resentment, -0.2f, -0.3f, -0.2f)]
        [InlineData(EmotionType.Satisfaction, 0.3f, -0.2f, 0.4f)]
        [InlineData(EmotionType.Shame, -0.3f, 0.1f, -0.6f)]
        public void ReturnExpectedPadModel_WhenGivenEmotion(EmotionType emotion, float expectedPleasure,
            float expectedArousal, float expectedDominance)
        {
            // Arrange

            // Act
            var emotionInPadSpace = emotion.MapToPadSpace();

            // Assert
            emotionInPadSpace.Pleasure.Should().BeApproximately(expectedPleasure, 1e-2f);
            emotionInPadSpace.Arousal.Should().BeApproximately(expectedArousal, 1e-2f);
            emotionInPadSpace.Dominance.Should().BeApproximately(expectedDominance, 1e-2f);
        }

        [Fact]
        public void ReturnExpectedVirtualEmotionCenter_WhenGiven_Two_EmotionsAndIntensity()
        {
            // Arrange
            var emotions = new[]
            {
                new EmotionAndIntensity(EmotionType.Hope, 0.5f),
                new EmotionAndIntensity(EmotionType.Admiration, 1.0f)
            };

            // Act
            var virtualEmotionCenter = emotions.GetVirtualEmotionCenter();

            // Assert
            virtualEmotionCenter.Intensity.Value.Should().BeApproximately(0.75f, 1e-2f);
            virtualEmotionCenter.Center.Pleasure.Should().BeApproximately(0.7f, 1e-2f);
            virtualEmotionCenter.Center.Arousal.Should().BeApproximately(0.5f, 1e-2f);
            virtualEmotionCenter.Center.Dominance.Should().BeApproximately(-0.3f, 1e-2f);
        }

        [Fact]
        public void ReturnExpectedVirtualEmotionCenter_WhenGiven_Many_EmotionsAndIntensity_AndClampIsNeeded()
        {
            // Arrange
            var emotions = new[]
            {
                new EmotionAndIntensity(EmotionType.Admiration, 0.5f),
                new EmotionAndIntensity(EmotionType.Gratification, 1.0f),
                new EmotionAndIntensity(EmotionType.Gratitude, 0.5f),
                new EmotionAndIntensity(EmotionType.HappyFor, 1.0f),
                new EmotionAndIntensity(EmotionType.Joy, 0.5f),
                new EmotionAndIntensity(EmotionType.Liking, 1.0f),
                new EmotionAndIntensity(EmotionType.Pride, 0.75f),
            };

            // Act
            var virtualEmotionCenter = emotions.GetVirtualEmotionCenter();

            // Assert
            virtualEmotionCenter.Intensity.Value.Should().BeApproximately(0.75f, 1e-2f);
            virtualEmotionCenter.Center.Pleasure.Should().BeApproximately(1.0f, 1e-2f);
            virtualEmotionCenter.Center.Arousal.Should().BeApproximately(1.0f, 1e-2f);
            virtualEmotionCenter.Center.Dominance.Should().BeApproximately(0.26f, 1e-2f);
        }

        [Fact]
        public void CalculateNewMood_WhenCurrentMoodIsInPullPhase()
        {
            // Arrange
            var virtualEmotionCenter = new VirtualEmotionCenter(new PadModel(0.5f, 0, 0), new Intensity(0.5f));
            var initialMood = new PadModel();

            // Act
            var finalMood = initialMood.ApplyPushPull(virtualEmotionCenter, 300);

            // Assert
            finalMood.Pleasure.Should().BeApproximately(0.25f, 1e-2f);
            finalMood.Arousal.Should().BeApproximately(0, 1e-2f);
            finalMood.Dominance.Should().BeApproximately(0, 1e-2f);
        }

        [Fact]
        public void CalculateNewMood_WhenCurrentMoodIsInPushPhase()
        {
            // Arrange
            var virtualEmotionCenter = new VirtualEmotionCenter(new PadModel(.25f, 0, 0), new Intensity(0.5f));
            var initialMood = new PadModel(0.5f, 0, 0);

            // Act
            var finalMood = initialMood.ApplyPushPull(virtualEmotionCenter, 300);

            // Assert
            finalMood.Pleasure.Should().BeApproximately(0.75f, 1e-2f);
            finalMood.Arousal.Should().BeApproximately(0, 1e-2f);
            finalMood.Dominance.Should().BeApproximately(0, 1e-2f);
        }

        [Fact]
        public void CalculateNewMood_WhenMovingToDefaultMood()
        {
            // Arrange
            var defaultMood = new PadModel();
            var initialMood = new PadModel(1f, 0, 0);

            // Act
            var finalMood = initialMood.ApplyReturnToDefaultMood(defaultMood, 600);

            // Assert
            finalMood.Pleasure.Should().BeApproximately(1.0f - AlmaConstants.Sqrt3 / 2.0f, 1e-2f);
            finalMood.Arousal.Should().BeApproximately(0, 1e-2f);
            finalMood.Dominance.Should().BeApproximately(0, 1e-2f);
        }

        [Theory]
        [InlineData(0.25f, -0.18f, 0.12f, MoodIntensity.Slightly, PadMoodOctants.Relaxed)]
        [InlineData(0.38f, -0.08f, 0.50f, MoodIntensity.Moderately, PadMoodOctants.Relaxed)]
        [InlineData(0.35f, 0.39f, 0.34f, MoodIntensity.Moderately, PadMoodOctants.Exuberant)]
        public void ReturnMoodAndIntensity(float pleasure, float arousal, float dominance,
            MoodIntensity expectedMoodIntensity, PadMoodOctants expectedPadMoodOctant)
        {
            // Arrange
            var moodPadModel = new PadModel(pleasure, arousal, dominance);

            // Act
            var mood = moodPadModel.ToMood();

            // Assert
            mood.MoodType.Should().Be(expectedPadMoodOctant);
            mood.Intensity.Should().Be(expectedMoodIntensity);
        }
    }
}