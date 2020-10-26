using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPN_Lab1
{
    public class RPNClass
    {
        string inputString;
        string outputString = "";
        Stack<string> tempStack = new Stack<string>();
        Stack<double> doubleStack = new Stack<double>();

        readonly string[] possibleWords = new[] { "sin", "cos", "tg", "exp", "abs", "sqrt", "pow", "neg", "+", "-", "/", "*", "%", "e", "pi", "x" };
        readonly string[] binOperations = new[] { "+", "-", "/", "*", "%", "^" };
        readonly string[] prefFunctions = new[] { "sin", "cos", "tg", "exp", "abs", "sqrt", "pow", "neg" };
        readonly string[] consts = new[] { "e", "pi", "x" };
        readonly string[] leftBinOperations = new[] { "-", "/", "%", "^" };

        readonly Dictionary<string, int> precedanceTable = new Dictionary<string, int>()
        {
            { "+", 6 }, { "-", 6 }, { "/", 8 }, { "*", 8 }, { "%", 8 }, { "^", 9 }
        };

        readonly Dictionary<string, double> constsTable = new Dictionary<string, double>()
        {
            { "e", Math.E }, { "pi", Math.PI }
        };

        public double Evaluate(string rpnString, double x)
        {
            double res = 0;
            var tokens = rpnString.Split(' ');
            for(int i = 0; i < tokens.Length; ++i)
            {
                if (IsNumber(tokens[i][0]))
                {
                    doubleStack.Push(double.Parse(tokens[i]));
                }
                else if (consts.Contains(tokens[i]))
                {
                    if(tokens[i] == "x")
                        doubleStack.Push(x);
                    else
                        doubleStack.Push(constsTable[tokens[i]]);
                }
                else if (binOperations.Contains(tokens[i]))
                {
                    if (tokens[i] == "+")
                    {
                        RequireArgs(2, "+");
                        res = doubleStack.Pop() + doubleStack.Pop();
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "*")
                    {
                        RequireArgs(2, "*");
                        res = doubleStack.Pop() * doubleStack.Pop();
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "-")
                    {
                        RequireArgs(2, "-");
                        double arg2 = doubleStack.Pop();
                        double arg1 = doubleStack.Pop();
                        res = arg1 - arg2;
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "/")
                    {
                        RequireArgs(2, "/");
                        double arg2 = doubleStack.Pop();
                        double arg1 = doubleStack.Pop();
                        res = arg1 / arg2;
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "%")
                    {
                        RequireArgs(2, "%");
                        double arg2 = doubleStack.Pop();
                        double arg1 = doubleStack.Pop();
                        res = arg1 % arg2;
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "^")
                    {
                        RequireArgs(2, "^");
                        double arg2 = doubleStack.Pop();
                        double arg1 = doubleStack.Pop();
                        res = Math.Pow(arg1, arg2);
                        doubleStack.Push(res);
                    }
                }
                else if (prefFunctions.Contains(tokens[i]))
                {
                    if (tokens[i] == "sin")
                    {
                        RequireArgs(1, "sin");
                        double arg1 = doubleStack.Pop();
                        res = Math.Sin(arg1);
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "cos")
                    {
                        RequireArgs(1, "cos");
                        double arg1 = doubleStack.Pop();
                        res = Math.Cos(arg1);
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "tg")
                    {
                        RequireArgs(1, "tg");
                        double arg1 = doubleStack.Pop();
                        res = Math.Tan(arg1);
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "exp")
                    {
                        RequireArgs(1, "exp");
                        double arg1 = doubleStack.Pop();
                        res = Math.Exp(arg1);
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "abs")
                    {
                        RequireArgs(1, "abs");
                        double arg1 = doubleStack.Pop();
                        res = Math.Abs(arg1);
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "sqrt")
                    {
                        RequireArgs(1, "sqrt");
                        double arg1 = doubleStack.Pop();
                        res = Math.Sqrt(arg1);
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "pow")
                    {
                        RequireArgs(2, "pow");
                        double arg2 = doubleStack.Pop();
                        double arg1 = doubleStack.Pop();
                        res = Math.Pow(arg1, arg2);
                        doubleStack.Push(res);
                    }
                    else if (tokens[i] == "neg")
                    {
                        RequireArgs(1, "neg");
                        double arg1 = doubleStack.Pop();
                        res = -1 * arg1;
                        doubleStack.Push(res);
                    }
                }
                else
                {
                    throw new Exception($"Невідомий токен {tokens[i]}.");
                }
            }



            return doubleStack.Pop();
        }

        public bool GetRPNString(string inpStr, out string result)
        {
            SetUp(inpStr);

            for(int i = 0; i < inputString.Length; ++i)
            {
                char pos = inputString[i];
                if (pos == ' ')
                    continue;
                else if (IsNumber(pos))
                {
                    while (IsNumber(pos) || pos == ',')
                    {
                        outputString += pos;
                        ++i;
                        if (i < inputString.Length)
                            pos = inputString[i];
                        else
                            break;
                    }
                    --i;

                    outputString += " ";    //to separate tokens
                }
                else if (IsLetter(pos))
                {
                    string word = "";
                    while (IsLetter(pos))
                    {
                        word += pos;
                        ++i;
                        if (i < inputString.Length)
                            pos = inputString[i];
                        else
                            break;
                    }
                    --i;

                    //find out if it's const, func or unknown word
                    if (consts.Contains(word))  //const
                    {
                        outputString += $"{word} ";
                    }
                    else if (prefFunctions.Contains(word))  //pref func
                    {
                        tempStack.Push(word);
                    }
                    else
                    {
                        result = $"Невідоме слово \"{word}\".";
                        return false;
                    }
                }
                else if(pos == '(')
                {
                    tempStack.Push($"{pos}");
                }
                else if(pos == ')')
                {
                    if (tempStack.Count == 0)
                    {
                        result = $"Невистачає дужки \"(\" для позиції {i + 1}.";
                        return false;
                    }
                    while (tempStack.Peek() != "(")
                    {
                        outputString += $"{tempStack.Pop()} ";
                        if (tempStack.Count == 0)
                        {
                            result = $"Невистачає дужки \"(\" для позиції {i + 1}.";
                            return false;
                        }
                    }
                    tempStack.Pop();
                }
                else if (binOperations.Contains($"{pos}"))
                {
                    if (tempStack.Count != 0)   //check for prefix func of priority
                    {
                        while( prefFunctions.Contains(tempStack.Peek()) 
                            || (binOperations.Contains(tempStack.Peek()) && precedanceTable[tempStack.Peek()] > precedanceTable[$"{pos}"])
                            || ( leftBinOperations.Contains(tempStack.Peek()) && precedanceTable[tempStack.Peek()] == precedanceTable[$"{pos}"]) )
                        {
                            outputString += $"{tempStack.Pop()} ";
                            if (tempStack.Count == 0)
                            {
                                break;
                            }
                        }
                    }

                    tempStack.Push($"{pos}");   //and put bin operation in stack
                }
            }

            while (tempStack.Count != 0)
            {
                string tempWord = $"{tempStack.Pop()} ";
                if(tempWord=="( ")
                {
                    var count = tempStack.ToArray().Where(x => x == "(").Count();
                    result = $"Невистачає дужки \")\" {count+1} раз(ів).";
                    return false;
                }
                outputString += tempWord;
            }
            
            result = outputString.TrimEnd();
            return true;
        }

        private bool IsNumber(char c)
        {
            return char.IsDigit(c);
        }

        private bool IsLetter(char c)
        {
            return char.IsLetter(c);
        }

        private bool IsConst(string str)
        {
            return consts.Contains(str);
        }

        private void SetUp(string inpStr)
        {
            inputString = inpStr.ToLower();
            outputString = "";
            tempStack = new Stack<string>();
            doubleStack = new Stack<double>();
        }

        private void RequireArgs(int count, string funcName)
        {
            if (doubleStack.Count < count)
                throw new Exception($"Операція \"{funcName}\" потребує ще один аргумент.");
        }

    }
}
