using WhiskyBLL.Dto;

namespace WhiskyBLL.Interfaces
{
    public interface IFavouriteRepository
    {
        void AddFavourite(FavouriteDto favourite);
        List<FavouriteDto> GetFavouritesByUserId(int userId);
        void RemoveFavourite(int userId, int whiskyId);
    }
} 