using Reflectum.SemanticSpace.Infrastructure.AntiLoop;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Infrastructure
{
    public class JaccardSimilarityMetricTests
    {
        [Fact]
        public void Calculate_IdenticalTexts_ReturnsOne()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var text = "miłość przyjaźń bliskość";

            // Act
            var result = metric.Calculate(text, text);

            // Assert
            Assert.Equal(1.0f, result);
        }

        [Fact]
        public void Calculate_CompletelyDifferentTexts_ReturnsZero()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var text1 = "miłość przyjaźń";
            var text2 = "samochód dom";

            // Act
            var result = metric.Calculate(text1, text2);

            // Assert
            Assert.Equal(0.0f, result);
        }

        [Fact]
        public void Calculate_PartiallyOverlapping_ReturnsCorrectValue()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var text1 = "miłość przyjaźń bliskość";
            var text2 = "miłość przyjaźń samochód";

            // Act
            var result = metric.Calculate(text1, text2);

            // Assert
            // Intersection: {miłość, przyjaźń} = 2
            // Union: {miłość, przyjaźń, bliskość, samochód} = 4
            // Jaccard = 2/4 = 0.5
            Assert.Equal(0.5f, result);
        }

        [Fact]
        public void Calculate_EmptyTexts_ReturnsZero()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();

            // Act
            var result = metric.Calculate("", "");

            // Assert
            Assert.Equal(0.0f, result);
        }

        [Fact]
        public void Calculate_OneEmptyText_ReturnsZero()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();

            // Act
            var result = metric.Calculate("miłość przyjaźń", "");

            // Assert
            Assert.Equal(0.0f, result);
        }

        [Fact]
        public void Calculate_WithDuplicateWords_HandlesCorrectly()
        {
            // Arrange
            var metric = new JaccardSimilarityMetric();
            var text1 = "miłość miłość przyjaźń";
            var text2 = "miłość przyjaźń przyjaźń";

            // Act
            var result = metric.Calculate(text1, text2);

            // Assert
            // Sets: {miłość, przyjaźń} vs {miłość, przyjaźń}
            // Intersection: 2, Union: 2, Jaccard = 1.0
            Assert.Equal(1.0f, result);
        }
    }
}

