using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChowHub.Data;
using ChowHub.Dtos.Products;
using ChowHub.helpers;
using ChowHub.Interfaces;
using ChowHub.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ChowHub.Repository.Restaurants
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public ProductRepository(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        public async Task<List<Product>> GetProductsAsync(ProductsQueryObject productsQuery){
            var products = _applicationDBContext.Products.AsQueryable().Where(p => p.RestaurantId == productsQuery.RestaurantId);

            if(!string.IsNullOrWhiteSpace(productsQuery.Name)){
                products.Where(p => p.Name.Contains(productsQuery.Name));
            }

            var skipNumber = (productsQuery.PageNumber - 1) * productsQuery.PageSize;

            return await products.Skip(skipNumber).Take(productsQuery.PageSize).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id){
            return await _applicationDBContext.Products.Include(c => c.Restaurant).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Product> CreateAsync(Product product){
            await _applicationDBContext.Products.AddAsync(product);
            await _applicationDBContext.SaveChangesAsync();
            return product;
        }
    }
}