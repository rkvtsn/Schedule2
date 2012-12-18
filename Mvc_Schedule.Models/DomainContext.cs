using System;
using Mvc_Schedule.Models.DataModels;
using Mvc_Schedule.Models.DataModels.Repositories;

namespace Mvc_Schedule.Models
{
	public class DomainContext : IDisposable
	{
		private readonly ConnectionContext _ctx;

		public DomainContext()
		{
			_ctx = new ConnectionContext();
		}

		public int SaveChanges() { return _ctx.SaveChanges(); }

		private RepositoryLessons _lessons;
		public RepositoryLessons Lessons
		{
			get { return _lessons ?? (_lessons = new RepositoryLessons(_ctx)); }
			set { _lessons = value; }
		}

		private RepositoryScheduleTable _schedule;
		public RepositoryScheduleTable Schedule
		{
			get { return _schedule ?? (_schedule = new RepositoryScheduleTable(_ctx)); }
			set { _schedule = value; }
		}

		private RepositorySubjects _subjects;
		public RepositorySubjects Subjects
		{
			get { return _subjects ?? (_subjects = new RepositorySubjects(_ctx)); }
			set { _subjects = value; }
		}

		private RepositoryFacults _facults;
		public RepositoryFacults Facults
		{
			get { return _facults ?? (_facults = new RepositoryFacults(_ctx)); }
			set { _facults = value; }
		}

		private RepositoryGroups _groups;
		public RepositoryGroups Groups
		{
			get { return _groups ?? (_groups = new RepositoryGroups(_ctx)); }
			set { _groups = value; }
		}

		private RepositoryWeekdays _weekdays;
		public RepositoryWeekdays Weekdays
		{
			get { return _weekdays ?? (_weekdays = new RepositoryWeekdays(_ctx)); }
			set { _weekdays = value; }
		}

		#region Implementation of IDisposable

		private bool _disposed = false;
		public void Dispose()
		{
			if (!_disposed)
			{
				_ctx.Dispose();
				_disposed = true;
			}
		}

		#endregion
	}
}
