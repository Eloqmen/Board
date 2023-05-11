using Board.Contracts.Advert;
using Board.Contracts.Comment;
using Board.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Application.AppData.Contexts.Comments.Repositories
{
    /// <summary>
    /// Репозиторий для работы с коментариями.
    /// </summary>
    public interface ICommentRepository
    {
        /// <summary>
        /// Получить список коментариев.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Список коментариев.</returns>
        Task<CommentShortInfo[]> GetAll(CancellationToken cancellationToken);

        /// <summary>
        /// Получить коментарии по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор коментария.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns>Модель коментариея.</returns>
        Task<CommentInfoDto> Get(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавить коментарии.
        /// </summary>
        /// <param name="entity">Коментарий.</param>
        /// <param name="cancellation">Токен отмены операции.</param>
        /// <returns>Модель добавленного коментария.</returns>
        Task<CreateCommentDto> Add(Advert entity, CancellationToken cancellation);

        /// <summary>
        /// Удалить коментарии.
        /// </summary>
        /// <param name="id">Идентификатор коментария.</param>
        /// <param name="cancellationToken">Токен отмены операции.</param>
        /// <returns></returns>
        Task Delete(Guid id, CancellationToken cancellationToken);
    }
}
