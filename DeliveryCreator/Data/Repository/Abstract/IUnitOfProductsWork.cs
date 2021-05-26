using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryCreator.Data.Repository.Abstract
{
    /// <summary>
    /// it's IUnitOfWork as repository pattern tell's
    /// </summary>
    interface IUnitOfWork : IDisposable
    {
        void Complete();
    }
}
