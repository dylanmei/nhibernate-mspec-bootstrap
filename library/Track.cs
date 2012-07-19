using System;

namespace Library
{
  public class Track {
    public virtual int Id { get; set; }
    public virtual string Title { get; set; }
    public virtual int Position { get; set; }
    public virtual Album Album { get; set; }
    public virtual Format Format { get; set; }   
  }
}
