using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Text;
using System.Web.Mvc;
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
			var result = new ScheduleTableIndex
							{
								Group = group,
								IsOddWeek = week,
							};
			if (group != null)
			{
				result.Schedule = List(groupId, week);
				result.Lessons = _ctx.Lessons.OrderBy(x => x.Time).ToList();
				result.Weekdays = _ctx.Weekdays.OrderBy(x => x.WeekdayId).ToList();
			}
			return result;
		}

		public ScheduleTableCreate ListForCreate(int groupId, bool week)
		{
			var group = _ctx.StudGroups.Find(groupId);
			if (group == null) return null;

			var result = new ScheduleTableCreate
							{
								GroupId = group.GroupId,
								GroupName = group.Name,
								IsOddWeek = week,
								Lessons = _ctx.Lessons.OrderBy(x => x.Time).ToList(),
								Weekdays = _ctx.Weekdays.OrderBy(x => x.WeekdayId).ToList(),
								ScheduleTableRows = List(groupId, week)
							};

			return result;
		}

		public IList<ScheduleTable> List(int groupId, bool week)
		{
			return _ctx.ScheduleTables.Include(x => x.Subject)
									  .Where(x => x.GroupId == groupId && week == x.IsWeekOdd).ToList();
		}

		public void ListAdd(ScheduleTableCreate table)
		{
			// Старое расписание (увы без него нельзя удалить [LINQ2SQL])
			var list = List(table.GroupId, table.IsOddWeek);

			// Очищаем расписание на неделю
			foreach (var row in list)
			{
				_ctx.ScheduleTables.Remove(row);
			}

			// Если есть новые записи, то Добавляем их
			if (table.ScheduleTableRows != null)
			{
				// Очередь дисциплин
				var subjectsQueue = new Dictionary<string, Subject>();

				// Блок "уникализации" дисциплин
				foreach (var subjectName in table.ScheduleTableRows.Select(x => x.Subject.Name.Trim()).Distinct())
				{
					subjectsQueue[subjectName] = _ctx.Subjects.SingleOrDefault(x => x.Name == subjectName);
					if (subjectsQueue[subjectName] == null)
					{
						subjectsQueue[subjectName] = new Subject { Name = subjectName };
						_ctx.Subjects.Add(subjectsQueue[subjectName]);
					}
				}
				
				// Добавляем новые дисциплины
				_ctx.SaveChanges();
				
				// Теперь можно добавить информацию о расписнии в таблицу
				foreach (var row in table.ScheduleTableRows)
				{
					row.Subject = subjectsQueue[row.Subject.Name.Trim()];
					_ctx.ScheduleTables.Add(row);
				}
			}
		}

		/// <summary>
		/// Метод "псевдосериализации" массива строк
		/// Не абстрактный, Высокий уровень зависимостей! 
		/// -> необходимо модифицировать при изменении во вне
		/// </summary>
		/// <param name="scheduleRows">Массив строковых из формы Представления</param>
		/// <param name="isValid">Параметр валидности данных</param>
		/// <returns>Instance of ScheduleTableCreate for Adding It in DB</returns>
		public ScheduleTableCreate FormToTable(FormCollection scheduleRows, out bool isValid)
		{
			var result = new ScheduleTableCreate
			{
			    IsOddWeek = bool.Parse(scheduleRows[2]),
			    GroupId = int.Parse(scheduleRows[1]),
			    ScheduleTableRows = new List<ScheduleTable>()
			};
			
			isValid = true;

			for (var i = 3; i < scheduleRows.Count; i++)
			{
				if (scheduleRows.GetKey(i).EndsWith("ScheduleTableId")) i++;
								
				var item = new ScheduleTable
				{
					Auditory = scheduleRows[i++],
					Subject = new Subject { Name = scheduleRows[i++] },
					LectorName = scheduleRows[i++],
					LessonId = int.Parse(scheduleRows[i++]),
					GroupId = int.Parse(scheduleRows[i++]),
					WeekdayId = int.Parse(scheduleRows[i]),
				};

				if (item.Auditory.Trim() == string.Empty || item.Subject.Name.Trim() == string.Empty)
					isValid = false;
				
				result.ScheduleTableRows.Add(item);
			}

			return result;
		}

	}
}
