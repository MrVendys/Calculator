using Calculator.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    public class Counting
    { 
        //List dostupnych operaci
        List<(string Operation, IOperationStrategy Strategy)> allOperations = new List<(string, IOperationStrategy)>
            {
            ("+",new PlusStrategy()),
            ("-",new MinusStrategy()),
            ("*",new MultiplyStrategy()),
            ("/",new DivideStrategy()),
            ("^",new PowerStrategy()),
            ("√",new SquareRootStrategy()),
            ("!", new FactorialStrategy())
            };
        private bool error = false;
        public Counting() {
        }
        public float? Count(string example)
        {
            return Evaluate(Tokenize(example));
        }

        //Prepsani kazdeho znaku z textu do pole
        //Osetreni desetinych cisel
        private string[] Tokenize(string expression)
        {

            List<string> tokens = new List<string>();
            int index = 0;
            string number = "";
            while (index < expression.Length)
            {
                string token = expression.Substring(index, 1);
                if (int.TryParse(token, out _) || token == ",")
                {
                    number += token;
                }
                else if (token != " ")
                {
                    if (number != "")
                    {
                        tokens.Add(number);
                        number = "";
                    }
                    tokens.Add(token);
                }
                index++;
            }
            if(!number.Equals(""))
                tokens.Add(number);
            return tokens.ToArray();

        }
        //Funkce pro vypocitani celeho prikladu
        private float? Evaluate(string[] tokens)
        {
            //Funkce s rekurzi na naleznuti zavorek a jejich vypocitani
            while (tokens.Contains("("))
            {
                int openIndex = FindLast("(", tokens);
                int closeIndex = FindFirst(")", tokens, openIndex);
                //Odchytnuti chyby: Nekompletni zavorka
                if(error)
                {
                    return null;
                } 
                string[] brcTokens = new string[closeIndex - openIndex - 1];
                Array.Copy(tokens, openIndex + 1, brcTokens, 0, (closeIndex - openIndex - 1));
                var innerResult = Evaluate(brcTokens);
                //Odchytnuti chyby: Nekonzistentni priklad
                if (innerResult == null)
                    return null;
                tokens = tokens.Take(openIndex).Concat(new string[] { innerResult.ToString() }).Concat(tokens.Skip(closeIndex + 1)).ToArray();

            }

            //Naleznuti operatoru v prikladu
            List<(string Operation, IOperationStrategy Strategy)> currentOperations = FindOperators(tokens);

            //Serazeni operatoru podle pirority - Sestupne
            currentOperations = currentOperations.OrderByDescending(item => item.Strategy.Priority).ToList();

            
            //Cyklus pro vypocet vsech cisel v poli. Pocita se zleva, podle priority operaci
            int index = 0;
            while (tokens.Length > 1)
            {
                //Nehledam cisla, jenom operatory. Jednoducha podminka na zjisteni, jestli je charakter cislo
                if (!int.TryParse(tokens[index], out _))
                {
                    //Kontrola momentalniho operatoru s operatorem s nejvetsi prioritou
                    if (tokens[index] == currentOperations[0].Operation)
                    {
                        //Pouziti spravne strategie, ulozeni mezivysledku zpatky do pole pro nasledne pokracovani
                        IOperationStrategy currentOperation = currentOperations[0].Strategy;

                        //Kazda operace prijima pole string[] s operatorem a znakem pred nim a za nim.
                        //Jednotlive operatory si s tim poradi a vrati pole string[], kterym se nahradi cast zadavaciho pole "tokens[]"
                        //Duvodem je nejjednoznacny pocet a usporadani znaku pro vypocet urciteho operatoru: 
                        //1 + 2 = 3 znaky
                        //3! = 2 znaky.. jeden pred operatorem
                        //√4 = 2 znaky.. jeden za operatorem


                        string[] result = currentOperation.Count(tokens.Skip(index - 1).Take(3).ToArray());
                        //Odchytnuti chyby: Nekonzistentni priklad
                        if (result.Length == 0)
                            { return null; }
                        //Prepsani meziprikladu na mezivysledek
                        tokens = tokens.Take(index - 1).Concat(result).Concat(tokens.Skip(index + 2)).ToArray();

                        currentOperations.RemoveAt(0);
                        index = -1;
                    }
                }
                index++;
            }
            return float.Parse(tokens[0]);
        }
        //Funkce na naleznuti momentalne potrebnych operatoru
        private List<(string, IOperationStrategy)> FindOperators(string[] tokens)
        {
            var currentOperations = new List<(string Operation, IOperationStrategy Strategy)>();

            for (int i = 0; i < tokens.Length; i++)
            {
                for (int j = 0; j < allOperations.Count; j++)
                {
                    if (!int.TryParse(tokens[i], out _))
                    {
                        if (tokens[i].Equals(allOperations[j].Operation))
                        {
                            currentOperations.Add((allOperations[j].Operation, allOperations[j].Strategy));
                        }
                    }
                }
            }
            return currentOperations;
        }
        //Funkce na naleznuti prvniho a posledniho symbolu dane zavorky
        private int FindLast(string symbol, string[] tokens)
        {
            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                if (tokens[i] == symbol)
                {
                    return i;
                }
            }
            error = true;
            return 0;

        }
        private int FindFirst(string symbol, string[] tokens, int startIndex)
        {
            for (int i = startIndex; i < tokens.Length; i++)
            {
                if (tokens[i] == symbol)
                {
                    return i;
                }
            }
            error = true;
            return 0;
        }
       
    }
}
