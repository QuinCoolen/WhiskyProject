namespace WhiskyBLL.Interfaces
{
  public interface IWhiskyRepository
  {
    void CreateWhisky(WhiskyDTO whisky);
    List<WhiskyDTO> GetWhiskys();
    WhiskyDTO GetWhiskyById(int id);
    Task UpdateWhisky(WhiskyDTO whisky);
    Task DeleteWhisky(int id);
  }
}