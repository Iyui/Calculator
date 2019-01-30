using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();

        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            CalculationType = EqualFactory.createOperate(1);
        }

        public virtual string Equal(string strOperator = "+", bool isEqualSign = false) { return ""; }
        public virtual string Click_Num_Button(string num) { return null; }

        public class Calculate
        {
            public double NumberA { get; set; } = 0;
            public double NumberB { get; set; } = 0;

            public virtual double GetResult()
            {
                double Result = 0;
                return Result;
            }
        }

        /// <summary>
        /// 加法
        /// </summary>
        public class CalculateAdd : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                Result = NumberA + NumberB;
                return Result;
            }
        }

        /// <summary>
        /// 减法
        /// </summary>
        public class CalculateSub : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                Result = NumberA - NumberB;
                return Result;
            }
        }


        /// <summary>
        /// 乘法
        /// </summary>
        public class CalculateMul : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                Result = NumberA * NumberB;
                return Result;
            }
        }

        /// <summary>
        /// 除法
        /// </summary>
        public class CalculateDiv : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                if (NumberB != 0)
                    Result = NumberA / NumberB;
                return Result;
            }
        }

        public class operationFactory               //处理运算符的类
        {
            public static Calculate createOperate(string operate)
            {
                Calculate cal = null;

                switch (operate)
                {
                    case "+":
                        cal = new CalculateAdd();          //实例化加法运算
                        break;
                    case "-":
                        cal = new CalculateSub();
                        break;
                    case "*":
                        cal = new CalculateMul();
                        break;
                    case "/":
                        cal = new CalculateDiv();
                        break;
                }
                return cal;
            }
        }

        static string total = null;
        static string sOperatorNum;                                  //用来储存输入的每个数
        string sOperator = "";                                 //用来辨别进行何种运算,与显示框tbDisplayScreen.Text相同
        string lastOperator = "";
        bool needClear = false;     //"CE"键清除信息
        bool needReset = false;     //按"="后输入为数字时重新开始计算
        static bool canBackSpace = false;  //只有在用户输入数字的情况下能使用退格键,计算出来的数字无法使用
        bool OperatorClicked = false;
        Calculate calculate;


        private void btEqual_Click(object sender, EventArgs e)
        {
            OperatorClicked = false;
            tbDisplayScreen.Text = CalculationType.Equal(sOperator, true);

        }

        Calculator CalculationType;
        private void CalculatorEqual(string Operator = "+", bool isEqualSign = false)
        {
            //CalculationType = EqualFactory.createOperate(1);
            tbDisplayScreen.Text = CalculationType.Equal(sOperator, true);

        }

        private void Click_Num(string num)
        {
            tbDisplayScreen.Text = CalculationType.Click_Num_Button(num);
        }

        private void bt1_Click(object sender, EventArgs e)
        {
            Click_Num("1");
        }

        private void bt2_Click(object sender, EventArgs e)
        {
            Click_Num("2");
        }

        private void bt3_Click(object sender, EventArgs e)
        {
            Click_Num("3");
        }

        private void bt4_Click(object sender, EventArgs e)
        {
            Click_Num("4");
        }

        private void bt5_Click(object sender, EventArgs e)
        {
            Click_Num("5");
        }

        private void bt6_Click(object sender, EventArgs e)
        {
            Click_Num("6");
        }

        private void bt7_Click(object sender, EventArgs e)
        {
            Click_Num("7");
        }

        private void bt8_Click(object sender, EventArgs e)
        {
            Click_Num("8");
        }

        private void bt9_Click(object sender, EventArgs e)
        {
            Click_Num("9");
        }

        private void bt0_Click(object sender, EventArgs e)
        {
            Click_Num("0");

        }
        private void btPlus_Click(object sender, EventArgs e)
        {
            sOperator = "+";
            CalculatorEqual(sOperator);
        }
        private void btMinus_Click(object sender, EventArgs e)
        {
            sOperator = "-";
            CalculatorEqual(sOperator);
        }

        private void btMultiply_Click(object sender, EventArgs e)
        {
            sOperator = "*";
            CalculatorEqual(sOperator);
        }

        private void btDivide_Click(object sender, EventArgs e)
        {
            sOperator = "/";
            CalculatorEqual(sOperator);
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            total = null;
            sOperatorNum = "";
            tbDisplayScreen.Text = "";
            needClear = true;
        }

        private void btBackSpace_Click(object sender, EventArgs e)
        {
            if (sOperatorNum.Length > 0 && canBackSpace)
            {
                sOperatorNum = sOperatorNum.Substring(0, sOperatorNum.Length - 1);
                tbDisplayScreen.Text = sOperatorNum;
            }
        }

        private void btPoint_Click(object sender, EventArgs e)
        {
            if (sOperatorNum.IndexOf(".") == -1)
            {
                if (sOperatorNum.Length == 0)
                {
                    sOperatorNum += "0.";
                    tbDisplayScreen.Text = sOperatorNum;
                }
                else
                {
                    sOperatorNum += ".";
                    tbDisplayScreen.Text = sOperatorNum;
                }
            }
        }


        //四则运算
        public class SimpleOperator : Calculator
        {
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
                    tbDisplayScreen.Text = "除数不能为零";

                    needClear = true;
                    needReset = true;
                    total = null;
                    return tbDisplayScreen.Text;
                }
                calculate = operationFactory.createOperate(lastOperator);
                calculate.NumberA = Convert.ToDouble(total);
                calculate.NumberB = Convert.ToDouble(sOperatorNum);
                if (isEqualSign)
                {
                    needReset = true;
                }
                total = calculate.GetResult().ToString();
                tbDisplayScreen.Text = total;
                lastOperator = strOperator;
                canBackSpace = false;
                needClear = true;
                return tbDisplayScreen.Text;
            }

            public override string Click_Num_Button(string num)
            {
                if (needClear)
                {
                    tbDisplayScreen.Text = "";
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
                    tbDisplayScreen.Text = sOperatorNum;//去除头部多余的0
                }
                canBackSpace = true;
                OperatorClicked = false;
                return sOperatorNum;
            }
        }

        //表达式运算
        public class OperationExpression : Calculator
        {
            public override string Equal(string strOperator = "+", bool isEqualSign = false)
            {
                return "未实现";
            }

            public override string Click_Num_Button(string num)
            {
                return "未实现";
            }
        }

        public class EqualFactory               //处理运算的类
        {
            public static Calculator createOperate(int equal)
            {
                Calculator cal = null;
                switch (equal)
                {
                    case 1:
                        cal = new SimpleOperator();          //实例化具体的类
                        break;
                    case 2:
                        cal = new OperationExpression();
                        break;
                }
                return cal;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSimpleOperator.Checked)
            {
                CalculationType = EqualFactory.createOperate(1);        //实例化具体的类
                btClear_Click(sender, e);
            }
        }

        private void rbOperationExpression_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOperationExpression.Checked)
            {
                CalculationType = EqualFactory.createOperate(2);        //实例化具体的类
                btClear_Click(sender, e);
            }
        }
    }
}

