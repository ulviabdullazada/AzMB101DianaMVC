using System.Collections;

namespace Diana.ViewModels.CommonVM
{
    public class PaginatonVM<T> where T : IEnumerable
    {
        public int TotalCount { get; }
        public int LastPage { get; }
        public int CurrentPage { get; }
        public bool HasPrev { get;  }
        public bool HasNext { get;  }
        public T Items { get; }
        public PaginatonVM(int totalCount, int currentPage, int lastPage, T items) 
        {
            if (currentPage <= 0)
            {
                throw new ArgumentException();
            }
            TotalCount = totalCount;
            CurrentPage = currentPage;
            LastPage = lastPage;
            Items = items;
            if (currentPage <= lastPage)
            {
                if (currentPage == 1)
                {
                    HasPrev = false;
                }
                else
                {
                    HasPrev = true;
                }
                if (currentPage == lastPage)
                {
                    HasNext = false;
                }
                else
                {
                    HasNext = true;
                }
            }
        }
    }
}
