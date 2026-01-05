using Moq;
using Reflectum.SemanticSpace.Application.Services;
using Reflectum.SemanticSpace.Domain.Entities;
using Reflectum.SemanticSpace.Domain.Interfaces;
using Xunit;

namespace Reflectum.SemanticSpace.Tests.Application
{
    public class ResonanceCalculatorTests
    {
        private readonly Mock<ISemanticSpace> _mockSpace;
        private readonly Mock<ISimilarityCalculator> _mockCalculator;

        public ResonanceCalculatorTests()
        {
            _mockSpace = new Mock<ISemanticSpace>();
            _mockCalculator = new Mock<ISimilarityCalculator>();
        }

        [Fact]
        public void Constructor_WithNullDependencies_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
                new ResonanceCalculator(null!, _mockCalculator.Object));

            Assert.Throws<ArgumentNullException>(() =>
                new ResonanceCalculator(_mockSpace.Object, null!));
        }

        [Fact]
        public void CalculateResonance_EmptySetA_ReturnsZero()
        {
            // Arrange
            var calculator = new ResonanceCalculator(_mockSpace.Object, _mockCalculator.Object);
            var setA = Array.Empty<string>();
            var setB = new[] { "miłość", "przyjaźń" };

            // Act
            var result = calculator.CalculateResonance(setA, setB);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public void CalculateResonance_NoMatches_ReturnsZero()
        {
            // Arrange
            var setA = new[] { "miłość", "przyjaźń" };
            var setB = new[] { "samochód", "dom" };

            var vector1 = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var vector2 = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 4.0, 5.0, 6.0 } };
            var vector3 = new MeaningVector { Word = "samochód", Vector = new List<double> { 10.0, 20.0, 30.0 } };
            var vector4 = new MeaningVector { Word = "dom", Vector = new List<double> { 11.0, 21.0, 31.0 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(vector1);
            _mockSpace.Setup(s => s.Find("przyjaźń")).Returns(vector2);
            _mockSpace.Setup(s => s.Find("samochód")).Returns(vector3);
            _mockSpace.Setup(s => s.Find("dom")).Returns(vector4);

            _mockCalculator.Setup(c => c.Calculate(It.IsAny<MeaningVector>(), It.IsAny<MeaningVector>()))
                .Returns(0.5); // Below threshold

            var calculator = new ResonanceCalculator(_mockSpace.Object, _mockCalculator.Object);

            // Act
            var result = calculator.CalculateResonance(setA, setB, threshold: 0.75);

            // Assert
            Assert.Equal(0.0, result);
        }

        [Fact]
        public void CalculateResonance_AllMatch_ReturnsOne()
        {
            // Arrange
            var setA = new[] { "miłość", "przyjaźń" };
            var setB = new[] { "uczucie", "relacja" };

            var vector1 = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var vector2 = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 4.0, 5.0, 6.0 } };
            var vector3 = new MeaningVector { Word = "uczucie", Vector = new List<double> { 1.1, 2.1, 3.1 } };
            var vector4 = new MeaningVector { Word = "relacja", Vector = new List<double> { 4.1, 5.1, 6.1 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(vector1);
            _mockSpace.Setup(s => s.Find("przyjaźń")).Returns(vector2);
            _mockSpace.Setup(s => s.Find("uczucie")).Returns(vector3);
            _mockSpace.Setup(s => s.Find("relacja")).Returns(vector4);

            _mockCalculator.Setup(c => c.Calculate(vector1, vector3)).Returns(0.9);
            _mockCalculator.Setup(c => c.Calculate(vector2, vector4)).Returns(0.9);

            var calculator = new ResonanceCalculator(_mockSpace.Object, _mockCalculator.Object);

            // Act
            var result = calculator.CalculateResonance(setA, setB, threshold: 0.75);

            // Assert
            Assert.Equal(1.0, result);
        }

        [Fact]
        public void CalculateResonance_PartialMatch_ReturnsCorrectRatio()
        {
            // Arrange
            var setA = new[] { "miłość", "przyjaźń", "bliskość" };
            var setB = new[] { "uczucie", "samochód", "relacja" };

            var vectors = new Dictionary<string, MeaningVector>
            {
                ["miłość"] = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } },
                ["przyjaźń"] = new MeaningVector { Word = "przyjaźń", Vector = new List<double> { 4.0, 5.0, 6.0 } },
                ["bliskość"] = new MeaningVector { Word = "bliskość", Vector = new List<double> { 7.0, 8.0, 9.0 } },
                ["uczucie"] = new MeaningVector { Word = "uczucie", Vector = new List<double> { 1.1, 2.1, 3.1 } },
                ["samochód"] = new MeaningVector { Word = "samochód", Vector = new List<double> { 10.0, 20.0, 30.0 } },
                ["relacja"] = new MeaningVector { Word = "relacja", Vector = new List<double> { 4.1, 5.1, 6.1 } }
            };

            foreach (var kvp in vectors)
            {
                _mockSpace.Setup(s => s.Find(kvp.Key)).Returns(kvp.Value);
            }

            _mockCalculator.Setup(c => c.Calculate(vectors["miłość"], vectors["uczucie"])).Returns(0.9);
            _mockCalculator.Setup(c => c.Calculate(vectors["przyjaźń"], vectors["relacja"])).Returns(0.9);
            _mockCalculator.Setup(c => c.Calculate(vectors["bliskość"], It.IsAny<MeaningVector>())).Returns(0.5);

            var calculator = new ResonanceCalculator(_mockSpace.Object, _mockCalculator.Object);

            // Act
            var result = calculator.CalculateResonance(setA, setB, threshold: 0.75);

            // Assert
            // 2 out of 3 match = 2/3 ≈ 0.667
            Assert.InRange(result, 0.66, 0.67);
        }

        [Fact]
        public void CalculateResonance_WordNotFoundInSpace_SkipsIt()
        {
            // Arrange
            var setA = new[] { "miłość", "nonexistent" };
            var setB = new[] { "uczucie" };

            var vector1 = new MeaningVector { Word = "miłość", Vector = new List<double> { 1.0, 2.0, 3.0 } };
            var vector2 = new MeaningVector { Word = "uczucie", Vector = new List<double> { 1.1, 2.1, 3.1 } };

            _mockSpace.Setup(s => s.Find("miłość")).Returns(vector1);
            _mockSpace.Setup(s => s.Find("nonexistent")).Returns((MeaningVector?)null);
            _mockSpace.Setup(s => s.Find("uczucie")).Returns(vector2);

            _mockCalculator.Setup(c => c.Calculate(vector1, vector2)).Returns(0.9);

            var calculator = new ResonanceCalculator(_mockSpace.Object, _mockCalculator.Object);

            // Act
            var result = calculator.CalculateResonance(setA, setB, threshold: 0.75);

            // Assert
            // Only "miłość" is processed (nonexistent is skipped), and it matches = 1/2 = 0.5
            // Because total is count of setA (2), not count of processed words
            Assert.Equal(0.5, result);
        }
    }
}

