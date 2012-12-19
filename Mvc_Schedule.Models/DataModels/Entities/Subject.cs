using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Schedule.Models.DataModels.Entities
{
	public class Subject
	{
		[Key]
		public int SubjectId { get; set; }

		[Required(ErrorMessage = "���� ���������� ���������")]
		[MaxLength(128, ErrorMessage = "������������ ����� �������� {0}")]
		[Display(Name = "�������� ����������")]
		public string Name { get; set; }

		public virtual ICollection<ScheduleTable> ScheduleTable { get; set; }
	}
}