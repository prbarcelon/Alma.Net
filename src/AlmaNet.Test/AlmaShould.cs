using AlmaNet.Emotion;
using AlmaNet.Personality;
using FluentAssertions;
using Xunit;

namespace AlmaNet.Test
{
    public class AlmaShould
    {
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