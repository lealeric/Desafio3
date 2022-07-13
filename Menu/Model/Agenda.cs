using System.Globalization;
using AgendaConsultorio.Database;
using AgendaConsultorio.Validacao;

namespace AgendaConsultorio.Model
{
    /// <summary>
    /// Define uma nova agenda com pacientes e consultas.
    /// </summary>
    public class Agenda
    {
        public ConsultaDAO ConsultaDAO { get; }
        public PacienteDAO PacienteDAO { get; }
        private string[] DadosConsulta, DadosPaciente;

        /// <summary>
        /// Cria uma nova instância de agenda.
        /// </summary>
        public Agenda()
        {
            ConsultaDAO = new ConsultaDAO();
            PacienteDAO = new PacienteDAO();
        }

        /// <summary>
        /// Adiciona um paciente após validar os dados inseridos pelo usuário.
        /// </summary>
        public void addPaciente()
        {
            DadosPaciente = ValidacaoPaciente.dadosPacienteValidos(PacienteDAO);

            string nome = DadosPaciente[0].ToUpper();
            long cpf = Convert.ToInt64(DadosPaciente[1]);
            DateTime dtNascimento = DateTime.ParseExact(DadosPaciente[2], "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None);

            PacienteDAO.AddPaciente(new Paciente(nome, cpf, dtNascimento));

            Console.WriteLine("Cadastro realizado com sucesso!");
            DadosPaciente = null;
        }

        /// <summary>
        /// Remove um paciente se não possuir agendamentos futuros.
        /// </summary>
        public void removePaciente()
        {
            Paciente paciente = Interface.Interface.solicitaPacientePorCpf(PacienteDAO);

            if (ValidacaoPaciente.validaRemocaoPaciente(PacienteDAO, paciente))
            {
                PacienteDAO.RemovePaciente(paciente);
                Console.WriteLine("Paciente removido com sucesso!\n");
            }
        }

        /// <summary>
        /// Lista os pacientes cadastrados ordenados por nome ou CPF.
        /// </summary>
        /// <param name="parametro">Ordena os pacientes por 3 - CPF, 4 - Nome.</param>
        public void listaPacientes(int parametro)
        {
            if (parametro != 3 && parametro != 4)
            {
                Console.WriteLine("Opção inválida!");
                return;
            }

            string ordenacao = parametro == 3 ? "CPF" : "Nome";
            Interface.Interface.imprimeListaPaciente(PacienteDAO, ordenacao);
        }

        /// <summary>
        /// Adiciona um agendamento de consulta após validar os dados inseridos pelo usuário.
        /// </summary>
        public void addConsulta()
        {
            DadosConsulta = ValidacaoConsulta.validaDadosConsulta(PacienteDAO, ConsultaDAO);

            Paciente paciente;
            DateTime dtHrInicio, dtHrFim;

            paciente = PacienteDAO.recuperaPaciente(Convert.ToInt64(DadosConsulta[0]));
            dtHrInicio = DateTime.ParseExact(DadosConsulta[1] + " " + DadosConsulta[2], "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);
            dtHrFim = DateTime.ParseExact(DadosConsulta[1] + " " + DadosConsulta[3], "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);

            Consulta consulta = new Consulta(paciente, dtHrInicio, dtHrFim);
            ConsultaDAO.AddConsulta(consulta);

            DadosConsulta = null;
            Console.WriteLine("Agendamento realizado com sucesso!\n");

        }

        /// <summary>
        /// Cancela uma consulta após validar essa possibilidade.
        /// </summary>
        public void cancelaConsulta()
        {
            Paciente paciente = Interface.Interface.solicitaPacientePorCpf(PacienteDAO);

            if (paciente == null)
            {
                Console.WriteLine("Paciente não encontrado!\n");
            }
            else
            {
                DateTime dataConsultaCancelada = ValidacaoConsulta.retornaDataConsultaCancelada().AddHours(-3);
                Consulta consulta = ConsultaDAO.Consulta(paciente, dataConsultaCancelada);

                if (consulta != null)
                {
                    ConsultaDAO.RemoveConsulta(consulta);
                    Console.WriteLine("Consulta cancelada com sucesso!\n");
                }

                else Console.WriteLine("Consulta não encontrada.");
            }
        }
    }
}
