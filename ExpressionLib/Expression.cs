using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionLib
{
    public partial class Expression
    {
        enum Priority { LeftBracket, Add, Subtract = Add, Multiply, Divide = Multiply, Power, Function }
        enum Associativity { Left, Right }

        #region Tokens

        /// <summary>
        /// Abstract Token base class which must be passed the Expression Stack which Execute references
        /// A Token represents all elements parsed in the Expression string including numbers, operators, brackets and functions
        /// </summary>
        abstract class Token
        {
            protected Stack<double> stack;

            abstract public void Execute();

            protected Token(Stack<double> stack)
            {
                this.stack = stack;
            }
        }

        /// <summary>
        /// Abstract Operator class representing all operators and functions parsed in the Expression string
        /// </summary>
        abstract class Operator : Token
        {
            public Priority Priority { get; protected set; }
            public Associativity Associativity { get; protected set; }

            // Bracket does not need to pass stack because it doesn't use it in Execute
            protected Operator(Priority priority, Associativity associativity, Stack<double> stack = null) : base(stack)
            {
                Priority = priority;
                Associativity = associativity;
            }
        }

        /// <summary>
        /// Abstract class representing binary operators parsed in the Expression string
        /// </summary>
        abstract class BinaryOperator : Operator
        {
            protected BinaryOperator(Priority priority, Associativity associativity, Stack<double> stack) : base(priority, associativity, stack) { }
        }

        /// <summary>
        /// Abstract Function class representing function operations parsed in the Expression string
        /// </summary>
        abstract class Function : Operator
        {
            // UnaryPlus does not pass the stack
            protected Function(Priority priority, Associativity associativity, Stack<double> stack = null) : base(priority, associativity, stack) { }
        }

        /// <summary>
        /// Concrete Number class representing numbers parsed in the Expression string
        /// </summary>
        class Number : Token
        {
            public double Value { get; private set; }

            public Number(double value, Stack<double> stack) : base(stack)
            {
                Value = value;
            }

            public override void Execute()
            {
                stack.Push(Value);
            }
        }

        /// <summary>
        /// Concrete Variable class reprenting variables parsed in the Expression string
        /// </summary>
        class Variable : Token
        {
            //public string Name { get; private set; }
            public double Value { get; set; }

            public Variable(double val, Stack<double> stack) : base(stack)
            {
                Value = val;
                this.stack = stack;
            }

            public override void Execute()
            {
                stack.Push(Value);
            }
        }

        class Add : BinaryOperator
        {
            public Add(Stack<double> stack) : base(Priority.Add, Associativity.Left, stack) { }

            public override void Execute()
            {
                stack.Push(stack.Pop() + stack.Pop());
            }
        }

        class Subtract : BinaryOperator
        {
            public Subtract(Stack<double> stack) : base(Priority.Subtract, Associativity.Left, stack) { }

            public override void Execute()
            {
                stack.Push(-stack.Pop() + stack.Pop());
            }
        }

        class Multiply : BinaryOperator
        {
            public Multiply(Stack<double> stack) : base(Priority.Multiply, Associativity.Left, stack) { }

            public override void Execute()
            {
                stack.Push(stack.Pop() * stack.Pop());
            }
        }

        class Divide : BinaryOperator
        {
            public Divide(Stack<double> stack) : base(Priority.Divide, Associativity.Left, stack) { }

            public override void Execute()
            {
                double b = stack.Pop();
                stack.Push(stack.Pop() / b);
            }
        }

        class Power : BinaryOperator
        {
            public Power(Stack<double> stack) : base(Priority.Power, Associativity.Right, stack) { }

            public override void Execute()
            {
                double b = stack.Pop();
                stack.Push(Math.Pow(stack.Pop(), b));
            }
        }

        class LeftBracket : Operator
        {
            public LeftBracket() : base(Priority.LeftBracket, Associativity.Left) { }

            public override void Execute()
            {
            }
        }

        class RightBracket : Token
        {
            public RightBracket() : base(null) { }

            public override void Execute()
            {
            }
        }

        class UnaryMinus : Function
        {
            public UnaryMinus(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(-stack.Pop());
            }
        }

        class UnaryPlus : Function
        {
            public UnaryPlus() : base(Priority.Function, Associativity.Right) { }

            public override void Execute()
            {
            }
        }

        class Factorial : Function
        {
            public Factorial(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                double d = stack.Pop();
                int n = (int)d;
                if (d != n || n < 0)
                    stack.Push(double.PositiveInfinity);
                else
                {
                    double result = 1;
                    for (int i = 2; i <= n && !double.IsInfinity(result); i++)
                    {
                        result = result * i;
                    }
                    stack.Push(result);
                }
            }
        }

        class Sqrt : Function
        {
            public Sqrt(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Sqrt(stack.Pop()));
            }
        }

        class Sin : Function
        {
            public Sin(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Sin(stack.Pop()));
            }
        }

        class Cos : Function
        {
            public Cos(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Cos(stack.Pop()));
            }
        }

        class Tan : Function
        {
            public Tan(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Tan(stack.Pop()));
            }
        }

        class Asin : Function
        {
            public Asin(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Asin(stack.Pop()));
            }
        }

        class Acos : Function
        {
            public Acos(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Acos(stack.Pop()));
            }
        }

        class Atan : Function
        {
            public Atan(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Atan(stack.Pop()));
            }
        }

        class Log : Function
        {
            public Log(Stack<double> stack) : base(Priority.Function, Associativity.Right, stack) { }

            public override void Execute()
            {
                stack.Push(Math.Log(stack.Pop()));
            }
        }

        #endregion

        const char leftBracketChar = '(';
        const char rightBracketChar = ')';
        const char plusChar = '+';
        const char minusChar = '-';
        const char multiplyChar = '*';
        const char divideChar = '/';
        const char exponentiateChar = '^';
        const char unaryPlusChar = '+';
        const char unaryMinusChar = '-';
        const char factorialChar = '!';
        static readonly string allOperatorChars = string.Concat(plusChar + minusChar, multiplyChar, divideChar, exponentiateChar);
        public string ExpressionString { get; private set; }
        private int p;                                              // Current position into the string for reading Tokens
        Dictionary<string, Variable> variables;
        Stack<double> workStack;                                    // Stack workspace used when evaluating the RP list (Reverse Polish)
        Stack<Operator> operatorStack;                              // Stack to store the operators during the shunting yard algorithm
        List<Token> tokens;                                         // Final list of Tokens in reverse Polish after parsing the expression
        Dictionary<string, Function> functions;
        Dictionary<char, BinaryOperator> binaryOperators;
        Add opAdd;
        Subtract opSubtract;
        Multiply opMultiply;
        Divide opDivide;
        Power opPower;
        UnaryMinus opUnaryMinus;
        UnaryPlus opUnaryPlus;
        Factorial opFactorial;
        Sqrt opSqrt;
        Sin opSin;
        Cos opCos;
        Tan opTan;
        Asin opAsin;
        Acos opAcos;
        Atan opAtan;
        Log opLog;
        LeftBracket opLeftBracket;
        RightBracket opRightBracket;

        public Expression(string expression)
        {
            variables = new Dictionary<string, Variable>();
            workStack = new Stack<double>();
            operatorStack = new Stack<Operator>();
            tokens = new List<Token>();
            CreateOperators();
            let("pi", Math.PI);
            let("e", Math.E);
            let("inf", double.PositiveInfinity);
            let("NaN", double.NaN);
            let("Epsilon", double.Epsilon);
            let("MinValue", double.MinValue);
            let("MaxValue", double.MaxValue);
            ExpressionString = expression;
            try
            {
                parse();
            }
            catch (Exception)
            {
                tokens.Clear();
                tokens.Add(new Number(double.NaN, workStack));
                throw;
            }
        }

        void CreateOperators()
        {
            // Only one of each operation Token needs to be created
            opAdd = new Add(workStack);
            opSubtract = new Subtract(workStack);
            opMultiply = new Multiply(workStack);
            opDivide = new Divide(workStack);
            opPower = new Power(workStack);
            opLeftBracket = new LeftBracket();
            opRightBracket = new RightBracket();
            opUnaryMinus = new UnaryMinus(workStack);
            opUnaryPlus = new UnaryPlus();
            opFactorial = new Factorial(workStack);
            opSqrt = new Sqrt(workStack);
            opSin = new Sin(workStack);
            opCos = new Cos(workStack);
            opTan = new Tan(workStack);
            opLog = new Log(workStack);
            opAsin = new Asin(workStack);
            opAcos = new Acos(workStack);
            opAtan = new Atan(workStack);
            functions = new Dictionary<string, Function>
            {
                {"sqrt", opSqrt },
                {"sin", opSin },
                {"cos", opCos },
                {"tan", opTan },
                {"log", opLog },
                {"asin", opAsin },
                {"acos", opAcos },
                {"atan", opAtan }
            };
            binaryOperators = new Dictionary<char, BinaryOperator>
            {
                {plusChar, opAdd },
                {minusChar, opSubtract },
                {multiplyChar, opMultiply },
                {divideChar, opDivide },
                {exponentiateChar, opPower }
            };
        }

        // Used to set up predefined variables and called from the parser to create new variables with a default Value
        private void let(string name, double value = double.NaN)
        {
            if (!variables.ContainsKey(name))
            {
                variables.Add(name, new Variable(value, workStack));
            }
        }

        /// <summary>
        /// Set the Value of a Variable.
        /// If it doesn't exist do nothing.
        /// </summary>
        public void Let(string name, double value)
        {
            if (variables.ContainsKey(name))
                variables[name].Value = value;
        }

        /// <summary>
        /// Gets the Value of an existing variable
        /// </summary>
        public double Get(string name)
        {
            if (variables.ContainsKey(name))
                return variables[name].Value;
            else
                throw new InvalidOperationException($"No such variable '{name}'");
        }

        /// <summary>
        /// Evaluate the Expression after it has been parsed
        /// </summary>
        /// <returns>The value of the Expression</returns>
        public double Evaluate()
        {
            // If parse has done its job correctly there will be no Exceptions and the stack will finish with the evaluation on top which will be popped and returned
            // Division by zero will return an Infinity
            // Other errors return NaN
            for (int i = 0; i < tokens.Count; i++)
            {
                tokens[i].Execute();
            }
            return workStack.Pop();
        }

        private void parse()
        {
            Token token;
            Operator cmdOp;
            bool expectBinOp = false;
            p = 0;
            ExpressionString.SkipSpaces(ref p);
            while ((token = (expectBinOp ? ReadBinOpToken() : ReadUnaryOpToken())) != null)
            {
                if (token is Operator)
                {
                    if (token is LeftBracket)
                        operatorStack.Push(token as Operator);
                    else
                        PushOperator(token as Operator);
                    if (token is BinaryOperator)
                        expectBinOp = false;
                }
                else
                if (token is RightBracket)
                {
                    cmdOp = null;
                    while (operatorStack.Count > 0 && !((cmdOp = operatorStack.Pop()) is LeftBracket))
                        TokenAdd(cmdOp);
                    if (!(cmdOp is LeftBracket))
                        throw new ArgumentException($"Right bracket mismatch");
                }
                else
                {
                    tokens.Add(token);
                    expectBinOp = true;
                }
            }
            // Check that we end in the right parsing state
            if (!expectBinOp || p < ExpressionString.Length)
                throw new ArgumentException("Syntax error");
            // Pop remaining operators and check for unmatched left bracket
            while (operatorStack.Count > 0)
            {
                if ((cmdOp = operatorStack.Pop()) is LeftBracket)
                    throw new ArgumentException($"Missing close bracket(s)");
                else
                    TokenAdd(cmdOp);
            }
            // Check that there are 2 numbers or variables for every BinaryOperator and 1 number or Variable for every Function
            int expectedNumbers = 1 + tokens.Count((o) => o is BinaryOperator);
            int numbers = tokens.Count((o) => o is Number || o is Variable);
            if (numbers != expectedNumbers)
                throw new ArgumentException("Syntax error2");
        }

        private void PushOperator(Operator cmdOp)
        {
            if (operatorStack.Count == 0 || cmdOp.Priority > operatorStack.Peek().Priority)
                operatorStack.Push(cmdOp);
            else if (cmdOp.Priority == operatorStack.Peek().Priority && cmdOp.Associativity == Associativity.Right)
                operatorStack.Push(cmdOp);
            else
            {
                if (cmdOp == opFactorial)
                    TokenAdd(cmdOp);
                else
                {
                    while (operatorStack.Count > 0 && cmdOp.Priority <= operatorStack.Peek().Priority)
                        TokenAdd(operatorStack.Pop());
                    operatorStack.Push(cmdOp);
                }
            }
        }

        // Opimize out sequences that evaluate to constants during the parse
        private void TokenAdd(Operator cmdOp)
        {
            if (cmdOp is Function && tokens[tokens.Count - 1] is Number)
            {
                tokens[tokens.Count - 1].Execute();
                tokens.RemoveAt(tokens.Count - 1);
                cmdOp.Execute();
                tokens.Add(new Number(workStack.Pop(), workStack));
            }
            else
            if (cmdOp is BinaryOperator && tokens[tokens.Count - 2] is Number && tokens[tokens.Count - 1] is Number)
            {
                tokens[tokens.Count - 2].Execute();
                tokens[tokens.Count - 1].Execute();
                tokens.RemoveAt(tokens.Count - 1);
                tokens.RemoveAt(tokens.Count - 1);
                cmdOp.Execute();
                tokens.Add(new Number(workStack.Pop(), workStack));
            }
            else
                tokens.Add(cmdOp);
        }

        // Left bracket, number, variable, function, unary +/-
        private Token ReadUnaryOpToken()
        {
            ExpressionString.SkipSpaces(ref p);
            if (p >= ExpressionString.Length)
                return null;
            char c = ExpressionString[p];
            if (c == leftBracketChar)
            {
                p++;
                return opLeftBracket;
            }
            if (c.IsStartOfNumber())
            {
                return new Number(ExpressionString.ReadDouble(ref p), workStack);
            }
            if (c.IsASCIILetter())
            {
                string letters = ExpressionString.ReadLetters(ref p);
                if (functions.ContainsKey(letters))
                    return functions[letters];
                let(letters);
                return variables[letters];
            }
            if (c == unaryMinusChar)
            {
                p++;
                return opUnaryMinus;
            }
            if (c == unaryPlusChar)
            {
                p++;
                return opUnaryPlus;
            }
            return null;
        }

        // +, -, *, /, ^, !, right bracket
        private Token ReadBinOpToken()
        {
            ExpressionString.SkipSpaces(ref p);
            if (p >= ExpressionString.Length)
                return null;
            char c = ExpressionString[p];
            if (c.IsBinOperator())
                return binaryOperators[ExpressionString[p++]];
            if (c == rightBracketChar)
            {
                p++;
                return opRightBracket;
            }
            if (c == factorialChar)
            {
                p++;
                return opFactorial;
            }
            return null;
        }
    }
}
