using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 创建人：郑志冲
    /// <para>日期：2012-10-06</para>
    /// <para>公式计算(不使用)</para>
    /// <para>注:</para><para>1.字符串需要使用单引号''包含，字段不需要;</para>
    /// <para>2.布尔值使用: true、false;</para>
    /// <para>3.判断表达式格式(暂时不支持嵌套)：@If(判断表达式1;TrueReturn1;判断表达式2;TrueReturn2;判断表达式3;TrueReturn3...;FalseReturn)</para>
    /// </summary>
    public class FormulaCalc //: RPN
    {
        //public static void Calc(string a, DataRow b, string c) { }
        //private static List<string[]> ifList = new List<string[]>();

        ///// <summary>
        ///// 创建人：郑志冲
        ///// <para>日期：2012-10-06</para>
        ///// <para>计算公式</para>
        ///// </summary>
        ///// <param name="expression">表达式</param>
        ///// <param name="row">数据行</param>
        ///// <param name="fieldResult">返回结果对应字段名称</param>
        //public static void Calc(string expression, DataRow row, string fieldResult)
        //{
        //    if (!expression.ToUpper().Contains("@IF"))
        //    {
        //        if (Parse(expression))
        //        {
        //            SetExpValueByRow(expression, row);
        //            object o = Evaluate();
        //            MatchingDataRowField(row, fieldResult, o);
        //        }
        //    }
        //    else
        //    {
        //        if (ParseIF(expression))
        //        {
        //            object o = CalcIF(row);
        //            MatchingDataRowField(row, fieldResult, o);
        //        }
        //    }
        //}

        ///// <summary>
        ///// 创建人：方君业
        ///// <para>日期：2012-10-08</para>
        ///// <para>计算公式</para>
        ///// </summary>
        ///// <param name="expression">表达式</param>
        ///// <param name="row">数据行</param>
        ///// <param name="fieldResult">返回结果对应字段名称</param>
        ///// <returns>返回Object,不对DataRow进行修改</returns>
        //public static object Calcb(string expression, DataRow row, string fieldResult)
        //{
        //    object result = null ;
        //    if (!expression.ToUpper().Contains("@IF"))
        //    {
        //        if (Parse(expression))
        //        {
        //            SetExpValueByRow(expression, row);
        //            object o = Evaluate();
        //            result = MatchingDataRowFieldb(row, fieldResult, o);
        //        }
        //    }
        //    else
        //    {
        //        if (ParseIF(expression))
        //        {
        //            object o = CalcIF(row);
        //            result = MatchingDataRowFieldb(row, fieldResult, o);
        //        }
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 创建人：郑志冲
        ///// <para>日期：2012-10-06</para>
        ///// <para>匹配数据行的字段，把值代入对应字段</para>
        ///// </summary>
        ///// <param name="row">数据行</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <param name="value">值</param>
        //private static void MatchingDataRowField(DataRow row, string fieldName, object value)
        //{
        //    if (value.Equals("''"))
        //    {
        //        if (row[fieldName] is int)
        //            value = 0;
        //        else
        //            value = "";
        //    }



        //    if (row[fieldName] is bool)
        //        row[fieldName] = Convert.ToBoolean(value);
        //    else
        //    {
        //        string str = value.ToString();
        //        string f = str.Substring(0, 1);
        //        string l = str.Substring(str.Length - 1, 1);

        //        if (str.Length > 1 && f.Equals("'") && l.Equals("'"))
        //        {
        //            str = str.Substring(1, str.Length - 2);
        //        }
        //        //if (row[fieldName] is DateTime)
        //        //    row[fieldName] = str;
        //        //else
        //            row[fieldName] = str;
        //    }
        //}

        ///// <summary>
        ///// 创建人：方君业
        ///// <para>日期：2012-10-08</para>
        ///// <para>匹配数据行的字段，把值代入对应字段</para>
        ///// </summary>
        ///// <param name="row">数据行</param>
        ///// <param name="fieldName">字段名称</param>
        ///// <param name="value">值</param>
        ///// <returns>返回Object,不对DataRow进行修改</returns>
        //private static object MatchingDataRowFieldb(DataRow row, string fieldName, object value)
        //{
        //    object result = null;
        //    if (value.Equals("''"))
        //    {
        //        if (row[fieldName] is int)
        //            value = 0;
        //        else
        //            value = "";
        //    }



        //    if (row[fieldName] is bool)
        //        result = Convert.ToBoolean(value);
        //    else
        //    {
        //        string str = value.ToString();
        //        string f = str.Substring(0, 1);
        //        string l = str.Substring(str.Length - 1, 1);

        //        if (str.Length > 1 && f.Equals("'") && l.Equals("'"))
        //        {
        //            result = str.Substring(1, str.Length - 2);
        //        }
        //        //if (row[fieldName] is DateTime)
        //        //    row[fieldName] = str;
        //        else
        //            result = str;
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// 创建人：郑志冲
        ///// <para>日期：2012-10-06</para>
        ///// <para>计算IF语句</para>
        ///// </summary>
        ///// <param name="row"></param>
        ///// <returns></returns>
        //private static object CalcIF(DataRow row)
        //{
        //    if (ifList.Count == 0)
        //        return null;

        //    for (int i = 0; i < ifList.Count; i++)
        //    {
        //        string[] exps = ifList[i];
        //        if (exps[1] == null)    //整个@IF语句中最后一个表达式
        //        {
        //            if (Parse(exps[0]))
        //            {
        //                SetExpValueByRow(exps[0], row);
        //                return Evaluate();
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            if (Parse(exps[0])) //数组中的第一个表达式
        //            {
        //                SetExpValueByRow(exps[0], row);
        //                bool b = Convert.ToBoolean(Evaluate());
        //                if (b)  //第一个表达式为真是，按第二个表达式返回
        //                {
        //                    if (Parse(exps[1]))
        //                    {
        //                        SetExpValueByRow(exps[1], row);
        //                        return Evaluate();
        //                    }
        //                    else
        //                    {
        //                        return null;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 创建人：郑志冲
        ///// <para>日期：2012-10-06</para>
        ///// <para>解析IF语句</para>
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //private static bool ParseIF(string expression)
        //{
        //    ifList.Clear();
        //    //判断书写是否正确
        //    string strIF = expression.Substring(0, 4);
        //    string strEnd = expression.Substring(expression.Length - 1, 1);
        //    if (!strIF.ToUpper().Equals("@IF(") || !strEnd.Equals(")"))
        //        return false;

        //    Regex reg = new Regex("@IF");
        //    string[] temp = reg.Split(expression.ToUpper());
        //    if (temp.Length > 2)
        //    {

        //        return false;
        //    }
        //    //把同一组判断、返回表达式放置一个数组里面，最后一组只有一个返回表达式
        //    expression = expression.Substring(strIF.Length, expression.Length - strIF.Length - 1);
        //    string[] exps = expression.Split(new char[] { ';' });
        //    if (exps.Length > 2 && exps.Length % 2 == 1)
        //    {
        //        string[] param = new string[2];
        //        for (int i = 0; i < exps.Length; i++)
        //        {
        //            if ((i + 1) % 2 == 1)
        //            {
        //                param[0] = exps[i];
        //            }
        //            else
        //            {
        //                param[1] = exps[i];
        //                ifList.Add(param);
        //                param = new string[2];
        //            }
        //        }
        //        ifList.Add(param);
        //        return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 创建人：郑志冲
        ///// <para>日期：2012-10-06</para>
        ///// <para>把数据行对应的值替换表达式的字段名</para>
        ///// </summary>
        ///// <param name="expression">表达式</param>
        ///// <param name="row">数据行</param>
        //private static void SetExpValueByRow(string expression, DataRow row)
        //{
        //    foreach (object o in Tokens)
        //    {
        //        if (o is Operand)
        //        {
        //            Operand op = (Operand)o;
        //            if (op.Type == OperandType.STRING)
        //            {
        //                string f = op.Value.ToString().Substring(0, 1);
        //                string l = op.Value.ToString().Substring(op.Value.ToString().Length - 1, 1);

        //                if (f.Equals("'") && l.Equals("'"))
        //                    continue;

        //                if (row[op.Value.ToString()] is string)
        //                {
        //                    op.Value = "'" + Convert.ToString(row[op.Value.ToString()]) + "'";
        //                }
        //                else if (row[op.Value.ToString()] is DateTime)
        //                {
        //                    op.Value = "'" + Convert.ToDateTime(row[op.Value.ToString()]).ToString("yyyy-MM-dd HH:mm:ss") + "'";
        //                }
        //                else
        //                {
        //                    op.Type = OperandType.NUMBER;
        //                    op.Value = Convert.ToString(row[op.Value.ToString()]);
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
