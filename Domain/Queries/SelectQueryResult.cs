using System.Collections;

namespace ApiRest.Controllers
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
