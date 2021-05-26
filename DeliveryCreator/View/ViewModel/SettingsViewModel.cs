using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.View.ViewModel
{
    internal class SettingsViewModel : BaseViewModel<SettingsViewModel>
    {
        public SettingsViewModel(DatabaseUnit db) : base(db)
        {
            var str = db.Settings.FirstOrDefault()?.Value;
            Settings = str == null ? new SettingsContent() : JsonConvert.DeserializeObject<SettingsContent>(str);
        }

        public SettingsContent Settings { get; set; }

        public void Save()
        {
            if (db.Settings.Count() == 0)
                db.Settings.Add(new Settings() { Value = JsonConvert.SerializeObject(Settings) });
            else
            {
                var s = db.Settings.FirstOrDefault();
                s.Value = JsonConvert.SerializeObject(Settings);
            }
            db.Complete();
        }

        public List<TNData> TNs
        {
            get
            {
                return db.TNsData.GetAll().ToList();
            }
        }

        public string RecipientsFileName
        {
            get
            {
                return Settings.RecipientsFileName;
            }
            set
            {
                Settings.RecipientsFileName = value;
                PropChanged("RecipientsFileName");
            }
        }
    }
}
