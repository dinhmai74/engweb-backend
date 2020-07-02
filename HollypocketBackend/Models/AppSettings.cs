using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollypocketBackend.Models
{
    public class AppSettings : IAppSettings
    {
        public string Secret { get; set; }
        public string AccountsCollectionName { get; set; }
        public string BooksCollectionName { get; set; }
        public string ProductCollectionName { get; set; }
        public string QuestionCollectionName { get; set; }
        public string CartsCollectionName { get; set; }
        public string OrdersCollectionName { get; set; }
        public string SavesCollectionName { get; set; }
        public string RateCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string WarehouseCollectionName { get; set; }
        public string ProviderCollectionName { get; set; }
        public string ReviewCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string FavoriteCollectionName { get; set; }
    }

    public interface IAppSettings
    {
        string Secret { get; set; }
        string AccountsCollectionName { get; set; }
        string BooksCollectionName { get; set; }
        string ProductCollectionName { get; set; }
        string FavoriteCollectionName { get; set; }

        string ReviewCollectionName { get; set; }
        string QuestionCollectionName { get; set; }
        string WarehouseCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CartsCollectionName { get; set; }
        string OrdersCollectionName { get; set; }
        string SavesCollectionName { get; set; }

        string ProviderCollectionName { get; set; }
    }
}
