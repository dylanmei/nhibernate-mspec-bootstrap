using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Data;
using StructureMap;
using Mono.Data.Sqlite;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;
using NHibernate.Tool.hbm2ddl;
using FluentNHibernate.Cfg;

namespace Specs {

  public class SpecDb {
    const string DBFILE = "specs.db";

    public static void Setup() {
      var sessionFactory = SpecDb.Configure()
        .BuildSessionFactory();
      ObjectFactory.Configure(config => {
        config.For<ISessionFactory>().Use(sessionFactory);
        config.For<ISession>().Use(c => GetOrOpenSession(sessionFactory));
      });      
    }

    static FluentConfiguration Configure() {
      var filePath = GetPathToDatabase(DBFILE);
      return Fluently.Configure()
        .Database(new SpecDbConfiguration().UsingFile(filePath))
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Library.Data.AlbumMap>()
                        .Conventions.AddFromAssemblyOf<Library.Data.AlbumMap>())
        .ExposeConfiguration(SetupNHibernateConfiguration);
    }

    static ISession GetOrOpenSession(ISessionFactory sessionFactory) {
      if (NHibernate.Context.CurrentSessionContext.HasBind(sessionFactory))
          return sessionFactory.GetCurrentSession();

      var session = sessionFactory.OpenSession();
      NHibernate.Context.CurrentSessionContext.Bind(session);
      return session;
    }

    static void SetupNHibernateConfiguration(Configuration config) {
      Destroy();
      config.SetProperty(NHibernate.Cfg.Environment
        .CurrentSessionContextClass, "thread_static");

      // Create(true if the ddl should be outputted in the Console,
      //        true if the ddl should be executed against the Database)
      new SchemaExport(config).Create(false, true);
    }

    static string GetPathToDatabase(string databasePath) {
      if (!Path.IsPathRooted(databasePath)) {
        var rootedPath = AppDomain.CurrentDomain.BaseDirectory;
        var directory = Path.GetDirectoryName(rootedPath);

        if (directory.EndsWith("bin"))
          directory = Path.GetDirectoryName(directory);

        databasePath = Path.Combine(directory, databasePath);
      }

      return databasePath;
    }

    public static void Teardown() {
      var session = ObjectFactory.GetInstance<ISession>();
      if (session != null) {
        session.Dispose();
      }

      var sessionFactory = ObjectFactory.GetInstance<ISessionFactory>();
      if (sessionFactory != null) {
        sessionFactory.Dispose();
      }
    }

    public static void Clean() {
      var session = ObjectFactory.GetInstance<ISession>();
      if (session != null)
        session.Clear();

      var filePath = GetPathToDatabase(DBFILE);
      using (IDbConnection connection = new SqliteConnection("URI=file:" + filePath)) {
        connection.Open();
        foreach (var table in GetTableNames(connection)) {
          var command = connection.CreateCommand();
          command.CommandText = "delete from " + table;
          command.ExecuteNonQuery();          
        }
      }
    }

    public static void Destroy() {
      var filePath = GetPathToDatabase(DBFILE);
      if (File.Exists(filePath))
        File.Delete(filePath);
    }

    static IEnumerable<string> GetTableNames(IDbConnection connection) {
      var list = new List<string>();
      var command = connection.CreateCommand();
      command.CommandText = "select name from sqlite_master where type='table' and name not like 'sqlite_%'";

      var reader = command.ExecuteReader();
      while(reader.Read())
        list.Add(reader.GetString(0));
      return list;
    }
  }
}