using NHibernate;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Impl;
using NHibernate.Loader.Criteria;
using NHibernate.Persister.Entity;
using SpiritNet.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Nhibernate
{
    public static class NhibernateHelper
    {

        public static bool IsEmpty(ICriterion criterion)
        {
            return criterion.ToString() == "()";
        }


        public static string GetCritionSql(Type type, Action<ICriteria> addCriteria)
        {
            var session = NHibernateSessionManager.Instance.GetSession();
            ICriteria crit =
                session.CreateCriteria(type);
            addCriteria(crit);
            string resultString = GetCritionSql(crit);

            return resultString;

        }

        public static String GetCritionSql(NHibernate.ICriteria criteria)
        {
            var criteriaImpl = (CriteriaImpl)criteria;
            var sessionImpl = (SessionImpl)criteriaImpl.Session;
            var factory = (SessionFactoryImpl)sessionImpl.SessionFactory;
            var implementors = factory.GetImplementors(criteriaImpl.EntityOrClassName);
            var loader = new CriteriaLoader((IOuterJoinLoadable)factory.GetEntityPersister(implementors[0]), factory, criteriaImpl, implementors[0], sessionImpl.EnabledFilters);

            var originalSql = loader.SqlString.ToString();
            if (loader.Translator.CollectedParameters.Count > 0)
            {
                foreach (var param in loader.Translator.CollectedParameters)
                {
                    int paramIndex = originalSql.IndexOf("?");
                    if (param.Type.GetType().Name == "StringType"
                        || param.Type.GetType().Name == "DateTimeType"
                        || param.Type.GetType().Name == "DateTime2Type"
                        || param.Type.GetType().Name == "GuidType")
                    {
                        originalSql = originalSql.Substring(0, paramIndex) + "'" + param.Value + "'" + originalSql.Substring(paramIndex + 1);
                    }
                    else
                    {
                        originalSql = originalSql.Substring(0, paramIndex) + param.Value + originalSql.Substring(paramIndex + 1);
                    }
                }
            }
            var fromIndex = 0;
            var lastCommaIndex = 0;
            while (true)
            {
                fromIndex = originalSql.IndexOf("from", lastCommaIndex, StringComparison.OrdinalIgnoreCase);
                var asIndex = originalSql.IndexOf(" as ", lastCommaIndex, StringComparison.OrdinalIgnoreCase);

                if (asIndex < 0 || asIndex > fromIndex)
                {
                    break;
                }
                else
                {
                    lastCommaIndex = originalSql.IndexOf(",", lastCommaIndex + 1, StringComparison.OrdinalIgnoreCase);
                    if (lastCommaIndex >= 0 && lastCommaIndex < fromIndex)
                    {
                        originalSql = originalSql.Remove(asIndex, lastCommaIndex - asIndex);
                        lastCommaIndex = asIndex;
                    }
                    else
                    {
                        originalSql = originalSql.Remove(asIndex + 1, fromIndex - asIndex - 1);
                        break;
                    }
                }
            }
            return originalSql;
        }

        public static String GetGeneratedSql(IQuery query, ISession session)
        {
            var sessionImp = (ISessionImplementor)session;
            var translatorFactory = new ASTQueryTranslatorFactory();
            var translators = translatorFactory.CreateQueryTranslators(query.QueryString, null, false, sessionImp.EnabledFilters, sessionImp.Factory);
            return translators[0].SQLString;
        }
    }
}
