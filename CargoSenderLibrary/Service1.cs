using CargoSenderLibrary.Parser;
using CargoSenderLibrary.DB;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CargoSenderLibrary.Models;
using NLog;

namespace CargoSenderLibrary
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    public class Service1 : IService1
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        List<CargoModel> ListDebug = new List<CargoModel>();

        public List<CargoModel> GetExcel()
        {
            logger.Debug("Запущен метод GetExcel");
            ParserProcess Pars = new ParserProcess();

            logger.Debug("Вызван ParsExcel");
            Pars.ParsExcel();
            logger.Debug("Вызван Saver");
            Pars.Saver();
            logger.Debug("Метод GetExcel завершён");

            return null;
        }

        public List<CargoModel> SendData()
        {
            logger.Debug("Запущен метод SendData");

            CargoSendProcess Send = new CargoSendProcess();
            logger.Debug("Вызван Sender");
            Send.Sender();
            logger.Debug("Метод SendData завершён");

            return null;
        }
    }
}
