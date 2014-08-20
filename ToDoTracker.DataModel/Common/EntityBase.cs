using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoTracker.DataModel.Common {
    //[DataContract]
    /// <summary>
    /// Абстрактный класс сущностей модели
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// Идентификатор объекта
        /// </summary>
        [Column("Id")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Пометка что запись удалена
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Дата создания записи
        /// </summary>

        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Дата последнего изменения
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Флаг историчности
        /// </summary>
        public bool IsHistoric { get { return HistoryDate != null; } }

        /// <summary>
        /// Дата историчности(если, запись помечена как историческая)
        /// </summary>
        public DateTime? HistoryDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is EntityBase)
            {
                var entityBase = (EntityBase)obj;
                return this.Id == entityBase.Id;
            }
            return false;
        }
    }
}
