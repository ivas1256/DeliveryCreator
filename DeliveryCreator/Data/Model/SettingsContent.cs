using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.Data.Model
{
    class SettingsContent
    {
        public DateTime DateFrom { get; set; } = DateTime.Now;
        public DateTime DateTo { get; set; } = DateTime.Now.AddMonths(1);
        public decimal TotalSum { get; set; }
        public string StartingTNNum { get; set; }
        public int ItemsCount { get; set; }
        public bool IsAutoItemsCount { get; set; }

        public int TNDataId { get; set; }

        public string RecipientsFileName { get; set; }

        public decimal MinStep { get; set; }
        public decimal MaxStep { get; set; }

        public decimal MinWeight { get; set; }
        public decimal MaxWeight { get; set; }

        public int MinHour { get; set; }
        public int MaxHour { get; set; }
    }
}
