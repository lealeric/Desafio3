using AgendaConsultorio.Database;
using AgendaConsultorio.Model;
using AgendaConsultorio.Utils;
using System.Globalization;

namespace AgendaConsultorio.Interface
{
    /// <summary>
    /// Apresentação e solicitação de dados.
    /// </summary>
    public class Interface
    {
        /// <summary>
        /// Solicita os dados referentes a um novo paciente.
        /// </summary>
        /// <param name="entradasPaciente">Array de string vazia ou com dados pré-validados.</param>
        /// <returns>Array de string com os dados inseridos pelo usuário.</returns>
        public static string[] solicitaDadosPaciente(string[] entradasPaciente)
        {
            string[] textos = new string[3];

            textos[0] = "Insira o nome: ";
            textos[1] = "Insira o CPF: ";
            textos[2] = "Insira a data de nascimento: (DD/MM/YYYY)";

            for (int i = 0; i < 3; i++)
            {
                if (entradasPaciente[i] == null || entradasPaciente[i] == "")
                {
                    Console.WriteLine(textos[i]);
                    entradasPaciente[i] = Console.ReadLine();
                }
            }

            Console.WriteLine(string.Format(
                            "\nNome: {0}\n" +
                            "CPF: {1}\n" +
                            "Data de nascimento: {2}\n", entradasPaciente[0], entradasPaciente[1], entradasPaciente[2]));

            return entradasPaciente;
        }

        /// <summary>
        /// Solicita o CPF de um paciente.
        /// </summary>
        /// <param name="pacienteDao">Contexto dos pacientes no banco de dados.</param>
        /// <returns>Um paciente caso exista no banco e nulo caso contrário.</returns>
        public static Paciente solicitaPacientePorCpf(PacienteDAO pacienteDao)
        {
            Console.WriteLine("Insira o CPF do paciente:");
            Paciente paciente = pacienteDao.recuperaPaciente(Convert.ToInt64(Console.ReadLine()));

            return paciente;
        }

        /// <summary>
        /// Solicita os dados referentes a uma nova consulta.
        /// </summary>
        /// <param name="entradasConsulta">Array de string vazia ou com dados pré-validados.</param>
        /// <returns>Array de string com os dados inseridos pelo usuário.</returns>
        public static string[] solicitaDadosConsulta(string[] entradasConsulta)
        {
            string[] textosConsulta = new string[4];

            textosConsulta[0] = "Insira o CPF do paciente:";
            textosConsulta[1] = "Insira a data da consulta: (DD/MM/YYYY)";
            textosConsulta[2] = "Insira o horário de início do atendimento: (HHMM)";
            textosConsulta[3] = "Insira o horário do fim do atendimento: (HHMM)";

            for (int i = 0; i < 4; i++)
            {
                if (entradasConsulta[i] == null || entradasConsulta[i] == "")
                {
                    Console.WriteLine(textosConsulta[i]);
                    entradasConsulta[i] = Console.ReadLine();
                }
            }

            Console.WriteLine(string.Format("\nCPF: {0}\n" +
                                            "Data da consulta: {1}\n" +
                                            "Hora Inicial: {2}\n" +
                                            "Hora Final: {3}\n", entradasConsulta[0], entradasConsulta[1], entradasConsulta[2], entradasConsulta[3]));
            return entradasConsulta;
        }

        /// <summary>
        /// Solicita os dados da consulta a ser cancelada.
        /// </summary>
        /// <returns>Um array com os dados inseridos pelo usuário.</returns>
        public static string[] solicitaDadosConsultaCancelada()
        {
            string[] dadosConsultaCancelada = new string[2];
            
            Console.WriteLine("Insira a data da consulta:");
            dadosConsultaCancelada[0] = Console.ReadLine();
            Console.WriteLine("Insira o horário inicial da consulta:");
            dadosConsultaCancelada[1] = Console.ReadLine();

            return dadosConsultaCancelada;
        }

        /// <summary>
        /// Imprime a lista de todos os pacientes ordenados.
        /// </summary>
        /// <param name="pacienteDAO">Contexto dos pacientes no banco.</param>
        /// <param name="ordenacao">Parâmetro de orendação da lista.</param>
        public static void imprimeListaPaciente(PacienteDAO pacienteDAO, String ordenacao)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 60)) + "\n" +
                                string.Format("{0:11} {1:32} {2} {3:-5}\n", "CPF".PadRight(11), "Nome".PadRight(33), "Dt.Nasc.", "Idade".PadLeft(1)) +
                                string.Concat(Enumerable.Repeat("-", 60)));

            IList<Paciente> pacientes = pacienteDAO.Pacientes(ordenacao);

            foreach (Paciente paciente in pacientes)
            {

                Consulta proxConsulta = pacienteDAO.retornaProximaConsulta(paciente.Id);

                if (proxConsulta != null)
                {
                    Console.WriteLine(string.Format("{0} {1} {2} {3}\n" +
                                                "{4} Agendado para: {5}\n" +
                                                "{6} {7} às {8}", paciente.Cpf.ToString().PadRight(11),
                                                paciente.Nome.PadRight(32), paciente.DtNascimento.ToString("dd/MM/yyyy"),
                                                paciente.Idade.ToString().PadLeft(4), "".PadRight(11),
                                                proxConsulta.DtHrInicio.ToString("dd/MM/yyy"), "".PadRight(11),
                                                proxConsulta.DtHrInicio.AddHours(3).ToString("HH:mm"), proxConsulta.DtHrFim.AddHours(3).ToString("HH:mm")));
                }

                else Console.WriteLine(paciente.ToString());
            }

            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 60)));
        }

        /// <summary>
        /// Lista os agendamentos em toda a lista ou por um período definido.
        /// </summary>
        /// <param name="consultaDAO">Contexto de banco de dados com os agendamentos de consulta.</param>
        /// <param name="pacienteDAO">Contexto de banco de dados com os pacientes.</param>
        /// <param name="c">T - exibe todos os agendamentos; P - usuário define um período para exibir.</param>
        public static void imprimeListaAgenda(ConsultaDAO consultaDAO, PacienteDAO pacienteDAO, char c)
        {
            IList<Consulta> consultas;
            int controleLista = 0;

            if (c == 'P')
            {
                DateTime dtInicial, dtFinal;
                bool validaDataInicial = false, validaDataFinal = false;

                do
                {
                    Console.Write("Data inicial: ");
                    validaDataInicial = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dtInicial);
                    Console.Write("Data final: ");
                    validaDataFinal = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dtFinal);

                    if (!validaDataInicial)
                    {
                        Console.WriteLine("Data/hora inicial inválida!\n");
                    }
                    else if (!validaDataFinal)
                    {
                        Console.WriteLine("Data/hora final inválida!\n");
                    }
                } while (!validaDataInicial || !validaDataFinal);

                consultas = consultaDAO.Consultas(dtInicial.SetKindUtc(), dtFinal.SetKindUtc().AddDays(1));

                Console.WriteLine(dtFinal);
            }
            else consultas = consultaDAO.Consultas();

            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 60)) + "\n" +
                                            string.Format("{0} {1} {2} {3} {4} {5} {6}\n", "".PadRight(2), "Data".PadRight(7),
                                            "H.Ini".PadRight(3), "H.Fim", "Tempo", "Nome".PadRight(22), "Dt.Nasc.") +
                                            string.Concat(Enumerable.Repeat("-", 60)));
            
            foreach (var consulta in consultas)
            {

                if (consultas.First().Equals(consulta) ||
                    !(consultas.ElementAt(controleLista - 1).DtHrInicio.Date.Equals(consulta.DtHrInicio.Date)))
                {
                    Console.WriteLine(string.Format("{0} {1}", consulta.DtHrInicio.Date.ToString("dd/MM/yyyy"),
                                    consulta.ToString()));
                }
                else
                {
                    Console.WriteLine(string.Format("{0} {1}", " ".PadRight(10), consulta.ToString()));
                }                    

                controleLista++;
            }
            Console.WriteLine(string.Concat(Enumerable.Repeat("-", 60)));
        }
    }
}
