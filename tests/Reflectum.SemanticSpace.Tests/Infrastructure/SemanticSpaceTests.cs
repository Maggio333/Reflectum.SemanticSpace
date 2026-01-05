using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Infrastructure.Indexing;
using SemanticSpaceImpl = Reflectum.SemanticSpace.Infrastructure.SemanticSpace.SemanticSpace;
using Moq;
using Reflectum.SemanticSpace.Domain.Interfaces;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Infrastructure
{
    public class SemanticSpaceTests
    {
        private readonly Mock<IIndexBuilder> _mockIndexBuilder;

        public SemanticSpaceTests()
        {
            _mockIndexBuilder = new Mock<IIndexBuilder>();
        }

        [Fact]
        public void Constructor_WithNullIndexBuilder_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new SemanticSpaceImpl(null!));
        }

        [Fact]
        public void Load_LoadsVectors()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } },
                new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 4.0, 5.0, 6.0 } }
            };

            // Act
            space.Load(vectors);

            // Assert
            var allVectors = space.GetAll().ToList();
            Assert.Equal(2, allVectors.Count);
        }

        [Fact]
        public void Load_NormalizesVectors()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vector = new MeaningVector
            {
                Word = "test",
                Vector = new List<double> { 3.0, 4.0, 0.0 }
            };
            var vectors = new List<MeaningVector> { vector };

            // Act
            space.Load(vectors);

            // Assert
            var loaded = space.Find("test");
            Assert.NotNull(loaded);
            var norm = System.Math.Sqrt(
                loaded!.Vector[0] * loaded.Vector[0] +
                loaded.Vector[1] * loaded.Vector[1] +
                loaded.Vector[2] * loaded.Vector[2]);
            Assert.Equal(1.0, norm, 5);
        }

        [Fact]
        public void Load_HandlesDuplicateWords_KeepsFirst()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "test", Vector = new List<double> { 1.0, 2.0, 3.0 } },
                new MeaningVector { Word = "test", Vector = new List<double> { 4.0, 5.0, 6.0 } }
            };

            // Act
            space.Load(vectors);

            // Assert
            var found = space.Find("test");
            Assert.NotNull(found);
            // Vector is normalized, so we check if it's the first vector (normalized)
            var expectedNorm = System.Math.Sqrt(1.0 * 1.0 + 2.0 * 2.0 + 3.0 * 3.0);
            Assert.Equal(1.0 / expectedNorm, found!.Vector[0], 5);
        }

        [Fact]
        public void Find_ExistingWord_ReturnsVector()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } }
            };
            space.Load(vectors);

            // Act
            var result = space.Find("miłość");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("miłość", result!.Word);
        }

        [Fact]
        public void Find_NonExistingWord_ReturnsNull()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } }
            };
            space.Load(vectors);

            // Act
            var result = space.Find("nieistniejące");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Find_CaseInsensitive()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "Miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } }
            };
            space.Load(vectors);

            // Act
            var result1 = space.Find("miłość");
            var result2 = space.Find("MIŁOŚĆ");

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
        }

        [Fact]
        public void GetAll_ReturnsAllVectors()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } },
                new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 4.0, 5.0, 6.0 } },
                new MeaningVector { Word = "bliskość", Vector = new List<double> { 7.0, 8.0, 9.0 } }
            };
            space.Load(vectors);

            // Act
            var all = space.GetAll().ToList();

            // Assert
            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void BuildPrefixIndex_CallsIndexBuilder()
        {
            // Arrange
            var space = new SemanticSpaceImpl(_mockIndexBuilder.Object);
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } }
            };
            space.Load(vectors);

            // Act
            space.BuildPrefixIndex(2);

            // Assert
            _mockIndexBuilder.Verify(b => b.BuildPrefixIndex(It.IsAny<IEnumerable<MeaningVector>>(), 2), Times.Once);
        }
    }
}

