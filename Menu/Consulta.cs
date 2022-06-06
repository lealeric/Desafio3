using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AgendaConsultorio
{
    /// <summary>
    /// Gerencia agendamentos para um paciente.
    /// </summary>
    public class Consulta
    {
        public Paciente Paciente { get; }

        public DateTime DtHrInicio { get; }

        public DateTime DtHrFim { get; }

        private TimeSpan DuracaoConsulta
        {
            get
            {
                return DtHrFim - DtHrInicio;
            }
        }

        /// <summary>
        /// Cria uma nova instância de Consulta.
        /// </summary>
        /// <param name="paciente">Representa a propriedade <see cref="Paciente"/> e deve existir previamente na agenda.</param>
        /// <param name="dtHrInicio">Representa a propriedade <see cref="DtHrInicio"/>, deve ser uma data/hora válida e deve atender ao estipulado pelo consultório.</param>
        /// <param name="dtHrFim">Representa a propriedade <see cref="DtHrFim"/>, deve ser uma data/hora válida e deve atender ao estipulado pelo consultório.</param>
        public Consulta(Paciente paciente, DateTime dtHrInicio, DateTime dtHrFim)
        {
            DtHrInicio = dtHrInicio;
            DtHrFim = dtHrFim;
            Paciente = paciente;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            Consulta other = (Consulta)obj;
            return this.DtHrInicio.Equals(other.DtHrInicio);
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4}", DtHrInicio.ToString("HH:mm"), DtHrFim.ToString("HH:mm"), DuracaoConsulta.ToString("hh\\:mm"),
                                Paciente.Nome.PadRight(20), Paciente.DtNascimento.ToString("dd/MM/yyyy"));
        }

        /// <summary>
        /// Compara uma consulta com outra baseado na data/hora inicial.
        /// </summary>
        /// <param name="other">Segunda consulta para fazer a comparação.</param>
        /// <returns><para>Um valor entre -1 e 1 indicando a relação de data/hora inicial entre as consultas.</para>
        /// <para></para>
        /// <para>-1 se <paramref name="other"/> for após a instância</para>
        /// <para></para>
        /// <para>0 se forem na mesma data/hora</para>
        /// <para></para>
        /// 1 se <paramref name="other"/> for antes da instância</returns>
        public int CompareTo(Consulta other)
        {
            return this.DtHrInicio.CompareTo(other.DtHrInicio);
        }
    }
}
