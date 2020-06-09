using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
//using IronPython.Hosting;
using System.IO;
using System.Reflection;

namespace Common
{
    /// <summary>
    /// 创建人：郑志冲
    /// 日期：2012-09-20
    /// 通用脚本处理类
    /// </summary>
    public class ScriptHandler
    {
/*
        ScriptRuntime python = null;
        ScriptEngine engine = null;
        ScriptScope scope = null;
        public ScriptHandler()
        {
            python = Python.CreateRuntime();
            engine = python.GetEngine("Python");
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2012-09-22
        /// 执行脚本，返回对象
        /// </summary>
        /// <param name="expression">脚本</param>
        /// <param name="funtionName">脚本对应的函数名称</param>
        /// <param name="param">参数，对象数组</param>
        /// <returns></returns>
        public object ExecuteScriptFromString(string expression, string funtionName, object[] param)
        {
            ScriptSource source = engine.CreateScriptSourceFromString(expression);
            scope = engine.CreateScope();
            source.Execute(scope);
            
            var function = scope.GetVariable<Func<object, object>>(funtionName);
            return function(param);
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2012-09-22
        /// 执行脚本文件，返回对象
        /// </summary>
        /// <param name="path">脚本文件路径</param>
        /// <param name="funtionName">脚本对应的函数名称</param>
        /// <param name="param">参数，对象数组</param>
        /// <returns></returns>
        public object ExecuteScriptFromFile(string path, string funtionName, object[] param)
        {
            string expression = GetStringByFile(path);
            ScriptSource source = engine.CreateScriptSourceFromString(expression);
            scope = engine.CreateScope();
            source.Execute(scope);

            var function = scope.GetVariable<Func<object, object>>(funtionName);
            return function(param);
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2012-09-22
        /// 加载相关引用的程序集
        /// </summary>
        /// <param name="typeFullName">程序集类型全称</param>
        public void LoadAssembly(string typeFullName)
        {
            python.LoadAssembly(Assembly.GetAssembly(Type.GetType(typeFullName)));
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2012-09-22
        /// 加载相关引用的程序集
        /// </summary>
        /// <param name="type">程序集类型</param>
        public void LoadAssembly(Type type)
        {
            python.LoadAssembly(Assembly.GetAssembly(type));
        }

        /// <summary>
        /// 创建人：郑志冲
        /// 日期：2012-09-22
        /// 读取文件字符串
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        private string GetStringByFile(string path)
        {
            if (!File.Exists(path))
                return "";

            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            string strs = sr.ReadToEnd();
            sr.Close();
            fs.Close();

            return strs;
        }
        */
    }
}
