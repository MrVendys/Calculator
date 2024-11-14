using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PokrocileOperaceTest
    {
        private readonly double delta = 0.00000001;


        [TestMethod]
        [DataRow(2, 3, 8)]
        [DataRow(5, 0, 1)]
        [DataRow(-2, 3, -8)]
        [DataRow(4, 0.5, 2)]
        [DataRow(3, -2, 0.1111)]
        public void Power(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new PowerStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow(9, 3)]
        [DataRow(0, 0)]
        [DataRow(2.25, 1.5)]
        [DataRow(0.25, 0.5)]
        [DataRow(10, 3.1623)]
        public void SquareRoot(double cislo1, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new SquareRootStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, ocekavanyVysledek);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 1)]
        [DataRow(7, 5040)]
        [DataRow(3, 6)]
        public void Factorial(double cislo1, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new FactorialStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 3, 1)]
        [DataRow(9, -4, 1)]
        [DataRow(-7, 3, -1)]
        [DataRow(15, 5, 0)]
        public void Modulo(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new ModuloStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }
    }
}