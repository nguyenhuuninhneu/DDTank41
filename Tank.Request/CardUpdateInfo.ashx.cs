﻿using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Bussiness;
using Road.Flash;
using log4net;
using System.Reflection;
using SqlDataProvider.Data;

namespace Tank.Request
{
    /// <summary>
    /// Summary description for CardUpdateInfo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CardUpdateInfo : IHttpHandler
    {

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {

            context.Response.Write(Bulid(context));

        }

        public static string Bulid(HttpContext context)
        {
            bool value = false;
            string message = "Fail!";
            XElement result = new XElement("Result");

            try
            {
                using (ProduceBussiness db = new ProduceBussiness())
                {
                    SqlDataProvider.Data.CardUpdateInfo[] infos = db.GetAllCardUpdateInfo();
                    foreach (SqlDataProvider.Data.CardUpdateInfo info in infos)
                    {
                        result.Add(FlashUtils.CreateCardUpdateInfo(info));
                    }

                    value = true;
                    message = "Success!";
                }
            }
            catch (Exception ex)
            {
                log.Error("Load CardUpdateInfo is fail!", ex);
            }

            result.Add(new XAttribute("value", value));
            result.Add(new XAttribute("message", message));

            return csFunction.CreateCompressXml(context, result, "CardUpdateInfo", true);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}