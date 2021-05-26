using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.View.ViewModel
{
    internal class TNDataListViewModel : BaseViewModel<TNData>
    {
        public TNDataListViewModel(DatabaseUnit db) : base(db)
        {
        }
    }
}
