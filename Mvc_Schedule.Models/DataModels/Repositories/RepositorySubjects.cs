using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Mvc_Schedule.Models.DataModels.Entities;

namespace Mvc_Schedule.Models.DataModels.Repositories
{
	public class RepositorySubjects : RepositoryBase<ConnectionContext>
	{
		public RepositorySubjects(ConnectionContext ctx) : base(ctx) { }

		public List<string> List(string start)
		{
			//return (from x in _ctx.Subjects orderby x.Name where x.Name.StartsWith(start) select x.Name).ToList();
			return (from x in _ctx.Subjects orderby x.Name where x.Name.ToLower().StartsWith(start.ToLower()) select x.Name).ToList();
		}

		public List<Subject> List()
		{
			return (from x in _ctx.Subjects orderby x.Name select x).ToList();
		}

		public Subject[] Array()
		{
			return (from x in _ctx.Subjects orderby x.Name select x).ToArray();
		}
		
		public Subject Get(int id)
		{
			return _ctx.Subjects.Find(id);
		}

		public void AddList(IList<Subject> subjects)
		{
			foreach (var subject in subjects)
				Add(subject);
		}

		public void Add(Subject subject)
		{
			_ctx.Subjects.Add(subject);
		}

		public void Edit(Subject subject)
		{
			var old = Get(subject.SubjectId);
			old.Name = subject.Name;
		}

		public void Delete(int id)
		{
			_ctx.Subjects.Remove(Get(id));
		}

	}
}
