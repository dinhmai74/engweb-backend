namespace HollypocketBackend.Models
{
  public class AccountstoreDatabaseSettings : IAccountstoreDatabaseSettings
  {
    public string AccountsCollectionName { get; set; }
    public string ConnectionString { get; set; }
    public string DatabaseName { get; set; }
  }

  public interface IAccountstoreDatabaseSettings
  {
    string AccountsCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
  }
}