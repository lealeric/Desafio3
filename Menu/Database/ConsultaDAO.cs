using AgendaConsultorio.Model;
using AgendaConsultorio.Utils;

namespace AgendaConsultorio.Database
{
    public class ConsultaDAO : IDisposable
    {
        private AgendaContext contexto;

        public ConsultaDAO()
        {
            this.contexto = new AgendaContext();
        }

        public void AddConsulta(Consulta consulta)
        {
            contexto.Consultas.Add(consulta);
            contexto.SaveChanges();
        }

        public IList<Consulta> Consultas()
        {
            var query = from consulta in contexto.Consultas
                        join paciente in contexto.Pacientes on consulta.PacienteId equals paciente.Id
                        orderby consulta.DtHrInicio
                        select consulta;    

            return query.ToList();
        }

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

        public void RemoveConsulta(Consulta consulta)
        {
            contexto.Consultas.Remove(consulta);
            contexto.SaveChanges();
        }
    }
}
