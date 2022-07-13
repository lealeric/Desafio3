using AgendaConsultorio.Model;

namespace AgendaConsultorio.Database
{
    public class PacienteDAO : IDisposable
    {
        private AgendaContext contexto;

        /// <summary>
        /// Cria um contexto no banco de dados para os pacientes.
        /// </summary>
        public PacienteDAO()
        {
            this.contexto = new AgendaContext();
        }

        /// <summary>
        /// Adiciona um novo paciente no contexto.
        /// </summary>
        /// <param name="paciente">Objeto paciente a ser adicionado.</param>
        public void AddPaciente(Paciente paciente)
        {
            contexto.Pacientes.Add(paciente);
            contexto.SaveChanges();
        }

        /// <summary>
        /// Lista todos os pacientes do contexto baseado em um parâmetro.
        /// </summary>
        /// <param name="paramentro">"CPF" - ordena os pacientes por CPF, "Nome" - ordena os pacientes por nome.</param>
        /// <returns>Uma lista com os pacientes ordenados.</returns>
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

        /// <summary>
        /// Recupera um paciente baseado no seu Id.
        /// </summary>
        /// <param name="id">Número identificador único do paciente no banco de dados.</param>
        /// <returns>Um paciente específico.</returns>
        public Paciente recuperaPaciente(int id)
        {
            var query = from paciente in contexto.Pacientes
                        where paciente.Id == id
                        select paciente;

            if (query.Any()) return query.First();
            else return null;
        }

        /// <summary>
        /// Recupera um paciente baseado no seu CPF.
        /// </summary>
        /// <param name="cpf">Número de CPF do paciente.</param>
        /// <returns>Um paciente específico.</returns>
        public Paciente recuperaPaciente (long cpf)
        {
            var query = from paciente in contexto.Pacientes
                        where paciente.Cpf == cpf
                        select paciente;

            if (query.Any()) return query.First();
            else return null;
        }

        /// <summary>
        /// Consulta o agendamento futuro de um paciente.
        /// </summary>
        /// <param name="idPaciente">Número identificador de um paciente no banco de dados.</param>
        /// <returns>Um agendamento futuro ou nulo caso não exista.</returns>
        public Consulta retornaProximaConsulta(int idPaciente)
        {
            var proximaConsulta = from consulta in contexto.Consultas
                                  where (consulta.DtHrInicio > DateTime.UtcNow && consulta.PacienteId == idPaciente)
                                  select consulta;

            if (proximaConsulta.Any()) return proximaConsulta.First();
            else return null;
        }

        public void Dispose()
        {
            contexto.Dispose();
        }
        
        /// <summary>
        /// Remove um paciente do banco de dados.
        /// </summary>
        /// <param name="paciente">Paciente a ser removido.</param>
        public void RemovePaciente(Paciente paciente)
        {
            contexto.Remove(paciente);
            contexto.SaveChanges();
        }
    }
}
