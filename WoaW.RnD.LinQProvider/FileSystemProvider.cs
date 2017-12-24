using System.Linq;
using System.Linq.Expressions;

namespace WoaW.RnD.LinQProvider
{
    public class FileSystemProvider : IQueryProvider
    {
        private readonly string root;

        public FileSystemProvider(string root)
        {
            this.root = root;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new FileSystemContext(this, expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return (IQueryable<TElement>)new FileSystemContext(this, expression);
        }
        
        public object Execute(Expression expression)
        {
            return Execute<FileSystemElement>(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            var isEnumerable = (typeof(TResult).Name == "IEnumerable`1");
            return (TResult)FileSystemQueryContext.Execute(expression, isEnumerable, root);
        }
    }
}
