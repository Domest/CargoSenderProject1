using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CargoSenderLibrary.Data
{
    [Serializable]
    public class DataSendList
    {
        public List<Models.ParsModel> SendList = new List<Models.ParsModel>();

        public DataSendList() { }
    }
}