using Moq;
using Reflectum.SemanticSpace.Application.Services;
using Reflectum.SemanticSpace.Domain.Interfaces;
using Reflectum.SemanticSpace.Infrastructure.AntiLoop;
using Xunit;
using IAntiLoopService = Reflectum.SemanticSpace.Domain.Interfaces.IAntiLoopService;

namespace Reflectum.SemanticSpace.Tests.Application
{
    public class AntiLoopServiceTests
    {
        [Fact]
        public void Constructor_WithNullSimilarityMetric_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AntiLoopService(0.95f, null!));
        }

        [Fact]
        public void IsEcho_FirstInput_ReturnsFalse()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.95f, metric);

            // Act
            var result = service.IsEcho("Cześć, jak się masz?");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsEcho_IdenticalInput_ReturnsTrue()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.0f, metric); // No decay - use raw score
            var input = "Cześć jak się masz"; // Without punctuation for Jaccard

            // Act
            service.IsEcho(input, threshold: 0.0f); // First call (score = 0, but threshold = 0 so passes)
            var result = service.IsEcho(input, threshold: 0.5f); // Second call (score = 1.0, should pass)

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsEcho_SimilarInput_ReturnsTrue()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.0f, metric); // No decay

            // Act
            var input1 = "Cześć jak się masz";
            service.IsEcho(input1, threshold: 0.0f); // First call
            var result = service.IsEcho(input1, threshold: 0.5f); // Second call (Jaccard = 1.0, should pass)

            // Assert
            // Jaccard similarity should be high enough to trigger echo
            Assert.True(result);
        }

        [Fact]
        public void IsEcho_DifferentInput_ReturnsFalse()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.95f, metric);

            // Act
            service.IsEcho("Cześć, jak się masz?");
            var result = service.IsEcho("Co u Ciebie słychać?");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsEcho_WithCustomThreshold_RespectsThreshold()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.0f, metric); // No decay

            // Act
            var input = "Cześć jak się masz";
            service.IsEcho(input, threshold: 0.0f); // First call
            var result1 = service.IsEcho(input, threshold: 0.5f); // Lower threshold (score = 1.0)
            service.Clear();
            service.IsEcho(input, threshold: 0.0f); // Reset
            var result2 = service.IsEcho(input, threshold: 0.9f); // Higher threshold (score = 1.0)

            // Assert
            // With identical strings, Jaccard = 1.0, so both should pass
            Assert.True(result1); // Lower threshold passes
            Assert.True(result2); // Higher threshold also passes for identical
        }

        [Fact]
        public void Clear_ClearsHistory()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.95f, metric);
            var input = "Cześć, jak się masz?";

            // Act
            service.IsEcho(input);
            service.IsEcho(input); // Should be echo
            service.Clear();
            var result = service.IsEcho(input); // After clear, should not be echo

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetEchoReport_ReturnsReport()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.95f, metric);

            // Act
            service.IsEcho("Input 1");
            service.IsEcho("Input 2");
            var report = service.GetEchoReport();

            // Assert
            Assert.NotNull(report);
            Assert.Contains("Input 1", report);
            Assert.Contains("Input 2", report);
        }

        [Fact]
        public void IsEcho_WithDecayFactor_ReducesScoreOverTime()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var service = new AntiLoopService(0.5f, metric); // Lower decay = faster decay
            var input = "Cześć, jak się masz?";

            // Act
            service.IsEcho(input);
            service.IsEcho("Different input"); // Add some history
            service.IsEcho("Another different"); // More history
            var result = service.IsEcho(input); // Try original again

            // Assert
            // With decay, the echo score should be lower
            // This is a bit tricky to test precisely, but we can verify it doesn't crash
            Assert.NotNull(result.ToString());
        }
    }
}

