using FluentNHibernate.Mapping;

namespace Library.Data
{
  public class FormatMap : ClassMap<Format> {
    public FormatMap() {
      Id(f => f.Id)
        .GeneratedBy.Identity();
      Map(f => f.Name);
    }
  }  
}