using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Schedule.Models.DataModels.Entities
{
	public class Subject
	{
		[Key]
		public int SubjectId { get; set; }

		[Required]
		[MaxLength(128, ErrorMessage = "Максимальное число символов 128")]
		[Display(Name = "Название дисциплины")]
		public string Name { get; set; }

		public virtual ICollection<ScheduleTable> ScheduleTable { get; set; }
	}
}