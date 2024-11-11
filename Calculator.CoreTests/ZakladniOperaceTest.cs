using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class ZakladniOperaceTest
    {

        [TestMethod]
        [DataRow(5, 3, 8)]
        [DataRow(-2, 4, 2)]
        [DataRow(0, 7, 7)]
        [DataRow(2.5, 3.2, 5.7)]
        [DataRow(-1.5, -3.5, -5)]
        public void Scitani(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new PlusStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 5, 5)]
        [DataRow(3, -4, 7)]
        [DataRow(0, 8, -8)]
        [DataRow(7.5, 2.3, 5.2)]
        [DataRow(-6, -2, -4)]
        public void Odcitani(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new MinusStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(4, 3, 12)]
        [DataRow(-3, 5, -15)]
        [DataRow(0, 6, 0)]
        [DataRow(1.5, 2.5, 3.75)]
        [DataRow(-2, -4, 8)]
        public void Nasobeni(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new MultiplyStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 2, 5)]
        [DataRow(-9, 3, -3)]
        [DataRow(0, 5, 0)]
        [DataRow(7.5, 2.5, 3)]
        [DataRow(-6, -2, 3)]
        public void Deleni(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperationStrategyBase op = new DivideStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }
    }
}