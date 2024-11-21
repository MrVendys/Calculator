﻿using Calculator.Core;

namespace Calculator.CoreTests
{
    [TestClass]
    public class PrikladValidatorTest
    {
        [TestMethod]
        [DataRow('1', "")]
        [DataRow('(', "1+")]
        [DataRow(')', "1+(2+3")]
        [DataRow('+', "2")]
        //Test přidávání symbolu do rozpracovaného příkladu
        public void ValidatePridejSymbol(char symbol, string priklad)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            counting.TryPridejPriklad(priklad);

            bool valid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsTrue(valid);
        }

        [TestMethod]
        [DataRow('a')]
        [DataRow('{')]
        //Test nepovolenych znaku
        public void ValidatePridejSymbol_NepovolenySymbol_False(char symbol)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);

            bool valid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsFalse(valid);
        }

        [TestMethod]
        [DataRow('+', "")]
        [DataRow('2', "3!")]
        [DataRow(')', "(")]
        [DataRow(')', "1+")]
        [DataRow('(', "1+(2+3")]
        [DataRow('√', "2")]
        [DataRow('-', "2+")]
        //Test na přidávání symbolů, které nejdou přidat do rozpracovaného příkladu
        public void ValidatePridejSymbol_LogickyNespravnySymbol_False(char symbol, string priklad)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);
            counting.TryPridejPriklad(priklad);

            bool valid = prikladValidator.ValidatePridejSymbol(symbol);

            Assert.IsFalse(valid);
        }

        [TestMethod]
        [DataRow("3/(5+2)")]
        [DataRow("4!*5+√4")]
        public void ValidatePriklad(string priklad)
        {
            PrikladValidator prikladValidator = GetValidator(out Counting counting);

            bool valid = prikladValidator.ValidatePridejPriklad(priklad);

            Assert.IsTrue(valid);
        }

        private PrikladValidator GetValidator(out Counting counting)
        {
            counting = new Counting();
            return new PrikladValidator(counting);
        }
    }
}
