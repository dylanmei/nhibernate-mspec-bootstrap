using FluentNHibernate.Mapping;

namespace Library.Data
{
  public class AlbumMap : ClassMap<Album> {
    public AlbumMap() {
      Id(a => a.Id)
        .GeneratedBy.Identity();
      Map(a => a.Name);
      Map(a => a.Artist);
      HasMany(a => a.Tracks)
        .KeyColumn("AlbumId")
        .OrderBy("Position")
//        .Fetch.Join()
        .Cascade.AllDeleteOrphan();
    }
  }  
}