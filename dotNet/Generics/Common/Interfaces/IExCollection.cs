using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IExCollection<T>: IExEnumerable<T>, IEnumerable
	{
		int Count
		{
			get;
		}

		bool IsReadOnly
		{
			get;
		}

		void Add(T item);

		void Clear();

		bool Contains(T item);

		void CopyTo(T[] array, int arrayIndex);

		bool Remove(T item);
	}
}
