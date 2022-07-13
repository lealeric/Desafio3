using AgendaConsultorio.Model;

namespace AgendaConsultorio.Interface
{
    /// <summary>
    /// Apresenta as interfaces de menu para o usuário. 
    /// </summary>
    public class Menu
    {
        public Agenda Agenda;

        /// <summary>
        /// Cria uma instância do menu que acessa a Agenda.
        /// </summary>
        public Menu()
        {
            Agenda = new();
            menuPrincipal();
        }
        private void menuPrincipal()
        {
            int escolha = 0;

            while (escolha != 3)
            {
                Console.WriteLine("Menu Principal\n" +
                                    "1 - Cadastro de pacientes\n" +
                                    "2 - Agenda\n" +
                                    "3 - Fim");

                escolha = Convert.ToInt32(Console.ReadLine());

                switch (escolha)
                {
                    case 1:
                        menuCadastro();
                        break;
                    case 2:
                        menuAgenda();
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("Comando inválido.\n");
                        break;
                }
            }
            Console.WriteLine("Aplicação encerrada.");
        }
        private void menuCadastro()
        {
            int escolha = 0;

            while (escolha != 5)
            {
                Console.WriteLine("Menu do Cadastro de Pacientes\n" +
                                    "1 - Cadastrar novo paciente\n" +
                                    "2 - Excluir paciente\n" +
                                    "3 - Listar pacientes(ordenado por CPF)\n" +
                                    "4 - Listar pacientes(ordenado por nome)\n" +
                                    "5 - Voltar p / menu principal");

                escolha = Convert.ToInt32(Console.ReadLine());

                switch (escolha)
                {
                    case 1:
                        Agenda.addPaciente();
                        break;
                    case 2:
                        Agenda.removePaciente();
                        break;
                    case 3:
                        Agenda.listaPacientes(escolha);
                        break;
                    case 4:
                        Agenda.listaPacientes(escolha);
                        break;
                    case 5:
                        break;
                    default:
                        Console.WriteLine("Comando inválido.\n");
                        break;
                }
            }

        }
        private void menuAgenda()
        {
            int escolha = 0;

            while (escolha != 4)
            {
                Console.WriteLine("Agenda\n" +
                                    "1 - Agendar consulta\n" +
                                    "2 - Cancelar agendamento\n" +
                                    "3 - Listar agenda\n" +
                                    "4 - Voltar p / menu principal");

                escolha = Convert.ToInt32(Console.ReadLine());

                switch (escolha)
                {
                    case 1:
                        Agenda.addConsulta();
                        break;
                    case 2:
                        Agenda.cancelaConsulta();
                        break;
                    case 3:
                        Console.Write("Apresentar a agenda T-Toda ou P-Período: ");
                        Interface.imprimeListaAgenda(Agenda.ConsultaDAO, Agenda.PacienteDAO, Convert.ToChar(Console.ReadLine().ToUpper()));
                        break;
                    case 4:
                        break;
                    default:
                        Console.WriteLine("Comando inválido.\n");
                        break;
                }
            }
        }
    }
}
