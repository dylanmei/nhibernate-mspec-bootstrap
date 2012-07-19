using System;
using NHibernate.Driver;
using NHibernate.Dialect;
using FluentNHibernate;
using FluentNHibernate.Cfg.Db;

namespace Specs
{
	public class SpecDbConfiguration
		: PersistenceConfiguration<SpecDbConfiguration>
	{
	    public static SpecDbConfiguration Standard
	    {
	        get { return new SpecDbConfiguration(); }
	    }

	    public SpecDbConfiguration()
	    {
	        Driver<SpecDbDriver>();
	        Dialect<SQLiteDialect>();
	        Raw("query.substitutions", "true=1;false=0");
	    }

	    public SpecDbConfiguration InMemory()
	    {
	        Raw("connection.release_mode", "on_close");
	        return ConnectionString(c => c
	            .Is("Data Source=:memory:;Version=3;New=True;"));
	    }

	    public SpecDbConfiguration UsingFile(string fileName)
	    {
	        return ConnectionString(c => c
	            .Is(string.Format("Data Source={0};Version=3;New=True;", fileName)));
	    }

	    public SpecDbConfiguration UsingFileWithPassword(string fileName, string password)
	    {
	        return ConnectionString(c => c
	            .Is(string.Format("Data Source={0};Version=3;New=True;Password={1};", fileName, password)));
	    }
	}
}

