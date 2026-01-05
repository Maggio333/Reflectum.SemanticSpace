using Reflectum.SemanticSpace.Domain.Entities;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Domain
{
    public class MeaningVectorTests
    {
        [Fact]
        public void Constructor_InitializesProperties()
        {
            // Arrange & Act
            var vector = new MeaningVector
            {
                Word = "test",
                Definition = "test definition",
                Vector = new List<double> { 1.0, 2.0, 3.0 }
            };

            // Assert
            Assert.Equal("test", vector.Word);
            Assert.Equal("test definition", vector.Definition);
            Assert.Equal(3, vector.Vector.Count);
            Assert.NotNull(vector.Relations);
            Assert.NotNull(vector.Tags);
        }

        [Fact]
        public void Normalize_NormalizesVector()
        {
            // Arrange
            var vector = new MeaningVector
            {
                Word = "test",
                Vector = new List<double> { 3.0, 4.0, 0.0 }
            };

            // Act
            vector.Normalize();

            // Assert
            var expectedNorm = System.Math.Sqrt(3.0 * 3.0 + 4.0 * 4.0);
            Assert.Equal(3.0 / expectedNorm, vector.Vector[0], 5);
            Assert.Equal(4.0 / expectedNorm, vector.Vector[1], 5);
            Assert.Equal(0.0, vector.Vector[2], 5);
        }

        [Fact]
        public void Normalize_ZeroVector_DoesNotCrash()
        {
            // Arrange
            var vector = new MeaningVector
            {
                Word = "test",
                Vector = new List<double> { 0.0, 0.0, 0.0 }
            };

            // Act & Assert (should not throw)
            vector.Normalize();
            Assert.All(vector.Vector, v => Assert.Equal(0.0, v));
        }

        [Fact]
        public void Normalize_EmptyVector_DoesNotCrash()
        {
            // Arrange
            var vector = new MeaningVector
            {
                Word = "test",
                Vector = new List<double>()
            };

            // Act & Assert (should not throw)
            vector.Normalize();
            Assert.Empty(vector.Vector);
        }

        [Fact]
        public void ToArray_ReturnsCorrectArray()
        {
            // Arrange
            var vector = new MeaningVector
            {
                Word = "test",
                Vector = new List<double> { 1.0, 2.0, 3.0 }
            };

            // Act
            var array = vector.ToArray();

            // Assert
            Assert.Equal(3, array.Length);
            Assert.Equal(1.0, array[0]);
            Assert.Equal(2.0, array[1]);
            Assert.Equal(3.0, array[2]);
        }

        [Fact]
        public void ToArray_EmptyVector_ReturnsEmptyArray()
        {
            // Arrange
            var vector = new MeaningVector
            {
                Word = "test",
                Vector = new List<double>()
            };

            // Act
            var array = vector.ToArray();

            // Assert
            Assert.Empty(array);
        }
    }
}

