using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Estudantes
{
	public static class EstudantesRotas
	{
		public static void AddRotasEstudantes(this WebApplication app)
		{
			var rotasEstudantes = app.MapGroup("estudantes");


			//CancellationToken ct impede que o banco fique travado caso a operação demore muito e a execução da api já tenha sido encerrada
			//é recomendado usa-lo nas operações Async.
			rotasEstudantes.MapPost("", async (AddEstudanteRequest request, 
				AppDbContext context, CancellationToken ct) =>
			{
				var jaExiste = await context.Estudantes.AnyAsync(estudante => estudante.Nome == request.Nome);

				if (jaExiste) {
					return Results.Conflict("Já existe!");
				}
				var novoEstudante = new Estudante(request.Nome);
				await context.Estudantes.AddAsync(novoEstudante, ct);
				await context.SaveChangesAsync(ct);

				var estudanteRetorno = new EstudanteDTO(novoEstudante.Id, novoEstudante.Nome);

				return Results.Ok(estudanteRetorno);
			});

			rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) => {
				// o Select funciona semelhante ao .map() do javascript.
				var estudantes = await context.Estudantes.Where(estudante => estudante.Ativo).Select(estudante => new EstudanteDTO(estudante.Id, estudante.Nome)).ToListAsync(ct);
				return estudantes;
			});

			rotasEstudantes.MapPut("{id}", async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
			{
				var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id);

				if (estudante == null) {
					return Results.NotFound();
				}

				estudante.atualizarNome(request.Nome);

				await context.SaveChangesAsync(ct);
				return Results.Ok(new EstudanteDTO(estudante.Id, estudante.Nome));
			});

			rotasEstudantes.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
			{
				var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id, ct);

				if (estudante == null) {
					return Results.NotFound();
				}
				estudante.atualizarAtivo(false);

				await context.SaveChangesAsync(ct);
				return Results.Ok();
			});
		}
	}
}
