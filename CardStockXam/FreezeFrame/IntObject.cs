using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardEngine;
using FreezeFrame;

namespace CardStockXam.FreezeFrame
{
    public abstract class IntObject
    {
        public abstract int Calc();
    }

    public class VarIntObject : IntObject //var
    {
        string name;
        RecycleVariables variables;

        public VarIntObject(string n, RecycleVariables vars)
        {
            name = n;
            variables = vars;
        }

        public override int Calc()
        {
            return (int)variables.Get(name);
        }
    }

    public class BinaryOpObject : IntObject //mult, subtract, mod, add, divide
    {
        string op;
        IntObject obj1;
        IntObject obj2;

        public BinaryOpObject(string o, IntObject ob1, IntObject ob2) 
        { 
            op = o;
            obj1 = ob1;
            obj2 = ob2;
        }

        public override int Calc()
        {
            switch (op)
            {
                case "*":
                    return obj1.Calc() * obj2.Calc();
                case "-":
                    return obj1.Calc() - obj2.Calc();
                case "%":
                    return obj1.Calc() % obj2.Calc();
                case "+":
                    return obj1.Calc() + obj2.Calc();
                case "//":
                    return obj1.Calc() / obj2.Calc();
                case "^":
                    return (int)Math.Pow(obj1.Calc(), obj2.Calc());
                default:
                    return -1;
            }
        }
    }

    public class UnaryOpObject : IntObject //tri, fib
    {
        string op;
        IntObject obj1;

        public UnaryOpObject(String o, IntObject ob1)
        {
            op = o;
            obj1 = ob1;
        }

        public override int Calc()
        {
            switch (op)
            {
                case "exp":
                    return (obj1.Calc() * (obj1.Calc() + 1)) / 2;
                case "fib":
                    return Convert.ToInt32(((Math.Pow((1 + Math.Sqrt(5)) / 2, obj1.Calc())) - (Math.Pow((1 - Math.Sqrt(5)) / 2, obj1.Calc()))) / Math.Sqrt(5));
                default:
                    return -1;
            }
        }
    }

    public class LiteralIntObject : IntObject //INTNUM+
    {
        int num;
        
        public LiteralIntObject(int n)
        {
            num = n;
        }

        public override int Calc()
        {
            return num;
        }
    }

    // singular random and double random

    public class ScoreObject : IntObject //score
    {
        Card card;
        PointMap points;

        public ScoreObject(Card c, PointMap pts)
        {
            card = c;
            points = pts;
        }

        public override int Calc()
        {
            return points.GetScore(card);
        }
    }

}
