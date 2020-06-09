using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DbFactory;
using System.Configuration;
using System.Reflection;
using System.Diagnostics;
using System.IO;


namespace Bll
{
    public class DataProvider
    {
        private static Assembly dll = null;
        private static string dalFactoryClassName = ConfigurationManager.AppSettings["DataProviderFactoryName"];
        private static string BinFolder = "Bin\\";

        public static void SetNullBinFolder() { BinFolder = "";}

        private static Assembly Dll
        {
            get
            {
                if (dll == null)
                {
                    string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                    //Debug.WriteLine(filePath);
                    string dllFileName = ConfigurationManager.AppSettings["DataProviderDllFile"];
                    string file = filePath + BinFolder + dllFileName;
                    if (!File.Exists(file))
                    {
                        file = filePath + dllFileName;
                    }
                    dll = Assembly.LoadFile(file);
                }
                return dll;
            }
        }

        public static DalFactory DataFactory
        {
            get
            {
                try
                {
                    return Dll.CreateInstance(dalFactoryClassName) as DalFactory;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }  


        /*
        private  DalFactory _DataFactory = null;
        public  DataProvider()
        {
            try
            {
                string filePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                //Debug.WriteLine(filePath);
                string dllFileName = ConfigurationManager.AppSettings["DataProviderDllFile"];
                string dalFactoryClassName = ConfigurationManager.AppSettings["DataProviderFactoryName"];
                Assembly dll = Assembly.LoadFile(filePath + "Bin\\" + dllFileName);
                //Assembly dll = Assembly.LoadFile(filePath + "Debug\\" + dllFileName);
                //string path = @"c:\\DataProvider\SQLServerDal.dll";
                //Assembly dll = Assembly.LoadFile(path);
                //_DataFactory = dll.CreateInstance(dalFactoryClassName) as DalFactory;
                _DataFactory = dll.CreateInstance(dalFactoryClassName) as DalFactory;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
        public  DalFactory DataFactory { get { return _DataFactory; } }  
         */
    }
}
