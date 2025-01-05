using WhiskyBLL.Dto;
using WhiskyBLL.Interfaces;

namespace WhiskyBLL.Services
{
    public class FavouriteService
    {
        private readonly IFavouriteRepository _favouriteRepository;

        public FavouriteService(IFavouriteRepository favouriteRepository)
        {
            _favouriteRepository = favouriteRepository;
        }

        public void AddFavourite(FavouriteDto favourite)
        {
            _favouriteRepository.AddFavourite(favourite);
        }

        public List<FavouriteDto> GetFavouritesByUserId(int userId)
        {
            return _favouriteRepository.GetFavouritesByUserId(userId);
        }

        public bool IsWhiskyInFavourites(int userId, int whiskyId)
        {
            var favourites = GetFavouritesByUserId(userId);
            return favourites.Any(f => f.WhiskyId == whiskyId);
        }

        public void RemoveFavourite(int userId, int whiskyId)
        {
            _favouriteRepository.RemoveFavourite(userId, whiskyId);
        }
    }
} 