namespace DFW_CW_40452913.Middleware
{
    using global::DFW_CW_40452913.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    namespace DFW_CW_40452913.Middleware
    {
        public class DeletePetitionMiddleware
        {
            private readonly RequestDelegate _next;

            public DeletePetitionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
            {
                if (context.Request.Method == "DELETE" && context.Request.Path == "/Home/DeletePetition")
                {
                    if (int.TryParse(context.Request.Query["id"], out int petitionId))
                    {
                        var petition = await dbContext.Petitions.FindAsync(petitionId);
                        if (petition != null)
                        {
                            dbContext.Petitions.Remove(petition);
                            await dbContext.SaveChangesAsync();
                            context.Response.StatusCode = StatusCodes.Status204NoContent;
                            return;
                        }
                        else
                        {
                            context.Response.StatusCode = StatusCodes.Status404NotFound;
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status400BadRequest;
                        return;
                    }
                }

                await _next(context);
            }
        }

        public static class DeletePetitionMiddlewareExtensions
        {
            public static IApplicationBuilder UseDeletePetitionMiddleware(this IApplicationBuilder builder)
            {
                return builder.UseMiddleware<DeletePetitionMiddleware>();
            }
        }
    }


}
