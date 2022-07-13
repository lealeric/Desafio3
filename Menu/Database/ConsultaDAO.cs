using AgendaConsultorio.Model;
using AgendaConsultorio.Utils;

namespace AgendaConsultorio.Database
{
    public class ConsultaDAO : IDisposable
    {
        private AgendaContext contexto;

        /// <summary>
        /// Cria um contexto no banco de dados para as consultas.
        /// </summary>
        public ConsultaDAO()
        {
            this.contexto = new AgendaContext();
        }

        /// <summary>
        /// Adiciona uma consulta no banco de dados.
        /// </summary>
        /// <param name="consulta">Consulta a ser inserida no banco de dados.</param>
        public void AddConsulta(Consulta consulta)
        {
            contexto.Consultas.Add(consulta);
            contexto.SaveChanges();
        }

        /// <summary>
        /// Lista todas as consultas do banco de dados.
        /// </summary>
        /// <returns>Uma lista das consultas ordenadas por data/hora inicial.</returns>
        public IList<Consulta> Consultas()
        {
            var query = from consulta in contexto.Consultas
                        join paciente in contexto.Pacientes on consulta.PacienteId equals paciente.Id
                        orderby consulta.DtHrInicio
                        select consulta;    

            return query.ToList();
        }

        /// <summary>
        /// Lista as consultas dentro de um período de tempo.
        /// </summary>
        /// <param name="dtHrInicial">Dia inicial da consulta.</param>
        /// <param name="dtHrFinal">Dia final da consulta.</param>
        /// <returns>Uma lista das consultas ordenadas por data/hora inicial dentro do período definido.</returns>
        public IList<Consulta> Consultas(DateTime dtHrInicial, DateTime dtHrFinal)
        {
            dtHrInicial = dtHrInicial.SetKindUtc();
            dtHrFinal = dtHrFinal.SetKindUtc();

            var query = from consulta in contexto.Consultas
                        where consulta.DtHrInicio >= dtHrInicial && consulta.DtHrFim <= dtHrFinal
                        orderby consulta.DtHrInicio
                        select consulta;

            return query.ToList();
        }

        /// <summary>
        /// Recupera uma consulta específica de um paciente.
        /// </summary>
        /// <param name="paciente">Paciente de quem recuperar a consulta.</param>
        /// <param name="dtHrInicial">Data/hora inicial da consulta.</param>
        /// <returns>Uma consulta específica, ou nulo caso não exista.</returns>
        public Consulta Consulta(Paciente paciente, DateTime dtHrInicial)
        {
            var query = from consulta in contexto.Consultas
                        where consulta.DtHrInicio.Equals(dtHrInicial) && consulta.PacienteId == paciente.Id
                        select consulta;

            if (query.Any()) return query.First();
            else return null;
        }

        public void Dispose()
        {
            contexto.Dispose();
        }

        /// <summary>
        /// Remove uma consulta do banco de dados.
        /// </summary>
        /// <param name="consulta">Consulta a ser removida.</param>
        public void RemoveConsulta(Consulta consulta)
        {
            contexto.Consultas.Remove(consulta);
            contexto.SaveChanges();
        }
    }
}
