using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator
{

    #region 简易四则运算
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
            needReset = false;
            if (OperatorClicked)
                return total;
            OperatorClicked = true;
            if (total == null)
            {
                lastOperator = strOperator;
                total = sOperatorNum;
                sOperatorNum = "";
                //tbDisplayScreen.Text = "0";
                needClear = true;
                return total;
            }
            if (lastOperator == "" || sOperatorNum == "")
                return total;
            if (sOperatorNum == "0" && lastOperator == "/")
            {
                needClear = true;
                needReset = true;
                total = null;
                return "除数不能为零";
            }
            calculate = operationFactory.createOperate(lastOperator);

            calculate.NumberA = Convert.ToDouble(total);
            calculate.NumberB = Convert.ToDouble(sOperatorNum);
            total = calculate.GetResult().ToString();

            if (isEqualSign)
            {
                needReset = true;
            }

            //tbDisplayScreen.Text = total;
            lastOperator = strOperator;
            canBackSpace = false;
            needClear = true;
            return total;
        }

        public override string Click_Num_Button(string num)
        {
            if (needClear)
            {
                //tbDisplayScreen.Text = "";
                sOperatorNum = "0";
                needClear = false;
            }
            if (needReset)
            {
                total = null;
                needReset = false;
            }
            //tbDisplayScreen.Text += num;
            sOperatorNum += num;
            if (sOperatorNum.IndexOf(".") == -1)
            {

                sOperatorNum = Convert.ToDouble(sOperatorNum).ToString();
                //tbDisplayScreen.Text = sOperatorNum;//去除头部多余的0
            }
            canBackSpace = true;
            OperatorClicked = false;
            return sOperatorNum;
        }
    }
    #endregion

}
