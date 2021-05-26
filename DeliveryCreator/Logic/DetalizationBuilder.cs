using DeliveryCreator.Data;
using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
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
    class DetalizationBuilder
    {
        private int itemsCount;
        private List<Receiver> receivers;        
        private DatabaseUnit db;
        private SettingsContent settings;
        private string currTNNumber;
        private Random random;

        public DetalizationBuilder(int itemsCount, SettingsContent settings)
        {
            this.itemsCount = itemsCount;
            this.settings = settings;

            random = new Random();
            currTNNumber = settings.StartingTNNum;           
            receivers = ParseReceiversFile(settings.RecipientsFileName);
            db = new DatabaseUnit();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public void SaveToFile(string fileName, List<DetalizationNode> nodes)
        {            
            using (var excel = new ExcelPackage(new FileInfo(Path.Combine(Environment.CurrentDirectory, "detalization_pattern.xlsx"))))
            {
                var ws = excel.Workbook.Worksheets[0];
                var row = 4;
                ws.InsertRow(row, nodes.Count, 3);
                ws.Cells[1, 1].Value = ws.Cells[1, 1].Value.ToString().Replace("{{date}}", nodes.Select(x => x.Date).Max().ToString("dd.MM.yyyy"));

                foreach (var node in nodes.OrderBy(x => x.Date))
                {
                    ws.Cells[row, 1].Value = node.Date.ToString("dd.MM.yyyy");
                    ws.Cells[row, 2].Value = $"Москва-{node.PriceList.region_name}";
                    ws.Cells[row, 3].Value = $"{Math.Round(node.Weight, MidpointRounding.AwayFromZero)}кг";
                    ws.Cells[row, 4].Value = Math.Round(node.Price, 2);
                    ws.Cells[row, 5].Value = node.TNNumber;
                    row++;
                }

                ws.Cells[row, 4].Formula = $"=sum(D4:D{row-1})";
                excel.SaveAs(new FileInfo(fileName));
            }
        }        
    
        public List<DetalizationNode> Build()
        {            
            var r = new List<DetalizationNode>();            
            // если получателей меньше чем доставок надо сгенерить, то берем всех получаетелей
            r.AddRange(receivers.Select(x => GenerateDetalizationNode(x))
                .Take(itemsCount >= receivers.Count ? receivers.Count : itemsCount));

            //заполняем случайными городами оставшееся число
            if (r.Count() < itemsCount)
                for (var i = 0; i < itemsCount - r.Count(); i++)                   
                    r.Add(GenerateDetalizationNode());                    

            //заполням веса 
            foreach (var node in r)
                node.Weight = (double)settings.MinWeight;
            r = CalcPrices(r);

            while (!CheckCorrect(r))
            {
                var diff = settings.TotalSum - r.Sum(x => x.Price);
                var percent = (100 * diff) / settings.TotalSum;

                int affectedCount = (int)Math.Ceiling(r.Count * percent / 100);
                for (int i = 0; i < affectedCount; i++)
                {
                    var item = r[random.Next(0, r.Count - 1)];
                    if(item.Weight >= (double)settings.MaxWeight)
                    {
                        i--;
                        continue;
                    }
                    
                    item.Weight += Math.Round(random.NextDouble() * (double)((settings.MaxStep - settings.MinStep) + settings.MinStep), 2);
                }
                r = CalcPrices(r);
            }

            if (r.Sum(x => x.Price) > settings.TotalSum)
                r = FixTotalSum(r);            
            
            var j = 0;
            foreach (var node in r.OrderBy(x => x.Date))            
                node.TNNumber = GetNextTNNUmber(j++);

            return r;
        }

        List<DetalizationNode> FixTotalSum(List <DetalizationNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.Weight = Math.Round(node.Weight, MidpointRounding.AwayFromZero);
            }

            var diff = nodes.Sum(x => x.Price) - settings.TotalSum;

            foreach (var node in nodes)
                node.Price -= diff / nodes.Count;

            if (nodes.Sum(x => x.Price) > settings.TotalSum)
                nodes[random.Next(0, nodes.Count - 1)].Price -= nodes.Sum(x => x.Price) - settings.TotalSum;

            return nodes;
        }

        DetalizationNode GenerateDetalizationNode(Receiver receiver = null)
        {
            var range = (settings.DateTo - settings.DateFrom).Days;
            var priceList = db.PriceLists.GetByCityName(receiver.City) ?? db.PriceLists.Get(random.Next(1, db.PriceLists.Count()));
            if (receiver == null)
                priceList = db.PriceLists.GetByCityName(receivers.ElementAt(random.Next(1, receivers.Count - 1)).City);

            var r = new DetalizationNode()
            {
                Date = settings.DateFrom.AddDays(random.Next(range)),
                PriceList = priceList,                
                Receiver = receiver
            };

            r.Date = new DateTime(r.Date.Year, r.Date.Month, r.Date.Day,
                random.Next(settings.MinHour, settings.MaxHour),
                random.Next(0, 59)
                , 0);            

            return r;
        }

        private string GetNextTNNUmber(int increase)
        {            
            var match = "";
            var num = "";
            if (!settings.StartingTNNum.Contains('-'))
            {
                match = Regex.Match(settings.StartingTNNum, @"\d+").Value;
                num = settings.StartingTNNum.Replace(match, (int.Parse(match) + increase).ToString());
            }
            else
            {
                var parts = settings.StartingTNNum.Split('-');

                match = parts.Last();
                num = string.Join("-", parts.Take(parts.Count() - 1)) + "-" + (int.Parse(match) + increase).ToString();
            }
            
            return num;
        }

        bool CheckCorrect(List<DetalizationNode> nodes)
        {
            return nodes.Sum(x => x.Price) >= settings.TotalSum;
        }
    
        List<DetalizationNode> CalcPrices(List<DetalizationNode> nodes)
        {
            foreach (var node in nodes)
            {
                node.Price = node.Weight <= 10 ? node.PriceList.price_before_10_kg :
                    node.PriceList.price_before_10_kg + node.PriceList.price_after_10_kg * (decimal)(node.Weight - 10);

                node.Price = Math.Round(node.Price, MidpointRounding.AwayFromZero);
            }

            return nodes;
        }

        List<Receiver> ParseReceiversFile(string receiversFileName)
        {
            var results = new List<Receiver>();
            using (var excel = new ExcelPackage(new FileInfo(receiversFileName)))
            {
                var ws = excel.Workbook.Worksheets.First();
                int row = 1;
                while (row <= GetLastUsedRow(ws))
                {
                    var receiver = new Receiver()
                    {
                        Name = ws.Cells[row, 1].Value?.ToString(),
                        Address = ws.Cells[row, 3].Value?.ToString(),
                        City = ws.Cells[row, 2].Value?.ToString(),
                        Fio = ws.Cells[row, 4].Value?.ToString(),
                        Phone = ws.Cells[row, 5].Value?.ToString(),
                    };
                    results.Add(receiver);
                    row++;
                }
            }
            return results;
        }
        int GetLastUsedRow(ExcelWorksheet sheet)
        {
            if (sheet.Dimension == null)
                return 0;
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }
    }
}
