using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoviesDBManager.Models
{
    public class MoviesRepository : Repository<Movie>
    {
        public int Add(Movie movie, List<int> actorsId, List<int> distributorsId)
        {
            BeginTransaction(); 
            int movieId = base.Add(movie);
            UpdateCasting(Get(movieId), actorsId);
            EndTransaction();
            return movieId;
        }
        private void UpdateCasting(Movie movie, List<int> actorsId)
        {
            DeleteCastings(movie);
            if (actorsId != null && actorsId.Count > 0)
            {
                foreach (var actorId in actorsId)
                {
                    DB.Castings.Add(movie.Id, actorId);
                }
            }
        }
        private void DeleteCastings(Movie movie)
        {
            foreach (var actor in movie.Actors)
            {
                DB.Castings.Delete(movie.Id, actor.Id);
            }
        }
       
        public bool Update(Movie movie, List<int> actorsId, List<int> distributorsId)
        {
            BeginTransaction();
            base.Update(movie);
            UpdateCasting(movie, actorsId);
            EndTransaction();
            return true;
        }
        public override bool Delete(int Id)
        {
            BeginTransaction();
            Movie movie = Get(Id);
            if (movie != null)
            {
                DeleteCastings(movie);
                base.Delete(Id);
            }
            EndTransaction();
            return true;
        }
    }
}