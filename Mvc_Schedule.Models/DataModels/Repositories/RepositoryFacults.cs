using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Mvc_Schedule.Models.DataModels.Entities;
using Mvc_Schedule.Models.DataModels.ModelViews;

namespace Mvc_Schedule.Models.DataModels.Repositories
{
	public class RepositoryFacults : RepositoryBase<ConnectionContext>
	{
		public RepositoryFacults(ConnectionContext ctx) : base(ctx) { }
		
		public Facult Get(int id)
		{
			return _ctx.Facults.Find(id);
		}

		public IList<Facult> List()
		{
			return _ctx.Facults
					.Include(x => x.StudGroups)
					.OrderBy(x => x.Name)
					.ToList();
		}

		public IList<Facult> ListNames()
		{
			return _ctx.Facults
					.OrderBy(x => x.Name)
					.ToList();
		}

		public void Add(FacultCreate facult)
		{
			_ctx.Facults.Add(facult.FacultInstance);
			if (facult.StudGroupsNames != null)
				foreach (var group in facult.StudGroupsNames)
					_ctx.StudGroups.Add(group);
		}

		public void Edit(Facult facult)
		{
			var old = Get(facult.FacultId);
			old.Name = facult.Name;
		}

		public void Delete(int id)
		{
			_ctx.Facults.Remove(Get(id));
		}
	}
}