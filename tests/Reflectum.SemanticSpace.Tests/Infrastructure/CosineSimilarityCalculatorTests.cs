using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Infrastructure.Similarity;
using Reflectum.SemanticSpace.Math;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Infrastructure
{
    public class CosineSimilarityCalculatorTests
    {
        [Fact]
        public void Calculate_WithValidVectors_ReturnsSimilarity()
        {
            // Arrange
            var calculator = new CosineSimilarityCalculator();
            var vector1 = new MeaningVector
            {
                Word = "test1",
                Vector = new List<double> { 1.0, 2.0, 3.0 }
            };
            var vector2 = new MeaningVector
            {
                Word = "test2",
                Vector = new List<double> { 4.0, 5.0, 6.0 }
            };

            // Act
            var result = calculator.Calculate(vector1, vector2);

            // Assert
            var expected = CosineSimilarity.Between(vector1.ToArray(), vector2.ToArray());
            Assert.Equal(expected, result, 5);
        }

        [Fact]
        public void Calculate_WithIdenticalVectors_ReturnsOne()
        {
            // Arrange
            var calculator = new CosineSimilarityCalculator();
            var vector = new MeaningVector
            {
                Word = "test",
                Vector = new List<double> { 1.0, 2.0, 3.0 }
            };

            // Act
            var result = calculator.Calculate(vector, vector);

            // Assert
            Assert.Equal(1.0, result, 5);
        }

        [Fact]
        public void Calculate_WithOrthogonalVectors_ReturnsZero()
        {
            // Arrange
            var calculator = new CosineSimilarityCalculator();
            var vector1 = new MeaningVector
            {
                Word = "test1",
                Vector = new List<double> { 1.0, 0.0, 0.0 }
            };
            var vector2 = new MeaningVector
            {
                Word = "test2",
                Vector = new List<double> { 0.0, 1.0, 0.0 }
            };

            // Act
            var result = calculator.Calculate(vector1, vector2);

            // Assert
            Assert.Equal(0.0, result, 5);
        }
    }
}

