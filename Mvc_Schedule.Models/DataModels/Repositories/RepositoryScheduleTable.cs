using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Security;
using Mvc_Schedule.Models.DataModels.Entities;
using Mvc_Schedule.Models.DataModels.ModelViews;

namespace Mvc_Schedule.Models.DataModels.Repositories
{
	public class RepositoryScheduleTable : RepositoryBase<ConnectionContext>
	{
		public RepositoryScheduleTable(ConnectionContext ctx) : base(ctx) { }

		public ScheduleTableIndex ListForIndex(int groupId, bool week)
		{
			var group = _ctx.StudGroups.Find(groupId);

			if (group == null) return null;
			var result = new ScheduleTableIndex
							{
								Group = group,
								IsWeekOdd = week,
								Schedule = List(groupId, week),
								Lessons = _ctx.Lessons.OrderBy(x => x.Time).ToList(),
								Weekdays = _ctx.Weekdays.OrderBy(x => x.WeekdayId).ToList(),
							};

			return result;
		}

		public ScheduleTableCreate ListForCreate(int groupId, bool week)
		{
			var group = _ctx.StudGroups.Find(groupId);
			if (group == null || (!Roles.IsUserInRole(group.FacultId.ToString(CultureInfo.InvariantCulture)) && !Roles.IsUserInRole("Admin")))
				return null;

			var result = new ScheduleTableCreate
							{
								GroupId = group.GroupId,
								GroupName = group.Name,
								IsWeekOdd = week,
								Lessons = _ctx.Lessons.OrderBy(x => x.Time).ToList(),
								Weekdays = _ctx.Weekdays.OrderBy(x => x.WeekdayId).ToList(),
								ScheduleTableRows = List(groupId, week)
							};

			return result;
		}

		public IList<ScheduleTable> List(int groupId, bool week)
		{
			return _ctx.ScheduleTables.Where(x => x.GroupId == groupId && week == x.IsWeekOdd).ToList();
		}


		// алгоритм добавления расписания с уникальникализацией дисциплин
		//public void ListAdd(ScheduleTableCreate table)
		//{
		//    // Аутентификация
		//    var group = _ctx.StudGroups.Find(table.GroupId);
		//    if (group == null || (!Roles.IsUserInRole(group.FacultId.ToString(CultureInfo.InvariantCulture)) && !Roles.IsUserInRole("Admin")))
		//        return;
			
		//    // Старое расписание (увы без него нельзя удалить [LINQ2SQL])
		//    var list = List(table.GroupId, table.IsOddWeek);

		//    // Очищаем расписание на неделю
		//    foreach (var row in list)
		//    {
		//        _ctx.ScheduleTables.Remove(row);
		//    }

		//    // Если есть новые записи, то Добавляем их
		//    if (table.ScheduleTableRows != null)
		//    {
		//        // Очередь дисциплин
		//        var subjectsQueue = new Dictionary<string, Subject>();

		//        // Блок "уникализации" дисциплин
		//        foreach (var subjectName in table.ScheduleTableRows.Select(x => x.Subject.Name.Trim()).Distinct())
		//        {
		//            subjectsQueue[subjectName] = _ctx.Subjects.SingleOrDefault(x => x.Name == subjectName);
		//            if (subjectsQueue[subjectName] == null)
		//            {
		//                subjectsQueue[subjectName] = new Subject { Name = subjectName };
		//                _ctx.Subjects.Add(subjectsQueue[subjectName]);
		//            }
		//        }

		//        // Добавляем новые дисциплины
		//        _ctx.SaveChanges();

		//        // Теперь можно добавить информацию о расписнии в таблицу
		//        foreach (var row in table.ScheduleTableRows)
		//        {
		//            row.Subject = subjectsQueue[row.Subject.Name.Trim()];
		//            _ctx.ScheduleTables.Add(row);
		//        }
		//    }
		//}

		public ScheduleTableCreate FormToTable(FormCollection scheduleRows, out bool isValid)
		{
			var result = new ScheduleTableCreate
			{
				IsWeekOdd = bool.Parse(scheduleRows[2]),
				GroupId = int.Parse(scheduleRows[1]),
				ScheduleTableRows = new List<ScheduleTable>()
			};

			isValid = true;

			for (var i = 3; i < scheduleRows.Count; i++)
			{
				if (scheduleRows.GetKey(i).EndsWith("ScheduleTableId")) i++;

				var item = new ScheduleTable
				{
					Auditory = scheduleRows[i++].Trim(),
					SubjectName = scheduleRows[i++].Trim(),
					LectorName = scheduleRows[i++].Trim(),
					LessonId = int.Parse(scheduleRows[i++]),
					GroupId = int.Parse(scheduleRows[i++]),
					WeekdayId = int.Parse(scheduleRows[i]),
				};

				if (item.Auditory.Trim() == string.Empty || item.SubjectName.Trim() == string.Empty)
					isValid = false;

				result.ScheduleTableRows.Add(item);
			}

			return result;
		}

		public void ListAdd(ScheduleTableCreate table)
		{
			var group = _ctx.StudGroups.Find(table.GroupId);
			if (group == null || (!Roles.IsUserInRole(group.FacultId.ToString(CultureInfo.InvariantCulture)) && !Roles.IsUserInRole("Admin")))
				return;
			foreach (var row in List(table.GroupId, table.IsWeekOdd)) _ctx.ScheduleTables.Remove(row);
			if (table.ScheduleTableRows != null)
				foreach (var row in table.ScheduleTableRows)
					_ctx.ScheduleTables.Add(row);
		}

		public string[] ListSubjects(string firstLetter)
		{
			return (from x in _ctx.ScheduleTables 
					orderby x.SubjectName 
					where x.SubjectName.ToLower().StartsWith(firstLetter.ToLower()) 
					select x.SubjectName).Distinct().ToArray();
		}
	}
}
