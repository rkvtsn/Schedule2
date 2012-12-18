using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mvc_Schedule.Models.DataModels.Entities;

namespace Mvc_Schedule.Models.DataModels.ModelViews
{
	public class SubjectIndex
	{
		public Subject NewSubject { get; set; }

		public IList<Subject> ListSubjects { get; set; }
	}
}
