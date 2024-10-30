namespace WebApplication1.Estudantes
{
	public class Estudante
	{
		//Guid gera uma identificador unico com varios caracteres.
		public Guid Id { get; set; }
		public string Nome { get; private set; }
		public bool Ativo { get; private set; }

		public Estudante(string nome) {
			Nome = nome;
			Id = Guid.NewGuid();
			Ativo = true;
		}

		public void atualizarNome(string nome) {
			Nome = nome.Trim();
		}

		public void atualizarAtivo(bool ativo)
		{
			Ativo = ativo;
		}
	}
}
