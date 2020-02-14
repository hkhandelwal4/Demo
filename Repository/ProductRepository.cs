using Google.Cloud.Firestore;
using ProductsAPI.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly FirestoreDb _dbContext;

        public ProductRepository(FirestoreDb dbContext)
        {
            _dbContext = dbContext;
        }
        public void DeleteProduct(int productId)
        {

        }

        public Dictionary<string, object> GetProductByID(int productId)
        {
            CollectionReference usersRef = _dbContext.Collection("Product");
            Query query = usersRef.WhereEqualTo("Id", productId);
            QuerySnapshot querySnapshot = query.GetSnapshotAsync().Result;
            if (querySnapshot.Count > 0)
            {
                return querySnapshot.Documents[0].ToDictionary();
            }

            return null;
        }

        public async Task<Dictionary<string, object>> GetAsyncProductByID(int productId)
        {
            CollectionReference usersRef = _dbContext.Collection("Product");
            Query query = usersRef.WhereEqualTo("Id", productId);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            if (querySnapshot.Count > 0)
            {
                return querySnapshot.Documents[0].ToDictionary();
            }

            return null;
        }

        public async Task<Product> GetAsyncProductInternalByID(int productId)
        {
            CollectionReference usersRef = _dbContext.Collection("Product");
            Query query = usersRef.WhereEqualTo("Id", productId);
            QuerySnapshot querySnapshot = await query.GetSnapshotAsync();
            var result = querySnapshot.Documents[0].ToDictionary();
            return new Product()
            {
                Id = Convert.ToInt32(result["Id"]),
                Price = Convert.ToInt32(result["Price"]),
                Description = Convert.ToString(result["Description"]),
                Category = Convert.ToString(result["Category"]),
                Name = Convert.ToString(result["Brand"])
            };
        }

        public async Task<long> GetAsyncProductCount()
        {
            return 0;
        }

        public async Task<List<Dictionary<string, object>>> GetProducts()
        {

            CollectionReference usersRef = _dbContext.Collection("Product");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Console.WriteLine("Id: {0}", document.Id);
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                documents.Add(documentDictionary);
            }
            return documents;
        }

        public void InsertProduct(Product product)
        {

        }

        public void Save()
        {

        }

        public void UpdateProduct(Product product)
        {

        }

        public async Task<List<Dictionary<string, object>>> GetSubcategorys()
        {
            CollectionReference usersRef = _dbContext.Collection("Subcategory");
            QuerySnapshot snapshot = await usersRef.GetSnapshotAsync();
            List<Dictionary<string, object>> documents = new List<Dictionary<string, object>>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                Console.WriteLine("Id: {0}", document.Id);
                Dictionary<string, object> documentDictionary = document.ToDictionary();
                documents.Add(documentDictionary);
            }
            return documents;
        }

    }
}
