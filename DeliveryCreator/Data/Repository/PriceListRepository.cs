using DeliveryCreator.Data.Model;
using DeliveryCreator.Data.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.Data.Repository
{
    class PriceListRepository : Repository<PriceList>
    {
        public PriceListRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public PriceList GetByCityName(string name)
        {
            return dbContext.Set<PriceList>().Where(x => x.region_name.ToLower().Contains(name.ToLower())).FirstOrDefault();
        }
    }
}
