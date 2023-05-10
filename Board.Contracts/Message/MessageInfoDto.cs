using Board.Contracts.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Contracts.Message
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public class MessageInfoDto
    {
        /// <summary>
        /// Индификатор Сообщения
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Отправитель
        /// </summary>
        public UserDto Sender { get; set; }

        /// <summary>
        /// Получатель
        /// </summary>
        public UserDto Reciever { get; set; }

        /// <summary>
        /// Содержимое сообщения
        /// </summary>
        public string Containment { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public DateTime SendDate { get; set; }
    }
}
