using System;
using NHibernate;
using Machine.Specifications;
using StructureMap;

#if PROFILER
  using HibernatingRhinos.Profiler.Appender.NHibernate;
#endif

namespace Specs
{
  public class SpecDbContext
    : IAssemblyContext, ICleanupAfterEveryContextInAssembly {

    public void OnAssemblyStart() {
      Setup();
#if PROFILER 
      Console.WriteLine("SpecDbContext: Setup profiling");
      NHibernateProfiler.Initialize();
//      NHibernateProfiler.Initialize(
//        new NHibernateAppenderConfiguration { HostToSendProfilingInformationTo = "192.168.41.137" });
#endif 
    }

    public void OnAssemblyComplete() {
      Destroy();
    }

    public void AfterContextCleanup() {
      Clean();
    }

    void Setup() {
      Console.WriteLine("SpecDbContext: Destroy database");
      SpecDb.Destroy();
      Console.WriteLine("SpecDbContext: Setup schema");
      SpecDb.Setup();
    }

    void Destroy() {
      Console.WriteLine("SpecDbContext: Teardown session");
      SpecDb.Teardown();
      Console.WriteLine("SpecDbContext: Destroy database");
      SpecDb.Destroy();
    }

    void Clean() {
      Console.WriteLine("SpecDbContext: Clean database");
      SpecDb.Clean();
    }
  }
}
