using WhiskyBLL.Interfaces;

namespace WhiskyBLL
{
  public class WhiskyService
  {
    private readonly IWhiskyRepository _whiskyRepository;

    public WhiskyService(IWhiskyRepository whiskyRepository)
    {
      _whiskyRepository = whiskyRepository;
    }

    public void CreateWhisky(WhiskyDto whisky)
    {
      _whiskyRepository.CreateWhisky(whisky);
    }

    public List<WhiskyDto> GetWhiskys()
    {
      return _whiskyRepository.GetWhiskys();
    }

    public WhiskyDto GetWhiskyById(int id)
    {
      return _whiskyRepository.GetWhiskyById(id);
    }

    public void UpdateWhisky(WhiskyDto whisky)
    {
      _whiskyRepository.UpdateWhisky(whisky);
    }

    public void DeleteWhisky(int id)
    {
      _whiskyRepository.DeleteWhisky(id);
    }
  }
}