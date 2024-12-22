using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Dtos.Products;
using ChowHub.helpers;
using ChowHub.Models;

namespace ChowHub.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync(ProductsQueryObject productsQuery);
        Task<Product?> GetByIdAsync(int id);
        Task<Product> CreateAsync(Product product);
    }
}