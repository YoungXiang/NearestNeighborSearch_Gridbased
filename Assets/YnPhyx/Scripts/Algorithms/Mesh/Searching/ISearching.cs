using System.Collections.Generic;

namespace YnPhyx
{
	public interface ISearching
	{
        /// searching every match for searchingFor in searchingIn
        List<int> Search(BaseGroupable searchingIn, int data, int dataGroupId);
	}
}
