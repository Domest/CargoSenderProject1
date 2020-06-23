using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CargoSenderLibrary.Data
{
    [Serializable]
    public class DataLocalList
    {

        public List<Models.CargoModel> LocalList = new List<Models.CargoModel>();
        

        public DataLocalList() { }
    }
}
