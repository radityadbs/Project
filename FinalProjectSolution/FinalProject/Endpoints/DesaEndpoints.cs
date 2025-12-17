using FinalProject.Dtos;
using FinalProject.Services;

namespace FinalProject.Endpoints;

public static class DesaEndpoints
{
    public static void MapDesaEndpoints(this WebApplication app)
    {
        app.MapGet("/api/desa/search", async (
            string q,
            DesaService service) =>
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 3)
                return Results.Ok(new List<DesaDto>());

            var data = await service.SearchAsync(q);
            return Results.Ok(data);
        });

        app.MapGet("/api/desa/{desaname}", async (string desaname, DesaService desaService) =>
        {
            var desa = await desaService.GetDesaByNameAsync(desaname);
            if (desa == null)
                return Results.NotFound();

            return Results.Ok(desa);
        });

    }
}
