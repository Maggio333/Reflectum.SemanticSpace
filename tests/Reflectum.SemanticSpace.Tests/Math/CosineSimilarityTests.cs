using Reflectum.SemanticSpace.Math;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Math
{
    public class CosineSimilarityTests
    {
        [Fact]
        public void Between_IdenticalVectors_ReturnsOne()
        {
            // Arrange
            var vector = new double[] { 1.0, 2.0, 3.0 };

            // Act
            var result = CosineSimilarity.Between(vector, vector);

            // Assert
            Assert.Equal(1.0, result, 5);
        }

        [Fact]
        public void Between_OrthogonalVectors_ReturnsZero()
        {
            // Arrange
            var vector1 = new double[] { 1.0, 0.0, 0.0 };
            var vector2 = new double[] { 0.0, 1.0, 0.0 };

            // Act
            var result = CosineSimilarity.Between(vector1, vector2);

            // Assert
            Assert.Equal(0.0, result, 5);
        }

        [Fact]
        public void Between_OppositeVectors_ReturnsMinusOne()
        {
            // Arrange
            var vector1 = new double[] { 1.0, 0.0, 0.0 };
            var vector2 = new double[] { -1.0, 0.0, 0.0 };

            // Act
            var result = CosineSimilarity.Between(vector1, vector2);

            // Assert
            Assert.Equal(-1.0, result, 5);
        }

        [Fact]
        public void Between_DifferentLengthVectors_CalculatesCorrectly()
        {
            // Arrange
            var vector1 = new double[] { 1.0, 2.0, 3.0 };
            var vector2 = new double[] { 4.0, 5.0, 6.0 };

            // Act
            var result = CosineSimilarity.Between(vector1, vector2);

            // Assert
            // Expected: (1*4 + 2*5 + 3*6) / (sqrt(1+4+9) * sqrt(16+25+36))
            // = 32 / (sqrt(14) * sqrt(77)) ≈ 0.9746
            Assert.InRange(result, 0.97, 0.98);
        }

        [Fact]
        public void Between_ZeroVectors_ReturnsZero()
        {
            // Arrange
            var vector1 = new double[] { 0.0, 0.0, 0.0 };
            var vector2 = new double[] { 0.0, 0.0, 0.0 };

            // Act
            var result = CosineSimilarity.Between(vector1, vector2);

            // Assert
            Assert.Equal(0.0, result, 5);
        }

        [Fact]
        public void Between_DifferentDimensions_ThrowsArgumentException()
        {
            // Arrange
            var vector1 = new double[] { 1.0, 2.0 };
            var vector2 = new double[] { 1.0, 2.0, 3.0 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => CosineSimilarity.Between(vector1, vector2));
        }

        [Fact]
        public void Between_EmptyVectors_ReturnsZero()
        {
            // Arrange
            var vector1 = Array.Empty<double>();
            var vector2 = Array.Empty<double>();

            // Act
            var result = CosineSimilarity.Between(vector1, vector2);

            // Assert
            Assert.Equal(0.0, result, 5);
        }

        [Fact]
        public void Between_NormalizedVectors_ReturnsDotProduct()
        {
            // Arrange
            var vector1 = new double[] { 1.0 / System.Math.Sqrt(2), 1.0 / System.Math.Sqrt(2), 0.0 };
            var vector2 = new double[] { 1.0 / System.Math.Sqrt(2), 0.0, 1.0 / System.Math.Sqrt(2) };

            // Act
            var result = CosineSimilarity.Between(vector1, vector2);

            // Assert
            // Dot product: 0.5 + 0 + 0 = 0.5
            Assert.Equal(0.5, result, 5);
        }
    }
}

