using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Repositories
{
    internal class CustomerWriteRespository : WriteRepository<Customer> , ICustomerWriteRepository
    {
        public CustomerWriteRespository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
