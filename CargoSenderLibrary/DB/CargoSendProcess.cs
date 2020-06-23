using CargoSenderLibrary.Parser;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using CargoSenderLibrary.Data;
using CargoSenderLibrary.Models;
using NLog;

namespace CargoSenderLibrary.DB
{
    public class CargoSendProcess
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        DataLocalList dll = new DataLocalList();

        public List<CargoModel> Sender()
        {
            logger.Debug("Загрузка сохранённого файла");
            Loader();
            using (var context = new DBContext())
            {
                logger.Debug("Контекст БД начинает заполняться элементами для отправки");
                for (int i = 0; i < dll.LocalList.Count; i++)
                {
                    var wagon = new CargoModel()
                    {
                        ShippingDate = dll.LocalList[i].ShippingDate,
                        TCNumber = dll.LocalList[i].TCNumber,
                        WaybillNumber = dll.LocalList[i].WaybillNumber,
                        Cargo = dll.LocalList[i].Cargo,
                        Mark = dll.LocalList[i].Mark,
                        Station = dll.LocalList[i].Station,
                        NetWeight = dll.LocalList[i].NetWeight,
                        GrossWeight = dll.LocalList[i].GrossWeight,
                        DTNumber = dll.LocalList[i].DTNumber,
                        Addressee = dll.LocalList[i].Addressee,
                        ShipmentDateTo = dll.LocalList[i].ShipmentDateTo,
                        ShipmentDateFrom = dll.LocalList[i].ShipmentDateFrom
                    };
                    context.ParsData.Add(wagon);
                    context.SaveChanges();
                }
                logger.Debug("Данные успешно заполнены и отправлены в БД");
                return null;
            }
        }

        public void Loader()
        {
            string readpath = @"C:\Users\Demid\Desktop\le_practic\CargoSenderProj\CargoSenderService\WAGON_LIST 150520.xaml";
            XmlSerializer XmlSer = new XmlSerializer(typeof(DataLocalList));
            using (StreamReader sr = new StreamReader(readpath))
            {
                DataLocalList Deserdll = (DataLocalList)XmlSer.Deserialize(sr);
                dll = Deserdll;
            }
            logger.Debug("Загрузка успешно завершена");
        }
    }
}
