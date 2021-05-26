using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.View.ViewModel
{
    internal class PriceListAddViewModel : EntityBaseViewModel<PriceList>
    {
        public PriceListAddViewModel(DatabaseUnit db, int entityId = -1) : base(db, entityId)
        {
        }

        public override PriceList GetDefaultEntity()
        {
            return new PriceList();
        }

        public override void Save()
        {
            if ((Entity as PriceList).id <= 0)
                db.PriceLists.Add(Entity);

            db.Complete();
        }
    }
}
