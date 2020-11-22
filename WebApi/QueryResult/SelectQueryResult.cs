using System.Collections;

namespace WebApi.QueryResult
{
    public class SelectQueryResult
    {
        public SelectQueryResult()
        {
            Total = null;
        }
        public int? Total { get; set; }
        public IEnumerable Itens { get; set; }
    }
}
