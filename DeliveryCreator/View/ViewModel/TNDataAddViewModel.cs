using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.View.ViewModel
{
    internal class TNDataAddViewModel : EntityBaseViewModel<TNData>
    {
        public TNDataAddViewModel(DatabaseUnit db, int entityId = -1) : base(db, entityId)
        {
        }

        public override TNData GetDefaultEntity()
        {
            return new TNData();
        }

        public override void Save()
        {
            if ((Entity as TNData).id <= 0)
                db.TNsData.Add(Entity);

            db.Complete();
        }
    }
}
