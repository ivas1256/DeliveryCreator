using DeliveryCreator.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.View.ViewModel
{
    abstract class EntityBaseViewModel<T> where T : class
    {
        public T Entity { get; set; }

        protected DatabaseUnit db;
        protected int entityId;
        public EntityBaseViewModel(DatabaseUnit db, int entityId = -1)
        {
            this.db = db;
            this.entityId = entityId;
            Update();
        }
        
        public abstract T GetDefaultEntity();
        public abstract void Save();

        public void Update()
        {
            if (entityId > 0)
                Entity = db.Context.Set<T>().Find(entityId);
            else
                Entity = GetDefaultEntity();
        }        
    }
}
