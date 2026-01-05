using Reflectum.SemanticSpace.Infrastructure.Caching;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Infrastructure
{
    public class InMemoryVectorCacheTests
    {
        [Fact]
        public void TryGet_NonExistentKey_ReturnsFalse()
        {
            // Arrange
            var cache = new InMemoryVectorCache();

            // Act
            var result = cache.TryGet("nonexistent", out var vector);

            // Assert
            Assert.False(result);
            Assert.Null(vector);
        }

        [Fact]
        public void Set_AndTryGet_ReturnsCachedValue()
        {
            // Arrange
            var cache = new InMemoryVectorCache();
            var key = "test_key";
            var vector = new double[] { 1.0, 2.0, 3.0 };

            // Act
            cache.Set(key, vector);
            var result = cache.TryGet(key, out var retrieved);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrieved);
            Assert.Equal(vector, retrieved);
        }

        [Fact]
        public void Set_OverwritesExistingKey()
        {
            // Arrange
            var cache = new InMemoryVectorCache();
            var key = "test_key";
            var vector1 = new double[] { 1.0, 2.0, 3.0 };
            var vector2 = new double[] { 4.0, 5.0, 6.0 };

            // Act
            cache.Set(key, vector1);
            cache.Set(key, vector2);
            var result = cache.TryGet(key, out var retrieved);

            // Assert
            Assert.True(result);
            Assert.Equal(vector2, retrieved);
        }

        [Fact]
        public void Clear_RemovesAllEntries()
        {
            // Arrange
            var cache = new InMemoryVectorCache();
            cache.Set("key1", new double[] { 1.0 });
            cache.Set("key2", new double[] { 2.0 });

            // Act
            cache.Clear();

            // Assert
            Assert.False(cache.TryGet("key1", out _));
            Assert.False(cache.TryGet("key2", out _));
        }

        [Fact]
        public void Set_WithEmptyArray_Works()
        {
            // Arrange
            var cache = new InMemoryVectorCache();
            var key = "empty";
            var vector = Array.Empty<double>();

            // Act
            cache.Set(key, vector);
            var result = cache.TryGet(key, out var retrieved);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrieved);
            Assert.Empty(retrieved!);
        }
    }
}

