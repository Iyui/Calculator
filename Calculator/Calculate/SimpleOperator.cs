﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{
    #region 标准型计算
    public class SimpleOperator : Calculator
    {
        Calculate calculate;
        public override string SetPoint()
        {
            if (sOperatorNum.IndexOf(".") == -1)
            {
                if (sOperatorNum.Length == 0)
                {
                    sOperatorNum += "0.";
                }
                else
                {
                    sOperatorNum += ".";
                }
            }
            return sOperatorNum;
        }

        public override string Equal(string strOperator = "+", bool isEqualSign = false)
        {
            Reset = false;
            if (OperatorClicked && !isEqualSign)
                return total;
            OperatorClicked = true;
            if (total == null)
            {
                lastOperator = strOperator;
                total = sOperatorNum;
                sOperatorNum = null;
                //tbDisplayScreen.Text = "0";
                Button_Clear = true;
                return total;
            }
            if (lastOperator == "" || sOperatorNum == "")
                return total;
            if (sOperatorNum == "0" && lastOperator == "/")
            {
                Button_Clear = true;
                Reset = true;
                total = null;
                return "除数不能为零";
            }

            calculate = operationFactory.createOperate(lastOperator);

            calculate.NumberA = Convert.ToDouble(total);
            calculate.NumberB = Convert.ToDouble(sOperatorNum);
            total = calculate.GetResult().ToString();

            if (isEqualSign)
            {
                Reset = true;
            }

            lastOperator = strOperator;
            Button_BackSpace = false;
            Button_Clear = true;
            return total;
        }

        public override string Click_Num_Button(string num)
        {
            if (Button_Clear)
            {
                sOperatorNum = null;
                Button_Clear = false;
            }
            if (Reset)
            {
                sOperator = "+";
                total = null;
                Reset = false;
            }
            if (sOperatorNum == null||sOperatorNum.Length < 32 )
                sOperatorNum += num;
            if (sOperatorNum.IndexOf(".") == -1)
            {
                while (!(sOperatorNum == "0") && sOperatorNum.IndexOf("0") == 0)
                    sOperatorNum = sOperatorNum.Substring(1);//去除头部多余的0
            }
            Button_BackSpace = true;
            OperatorClicked = false;
            return sOperatorNum;
        }

        public override string Inverse(string num)
        {
            Button_Clear = true;
            sOperatorNum  = (0 - Convert.ToDouble(num)).ToString();
            return sOperatorNum;
        }
    }
    #endregion
}
