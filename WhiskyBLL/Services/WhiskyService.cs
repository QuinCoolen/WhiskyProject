using WhiskyBLL.Dto;
using WhiskyBLL.Exceptions;
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
      if (whisky == null)
      {
        throw new InvalidWhiskyDataException("Whisky data is invalid.");
      }

      if (_whiskyRepository.GetWhiskyByName(whisky.Name) != null)
      {
        throw new WhiskyAlreadyExistsException("Whisky already exists.");
      }

      _whiskyRepository.CreateWhisky(whisky);
    }

    public List<WhiskyDto> GetWhiskys()
    {
      return _whiskyRepository.GetWhiskys();
    }

    public WhiskyDto GetWhiskyById(int id)
    {
      WhiskyDto whisky = _whiskyRepository.GetWhiskyById(id);

      if (whisky == null)
      {
        throw new NotFoundException("Whisky not found.");
      }

      return whisky;
    }

    public WhiskyDto GetWhiskyByName(string name)
    {
      WhiskyDto whisky = _whiskyRepository.GetWhiskyByName(name);

      if (whisky == null)
      {
        throw new NotFoundException("Whisky not found.");
      }

      return whisky;
    }

    public void UpdateWhisky(WhiskyDto whisky)
    {
      if (whisky == null)
      {
        throw new InvalidWhiskyDataException("Whisky data is invalid.");
      }

      if (_whiskyRepository.GetWhiskyById(whisky.Id) == null)
      {
        throw new NotFoundException("Whisky not found.");
      }

      _whiskyRepository.UpdateWhisky(whisky);
    }

    public void DeleteWhisky(int id)
    {
      if (_whiskyRepository.GetWhiskyById(id) == null)
      {
        throw new NotFoundException("Whisky not found.");
      }

      _whiskyRepository.DeleteWhisky(id);
    }
  }
}