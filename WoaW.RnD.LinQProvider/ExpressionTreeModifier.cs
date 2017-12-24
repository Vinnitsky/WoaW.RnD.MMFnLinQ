using System.Linq;
using System.Linq.Expressions;

namespace WoaW.RnD.LinQProvider
{
    internal class ExpressionTreeModifier : ExpressionVisitor
    {
        private IQueryable<FileSystemElement> fileSystemElements;
        public ExpressionTreeModifier(IQueryable<FileSystemElement> queryableElements)
        {
            this.fileSystemElements = queryableElements;
        }
        internal Expression CopyAndModify(Expression expression)
        {
            return this.Visit(expression);
        }
        protected override Expression VisitConstant(ConstantExpression c)
        {
            // Replace the constant FileSystemContext arg with the queryable fileSystemElements.
            if (c.Type == typeof(FileSystemContext))
            {
                return Expression.Constant(fileSystemElements);
            }
            else
            {
                return c;
            }

        }
    }
}