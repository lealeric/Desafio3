using AgendaConsultorio.Utils;

namespace AgendaConsultorio.Model
{
    /// <summary>
    /// Define um novo paciente e gerencia pacientes existentes.
    /// </summary>
    public class Paciente
    {
        public int Id { get; set; }
        public string Nome { get; private set; }
        public long Cpf { get; private set; }
        public DateTime DtNascimento { get; private set; }
        public virtual IList<Consulta> Consultas
        {
            get; set;
        }
        public int Idade
        {
            get
            {
                return (int)((DateTime.Now - DtNascimento).TotalDays / 365.25);
            }
        }
        public Paciente() { }

        /// <summary>
        /// Cria uma nova instância de paciente com nome, CPF e data de nascimento.
        /// </summary>
        /// <param name="novoNome">Representa a propriedade <see cref="Nome"/> e deve ter mais que 5 caracteres</param>
        /// <param name="novoCpf">Representa a propriedade <see cref="Cpf" /> e deve atender às regras de definição do CPF</param>
        /// <param name="novaDtNascimento">Representa a propriedade <see cref="DtNascimento"/> e deve ser uma data válida para uma pessoa com 13 anos ou mais.</param>
        public Paciente(string novoNome, long novoCpf, DateTime novaDtNascimento)
        {
            Nome = novoNome;
            Cpf = novoCpf;
            DtNascimento = novaDtNascimento.AddHours(3).SetKindUtc();
            Consultas = new List<Consulta>();
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Paciente)) return false;

            Paciente other = (Paciente)obj;
            return Cpf.Equals(other.Cpf);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", Cpf.ToString().PadRight(11), Nome.PadRight(32),
                DtNascimento.ToString("dd/MM/yyyy"), Idade.ToString().PadLeft(4));
        }
    }
}
