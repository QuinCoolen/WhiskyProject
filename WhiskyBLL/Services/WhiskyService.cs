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

    public void CreateWhisky(WhiskyDTO whisky)
    {
      _whiskyRepository.CreateWhisky(whisky);
    }

    public List<WhiskyDTO> GetWhiskys()
    {
      return _whiskyRepository.GetWhiskys();
    }

    public WhiskyDTO GetWhiskyById(int id)
    {
      return _whiskyRepository.GetWhiskyById(id);
    }
  }
}