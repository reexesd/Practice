using Microsoft.AspNetCore.Mvc;
using Practice.Interfaces;

namespace Practice
{
    public static class ErrorManager<T> where T : class, IEntity
    {
        private static readonly string _entityName = typeof(T).Name;
        public static ProblemDetails InvalidId
        {
            get
            {
                ProblemDetails problem = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = $"Invalid {_entityName} ID",
                    Detail = "ID must be positive num and must match the id specified in the URI"
                };

                return problem;
            }
        }

        public static ProblemDetails EntityNotFound
        {
            get
            {
                ProblemDetails problem = new()
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Entity not found",
                    Detail = $"{_entityName} with this ID doesn't exists"
                };

                return problem;
            }
        }

        public static ProblemDetails EntityAlreadyExists
        {
            get
            {
                ProblemDetails problem = new()
                {
                    Status = StatusCodes.Status409Conflict,
                    Title = "Entity conflict",
                    Detail = $"{_entityName} with this ID is already exists"
                };

                return problem;
            }
        }
    }
}
