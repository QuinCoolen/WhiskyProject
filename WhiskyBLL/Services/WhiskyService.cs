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

    public async Task UpdateWhisky(WhiskyDto whisky)
    {
      await _whiskyRepository.UpdateWhisky(whisky);
    }

    public async Task DeleteWhisky(int id)
    {
      await _whiskyRepository.DeleteWhisky(id);
    }
  }
}