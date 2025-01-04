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
      try
      {
        if (whisky == null)
        {
          throw new InvalidWhiskyDataException("Whisky data is invalid.");
        }

        if (_whiskyRepository.GetWhiskyByName(whisky.Name).Name != null)
        {
          throw new WhiskyAlreadyExistsException("Whisky already exists.");
        }

        _whiskyRepository.CreateWhisky(whisky);
      }
      catch (Exception ex)
      {
        throw new Exception("Error creating whisky", ex);
      }
    }

    public List<WhiskyDto> GetWhiskys()
    {
      try
      {
        return _whiskyRepository.GetWhiskys();
      }
      catch (Exception ex)
      {
        throw new Exception("Error retrieving whiskys", ex);
      }
    }

    public WhiskyDto GetWhiskyById(int id)
    {
      try
      {
        WhiskyDto whisky = _whiskyRepository.GetWhiskyById(id);

        if (whisky == null)
        {
          throw new NotFoundException("Whisky not found.");
        }

        return whisky;
      }
      catch (Exception ex)
      {
        throw new Exception("Error retrieving whisky", ex);
      }
    }

    public WhiskyDto GetWhiskyByName(string name)
    {
      try
      {
        WhiskyDto whisky = _whiskyRepository.GetWhiskyByName(name);

        if (whisky == null)
        {
          throw new NotFoundException("Whisky not found.");
        }

        return whisky;
      }
      catch (Exception ex)
      {
        throw new Exception("Error retrieving whisky", ex);
      }
    }

    public void UpdateWhisky(WhiskyDto whisky)
    {
      try
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
      catch (Exception ex)
      {
        throw new Exception("Error updating whisky", ex);
      }
    }

    public void DeleteWhisky(int id)
    {
      try
      {
        if (_whiskyRepository.GetWhiskyById(id) == null)
        {
          throw new NotFoundException("Whisky not found.");
        }

        _whiskyRepository.DeleteWhisky(id);
      }
      catch (Exception ex)
      {
        throw new Exception("Error deleting whisky", ex);
      }
    }
  }
}