using System;
using System.Collections.Generic;
using System.Text;

namespace MovieMaster.Service.Model
{
    public class CreateMovieResult    {
        public CreateMovieStatus Status { get; set; }
        public Movie Movie { get; set; }
    }

    public enum CreateMovieStatus
    {
        Created,
        Error
    }

}
