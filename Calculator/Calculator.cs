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
            Binding();
        }


        /// <summary>
        /// 按键绑定事件
        /// </summary>
        private void Binding()
        {
            bt1.Click += new EventHandler(Number_button_Click);
            bt2.Click += new EventHandler(Number_button_Click);
            bt3.Click += new EventHandler(Number_button_Click);
            bt4.Click += new EventHandler(Number_button_Click);
            bt5.Click += new EventHandler(Number_button_Click);
            bt6.Click += new EventHandler(Number_button_Click);
            bt7.Click += new EventHandler(Number_button_Click);
            bt8.Click += new EventHandler(Number_button_Click);
            bt9.Click += new EventHandler(Number_button_Click);
            bt0.Click += new EventHandler(Number_button_Click);

            btPlus.Click += new EventHandler(Operator_button_Click);
            btSub.Click += new EventHandler(Operator_button_Click);
            btMultiply.Click += new EventHandler(Operator_button_Click);
            btDivide.Click += new EventHandler(Operator_button_Click);

            btLeftParenthesis.Click += new EventHandler(Operator_button_Click);
            btRightParenthesis.Click += new EventHandler(Operator_button_Click);


        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            //CalculationType = EqualFactory.createOperate(1);
            CalculationType = EqualFactory.createOperate(1);
        }

        public virtual string Equal(string strOperator = "+", bool isEqualSign = false) { return ""; }
        public virtual string Click_Num_Button(string num) { return null; }

        public virtual string SetPoint() { return null; }

        public int strStrCount(string S,string s) {return System.Text.RegularExpressions.Regex.Matches(S, "["+s+"]").Count; }
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
        #region 加减乘除类
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
                else
                {
                    MessageBox.Show("除数不能为零");
                    return 0;
                }
                return Result;
            }
        }
        #endregion

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

        #region 变量
        static string total = null;
        static string sOperatorNum;                                  //用来储存输入的每个数
        string sOperator = "";                                 //用来辨别进行何种运算,与显示框tbDisplayScreen.Text相同
        string lastOperator = "";
        bool needClear = false;     //"CE"键清除信息
        bool needReset = false;     //按"="后输入为数字时重新开始计算
        static bool canBackSpace = false;  //只有在用户输入数字的情况下能使用退格键,计算出来的数字无法使用
        bool OperatorClicked = false; //多次按加减乘除无效
        static string RightInput = "";
        HashSet<string> OperatorHs = new HashSet<string> {"+", "-", "*", "/", "(", ")" };
        HashSet<string> nphs = new HashSet<string> { "+", "-", "*", "/" };
        HashSet<string> norightphs = new HashSet<string> { "+", "-", "*", "/", "(" };
        HashSet<string> noleftphs = new HashSet<string> { "+", "-", "*", "/", ")" };
        Calculate calculate;
        #endregion

        private void btEqual_Click(object sender, EventArgs e)
        {
            OperatorClicked = false;
            tbDisplayScreen.Text = CalculationType.Equal(sOperator, true);
        }

        Calculator CalculationType;
        private void CalculatorEqual(string Operator = "+", bool isEqualSign = false)
        {
            tbDisplayScreen.Text = CalculationType.Equal(sOperator);
        }

        private void Click_Num(string num)
        {
            tbDisplayScreen.Text = CalculationType.Click_Num_Button(num);
        }

        //数字按键
        private void Number_button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Click_Num(b.Text);
        }

        //运算符按键
        private void Operator_button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            sOperator = b.Text;
            CalculatorEqual(sOperator);
        }

        //清除键
        private void btClear_Click(object sender, EventArgs e)
        {
            total = null;
            sOperatorNum = "";
            tbDisplayScreen.Text = "";
            needClear = true;
        }

        //退格键
        private void btBackSpace_Click(object sender, EventArgs e)
        {
            if (sOperatorNum.Length > 0 && canBackSpace)
            {
                sOperatorNum = sOperatorNum.Substring(0, sOperatorNum.Length - 1);
                if (RightInput.Length>0)
                    RightInput = RightInput.Substring(0, RightInput.Length - 1);
                tbDisplayScreen.Text = sOperatorNum;
                var lastPointIndex = tbDisplayScreen.Text.LastIndexOf('.');
                var lastOperatorIndex = 0;
                foreach(var s in OperatorHs)
                {
                    lastOperatorIndex = Math.Max(lastOperatorIndex, tbDisplayScreen.Text.LastIndexOf(s));
                }
                if (lastOperatorIndex == tbDisplayScreen.Text.Length-1)
                {
                    RightInput = "";
                }
                else
                {
                    RightInput = sOperatorNum.Substring(lastOperatorIndex, sOperatorNum.Length - lastOperatorIndex);
                }

                //var lastOperatorIndex = tbDisplayScreen.Text.IndexOf(, lastPointIndex);
                //RightInput
            }
            
        }
        //小数点
        private void btPoint_Click(object sender, EventArgs e)
        {
            tbDisplayScreen.Text = CalculationType.SetPoint();
        }
        #region 简易四则运算
        //简易四则运算
        public class SimpleOperator : Calculator
        {
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
                    tbDisplayScreen.Text = "除数不能为零";

                    needClear = true;
                    needReset = true;
                    total = null;
                    return tbDisplayScreen.Text;
                }
                calculate = operationFactory.createOperate(lastOperator);

                calculate.NumberA = Convert.ToDouble(total);
                calculate.NumberB = Convert.ToDouble(sOperatorNum);
                total = calculate.GetResult().ToString();

                if (isEqualSign)
                {
                    needReset = true;
                }

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
        #endregion

        #region 表达式运算
        //表达式运算
        public class OperationExpression : Calculator
        {
            
            public override string Equal(string strOperator = "+", bool isEqualSign = false)
            {
                if (!isEqualSign)
                {
                    if (canAddOperator(strOperator))
                    {
                        sOperatorNum += strOperator;
                        RightInput = "";
                    }
                }
                else
                {
                    if (!isExpressionHolds())
                        return sOperatorNum;
                    var pts = new Parenthesis();//之后版本中用factory代替
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
                RightInput += num;
                canBackSpace = true;
                sOperatorNum += num;
                return sOperatorNum;
            }

            /// <summary>
            /// 运算符是否能够继续添加
            /// </summary>
            /// <param name="strOperator"></param>
            /// <returns></returns>
            private bool canAddOperator(string strOperator)
            {
                var len = sOperatorNum.Length;
                if (len == 0)//第一个运算符只能添加左括号
                {
                    if (strOperator == "(")
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
                    var leftParenthesis = strStrCount(sOperatorNum, "(");
                    var rightParenthesis = strStrCount(sOperatorNum, ")");
                    if (leftParenthesis <= rightParenthesis)//左括号个数小于右括号个数不再添加右括号
                        return false;
                    return true;
                }
                else if (strOperator == "(" && norightphs.Contains(c.ToString()))//左括号可以添加在非数字和非右括号的右边
                    return true;
                else if ((!norightphs.Contains(c.ToString())) && nphs.Contains(strOperator)) //加减乘除符号可以添加在数字及右括号右边
                    return true;
                return false;
            }

            /// <summary>
            /// 表达式成立
            /// </summary>
            /// <returns></returns>
            private bool isExpressionHolds()
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
                if(!noleftphs.Contains(c.ToString())) //表达式最后为右括号或数字
                {
                    var leftParenthesis = strStrCount(sOperatorNum, "(");
                    var rightParenthesis = strStrCount(sOperatorNum, ")");
                    if (leftParenthesis > rightParenthesis)//若右括号数量少于左括号数量,自动补全
                    {
                        var dif = leftParenthesis - rightParenthesis;
                        for (int i = 0; i < dif; i++)
                            sOperatorNum += ')';
                    }
                    return true;
                }else
                    return false;

            }
        }
        #endregion
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
                        cal = new OperationExpression();//算术表达式
                        break;
                }
                return cal;
            }
        }

        private void rbSimpleOperator_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSimpleOperator.Checked)
            {
                btLeftParenthesis.Visible = false;
                btRightParenthesis.Visible = false;
                CalculationType = EqualFactory.createOperate(1);
                btClear_Click(sender, e);
            }
        }

        private void rbOperationExpression_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOperationExpression.Checked)
            {
                btLeftParenthesis.Visible = true;
                btRightParenthesis.Visible = true;
                CalculationType = EqualFactory.createOperate(2);
                btClear_Click(sender, e);
            }
        }
    }
}

