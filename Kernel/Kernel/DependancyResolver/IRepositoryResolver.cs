namespace Kernel.DependancyResolver
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IRepositoryResolver
	{
		object ResolveRepository(Type type);
	}
}
