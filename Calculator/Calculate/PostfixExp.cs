using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                if (isNum || c =='.')//分割数字与运算符
                {
                    sElement += c;
                    if(index !=length-1)
                        continue;
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
    }


    /// <summary>
    /// 无括号,只有先乘除后加减两个优先级
    /// </summary>
    public class NoParenthesis:PostfixExp
    {
        /// <summary>
        /// 中序表达式list转换为后序表达式list
        /// </summary>
        /// <returns></returns>
        public List<string> PostfixExpList()
        {
            var explist= ExpressionList();
            int count = explist.Count();
            Stack<string> stack = new Stack<string>();
            List<string> PostfixExpList = new List<string>();
            for (int index = 0;index<count;index++)
            {
                var element = explist[index];
                bool isNum = double.TryParse(element, out double r);
                if(isNum)
                {
                    PostfixExpList.Add(element);
                    if (stack.Any() && (stack.Peek() == "*" || stack.Peek() == "/"))//因该类中不含括号,所以乘除为最高优先级
                        PostfixExpList.Add(stack.Pop());
                }else
                {
                    stack.Push(element);
                }
            }
            while(stack.Any())
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
        
        public double CaculateElement(string Operator, double numA, double numB)
        {
            Calculator.Calculate calculate;
            calculate = Calculator.operationFactory.createOperate(Operator);
            calculate.NumberA = numA;
            calculate.NumberB = numB;
            return calculate.GetResult();
        }
    }
}
