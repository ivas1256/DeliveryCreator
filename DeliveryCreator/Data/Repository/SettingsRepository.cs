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
    internal class SettingsRepository : Repository<Settings>
    {
        public SettingsRepository(DbContext dbContext) : base(dbContext)
        {
        }
    }
}
