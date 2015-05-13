using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C3.Business.Interfaces;
using C3.Data;

namespace C3.Business
{
    /// <summary>
    /// NOT BEING USED
    /// </summary>
    //public class PayerService : IPayerService
    //{
    //    private static volatile PayerService _instance;
    //    private static object _syncRoot = new Object();

    //    //FF: I made this public for Dependency Injection Implementation.
    //    public PayerService()
    //    {

    //    }

    //    public static PayerService Instance
    //    {
    //        get
    //        {
    //            if (_instance == null)
    //            {
    //                lock (_syncRoot)
    //                {
    //                    if ( _instance == null)
    //                        _instance = new PayerService();
    //                }
    //            }
    //            return _instance;
    //        }
    //    }

    //    #region IPayerService Members

    //    public List<Payor>  GetPayersForCareOpportunityReport()
    //    {
    //                        throw new NotImplementedException();
    //    }

    //    #endregion

    //    #region Public Methods
    //    public string GetPayersXML(List<Payor> payers)
    //    {
    //        string _payers = "";
    //        StringBuilder payersXML = new StringBuilder();

    //        //Beginning of XML 
    //        payersXML.Append("<payers>");

    //        List<int> ids = new List<int>();
    //        foreach (Payor payer in payers)
    //        {
    //            if (!ids.Contains(payer.PayerId))
    //            {
    //                payersXML.AppendFormat("<payer>{0}</payer>", payer.PayerId);
    //            }
    //            ids.Add(payer.PayerId);
    //        }

    //        payersXML.Append("</payers>");
    //        _payers = payersXML.ToString();
    //        return _payers;

    //    }
    //    #endregion
    //}
}
