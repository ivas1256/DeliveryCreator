using DeliveryCreator.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.View.ViewModel
{
    class BaseViewModel<T> : INotifyPropertyChangedBase where T : class
    {
        protected string query;

        public ObservableCollection<T> Items
        {
            get
            {
                var res = new ObservableCollection<T>();
                foreach (var item in db.Context.Set<T>().ToList())
                {
                    if (!string.IsNullOrWhiteSpace(query))
                    {
                        if (item.ToString().Contains(query))
                            res.Add(item);
                    }
                    else
                        res.Add(item);
                }

                return res;
            }
        }

        protected DatabaseUnit db;
        public BaseViewModel(DatabaseUnit db)
        {
            this.db = db;
            Update();
        }        

        public virtual void Remove(T obj)
        {
            db.Context.Set<T>().Remove(obj);
            db.Complete();
            Update();
        }       

        public void Update()
        {
            PropChanged("Items");
        }

        public void SetQuery(string query)
        {
            this.query = query;
            Update();
        }
    }
}
