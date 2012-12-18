using System.Collections.Generic;
using System.Data.Entity;
using Mvc_Schedule.Models.DataModels.Entities;

namespace Mvc_Schedule.Models.DataModels
{
	public sealed class ConnectionContext : DbContext
	{
		public DbSet<ScheduleTable> ScheduleTables { get; set; }

		public DbSet<Subject> Subjects { get; set; }

		public DbSet<Lesson> Lessons { get; set; }

		public DbSet<StudGroup> StudGroups { get; set; }

		public DbSet<Facult> Facults { get; set; }

		public DbSet<Weekday> Weekdays { get; set; }
	}

	public class DbInitializer : DropCreateDatabaseIfModelChanges<ConnectionContext>
	{
		protected override void Seed(ConnectionContext context)
		{
			new List<string> { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" }
				.ForEach(x => context.Weekdays.Add(new Weekday {Name = x}));

			base.Seed(context);
		}
	}

	public class DbReCreate : DropCreateDatabaseAlways<ConnectionContext>
	{
		protected override void Seed(ConnectionContext context)
		{
			new List<string> { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" }
				.ForEach(x => context.Weekdays.Add(new Weekday { Name = x }));

			base.Seed(context);
		}
	}

	public class DbStartUp : CreateDatabaseIfNotExists<ConnectionContext>
	{
		protected override void Seed(ConnectionContext context)
		{
			new List<string> { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" }
				.ForEach(x => context.Weekdays.Add(new Weekday { Name = x }));

			base.Seed(context);
		}
	}
}
