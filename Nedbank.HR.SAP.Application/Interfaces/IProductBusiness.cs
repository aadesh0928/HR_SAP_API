using Nedbank.SBE.API.Resources.Models.Mongo.BusinessApplication.ProductCatalogue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nedbank.HR.SAP.BAL.Interfaces
{
    public interface IProductBusiness
    {
        Task<List<Product>> FetchProductsAsync();
    }
}
