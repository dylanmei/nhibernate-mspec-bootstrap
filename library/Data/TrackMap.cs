using FluentNHibernate.Mapping;

namespace Library.Data
{
  public class TrackMap : ClassMap<Track> {
    public TrackMap() {
      Id(t => t.Id)
        .GeneratedBy.Identity();
      Map(t => t.Title);
      Map(t => t.Position);
      References(t => t.Album, "AlbumId")
        .Not.Nullable();
      References(t => t.Format, "FormatId")
//        .Fetch.Join()
        .Not.Nullable();
    }
  }  
}