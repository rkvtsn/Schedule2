using System.ComponentModel.DataAnnotations;

namespace Mvc_Schedule.Models.DataModels.Entities
{
	public class ScheduleTable
	{
		[Key]
		public int ScheduleTableId { get; set; }
		
		[Required]
		[MaxLength(32)]
		[Display(Name = "Аудитория")]
		public string Auditory { get; set; }

		[MaxLength(127)]
		[Display(Name = "Преподаватель")]
		public string LectorName { get; set; }

		//Нечетная неделя
		[Display(Name = "Чётная неделя")]
		public bool IsWeekOdd { get; set; }

		[Display(Name = "Дисциплина")]
		public int SubjectId { get; set; }

		[Display(Name = "Звонок")]
		public int LessonId { get; set; }

		[Display(Name = "День недели")]
		public int WeekdayId { get; set; }

		[Display(Name = "Группа")]
		public int GroupId { get; set; }

		[ForeignKey("WeekdayId")]
		public virtual Weekday Weekday { get; set; }
		[ForeignKey("GroupId")]
		public virtual StudGroup StudGroup { get; set; }
		[ForeignKey("SubjectId")]
		public virtual Subject Subject { get; set; }
		[ForeignKey("LessonId")]
		public virtual Lesson Lesson { get; set; }
	}
}