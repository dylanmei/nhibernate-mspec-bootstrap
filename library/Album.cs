using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Library
{
  public class Album {
    public Album() {
      Tracks = new List<Track>();
    }

    public virtual int Id { get; set; }
    public virtual string Name { get; set; }
    public virtual string Artist { get; set; }
    public virtual IList<Track> Tracks { get; set; }

    public virtual void AddTrack(Track track) {
      track.Album = this;
      Tracks.Add(track);
    }

    public virtual void RemoveTrack(int trackId) {
      var track = Tracks.FirstOrDefault(t => t.Id == trackId);
      if (track != null) {
        Tracks.Remove(track);
        track.Album = null;
      }
    }
  }
}

