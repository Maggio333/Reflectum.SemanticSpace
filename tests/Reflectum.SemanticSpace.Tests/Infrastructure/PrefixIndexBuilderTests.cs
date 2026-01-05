using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Infrastructure.Indexing;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Infrastructure
{
    public class PrefixIndexBuilderTests
    {
        [Fact]
        public void BuildPrefixIndex_CreatesIndex()
        {
            // Arrange
            var builder = new PrefixIndexBuilder();
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0 } },
                new MeaningVector { Word = "miasto", Vector = new List<double> { 2.0 } },
                new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 3.0 } }
            };

            // Act
            builder.BuildPrefixIndex(vectors, prefixLength: 2);

            // Assert
            Assert.True(builder.IsBuilt);
        }

        [Fact]
        public void GetVectorsByPrefix_ReturnsMatchingVectors()
        {
            // Arrange
            var builder = new PrefixIndexBuilder();
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0 } },
                new MeaningVector { Word = "miasto", Vector = new List<double> { 2.0 } },
                new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 3.0 } }
            };
            builder.BuildPrefixIndex(vectors, prefixLength: 2);

            // Act
            var result = builder.GetVectorsByPrefix("mi");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result!.Count);
            Assert.Contains(result, v => v.Word == "miłość");
            Assert.Contains(result, v => v.Word == "miasto");
        }

        [Fact]
        public void GetVectorsByPrefix_NonExistentPrefix_ReturnsNull()
        {
            // Arrange
            var builder = new PrefixIndexBuilder();
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0 } }
            };
            builder.BuildPrefixIndex(vectors, prefixLength: 2);

            // Act
            var result = builder.GetVectorsByPrefix("xx");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetVectorsByPrefix_CaseInsensitive()
        {
            // Arrange
            var builder = new PrefixIndexBuilder();
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "Miłość", Vector = new List<double> { 1.0 } }
            };
            builder.BuildPrefixIndex(vectors, prefixLength: 2);

            // Act
            var result1 = builder.GetVectorsByPrefix("mi");
            var result2 = builder.GetVectorsByPrefix("MI");

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
        }

        [Fact]
        public void IsBuilt_BeforeBuilding_ReturnsFalse()
        {
            // Arrange
            var builder = new PrefixIndexBuilder();

            // Assert
            Assert.False(builder.IsBuilt);
        }

        [Fact]
        public void IsBuilt_AfterBuilding_ReturnsTrue()
        {
            // Arrange
            var builder = new PrefixIndexBuilder();
            var vectors = new List<MeaningVector>
            {
                new MeaningVector { Word = "test", Vector = new List<double> { 1.0 } }
            };

            // Act
            builder.BuildPrefixIndex(vectors, prefixLength: 2);

            // Assert
            Assert.True(builder.IsBuilt);
        }
    }
}

