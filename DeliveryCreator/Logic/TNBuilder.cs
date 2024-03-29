﻿using DeliveryCreator.Data;
using DeliveryCreator.Data.Model;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.Logic
{
    class TNBuilder
    {
        TNData tNData;
        int itemsCount;
        DetalizationNode detalizationNode;        

        public TNBuilder(TNData tNData, int itemsCount, DetalizationNode detalizationNode)
        {
            var oneBoxWeight = (new Random()).Next(10, 30);

            this.tNData = tNData;
            this.itemsCount = (int)Math.Round(detalizationNode.Weight / oneBoxWeight, MidpointRounding.AwayFromZero);
            this.detalizationNode = detalizationNode;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
       
        public void Build(string fileName)
        {
            if (File.Exists(fileName))
                File.Delete(fileName);

            var random = new Random();
            using (var excel = new ExcelPackage(new FileInfo(Path.Combine(Environment.CurrentDirectory, "tn_pattern.xlsx"))))
            {
                var usedDrivers = new List<int>();
                if (tNData.is_driver_1_active == 1)
                    usedDrivers.Add(0);
                if (tNData.is_driver_2_active == 1)
                    usedDrivers.Add(1);
                if (tNData.is_driver_3_active == 1)
                    usedDrivers.Add(2);

                var driverIndex = random.Next(0, usedDrivers.Count);
                var cargo_transporter_fio = "";
                var car_name = "";
                var car_number = "";

                switch (usedDrivers[driverIndex])
                {
                    case 0:
                        cargo_transporter_fio = tNData.cargo_transporter_fio;
                        car_name = tNData.car_name;
                        car_number = tNData.car_number;
                        break;
                    case 1:
                        cargo_transporter_fio = tNData.cargo_transporter_fio1;
                        car_name = tNData.car_name1;
                        car_number = tNData.car_number1;
                        break;
                    case 2:
                        cargo_transporter_fio = tNData.cargo_transporter_fio2;
                        car_name = tNData.car_name2;
                        car_number = tNData.car_number2;
                        break;
                }

                var dict = new Dictionary<string, string>()
                {
                    { "{{sender_full_name}}", tNData.sender_full_name }
                    ,{ "{{cargo_name}}", tNData.cargo_name }
                    ,{ "{{cargo_docs}}", tNData.cargo_docs }
                    ,{ "{{transport_type}}", tNData.transport_type }
                    ,{ "{{cargo_from_name}}", tNData.cargo_from_name }
                    ,{ "{{cargo_from_condition}}", tNData.cargo_from_condition }
                    ,{ "{{cargo_from_manager_position}}", tNData.cargo_from_manager_position }
                    ,{ "{{cargo_transporter_fio}}", cargo_transporter_fio }
                    ,{ "{{cargo_to_name}}", tNData.cargo_to_name }
                    ,{ "{{transportation_condition}}", tNData.transportation_condition }
                    ,{ "{{transporter_name}}", tNData.transporter_name }
                    ,{ "{{car_name}}", car_name }
                    ,{ "{{car_number}}", car_number }
                    ,{ "{{price_info}}", tNData.price_info }
                    ,{ "{{receiver_full_name}}", tNData.receiver_full_name }

                    ,{ "{{date}}", detalizationNode.Date.ToString("dd.MM.yyyy") }
                    ,{ "{{date_time}}", detalizationNode.Date.ToString("dd.MM.yyyy HH:mm")}
                    ,{ "{{items_count}}", itemsCount.ToString() }
                    ,{ "{{total_weight}}", Math.Round(detalizationNode.Weight, MidpointRounding.AwayFromZero).ToString() }
                    ,{ "{{num}}", detalizationNode.TNNumber }
                    ,{ "{{receiver}}", detalizationNode.Receiver.ToString() }
                    ,{ "{{short_receiver}}", detalizationNode.Receiver.ToShortString() }
                };

                var ws = excel.Workbook.Worksheets[0];
                var query = from cell in ws.Cells["A:XFD"]
                            where dict.Keys.Where(x => (cell.Value?.ToString() ?? "").Contains(x)).Count() != 0
                            select cell;

                foreach (var cell in query)
                {
                    foreach(var key in dict.Keys)
                        cell.Value = cell.Value = cell.Value.ToString().Replace(key, dict[key]);
                }

                excel.SaveAs(new FileInfo(fileName));
            }
        }
    }
}
