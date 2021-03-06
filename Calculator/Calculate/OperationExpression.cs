﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    #region 表达式运算
    public class OperationExpression : Calculator
    {
        HashSet<string> OperatorHs = new HashSet<string> { "+", "-", "*", "/", "(", ")" , "^", "√" };
        HashSet<string> nphs = new HashSet<string> { "+", "-", "*", "/", "^", "√" };//无括号
        HashSet<string> norightphs = new HashSet<string> { "+", "-", "*", "/", "(" ,"^", "√" };//无右括号
        HashSet<string> noleftphs = new HashSet<string> { "+", "-", "*", "/", ")" , "^", "√" };//无左括号
        public override string Equal(string strOperator = "+", bool isEqualSign = false)
        {
            if (!isEqualSign)
            {
                if (canAddOperator(strOperator))
                {
                    while (RightInput.Length>0&& (RightInput.IndexOf('.') != -1 
                        && sOperatorNum[sOperatorNum.Length - 1] == '0'
                        || RightInput[RightInput.Length - 1] == '.'))//删除多余的"."和"0"
                    {
                        sOperatorNum = sOperatorNum.Substring(0, sOperatorNum.Length - 1);
                        RightInput = RightInput.Substring(0, RightInput.Length - 1);
                    }
                    sOperatorNum += strOperator;
                    RightInput = "";
                }
            }
            else
            {
                if (!isExpressionHolds())
                    return sOperatorNum;
                var pts = new Parenthesis();//之后版本中用factory模式代替
                pts.Expression = sOperatorNum;
                sOperatorNum = pts.CalculatePostfixExp();
            }
            return sOperatorNum;
        }

        public override string SetPoint()
        {
            if (RightInput.IndexOf(".") == -1)
            {
                if (RightInput.Length != 0)
                {
                    RightInput += ".";
                    sOperatorNum += ".";
                }
            }
            return sOperatorNum;
        }

        public override string Click_Num_Button(string num)
        {
            Button_BackSpace = true;
            if (RightInput == "0")//运算符后第一个字符是"0"时,再输入数字时"0"删除
            {
                RightInput = num;
                sOperatorNum = sOperatorNum.Substring(0, sOperatorNum.Length - 1) + num;
            }
            else
            {
                RightInput += num;
                sOperatorNum += num;
            }
            return sOperatorNum;
        }

        /// <summary>
        /// 运算符是否能够继续添加
        /// </summary>
        /// <param name="strOperator"></param>
        /// <returns></returns>
        private bool canAddOperator(string strOperator)
        {
            int len = 0;
            if (null != sOperatorNum)
                len = sOperatorNum.Length;
            if (len == 0)//第一个运算符只能添加左括号
            {
                if (strOperator == "("|| strOperator == "√")
                    return true;
                return false;
            }
            var c = sOperatorNum[len - 1];
            if (c == '.')
            {
                sOperatorNum = sOperatorNum.Substring(0, sOperatorNum.Length - 1);
                RightInput = RightInput.Substring(0, RightInput.Length - 1);
            }
            if (strOperator == ")" && !norightphs.Contains(c.ToString())) //右括号只能添加在数字或右括号右边且个数不能大于左括号
            {
                var leftParenthesis = StrstrCount(sOperatorNum, "(");
                var rightParenthesis = StrstrCount(sOperatorNum, ")");
                if (leftParenthesis <= rightParenthesis)//左括号个数小于右括号个数不再添加右括号
                    return false;
                return true;
            }
            else if (strOperator == "(" && norightphs.Contains(c.ToString()))//左括号可以添加在非数字和非右括号的右边
                return true;
            else if ((!norightphs.Contains(c.ToString())) && nphs.Contains(strOperator)) //加减乘除符号可以添加在数字及右括号右边
                return true;
            else
                return true;
            //return false;
        }
        

        /// <summary>
        /// 表达式成立
        /// </summary>
        /// <returns></returns>
        public bool isExpressionHolds()
        {
            var len = sOperatorNum.Length;
            char c;
            if (len > 0)
            {
                c = sOperatorNum[len - 1];
            }
            else
            {
                return false;
            }
            if (!norightphs.Contains(c.ToString())) //表达式最后为右括号或数字
            {
                var leftParenthesis = StrstrCount(sOperatorNum, "(");
                var rightParenthesis = StrstrCount(sOperatorNum, ")");
                if (leftParenthesis > rightParenthesis)//若右括号数量少于左括号数量,自动补全
                {
                    var dif = leftParenthesis - rightParenthesis;
                    for (int i = 0; i < dif; i++)
                        sOperatorNum += ')';
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion
}
