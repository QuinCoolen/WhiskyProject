using WhiskyBLL.Dto;

namespace WhiskyBLL.Interfaces
{
    public interface IWhiskyRepository
  {
    void CreateWhisky(WhiskyDto whisky);
    List<WhiskyDto> GetWhiskys();
    WhiskyDto GetWhiskyById(int id);
    WhiskyDto GetWhiskyByName(string name);
    Task UpdateWhisky(WhiskyDto whisky);
    Task DeleteWhisky(int id);
  }
}