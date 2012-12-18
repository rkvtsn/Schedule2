using System;
using System.ComponentModel.DataAnnotations;

namespace Mvc_Schedule.Models.DataModels.ModelViews
{
	
	public class LessonsCreate
	{
		[Display(Name = "Время урока")]
		public LessonsTime[] Lessons { get; set; }
	}

	public class LessonsTime
	{
		[Required]
		[Range(0, 59)]
		[Display(Name = "Минуты")]
		public short Minutes { get; set; }

		[Required]
		[Range(0, 23)]
		[Display(Name = "Часы")]
		public short Hours { get; set; }

		public DateTime Time { get { return new DateTime(2012, 01, 01, Hours, Minutes, 0); } }
	}

}
