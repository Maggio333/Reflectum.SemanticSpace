using Moq;
using Reflectum.SemanticSpace.Application.Services;
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;
using Reflectum.SemanticSpace.Infrastructure.Indexing;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Application
{
    public class SemanticSearchServiceTests
    {
        private readonly Mock<ISemanticSpace> _mockSpace;
        private readonly Mock<ISimilarityCalculator> _mockCalculator;
        private readonly Mock<IIndexBuilder> _mockIndexBuilder;

        public SemanticSearchServiceTests()
        {
            _mockSpace = new Mock<ISemanticSpace>();
            _mockCalculator = new Mock<ISimilarityCalculator>();
            _mockIndexBuilder = new Mock<IIndexBuilder>();
        }

        [Fact]
        public void Constructor_WithNullDependencies_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new SemanticSearchService(null!, _mockCalculator.Object, _mockIndexBuilder.Object));

            Assert.Throws<ArgumentNullException>(() =>
                new SemanticSearchService(_mockSpace.Object, null!, _mockIndexBuilder.Object));

            Assert.Throws<ArgumentNullException>(() =>
                new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, null!));
        }

        [Fact]
        public void TopK_WordNotFound_ReturnsEmptyList()
        {
            // Arrange
            _mockSpace.Setup(s => s.Find(It.IsAny<string>())).Returns((MeaningVector?)null);
            var service = new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, _mockIndexBuilder.Object);

            // Act
            var result = service.TopK("nonexistent", k: 5);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void TopK_WithValidWord_ReturnsSimilarWords()
        {
            // Arrange
            var targetWord = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var similarWord1 = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 1.1, 2.1, 3.1 } };
            var similarWord2 = new MeaningVector { Word = "bliskość", Vector = new List<double> { 1.2, 2.2, 3.2 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(targetWord);
            _mockSpace.Setup(s => s.GetAll()).Returns(new[] { targetWord, similarWord1, similarWord2 });

            _mockCalculator.Setup(c => c.Calculate(targetWord, similarWord1)).Returns(0.9);
            _mockCalculator.Setup(c => c.Calculate(targetWord, similarWord2)).Returns(0.8);
            _mockCalculator.Setup(c => c.Calculate(targetWord, targetWord)).Returns(1.0);

            var service = new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, _mockIndexBuilder.Object);

            // Act
            var result = service.TopK("miłość", k: 2, threshold: 0.7);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, w => w.Word == "przyjaźń");
            Assert.Contains(result, w => w.Word == "bliskość");
        }

        [Fact]
        public void TopK_ExcludesTargetWord()
        {
            // Arrange
            var targetWord = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var similarWord = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 1.1, 2.1, 3.1 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(targetWord);
            _mockSpace.Setup(s => s.GetAll()).Returns(new[] { targetWord, similarWord });

            _mockCalculator.Setup(c => c.Calculate(targetWord, similarWord)).Returns(0.9);

            var service = new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, _mockIndexBuilder.Object);

            // Act
            var result = service.TopK("miłość", k: 5);

            // Assert
            Assert.DoesNotContain(result, w => w.Word == "miłość");
        }

        [Fact]
        public void TopK_RespectsThreshold()
        {
            // Arrange
            var targetWord = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var highSimilarity = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 1.1, 2.1, 3.1 } };
            var lowSimilarity = new MeaningVector { Word = "samochód", Vector = new List<double> { 10.0, 20.0, 30.0 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(targetWord);
            _mockSpace.Setup(s => s.GetAll()).Returns(new[] { targetWord, highSimilarity, lowSimilarity });

            _mockCalculator.Setup(c => c.Calculate(targetWord, highSimilarity)).Returns(0.9);
            _mockCalculator.Setup(c => c.Calculate(targetWord, lowSimilarity)).Returns(0.5);

            var service = new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, _mockIndexBuilder.Object);

            // Act
            var result = service.TopK("miłość", k: 5, threshold: 0.7);

            // Assert
            Assert.Single(result);
            Assert.Contains(result, w => w.Word == "przyjaźń");
            Assert.DoesNotContain(result, w => w.Word == "samochód");
        }

        [Fact]
        public void FindClosest_WordNotFound_ReturnsNull()
        {
            // Arrange
            _mockSpace.Setup(s => s.Find(It.IsAny<string>())).Returns((MeaningVector?)null);
            var service = new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, _mockIndexBuilder.Object);

            // Act
            var result = service.FindClosest("nonexistent");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void FindClosest_ReturnsMostSimilarWord()
        {
            // Arrange
            var targetWord = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var word1 = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 1.1, 2.1, 3.1 } };
            var word2 = new MeaningVector { Word = "bliskość", Vector = new List<double> { 1.2, 2.2, 3.2 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(targetWord);
            _mockSpace.Setup(s => s.GetAll()).Returns(new[] { targetWord, word1, word2 });

            _mockCalculator.Setup(c => c.Calculate(targetWord, word1)).Returns(0.9);
            _mockCalculator.Setup(c => c.Calculate(targetWord, word2)).Returns(0.8);

            var service = new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, _mockIndexBuilder.Object);

            // Act
            var result = service.FindClosest("miłość");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("przyjaźń", result!.Word);
        }

        [Fact]
        public void Project_WithPrefixIndex_UsesIndex()
        {
            // Arrange
            var targetWord = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var similarWord = new MeaningVector { Word = "miasto", Vector = new List<double> { 1.1, 2.1, 3.1 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(targetWord);

            var prefixBuilder = new PrefixIndexBuilder();
            prefixBuilder.BuildPrefixIndex(new[] { targetWord, similarWord }, prefixLength: 2);

            _mockCalculator.Setup(c => c.Calculate(targetWord, similarWord)).Returns(0.9);

            var service = new SemanticSearchService(_mockSpace.Object, _mockCalculator.Object, prefixBuilder);

            // Act
            var result = service.Project("miłość", topK: 1, threshold: 0.7);

            // Assert
            Assert.Single(result);
        }
    }
}

