using Moq;
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Domain
{
    public class SemanticVectorTests
    {
        private readonly Mock<IVectorCache> _mockCache;
        private readonly Mock<ITextTokenizer> _mockTokenizer;
        private readonly Mock<ISimilarityCalculator> _mockCalculator;

        public SemanticVectorTests()
        {
            _mockCache = new Mock<IVectorCache>();
            _mockTokenizer = new Mock<ITextTokenizer>();
            _mockCalculator = new Mock<ISimilarityCalculator>();
        }

        [Fact]
        public void Constructor_WithComponents_InitializesCorrectly()
        {
            // Arrange
            var components = new[] { "miłość", "przyjaźń", "bliskość" };

            // Act
            var vector = new SemanticVector(components, _mockCache.Object, _mockTokenizer.Object, _mockCalculator.Object);

            // Assert
            Assert.Equal(3, vector.Components.Count);
            Assert.Contains("miłość", vector.Components);
            Assert.Contains("przyjaźń", vector.Components);
            Assert.Contains("bliskość", vector.Components);
        }

        [Fact]
        public void Constructor_WithNullComponents_CreatesEmptyList()
        {
            // Act
            var vector = new SemanticVector(null, _mockCache.Object, _mockTokenizer.Object, _mockCalculator.Object);

            // Assert
            Assert.NotNull(vector.Components);
            Assert.Empty(vector.Components);
        }

        [Fact]
        public void Constructor_WithEmptyComponents_CreatesEmptyList()
        {
            // Arrange
            var components = Array.Empty<string>();

            // Act
            var vector = new SemanticVector(components, _mockCache.Object, _mockTokenizer.Object, _mockCalculator.Object);

            // Assert
            Assert.NotNull(vector.Components);
            Assert.Empty(vector.Components);
        }

        [Fact]
        public void Constructor_TrimsAndLowercasesComponents()
        {
            // Arrange
            var components = new[] { "  Miłość  ", "  PRZYJAŹŃ  ", "  Bliskość  " };

            // Act
            var vector = new SemanticVector(components, _mockCache.Object, _mockTokenizer.Object, _mockCalculator.Object);

            // Assert
            Assert.Contains("miłość", vector.Components);
            Assert.Contains("przyjaźń", vector.Components);
            Assert.Contains("bliskość", vector.Components);
        }

        [Fact]
        public void Constructor_RemovesDuplicates()
        {
            // Arrange
            var components = new[] { "miłość", "miłość", "przyjaźń", "przyjaźń" };

            // Act
            var vector = new SemanticVector(components, _mockCache.Object, _mockTokenizer.Object, _mockCalculator.Object);

            // Assert
            Assert.Equal(2, vector.Components.Count);
        }

        [Fact]
        public void Constructor_RemovesWhitespaceOnlyComponents()
        {
            // Arrange
            var components = new[] { "miłość", "   ", "\t", "przyjaźń", "" };

            // Act
            var vector = new SemanticVector(components, _mockCache.Object, _mockTokenizer.Object, _mockCalculator.Object);

            // Assert
            Assert.Equal(2, vector.Components.Count);
            Assert.Contains("miłość", vector.Components);
            Assert.Contains("przyjaźń", vector.Components);
        }

        [Fact]
        public void FromInput_TokenizesInput()
        {
            // Arrange
            var input = "miłość i przyjaźń";
            var expectedTokens = new[] { "miłość", "i", "przyjaźń" };
            _mockTokenizer.Setup(t => t.Tokenize(input)).Returns(expectedTokens);

            // Act
            var vector = SemanticVector.FromInput(input, _mockTokenizer.Object, _mockCache.Object, _mockCalculator.Object);

            // Assert
            _mockTokenizer.Verify(t => t.Tokenize(input), Times.Once);
            Assert.Equal(3, vector.Components.Count);
        }

        [Fact]
        public void FromInput_WithNullTokenizer_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                SemanticVector.FromInput("test", null!, _mockCache.Object, _mockCalculator.Object));
        }

        [Fact]
        public void FromInput_WithNullCache_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                SemanticVector.FromInput("test", _mockTokenizer.Object, null!, _mockCalculator.Object));
        }

        [Fact]
        public void FromInput_WithNullCalculator_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                SemanticVector.FromInput("test", _mockTokenizer.Object, _mockCache.Object, null!));
        }

        [Fact]
        public void ToMeanVector_WithCachedValue_ReturnsCached()
        {
            // Arrange
            var components = new[] { "miłość", "przyjaźń" };
            var vector = new SemanticVector(components, _mockCache.Object, _mockTokenizer.Object, _mockCalculator.Object);
            var cachedVector = new double[] { 0.1, 0.2, 0.3 };
            var cacheKey = "miłość_przyjaźń";

            _mockCache.Setup(c => c.TryGet(cacheKey, out cachedVector)).Returns(true);

            var mockSpace = new Mock<ISemanticSpace>();

            // Act
            var result = vector.ToMeanVector(mockSpace.Object);

            // Assert
            Assert.Equal(cachedVector, result);
            _mockCache.Verify(c => c.TryGet(cacheKey, out It.Ref<double[]?>.IsAny), Times.Once);
        }

        [Fact]
        public void SemanticSimilarity_CalculatesSimilarity()
        {
            // Arrange
            var vector1 = new SemanticVector(
                new[] { "miłość" },
                _mockCache.Object,
                _mockTokenizer.Object,
                _mockCalculator.Object);

            var vector2 = new SemanticVector(
                new[] { "przyjaźń" },
                _mockCache.Object,
                _mockTokenizer.Object,
                _mockCalculator.Object);

            var mockSpace = new Mock<ISemanticSpace>();
            var meaningVector1 = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 0.0, 0.0 } };
            var meaningVector2 = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 0.0, 1.0, 0.0 } };

            mockSpace.Setup(s => s.Find("miłość")).Returns(meaningVector1);
            mockSpace.Setup(s => s.Find("przyjaźń")).Returns(meaningVector2);

            _mockCache.Setup(c => c.TryGet(It.IsAny<string>(), out It.Ref<double[]?>.IsAny)).Returns(false);
            _mockCache.Setup(c => c.Set(It.IsAny<string>(), It.IsAny<double[]>()));

            // Act
            var similarity = vector1.SemanticSimilarity(vector2, mockSpace.Object);

            // Assert
            Assert.InRange(similarity, -1.0, 1.0); // Cosine similarity range
        }
    }
}

