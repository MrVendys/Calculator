using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PokrocileStrategieTest
    {
        //Odchylka u porovnávání desetinných čísel
        private const double delta = 0.00000001;

        [TestMethod]
        [DataRow(2, 3, 8)]
        [DataRow(5, 0, 1)]
        [DataRow(-2, 3, -8)]
        [DataRow(4, 0.5, 2)]
        [DataRow(3, -2, 0.11111111)]
        public void PowerStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            PowerStrategy op = new PowerStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow(9, 3)]
        [DataRow(0, 0)]
        [DataRow(2.25, 1.5)]
        [DataRow(10, 3.16227766)]
        public void SquareRootStrategyTest(double cislo1, double ocekavanyVysledek)
        {
            SquareRootStrategy op = new SquareRootStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, ocekavanyVysledek);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek, delta);
        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(7, 5040)]
        public void FactorialStrategyTest(double cislo1, double ocekavanyVysledek)
        {
            FactorialStrategy op = new FactorialStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 3, 1)]
        [DataRow(9, -4, 1)]
        [DataRow(-7, 3, -1)]
        [DataRow(15, 5, 0)]
        public void ModuloStrategyTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            ModuloStrategy op = new ModuloStrategy();

            double skutecnyVysledek = op.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }
    }
}