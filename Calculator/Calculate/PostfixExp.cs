using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Calculator.Calculator;
namespace Calculator
{
    public class PostfixExp
    {
        /// <summary>
        /// 计算后序表达式
        /// </summary>
        /// <returns></returns>
        public virtual string CalculatePostfixExp() { return null; }

        /// <summary>
        /// 传入的表达式字符串
        /// </summary>
        public string Expression { get; set; } = "0";

        /// <summary>
        /// 非运算符
        /// </summary>
        private readonly string sNotOperator = ".!";

    /// <summary>
    /// 表达式字符串转换成列表形式
    /// </summary>
    /// <returns></returns>
    public List<string> ExpressionList()
        {
            List<string> expressionList = new List<string>();
            int length = Expression.Length;
            string sElement = "";
            for (int index = 0; index < length; index++)
            {
                char c = Expression[index];
                bool isNum = int.TryParse(c.ToString(), out int result);
                if (isNum || sNotOperator.Contains(c))//分割数字与运算符
                {
                    if (c == '!')
                    {
                        var calculate = operationFactory.createOperate("!");
                        calculate.NumberA = Convert.ToDouble(sElement);
                        calculate.NumberB = 0;
                        var total = calculate.GetResult();
                        sElement = total.ToString();
                    }
                    else
                    {
                        sElement += c;
                        if (index != length - 1)
                            continue;
                    }
                }
                else
                {
                    expressionList.Add(sElement);
                    sElement = c.ToString();
                }
                expressionList.Add(sElement);
                sElement = "";
            }
            return expressionList;
        }


        public double CaculateElement(string Operator, double numA, double numB)
        {
            Calculate calculate;
            calculate = operationFactory.createOperate(Operator);
            calculate.NumberA = numA;
            calculate.NumberB = numB;
            return calculate.GetResult();
        }
    }


    public class Parenthesis : PostfixExp
    {
        /// <summary>
        /// 中序表达式list转换为后序表达式list(有括号)
        /// </summary>
        /// <returns></returns>
        public List<string> PostfixExpList()
        {
            var explist = ExpressionList();
            int count = explist.Count();
            Stack<string> stack = new Stack<string>();
            List<string> PostfixExpList = new List<string>();
            for (int index = 0; index < count; index++)
            {
                var element = explist[index];

                bool isNum = double.TryParse(element, out double r);
                if (element == "") //为什么会出现空字符暂时未知
                    continue;
                if (isNum)
                {
                    PostfixExpList.Add(element);
                    if (stack.Any() && Precedencehs2.Contains(stack.Peek()))
                        PostfixExpList.Add(stack.Pop());
                }
                else if (!isNum && element != ")")
                {
                    stack.Push(element);
                }
                else if (element == ")")
                {
                    while (stack.Any() && stack.Peek() != "(")
                    {
                        PostfixExpList.Add(stack.Pop());
                    }
                    stack.Pop();//弹出"("
                }
            }
            while (stack.Any())
            {
                PostfixExpList.Add(stack.Pop());
            }
            return PostfixExpList;
        }

        /// <summary>
        /// 计算后序表达式
        /// </summary>
        /// <returns></returns>
        public override string CalculatePostfixExp()
        {
            Stack<double> stack = new Stack<double> { };
            var postfixExpList = PostfixExpList();
            foreach (var postfixExpElement in postfixExpList)
            {
                
                bool isNum = double.TryParse(postfixExpElement, out double r);
                if (isNum)
                    stack.Push(r);
                else
                {
                    var numB = stack.Pop();//栈顶的元素先弹出
                    var numA = stack.Pop();
                    stack.Push(CaculateElement(postfixExpElement, numA, numB));
                }
            }
            return stack.Pop().ToString();
        }
    }
}
