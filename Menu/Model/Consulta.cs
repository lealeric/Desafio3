using AgendaConsultorio.Utils;

namespace AgendaConsultorio.Model
{
    /// <summary>
    /// Gerencia agendamentos para um paciente.
    /// </summary>
    public class Consulta
    {
        public int Id { get; set; }
        public virtual Paciente Paciente { get; set; }
        public int PacienteId { get; private set; }
        public DateTime DtHrInicio { get; private set; }
        public DateTime DtHrFim { get; private set; }
        public TimeSpan DuracaoConsulta
        {
            get
            {
                return DtHrFim - DtHrInicio;
            }
        }
        public Consulta() { }

        /// <summary>
        /// Cria uma nova instância de Consulta.
        /// </summary>
        /// <param name="paciente">Representa a propriedade <see cref="Paciente"/> e deve existir previamente na agenda.</param>
        /// <param name="dtHrInicio">Representa a propriedade <see cref="DtHrInicio"/>, deve ser uma data/hora válida e deve atender ao estipulado pelo consultório.</param>
        /// <param name="dtHrFim">Representa a propriedade <see cref="DtHrFim"/>, deve ser uma data/hora válida e deve atender ao estipulado pelo consultório.</param>
        public Consulta(Paciente paciente, DateTime dtHrInicio, DateTime dtHrFim)
        {
            DtHrInicio = dtHrInicio.SetKindUtc();
            DtHrFim = dtHrFim.SetKindUtc();
            PacienteId = paciente.Id;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !obj.GetType().Equals(GetType()))
            {
                return false;
            }

            Consulta other = (Consulta)obj;
            return DtHrInicio.Equals(other.DtHrInicio);
        }

        public string ToString()
        {
            return string.Format("{0} {1} {2} {3} {4}", DtHrInicio.AddHours(3).ToString("HH:mm"),
                                DtHrFim.AddHours(3).ToString("HH:mm"), DuracaoConsulta.ToString("hh\\:mm"),
                                Paciente.Nome.PadRight(20), Paciente.DtNascimento.ToString("dd/MM/yyyy"));
        }
    }
}
