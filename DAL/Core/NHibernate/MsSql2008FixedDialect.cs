using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Dialect;
using NHibernate.SqlCommand;

namespace Jumbleblocks.nHibernate
{
    public class MsSql2008FixedDialect : MsSql2008Dialect
    {
        public override SqlString GetLimitString(SqlString queryString, SqlString offset, SqlString limit)
        {
            SqlStringBuilder result = new SqlStringBuilder();

            if (offset == null)
            {
                int insertPoint = GetAfterSelectInsertPoint(queryString);

                return result
                    .Add(queryString.Substring(0, insertPoint))
                    .Add(" TOP (")
                    .Add(limit)
                    .Add(") ")
                    .Add(queryString.Substring(insertPoint))
                    .ToSqlString();
            }

            return base.GetLimitString(queryString, offset, limit);
        }

        private int GetAfterSelectInsertPoint(SqlString sql)
        {
            const string criteriaComment = "/* criteria query */ ";

            if (sql.StartsWithCaseInsensitive(criteriaComment))
                return GetAfterSelectInsertPoint(criteriaComment.Length, sql.Replace(criteriaComment, String.Empty));
            else
                return GetAfterSelectInsertPoint(0, sql);
        }

        private int GetAfterSelectInsertPoint(int prependCount, SqlString sql)
        {
            if (sql.StartsWithCaseInsensitive("select distinct"))
            {
                return prependCount + 15;
            }
            else if (sql.StartsWithCaseInsensitive("select"))
            {
                return prependCount + 6;
            }
            throw new NotSupportedException("The query should start with 'SELECT' or 'SELECT DISTINCT'");
        }
    }
}
