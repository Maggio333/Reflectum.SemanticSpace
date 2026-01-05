using Reflectum.SemanticSpace.Infrastructure.Tokenization;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Infrastructure
{
    public class DefaultTextTokenizerTests
    {
        [Fact]
        public void Tokenize_SimpleText_ReturnsTokens()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer();
            var input = "miłość przyjaźń bliskość";

            // Act
            var tokens = tokenizer.Tokenize(input).ToList();

            // Assert
            Assert.Equal(3, tokens.Count);
            Assert.Contains("miłość", tokens);
            Assert.Contains("przyjaźń", tokens);
            Assert.Contains("bliskość", tokens);
        }

        [Fact]
        public void Tokenize_WithPunctuation_SplitsCorrectly()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer();
            var input = "miłość, przyjaźń. bliskość!";

            // Act
            var tokens = tokenizer.Tokenize(input).ToList();

            // Assert
            Assert.Equal(3, tokens.Count);
            Assert.Contains("miłość", tokens);
            Assert.Contains("przyjaźń", tokens);
            Assert.Contains("bliskość", tokens);
        }

        [Fact]
        public void Tokenize_TrimsWhitespace()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer();
            var input = "  miłość   przyjaźń  ";

            // Act
            var tokens = tokenizer.Tokenize(input).ToList();

            // Assert
            Assert.Equal(2, tokens.Count);
            Assert.All(tokens, t => Assert.DoesNotContain(" ", t));
        }

        [Fact]
        public void Tokenize_LowercasesTokens()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer();
            var input = "MIŁOŚĆ Przyjaźń Bliskość";

            // Act
            var tokens = tokenizer.Tokenize(input).ToList();

            // Assert
            Assert.All(tokens, t => Assert.Equal(t, t.ToLowerInvariant()));
        }

        [Fact]
        public void Tokenize_EmptyString_ReturnsEmpty()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer();

            // Act
            var tokens = tokenizer.Tokenize("").ToList();

            // Assert
            Assert.Empty(tokens);
        }

        [Fact]
        public void Tokenize_WhitespaceOnly_ReturnsEmpty()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer();

            // Act
            var tokens = tokenizer.Tokenize("   \t\n  ").ToList();

            // Assert
            Assert.Empty(tokens);
        }

        [Fact]
        public void Tokenize_WithCustomSeparators_UsesCustomSeparators()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer(new[] { '|', ';' });
            var input = "miłość|przyjaźń;bliskość";

            // Act
            var tokens = tokenizer.Tokenize(input).ToList();

            // Assert
            Assert.Equal(3, tokens.Count);
        }

        [Fact]
        public void Tokenize_WithNullInput_ReturnsEmpty()
        {
            // Arrange
            var tokenizer = new DefaultTextTokenizer();

            // Act
            var tokens = tokenizer.Tokenize(null!).ToList();

            // Assert
            Assert.Empty(tokens);
        }
    }
}

