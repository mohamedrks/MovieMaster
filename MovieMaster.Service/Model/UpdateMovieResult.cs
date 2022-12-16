namespace MovieMaster.Service.Model
{
    public enum UpdateMovieStatus
    {
        Updated,
        NotFound,
        Error
    }
    
    public class UpdateMovieResult
    {
        public UpdateMovieStatus Status { get; set; }
        public Movie Movie { get; set; }
    }
}