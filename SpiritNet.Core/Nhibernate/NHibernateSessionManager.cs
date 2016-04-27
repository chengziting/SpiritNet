using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SpiritNet.Utility;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SpiritNet.Core.Nhibernate
{
    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public sealed class NHibernateSessionManager
    {

        public static string ConfigFile
        {
            get;
            set;
        }
        #region Thread-safe, lazy Singleton
        /// <summary>
        /// 当前的Nh管理层
        /// </summary>
        public static NHibernateSessionManager CurrentInstance { get; set; }


        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static NHibernateSessionManager Instance
        {
            get
            {
                if (CurrentInstance == null)
                {
                    //NHibernateSessionManager nHibernateSessionManagertest = new NHibernateSessionManager();
                    return Nested.nHibernateSessionManager;
                }
                else
                    return CurrentInstance;
            }
        }

        /// <summary>
        /// Initializes the NHibernate session factory upon instantiation.
        /// </summary>
        private NHibernateSessionManager()
        {
            InitSessionFactory();
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }

            internal static readonly NHibernateSessionManager nHibernateSessionManager = new NHibernateSessionManager();
        }

        #endregion
        /// <summary>
        /// 初始化session工程
        /// </summary>
        private void InitSessionFactory()
        {
            NHibernate.Cfg.Configuration sourceConfig
                = new NHibernate.Cfg.Configuration();


            Configuration config = null;
            if (string.IsNullOrWhiteSpace(ConfigFile))
            {
                config = sourceConfig.Configure();
            }
            else
            {
                config = sourceConfig.Configure(ConfigFile);
            }

            var fluentlyConfig = Fluently.Configure(config)
                .ExposeConfiguration(AddAuditor)
                .Mappings(m =>
                {

                    CommonFunctions.LoadAssembly(
                        assembly =>
                        {
                            m.HbmMappings.AddFromAssembly(assembly);
                        },
                         "*.xml.dll");
                    CommonFunctions.LoadAssembly(
                       assembly =>
                       {
                           m.FluentMappings.AddFromAssembly(assembly);

                       },
                        "*.Model.dll");
                    // m.MergeMappings();


                }
                )
               // .Mappings(m => m.FluentMappings.Add<AuditLogMasterMap>())
               // .Mappings(m => m.FluentMappings.Add<AuditLogDetailMap>())
                ;

            this.sessionFactory = fluentlyConfig.BuildSessionFactory();
        }

        /// <summary>
        /// 初始化session工程
        /// </summary>
        public static ISessionFactory ExposeConfiguration(
            Action<MappingConfiguration> mappings,
            Action<Configuration> config)
        {

            NHibernate.Cfg.Configuration sourceConfig = new NHibernate.Cfg.Configuration();

            return
                Fluently.Configure(sourceConfig.Configure())
                .Mappings(mappings)
                .ExposeConfiguration(config)
                .ExposeConfiguration(AddAuditor)
                .BuildSessionFactory();


        }

        private static void AddAuditor(Configuration config)
        {
            //config.EventListeners.FlushEventListeners = new IFlushEventListener[] { new FixedDefaultFlushEventListener() };
            //config.SetListener(NHibernate.Event.ListenerType.PostUpdate, new AuditUpdateListener());
            //config.SetListener(NHibernate.Event.ListenerType.PostInsert, new AuditInsertListener());
            //config.SetListener(NHibernate.Event.ListenerType.PostDelete, new AuditDeleteListener());
        }

        /// <summary>
        /// 初始化session工程
        /// </summary>
        public static ISessionFactory ExposeConfiguration(
            Action<MappingConfiguration> mappings,
            string fileName)
        {

            NHibernate.Cfg.Configuration sourceConfig = new NHibernate.Cfg.Configuration();

            return
                Fluently.Configure(sourceConfig.Configure())
                .Mappings(mappings)
                .ExposeConfiguration(config =>
                {


                    if (File.Exists(fileName))
                        File.Delete(fileName);


                    new SchemaExport(config).SetOutputFile(fileName).Create(true, false);
                })
                .ExposeConfiguration(AddAuditor)
                .BuildSessionFactory();


        }


        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptor(IInterceptor interceptor)
        {
            ISession session = threadSession;

            if (session != null && session.IsOpen)
            {
                throw new CacheException(
                    "You cannot register an interceptor once a session has already been opened"
                );
            }

            GetSession(interceptor);
        }
        /// <summary>
        /// 获取session
        /// </summary>
        /// <returns></returns>
        public ISession GetSession()
        {
            return GetSession(null);
        }

        public static void Evict(object obj)
        {
            NHibernateSessionManager.Instance
                .GetSession().Evict(obj);
        }
        public ISessionFactory Factory
        {
            get
            {
                return sessionFactory;
            }
        }
        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSession(IInterceptor interceptor)
        {
            ISession session = threadSession;

            if (session == null)
            {
                if (interceptor != null)
                {
                    session = sessionFactory.OpenSession(interceptor);
                    session.FlushMode = FlushMode.Auto;
                }
                else
                {
                    session = sessionFactory.OpenSession();
                    session.FlushMode = FlushMode.Auto;
                }
                threadSession = session;
            }

            return session;
        }
        /// <summary>
        /// 关闭session
        /// </summary>
        public void CloseSession()
        {
            ISession session = threadSession;
            threadSession = null;

            if (session != null && session.IsOpen)
            {
                session.Close();
            }
        }
        /// <summary>
        /// 开始事务
        /// </summary>
        public void BeginTransaction()
        {
            ITransaction transaction = threadTransaction;

            if (transaction == null)
            {
                transaction = GetSession().BeginTransaction();
                threadTransaction = transaction;
            }
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTransaction()
        {
            ITransaction transaction = threadTransaction;

            try
            {
                if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                {
                    transaction.Commit();
                    threadTransaction = null;
                }
            }
            catch (HibernateException ex)
            {
                RollbackTransaction();
                throw ex;
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            ITransaction transaction = threadTransaction;

            try
            {
                threadTransaction = null;

                if (transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack)
                {
                    transaction.Rollback();
                }
            }
            catch (HibernateException ex)
            {
                throw ex;
            }
            finally
            {
                CloseSession();
            }
        }
        /// <summary>
        /// 线程安全的事务
        /// </summary>
        private ITransaction threadTransaction
        {
            get
            {
                return (ITransaction)CallContext.GetData("THREAD_TRANSACTION");
            }
            set
            {
                CallContext.SetData("THREAD_TRANSACTION", value);
            }
        }
        /// <summary>
        /// 线程安全的session
        /// </summary>
        private ISession threadSession
        {
            get
            {
                return (ISession)CallContext.GetData("THREAD_SESSION");
            }
            set
            {
                CallContext.SetData("THREAD_SESSION", value);
            }
        }
        /// <summary>
        /// Nhibernate的session工程
        /// </summary>
        private ISessionFactory sessionFactory;

        /// <summary>
        /// 根据NHibernate配置节点，获得DbConnection
        /// </summary>
        /// <returns></returns>
        public static DbConnection CreateConnection()
        {
            NHibernate.Cfg.Configuration sourceConfig
               = new NHibernate.Cfg.Configuration();

            var configure = sourceConfig.Configure().Properties["connection.connection_string_name"];

            var providerName = System.Configuration.ConfigurationManager.ConnectionStrings[configure].ProviderName;
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings[configure].ConnectionString;

            var DbProviderFactory = DbProviderFactories.GetFactory(providerName);

            DbConnection connection = DbProviderFactory.CreateConnection();

            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}
