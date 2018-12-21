using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedHelper.Services.Abstracts.Contracts
{
	public interface IServiceBase<TObject>
	{
		Task AddAsync(TObject item);
		Task DeleteAsync(TObject item);
		Task<TObject> FindAsync(string id);
		Task UpdateAsync(TObject item);
		Task<List<TObject>> GetAll();
		IQueryable<TObject> GetFiltered(Expression<Func<TObject, bool>> filter);
	}
}
