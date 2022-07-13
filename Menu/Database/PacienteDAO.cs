using AgendaConsultorio.Model;

namespace AgendaConsultorio.Database
{
    public class PacienteDAO : IDisposable
    {
        private AgendaContext contexto;

        public PacienteDAO()
        {
            this.contexto = new AgendaContext();
        }

        public void AddPaciente(Paciente paciente)
        {
            contexto.Pacientes.Add(paciente);
            contexto.SaveChanges();
        }                

        public IList<Paciente> Pacientes(string paramentro)
        {
            var query = from paciente in contexto.Pacientes
                        select paciente;

            if (paramentro == "CPF")
            {
                
                query = query.OrderBy(p => p.Cpf);
            }
            else if (paramentro == "Nome")
            {
                query = query.OrderBy(p => p.Nome);
            }
            return query.ToList();
        }

        public Paciente recuperaPaciente(int id)
        {
            var query = from paciente in contexto.Pacientes
                        where paciente.Id == id
                        select paciente;

            if (query.Any()) return query.First();
            else return null;
        }

        public Paciente recuperaPaciente (long cpf)
        {
            var query = from paciente in contexto.Pacientes
                        where paciente.Cpf == cpf
                        select paciente;

            if (query.Any()) return query.First();
            else return null;
        }

        /// <summary>
        /// Verifica a existência de um agendamento futuro para o paciente.
        /// </summary>
        /// <returns>Verdadeiro se existe uma consulta após a data/hora atual, caso contrário retorna falso.</returns>
        public Consulta retornaProximaConsulta(int idPaciente)
        {
            var proximaConsulta = from consulta in contexto.Consultas
                                  where (consulta.DtHrInicio > DateTime.UtcNow && consulta.PacienteId == idPaciente)
                                  select consulta;

            if (proximaConsulta.Any()) return proximaConsulta.First();
            else return null;
        }

        /// <summary>
        /// Lista todas as consultas de um paciente ordenadas a partir da mais antiga.
        /// </summary>
        /// <returns>Uma lista com as consultas do paciente ou null.</returns>
        public IList<Consulta> Consultas(int idPaciente)
        {
            var query = from consulta in contexto.Consultas
                        where consulta.PacienteId == idPaciente
                        orderby consulta.DtHrInicio
                        select consulta;

            if (query.Any()) return query.ToList();
            else return null;
        }

        public void Dispose()
        {
            contexto.Dispose();
        }
        
        public void RemovePaciente(Paciente paciente)
        {
            contexto.Remove(paciente);
            contexto.SaveChanges();
        }
    }
}
