using Calculator.Core;
using Calculator.Core.Exceptions;
using Calculator.Core.Strategies;

namespace Calculator.CoreTests
{
    [TestClass]
    public class ZakladniStrategieTest
    {
        [TestMethod]
        [DataRow(5, 3, 8)]
        [DataRow(-2, 4, 2)]
        [DataRow(2.51233058, 3.20591493, 5.71824551)]
        public void OperaceScitaniTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperaceScitani sct = new OperaceScitani();

            double skutecnyVysledek = sct.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 5, 5)]
        [DataRow(7.5, 2.3, 5.2)]
        [DataRow(-6, -2, -4)]
        public void OperaceOdcitaniTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperaceOdcitani odt = new OperaceOdcitani();

            double skutecnyVysledek = odt.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(4, 3, 12)]
        [DataRow(0, 6, 0)]
        [DataRow(1.5, 2.5, 3.75)]
        [DataRow(-2, -4, 8)]
        public void OperaceNasobeniTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperaceNasobeni nas = new OperaceNasobeni();

            double skutecnyVysledek = nas.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        [DataRow(10, 2, 5)]
        [DataRow(0, 5, 0)]
        [DataRow(7.5, 2.5, 3)]
        [DataRow(-6, -2, 3)]
        public void OperaceDeleniTest(double cislo1, double cislo2, double ocekavanyVysledek)
        {
            OperaceDeleni del = new OperaceDeleni();

            double skutecnyVysledek = del.Vypocitej(cislo1, cislo2);

            Assert.AreEqual(ocekavanyVysledek, skutecnyVysledek);
        }

        [TestMethod]
        public void OperaceDeleni_Nula_ChybovyKodDeleniNulou()
        {
            double cislo = 5;
            double nula = 0;
            OperaceDeleni del = new OperaceDeleni();

            var exception = Assert.ThrowsException<InputValidationException>(() => del.Vypocitej(cislo, nula));

            Assert.AreEqual(exception.ChybovyKod, ChybovyKod.DeleniNulou);
        }
    }
}