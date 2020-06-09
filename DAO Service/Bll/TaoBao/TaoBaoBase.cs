using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Top.Api;

namespace Bll.TaoBao
{
    public class TaoBaoBase
    {
        private DefaultTopClient client = null;
        private string sessionKey = "";
        protected string zipPath = "D:/Downloads/Trade/Zip";
        protected string unzipPaht = "D:/Downloads/Trade/Unzip";

        public DefaultTopClient Client
        {
            get { return client; }
            set { client = value; }
        }

        public string SessionKey
        {
            get { return sessionKey; }
            set { sessionKey = value; }
        }

        public void Init(string appKey, string appSecret, string sessionKey, string serverUrl, string format)
        {
            //创建client对象
            client = new DefaultTopClient(serverUrl, appKey, appSecret, format);
            this.sessionKey = sessionKey;
        }

        public TaoBaoBase()
        {
        }

        public TaoBaoBase(DefaultTopClient client, string sessionKey)
        {
            this.client = client;
            this.sessionKey = sessionKey;
        }

        public TaoBaoBase(string appKey, string appSecret, string sessionKey, string serverUrl, string format)
        {
            client = new DefaultTopClient(serverUrl, appKey, appSecret, format);
            this.sessionKey = sessionKey;
        }



        /*
                public string Response2String(TopResponse response)
                {
                    if (!response.IsError)
                        return response.Body;
                    else
                        return "(" + response.ErrCode + ")" + response.ErrMsg;
                }

                public DataSet Response2DataSet(TopResponse response)
                {
                    if (!response.IsError)
                    {
                        DataSet ds = new DataSet();

                        StringReader sr = new StringReader(response.Body);
                        ds.ReadXml(sr);
                        sr.Close();

                        foreach (DataTable dt in ds.Tables)
                        {
                            string tname = dt.TableName;
                            foreach (DataColumn col in dt.Columns)
                            {
                                string colname = col.Caption;
                            }

                            foreach (DataRow row in dt.Rows)
                            {
                                object[] arr = row.ItemArray;
                            }

                        }

                        return ds;
                    }
                    return null;
                }
        */
    }
}
