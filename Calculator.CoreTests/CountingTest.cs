using Calculator.Core;
using Calculator.Core.Exceptions;

namespace Calculator.CoreTests
{
    [TestClass]
    public class CountingTest
    {
        private Counting _counting;
        private readonly double delta = 0.00000001;
        public CountingTest()
        {
            _counting = new Counting();
        }

        [TestMethod]
        [DataRow("1+2",3)]
        [DataRow("3*(4/2)+5!",126)]
        [DataRow("-5-4*5",-25)]
        [DataRow("√(2^2)",2)]
        [DataRow("0",0)]
        [DataRow("(3+5*2-(√9)+7^2/4!*(2^3))+((8/4)+√16)*5-(3!)", 50.33333333)]
        public void VypocitejTest(string priklad, double ocekavanyVysledek)
        {
            _counting.TryPridejPriklad(priklad);
            _counting.Vypocitej();

            double skutecnyVysledek = double.Parse(_counting.Priklad);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow("1 +2","")]
        [DataRow("","")]
        public void InvalidVypocitejTest(string priklad, string ocekavanyVysledek)
        {
            _counting.TryPridejPriklad(priklad);
            _counting.Vypocitej();

            string skutecnyVysledek = _counting.Priklad;

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow("(")]
        [DataRow(")")]
        [DataRow("()")]
        [DataRow("1+")]
        [DataRow("2++")]
        public void InputValidationExceptionVypocitejTest(string priklad)
        {
            _counting.TryPridejPriklad(priklad);

            Assert.ThrowsException<InputValidationException>(() => _counting.Vypocitej());
        }
    }
}
