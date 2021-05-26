using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.View.ViewModel
{
    internal class PriceListViewModel : BaseViewModel<PriceList>
    {
        public PriceListViewModel(DatabaseUnit db) : base(db)
        {
        }
    }
}
