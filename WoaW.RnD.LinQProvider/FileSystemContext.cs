using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WoaW.RnD.LinQProvider
{
    public class FileSystemContext : IOrderedQueryable<FileSystemElement>
    {
        public FileSystemContext(string root)
        {
            Provider = new FileSystemProvider(root);
            Expression = Expression.Constant(this);
        }

        internal FileSystemContext(IQueryProvider provider, Expression expression)
        {
            Provider = provider;
            Expression = expression;
        }

        public IEnumerator<FileSystemElement> GetEnumerator()
        {
            return Provider.Execute<IEnumerable<FileSystemElement>>(Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Type ElementType
        {
            get { return typeof(FileSystemElement); }
        }

        public Expression Expression { get; private set; }
        public IQueryProvider Provider { get; private set; }
    }
}
