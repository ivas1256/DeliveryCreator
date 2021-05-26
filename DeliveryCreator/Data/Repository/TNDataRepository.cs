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
    internal class TNDataRepository : Repository<TNData>
    {
        public TNDataRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
