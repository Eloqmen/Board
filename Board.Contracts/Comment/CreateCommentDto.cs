﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Board.Contracts.Comment
{
    /// <summary>
    /// Создание отзыва
    /// </summary>
    public class CreateCommentDto
    {
        /// <summary>
        /// Индификатор Пользователя оставившего отзыв
        /// </summary>
        [Required]
        public Guid SenderId { get; set; }

        /// <summary>
        /// Индификатор Пользователя которому оставили отзыв
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Содержимое отзыва
        /// </summary>
        [MinLength(1)]
        [Required]
        public string Text { get; set; }
    }
}
