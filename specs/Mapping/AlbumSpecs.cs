using System;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using Machine.Specifications;
using StructureMap;
using Library;

namespace Specs.Mapping {

  public class When_fetching_an_album
    : AlbumSpec {

    Establish context = () => {
      Album = new Album { Name = "abc", Artist = "xyz" };
      Session.Save(Album);
    };

    Because of = () => Fetch();

    It should_set_the_album_name = () =>
      Album.Name.ShouldEqual("abc");

    It should_set_the_artist = () =>
      Album.Artist.ShouldEqual("xyz");

    It should_set_a_collection_of_tracks = () =>
      Album.Tracks.ShouldBeEmpty();
  }

  public class When_fetching_an_album_with_tracks
    : AlbumSpec {

    Establish context = () => {
      Album = new Album { Name = "abc", Artist = "Amy Winehouse" };
      Album.AddTrack(new Track { Title = "t1", Position = 1, Format = CreateFormat("MP3 1") });
      Album.AddTrack(new Track { Title = "t2", Position = 2, Format = CreateFormat("MP3 2") });
      Session.Save(Album);
    };

    Because of = () => Fetch();

    It should_set_a_collection_of_tracks = () =>
      Album.Tracks.Count.ShouldEqual(2);

    It should_set_the_track_titles = () => {
      Album.Tracks.ShouldContain(t => t.Title == "t1");
      Album.Tracks.ShouldContain(t => t.Title == "t2");
    };

    It should_set_the_track_positions = () => {
      Album.Tracks.ShouldContain(t => t.Position == 1);
      Album.Tracks.ShouldContain(t => t.Position == 2);
    };

    It should_set_the_track_formats = () => {
      Album.Tracks.ShouldContain(t => t.Format.Name == "MP3 1");
      Album.Tracks.ShouldContain(t => t.Format.Name == "MP3 2");
    };

    It should_place_the_tracks_in_order = () =>
      Album.Tracks.First().Position.ShouldEqual(1);
  }

  public class AlbumSpec {
    protected static Album Album;
    protected static ISession Session {
      get { return ObjectFactory.GetInstance<ISession>(); }
    }

    protected static void Fetch() {
      Session.Clear();
      Album = Session.Query<Album>()
        .Single(a => a.Id == Album.Id);

//      Album = Session.QueryOver<Album>()
//        .Where(a => a.Id == Album.Id)
//        .Fetch(a => a.Tracks).Eager
//        .TransformUsing(Transformers.DistinctRootEntity)
//        .List().Single();
    }

    protected static Format CreateFormat(string name) {
      var format = new Format { Name = name };
      Session.Save(format);
      return format;
    }
  }
}