using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Xml.Serialization;
using System.IO;
using CargoSenderLibrary.Data;
using CargoSenderLibrary.Models;
using NLog;

namespace CargoSenderLibrary.Parser
{
    [Serializable]
    public class ParserProcess
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        DataLocalList dll = new DataLocalList();
        public DateTime? DateTo;
        public DateTime? DateFrom;
        public double Tons;
        public double KG;

        public ParserProcess () { }
        public List<CargoModel> ParsExcel()
        {
            logger.Debug("Начат процесс парсинга excel файла");
            var enumexcel = EnumerateExcel();
            
            foreach (var e in enumexcel)
            {
                SpliterDate(e.ShipmentDateBase);
                e.ShipmentDateTo = DateTo;
                e.ShipmentDateFrom = DateFrom;

                double[] ToConvert = { e.NetWeight, e.GrossWeight};
                TonKilConvert(ToConvert);
                e.NetWeight = ToConvert[0];
                e.GrossWeight = ToConvert[1];

                dll.LocalList.Add(new CargoModel() 
                { 
                    ShippingDate = e.ShippingDate, 
                    TCNumber = e.TCNumber, 
                    WaybillNumber = e.WaybillNumber, 
                    Cargo = e.Cargo, 
                    Mark = e.Mark, 
                    Station = e.Station, 
                    NetWeight = e.NetWeight, 
                    GrossWeight = e.GrossWeight, 
                    DTNumber = e.DTNumber, 
                    Addressee = e.Addressee,
                    ShipmentDateTo = e.ShipmentDateTo,
                    ShipmentDateFrom = e.ShipmentDateFrom
                });
            }
            logger.Info("Файл успешно распарсен. Было выгружено {0} элементов", dll.LocalList.Count);
            
            return dll.LocalList;
        }

        static IEnumerable<ParsModel> EnumerateExcel()
        {
            /* Пришлось заниматься сериализацией из-за проблемы со списком. Где бы я его не объявил - после заполнения парсером все элементы пропадают если обратиться к экземпляру списка 
             в методах других классов. 
            
            Изначально я хотел чтобы файл лежал в папке с проектом, без указания полного пути к нему. 
            Но по какой-то причине у WCF родной папкой считается "Profram Files/IIS Express". Не зная как решить проблему, вынужден был остановиться на таком варианте. 
            ЧТобы сериализация сработала - придётся вручную вбить путь к файлу. Тоже самое сделать в классе CargoSendProcess, а также в этом же классе, в методе Saver. */
            string xlsxpath = @"C:\Users\Demid\Desktop\le_practic\CargoSenderProj\CargoSenderService\WAGON_LIST 150520.xlsx";
            using (var workbook = new XLWorkbook(xlsxpath))
            {
                var worksheet = workbook.Worksheets.Worksheet(1);
                int rows = worksheet.RowsUsed().Count();
                for (int row = 2; row <= rows; ++row)
                {
                    var metric = new ParsModel
                    {
                        ShippingDate = worksheet.Cell(row, 1).GetValue<DateTime>(),
                        TCNumber = worksheet.Cell(row, 2).GetValue<int>(),
                        WaybillNumber = worksheet.Cell(row, 3).GetValue<string>(),
                        Cargo = worksheet.Cell(row, 4).GetValue<string>(),
                        Mark = worksheet.Cell(row, 5).GetValue<string>(),
                        Station = worksheet.Cell(row, 6).GetValue<string>(),
                        NetWeight = worksheet.Cell(row, 7).GetValue<double>(),
                        GrossWeight = worksheet.Cell(row, 8).GetValue<double>(),
                        DTNumber = worksheet.Cell(row, 9).GetValue<string>(),
                        Addressee = worksheet.Cell(row, 10).GetValue<string>(),
                        ShipmentDateBase = worksheet.Cell(row, 11).GetValue<string>()
                    };
                    yield return metric;
                }
            }
        }

        public void Saver()
        {
            logger.Debug("Начат процесс сохранения");
            XmlSerializer xsav = new XmlSerializer(typeof(DataLocalList));
            string WritePath = @"C:\Users\Demid\Desktop\le_practic\CargoSenderProj\CargoSenderService\WAGON_LIST 150520.xaml";

            using (StreamWriter sw = new StreamWriter(WritePath, false, System.Text.Encoding.UTF8))
            {
                xsav.Serialize(sw, dll);
            }
            logger.Debug("Сохранение успешно завершено");
        }

        public void SpliterDate(string DateSDB)
        {
            
            /* Сначала пытался заставить метод получить три значения и вернуть два (на возврат DateFrom и DateTo). 
             Пытался использовать ValueTuple, однако это у меня не вышло. Пришлось лепить паблик переменные в начале класса. Почти та же история и с методом перевода тонн в килограммы. */
            if (DateSDB != "")
            {
                string[] dates = DateSDB.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                DateFrom = Convert.ToDateTime(dates[0]);
                DateTo = Convert.ToDateTime(dates[1]);
            }
            else
            {
                DateTo = null;
                DateFrom = null;
            }
        }

        public void TonKilConvert(double[] NetGross)
        {
            NetGross[0] *= 1000;
            NetGross[1] *= 1000;
            return;
        }
    }
}
