using System;
using System.Collections.Generic;
using System.Collections;
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
        Calculate calculate;
        #region 变量
        static string _total = null;
        static bool _canBackSpace = false;  //只有在用户输入数字的情况下能使用退格键,计算出来的数字无法使用
        static string _RightInput = "";
        HashSet<string> OperatorHs = new HashSet<string> { "+", "-", "*", "/", "(", ")" };
        //private SimpleOperator so = new SimpleOperator();
        /// <summary>
        /// 该表中索引越大,运算符优先级越高
        /// </summary>
        public static HashSet<string> Precedencehs1 { get; } = new HashSet<string> { "+", "-" };
        public static HashSet<string> Precedencehs2 { get; } = new HashSet<string> { "*", "/", "^", "√" };
        public static HashSet<string> Precedencehs3 { get; } = new HashSet<string> { "!" };
        #endregion

        public static string total
        {
            get => _total;
            set => _total = value;
        }

        /// <summary>
        ///  用来储存输入的每个数
        /// </summary>
        public static string sOperatorNum { get; set; }

        /// <summary>
        /// 用来辨别进行何种运算,与显示框tbDisplayScreen.Text相同
        /// </summary>
        public static string sOperator { get; set; } = "";

        /// <summary>
        /// 记录简单运算中输入的上一个运算符
        /// 功能实现为 如输入1+2*3的"*"时计算出1+2
        /// </summary>
        public string lastOperator { get; set; } = "";

        /// <summary>
        /// "C"键清除信息
        /// </summary>
        public static bool Button_Clear { get; set; } = false;

        /// <summary>
        /// 按"="后输入为数字时重新开始计算
        /// </summary>
        public bool Reset { get; set; } = false;

        /// <summary>
        /// 只有在用户输入数字的情况下能使用退格键,计算出来的数字无法使用
        /// </summary>
        public bool Button_BackSpace
        {
            get => _canBackSpace;
            set => _canBackSpace = value;
        }

        /// <summary>
        /// 多次按加减乘除无效
        /// </summary>
        public bool OperatorClicked { get; set; } = false;

        
        /// <summary>
        /// 用来判断算术表达式中是否能继续添加数字或运算符
        /// </summary>
        public string RightInput
        {
            get => _RightInput;
            set => _RightInput = value;
        }

        public Calculator()
        {
            InitializeComponent();
            Binding();
            KeyDown += new KeyEventHandler(Calculator_KeyDown);//按键
            btEqual.LostFocus += new EventHandler(FocusEqual);
            Shown += new EventHandler(FocusEqual);
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
            btPower.Click += new EventHandler(Operator_button_Click);
            btSqrt.Click += new EventHandler(Operator_button_Click);
            btFac.Click += new EventHandler(Operator_button_Click);
            btCos.Click += new EventHandler(Operator_button_Click);
            btTan.Click += new EventHandler(Operator_button_Click);
            btSin.Click += new EventHandler(Operator_button_Click);
            btMod.Click += new EventHandler(Operator_button_Click);


            btLeftParenthesis.Click += new EventHandler(Operator_button_Click);
            btRightParenthesis.Click += new EventHandler(Operator_button_Click);



        }

        private void FocusEqual(object sender, EventArgs e)
        {
            btEqual.Focus();
        }

        private void Calculator_Load(object sender, EventArgs e)
        {
            CalculationType = EqualFactory.createOperate(1);
        }

        public virtual string Equal(string strOperator = "+", bool isEqualSign = false) { return ""; }
        public virtual string Click_Num_Button(string num) { return null; }
        public virtual string Inverse(string num) { return null; }
        public virtual string SetPoint() { return null; }

        public int StrstrCount(string S,string s) {return System.Text.RegularExpressions.Regex.Matches(S, "["+s+"]").Count; }
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
        #region 简单算术类
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

        /// <summary>
        /// 幂x^y,先输入x
        /// </summary>
        public class CalculatePow : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                Result = Math.Pow(NumberA, NumberB);
                return Result;
            }
        }

        /// <summary>
        /// 开x的y次方根,y√x,先输入x
        /// </summary>
        public class CalculateSqrt : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                if (NumberB != 0)
                {
                    NumberB = 1 / NumberB;
                    Result = Math.Pow(NumberA, NumberB); 
                }
                else
                {
                    MessageBox.Show("根指数不能为零");
                    return 0;
                }
                return Result;
            }
        }
        
        /// <summary>
        /// 阶乘
        /// </summary>
        public class CalculateFac : Calculate
        {         
            public override double GetResult()
            {
                double Result = 0;
                //Result = new Chart().DataManipulator.Statistics.GammaFunction(NumberA);
                if (!NumberA.ToString().Contains("."))
                    return Result = Factorial(NumberA);
                else
                    return double.NaN;
            }

            /// <summary>
            /// 求阶乘
            /// </summary>
            /// <param name="fac"></param>
            /// <returns></returns>
            private double Factorial(double fac)
            {
                if (fac == 1 || fac == 0)
                    return 1;
                return fac * Factorial(fac - 1);
            }
        }
        public class CalculateTan : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                if (NumberA % 180 == 0)
                    return 0;
                return Result = Math.Tan(NumberA * Math.PI / 180); 
            }
        }
        public class CalculateSin : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                return Result = Math.Sin(NumberA * Math.PI / 180);
            }
        }
        public class CalculateCos : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                return Result = Math.Cos(NumberA*Math.PI/180);
            }
        }

        public class CalculateMod : Calculate
        {
            public override double GetResult()
            {
                double Result = 0;
                return Result = NumberA % NumberB;
            }
        }
        #endregion

        public class operationFactory               //处理运算符的类
        {
            public static Calculate createOperate(string Operator)
            {
                Calculate cal = null;

                switch (Operator)
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
                    case "^":
                        cal = new CalculatePow();
                        break;
                    case "√":
                        cal = new CalculateSqrt();
                        break;
                    case "!":
                        cal = new CalculateFac();
                        break;
                    case "tan":
                        cal = new CalculateTan();
                        break;
                    case "sin":
                        cal = new CalculateSin();
                        break;
                    case "cos":
                        cal = new CalculateCos();
                        break;
                    case "%":
                        cal = new CalculateMod();
                        break;
                }
                return cal;
            }
        }

        

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

        //键盘数字按键
        private void Number_button_Click( int key)
        {
            //Button b = (Button)sender;
            Click_Num(key.ToString());
        }

        //运算符按键
        private void Operator_button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            sOperator = b.Tag.ToString();
            CalculatorEqual(sOperator);
        }

        private void Operator_button_Click(string ope)
        {
            sOperator = ope;
            CalculatorEqual(sOperator);
        }

        //清除键
        private void btClear_Click(object sender, EventArgs e)
        {
            total = null;
            sOperatorNum = null;
            RightInput = "";
            tbDisplayScreen.Text = "";
            Button_Clear = true;
        }

        //退格键
        private void btBackSpace_Click(object sender, EventArgs e)
        {
            if (sOperatorNum.Length > 0 && Button_BackSpace)
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
            }  
        }

        //小数点
        private void btPoint_Click(object sender, EventArgs e)
        {
            tbDisplayScreen.Text = CalculationType.SetPoint();
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


        private double dMemorySave = 0;
        private void btMemoryClear_Click(object sender, EventArgs e)
        {
            dMemorySave = 0;
            btClear_Click(sender,e);
        }

        private void btMemoryReply_Click(object sender, EventArgs e)
        {
            tbDisplayScreen.Text = sOperatorNum = dMemorySave.ToString();
            Button_Clear = true;
        }

        private void btMemorySave_Click(object sender, EventArgs e)
        {
            dMemorySave = Convert.ToDouble(tbDisplayScreen.Text);
            Button_Clear = true;
        }

        private void btMemoryPlus_Click(object sender, EventArgs e)
        {
            dMemorySave = Convert.ToDouble(sMemoryCalculate("+"));
            Button_Clear = true;
        }

        private void btMemorySub_Click(object sender, EventArgs e)
        {
            dMemorySave = Convert.ToDouble(sMemoryCalculate("-"));
            Button_Clear = true;
        }
        
        private string sMemoryCalculate(string ope)
        {
            calculate = operationFactory.createOperate(ope);
            calculate.NumberA = Convert.ToDouble(dMemorySave);
            calculate.NumberB = Convert.ToDouble(tbDisplayScreen.Text);
            total = calculate.GetResult().ToString();
            return total;
        }

        //相反数
        private void btInverse_Click(object sender, EventArgs e)
        {
            tbDisplayScreen.Text = CalculationType.Inverse(sOperatorNum);
        }

        //键盘输入
        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0:
                    Number_button_Click(0);
                    break;
                case Keys.D1:
                case Keys.NumPad1:
                    Number_button_Click(1);
                    break;
                case Keys.D2:
                case Keys.NumPad2:
                    Number_button_Click(2);
                    break;
                case Keys.D3:
                case Keys.NumPad3:
                    Number_button_Click(3);
                    break;
                case Keys.D4:
                case Keys.NumPad4:
                    Number_button_Click(4);
                    break;
                case Keys.D5:
                case Keys.NumPad5:
                    Number_button_Click(5);
                    break;
                case Keys.D6:
                case Keys.NumPad6:
                    Number_button_Click(6);
                    break;
                case Keys.D7:
                case Keys.NumPad7:
                    Number_button_Click(7);
                    break;
                case Keys.D8:
                case Keys.NumPad8:
                    Number_button_Click(8);
                    break;
                case Keys.D9:
                case Keys.NumPad9:
                    Number_button_Click(9);
                    break;
                case Keys.Decimal:
                    btPoint_Click(sender, e);
                    break;
                case Keys.Add:
                    Operator_button_Click("+");
                    break;
                case Keys.Subtract:
                    Operator_button_Click("-");
                    break;
                case Keys.Multiply:
                    Operator_button_Click("*");
                    break;
                case Keys.Divide:
                    Operator_button_Click("/");
                    break;
                case Keys.Back:
                    btBackSpace_Click(sender, e);
                    break;
            }
             
        }
    }
}

