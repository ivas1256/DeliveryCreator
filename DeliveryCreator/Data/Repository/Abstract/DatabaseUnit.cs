using DeliveryCreator.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.Data.Repository.Abstract
{
    public class DatabaseUnit : IUnitOfWork
    {
        internal PriceListRepository PriceLists { get; set; }
        internal TNDataRepository TNsData { get; set; }
        internal SettingsRepository Settings { get; private set; }
       

        private readonly DataContext _context;
        public DatabaseUnit()
        {
            _context = new DataContext();
          
            Settings = new SettingsRepository(_context);
            PriceLists = new PriceListRepository(_context);
            TNsData = new TNDataRepository(_context);
        }        

        public DataContext Context
        {
            get
            {
                return _context;
            }
        }

        public void Sql(string sql, params object[] args)
        {
            _context.Database.ExecuteSqlCommand(sql, args);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
