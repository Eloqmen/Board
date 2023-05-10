using Board.Contracts.Advert;
using Board.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Contracts.FavoriteAdvert
{
    /// <summary>
    /// Избранные объявление
    /// </summary>
    public class FavoriteAdvertInfoDto
    {
        /// <summary>
        /// Id Избранного объявления
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// ID Пользователя добавившего объявление в избранные
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Пользователь добавивший объявление в избранные
        /// </summary>
        public UserDto User { get; set; }

        /// <summary>
        /// ID Объявления
        /// </summary>
        public Guid AdId { set; get; }

        /// <summary>
        /// Объявление
        /// </summary>
        public AdvertShortInfoDto Advert { get; set; }
    }
}
