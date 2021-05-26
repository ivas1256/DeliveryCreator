using DeliveryCreator.Data;
using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using NLog;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeliveryCreator.Logic
{
    class MainBuilder
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        SettingsContent settings;
        TNData TNData;
        string folderName;

        public delegate void UpdateStatusDelegate(string message);
        public event UpdateStatusDelegate UpdateStatusEvent;

        public MainBuilder(SettingsContent settings, string folderName)
        {
            this.settings = settings;
            this.folderName = folderName;
            TNData = (new DatabaseUnit()).TNsData.Get(settings.TNDataId);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public void Build()
        {
            try
            {
                var itemsCount = settings.ItemsCount;
                if (settings.IsAutoItemsCount)
                {
                    UpdateStatusEvent("Вычисление кол-ва доставок");
                    itemsCount = CalcItemsCount();
                }

                UpdateStatusEvent("Генерация сумм доставки");
                var detalizationBuilder = new DetalizationBuilder(itemsCount, settings);
                var nodes = detalizationBuilder.Build();
                UpdateStatusEvent("Создание файла детализации");
                detalizationBuilder.SaveToFile(Path.Combine(folderName, "Детализиция.xlsx"), nodes);

                foreach (var node in nodes)
                {
                    UpdateStatusEvent($"Создание файла от {node.Date.ToString("dd.MM.yyyy")} ТН №{node.TNNumber}");

                    var tNBuilder = new TNBuilder(TNData, settings.ItemsCount, node);
                    tNBuilder.Build(Path.Combine(folderName, $"{node.TNNumber}.xlsx"));
                }
                MessagesProvider.Successfully("Файлы сформированы");
            }
            catch(Exception ex)
            {                
                logger.Error(ex);
                MessagesProvider.Error(ex, "Произошла ошибка");
            }
            finally
            {
                UpdateStatusEvent("");                
            }
        }

        int CalcItemsCount()
        {
            var random = new Random();
            var db = new DatabaseUnit();
            var nodes = new List<DetalizationNode>();

            while(nodes.Sum(x => x.Price) < settings.TotalSum / 2)
            {
                var priceList = db.PriceLists.Get(random.Next(1, db.PriceLists.Count() - 1));
                nodes.Add(new DetalizationNode()
                {
                    PriceList = priceList,
                    Weight = random.NextDouble() * (double)((settings.MinWeight * (decimal)0.2 - settings.MinWeight) + settings.MinWeight)
                });

                var lastNode = nodes.Last();

                nodes.Last().Price = lastNode.Weight <= 10 ? lastNode.PriceList.price_before_10_kg :
                    lastNode.PriceList.price_before_10_kg + (decimal)(lastNode.Weight - 10) * lastNode.PriceList.price_after_10_kg;
            }

            return nodes.Count;
        }
    }
}
