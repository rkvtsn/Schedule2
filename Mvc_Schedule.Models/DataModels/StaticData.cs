using System.Collections.Generic;

namespace Mvc_Schedule.Models.DataModels
{
	public static class StaticData
	{
		public const string AdminRoleName = "Admin";

		public static readonly IDictionary<int, string> WeekdaysConst = new Dictionary<int, string>
		                                                                	{
		                                                                		{ 1, "Понедельник" }, 
																				{ 2, "Вторник" },
																				{ 3, "Среда" },
																				{ 4, "Четверг" }, 
																				{ 5, "Пятница" }, 
																				{ 6, "Суббота" }, 
																				{ 7, "Восскресение" }
		                                                                	};
	}
}
