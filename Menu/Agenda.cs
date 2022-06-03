using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AgendaConsultorio
{
    public class Agenda
    {
        //Classe gerenciadora dos pacientes e das consultas

        private List<Paciente> Pacientes = new List<Paciente>();
        private List<Consulta> Consultas = new List<Consulta>();

        private String[] DadosConsulta, DadosPaciente;
        public Agenda() 
        {
            Pacientes.Add(new Paciente("Soraia", 64558436734, new DateTime(1960, 11, 17)));
            Pacientes.Add(new Paciente("Francisco", 72793490725, new DateTime(1962, 11, 28)));
            Consultas.Add(new Consulta(Pacientes[0], new DateTime(2022, 7, 3, 14, 0, 0), new DateTime(2022, 7, 3, 14, 30, 0)));
            Consultas.Add(new Consulta(Pacientes[0], new DateTime(2022, 5, 3, 14, 0, 0), new DateTime(2022, 5, 3, 14, 30, 0)));
            Consultas.Add(new Consulta(Pacientes[0], new DateTime(2022, 4, 3, 14, 0, 0), new DateTime(2022, 4, 3, 14, 30, 0)));
            Consultas.Add(new Consulta(Pacientes[0], new DateTime(2022, 4, 3, 15, 0, 0), new DateTime(2022, 4, 3, 15, 30, 0)));
            Consultas.Add(new Consulta(Pacientes[0], new DateTime(2022, 4, 3, 9, 0, 0), new DateTime(2022, 4, 3, 9, 45, 0)));
            Consultas.Add(new Consulta(Pacientes[1], new DateTime(2021, 6, 3, 14, 0, 0), new DateTime(2022, 6, 3, 14, 30, 0)));
            Pacientes[0].addConsulta(Consultas[0]);
            Pacientes[0].addConsulta(Consultas[1]); 
            Pacientes[0].addConsulta(Consultas[2]);
            Pacientes[0].addConsulta(Consultas[3]);
            Pacientes[0].addConsulta(Consultas[4]);
            Pacientes[1].addConsulta(Consultas[5]);
        }

        private String[] solicitaDadosPaciente(String[] entradasPaciente)
        {
            String[] textos = new String[3];

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

                Console.WriteLine(String.Format(
                                "\nNome: {0}\n" +
                                "CPF: {1}\n" +
                                "Data de nascimento: {2}\n", entradasPaciente[0], entradasPaciente[1], entradasPaciente[2]));

            return entradasPaciente;
        }

        private bool dadosPacienteValidos()
        {
            ValidacaoPaciente validacaoPaciente;
            Dictionary<String, String> dicErrosPaciente;
            String[] dadosEntradaPaciente = new String[3];

            do
            {
                dadosEntradaPaciente = solicitaDadosPaciente(dadosEntradaPaciente);
                validacaoPaciente = new ValidacaoPaciente(retornaPaciente(dadosEntradaPaciente[1]), dadosEntradaPaciente[0],
                    dadosEntradaPaciente[1], dadosEntradaPaciente[2]);
                dicErrosPaciente = validacaoPaciente.DicionarioErrosPaciente;

                foreach (KeyValuePair<String, String> item in dicErrosPaciente)
                {
                    if (item.Key == "Nome")
                    {
                        dadosEntradaPaciente[0] = "";
                    }
                    else if (item.Key == "CPF")
                    {
                        dadosEntradaPaciente[1] = "";
                    }
                    else if (item.Key == "Data de Nascimento")
                    {
                        dadosEntradaPaciente[2] = "";
                    }

                    Console.WriteLine(String.Format("Erro: {0}\n", item.Value));
                }
            } while (dicErrosPaciente.Count > 0);

            DadosPaciente = dadosEntradaPaciente;
            return true;
        }

        public void addPaciente()
        {
            if (dadosPacienteValidos())
            {
                String nome = DadosPaciente[0].ToUpper();
                long cpf = Convert.ToInt64(DadosPaciente[1]);
                DateTime dtNascimento = DateTime.ParseExact(DadosPaciente[2], "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None);

                Pacientes.Add(new Paciente(nome, cpf, dtNascimento));
                Console.WriteLine("Cadastro realizado com sucesso!");
                DadosPaciente = null;
            }            
        }

        public Paciente retornaPaciente(String cpfConsulta)
        {
            foreach (Paciente paciente in Pacientes)
            {
                if (paciente.Cpf == Convert.ToInt64(cpfConsulta))
                {
                    return paciente;
                }
            }

            return null;
        }

        private bool validaRemocaoPaciente(Paciente paciente)
        {
            if (paciente == null)
            {
                Console.WriteLine("Não é possível remover pacientes não cadastrados!\n");
                return false;
            }

            if (paciente.temConsultaFutura())
            {
                Console.WriteLine("Não é possível remover pacientes com agendamentos futuros!\n");
                return false;
            }

            if (paciente.Consultas != null)
            {
                Consulta consultaPaciente, consultaAgenda;
                for (int i = 0; i < paciente.Consultas.Count; i++)
                {
                    consultaPaciente = paciente.Consultas[i];

                    for (int j = 0; j < Consultas.Count; j++)
                    {
                        consultaAgenda = Consultas[j];
                        if (consultaPaciente.Equals(consultaAgenda))
                        {
                            Consultas.Remove(consultaAgenda);
                        }
                    }
                }
                paciente.removeTodasConsultas();
            }

            return true;
        }

        public void removePaciente()
        {
            Console.WriteLine("Insira o cpf do paciente que deseja remover:");
            String cpf = Console.ReadLine();
            Paciente paciente = retornaPaciente(cpf);

            if (validaRemocaoPaciente(paciente))
            {
                Pacientes.Remove(paciente);
                Console.WriteLine("Paciente removido com sucesso!\n");
            }
        }

        public void listaPacientes(int parametro)
        {
            List<Paciente> pacientesOrdenados;

            if (parametro == 3)
            {
                pacientesOrdenados = Pacientes.OrderBy(paciente => paciente.Cpf).ToList();
            }
            else pacientesOrdenados = Pacientes.OrderBy(paciente => paciente.Nome).ToList();

            Console.WriteLine(String.Concat(Enumerable.Repeat("-", 60)) + "\n" +
                                String.Format("{0:11} {1:32} {2} {3:-5}\n","CPF".PadRight(11), "Nome".PadRight(33), "Dt.Nasc.", "Idade".PadLeft(1)) +
                                String.Concat(Enumerable.Repeat("-", 60)));

            foreach (Paciente paciente in pacientesOrdenados)
            {
                Console.WriteLine(paciente.ToString());
            }

            Console.WriteLine(String.Concat(Enumerable.Repeat("-", 60)));
        }

        private String[] solicitaDadosConsulta(String[] entradasConsulta)
        {
            String[] textosConsulta = new String[4];

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

            Console.WriteLine(String.Format("\nCPF: {0}\n" +
                                            "Data da consulta: {1}\n" +
                                            "Hora Inicial: {2}\n" +
                                            "Hora Final: {3}\n", entradasConsulta[0], entradasConsulta[1], entradasConsulta[2], entradasConsulta[3]));
            return entradasConsulta;
        }

        private bool validaDadosConsulta()
        {
            Paciente paciente;
            ValidacaoConsulta validacaoConsulta;
            Dictionary<String, String> dicErrosConsulta;
            String[] dadosConsulta = new string[4];

            do
            {
                dadosConsulta = solicitaDadosConsulta(dadosConsulta);
                paciente = retornaPaciente(dadosConsulta[0]);
                validacaoConsulta = new ValidacaoConsulta(paciente, Consultas, dadosConsulta[1], dadosConsulta[2], dadosConsulta[3]);
                dicErrosConsulta = validacaoConsulta.DicionarioErrosConsulta;

                foreach (KeyValuePair<String, String> item in dicErrosConsulta)
                {
                    if (item.Key == "Paciente")
                    {
                        dadosConsulta[0] = "";
                    }
                    else if (item.Key == "Data/Hora Inicial" || item.Key == "Horário Inicial")
                    {
                        dadosConsulta[1] = dadosConsulta[2] = "";
                    }
                    else if (item.Key == "Data/Hora Final" || item.Key == "Horário Final")
                    {
                        dadosConsulta[1] = dadosConsulta[3] = "";
                    }
                    else if (item.Key == "Inicial >= Final")
                    {
                        dadosConsulta[2] = dadosConsulta[3] = "";
                    }
                    else if (item.Key == "Sobreposição")
                    {
                        dadosConsulta[1] = dadosConsulta[2] = dadosConsulta[3] = "";
                    }
                    Console.WriteLine("Erro: " + item.Value + "\n");
                }

            } while (dicErrosConsulta.Count > 0);

            DadosConsulta = dadosConsulta;
            return true;
        }

        public void addConsulta()
        {
            if (validaDadosConsulta())
            {
                Paciente paciente;
                DateTime dtHrInicio, dtHrFim;

                paciente = retornaPaciente(DadosConsulta[0]);
                dtHrInicio = DateTime.ParseExact(DadosConsulta[1] + " " + DadosConsulta[2], "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);
                dtHrFim = DateTime.ParseExact(DadosConsulta[1] + " " + DadosConsulta[3], "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);

                Consulta consulta = new Consulta(paciente, dtHrInicio, dtHrFim);
                Consultas.Add(consulta);
                paciente.addConsulta(consulta);
                DadosConsulta = null;
                Console.WriteLine("Agendamento realizado com sucesso!\n");
            }
        }

        public Consulta retornaConsulta(DateTime dtHrInicio)
        {
            foreach (Consulta consulta in Consultas)
            {
                if (consulta.DtHrInicio.Equals(dtHrInicio))
                {
                    return consulta;
                }
            }
            return null;
        }

        public void listaAgenda(Char c)
        {
            List<Consulta> consultasEmOrdem;

            if (c == 'P')
            {   
                DateTime dtInicial, dtFinal;
                while (true)
                {
                    bool validaDataInicial, validaDataFinal;
                    
                    Console.Write("Data inicial: ");
                    validaDataInicial = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dtInicial);
                    Console.Write("Data final: ");
                    validaDataFinal = DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out dtFinal);

                    if(validaDataInicial && validaDataFinal)
                    {
                        break;
                    }
                }
                consultasEmOrdem = Consultas.Where(consulta => consulta.DtHrInicio >= dtInicial && consulta.DtHrFim <= dtFinal).OrderBy(consulta => consulta.DtHrInicio).ToList();

            }
            else consultasEmOrdem = Consultas.OrderBy(consulta => consulta.DtHrInicio).ToList();

            Console.WriteLine(String.Concat(Enumerable.Repeat("-", 60)) + "\n" +
                                            String.Format("{0} {1} {2} {3} {4} {5} {6}\n", "".PadRight(2), "Data".PadRight(7),
                                            "H.Ini".PadRight(3), "H.Fim", "Tempo", "Nome".PadRight(22), "Dt.Nasc.") +
                                            String.Concat(Enumerable.Repeat("-", 60)));

            for (int i = 0; i < consultasEmOrdem.Count; i++)
            {
                Consulta consulta = consultasEmOrdem[i];
                if (consultasEmOrdem.Count > 1 && i > 0 && consulta.DtHrInicio.Date.CompareTo(consultasEmOrdem[i-1].DtHrInicio.Date) == 0)
                {
                    Console.WriteLine(String.Format("{0} {1}", " ".PadRight(10), consulta.ToString()));
                }
                else
                {
                    Console.WriteLine(String.Format("{0} {1}", consulta.DtHrInicio.Date.ToString("dd/MM/yyyy"), consulta.ToString()));
                }
                    
            }

            Console.WriteLine(String.Concat(Enumerable.Repeat("-", 60)));
        }

        private Consulta retornaConsultaCancelada()
        {
            Consulta consulta;
            String dtConsulta, hrConsulta;
            ValidacaoConsulta validacaoConsulta;
            DateTime dtHrConsulta;

            Console.WriteLine("Insira a data da consulta:");
            dtConsulta = Console.ReadLine();
            Console.WriteLine("Insira o horário da consulta:");
            hrConsulta = Console.ReadLine();
            validacaoConsulta = new ValidacaoConsulta(dtConsulta, hrConsulta);

            if(validacaoConsulta.DicionarioErrosConsulta.Count != 0)
            {
                foreach (KeyValuePair<String, String> item in validacaoConsulta.DicionarioErrosConsulta)
                {
                    Console.WriteLine(item.Value + "\n");
                }                
                return null;
            }
            else
            {
                dtHrConsulta = DateTime.ParseExact(dtConsulta + " " + hrConsulta, "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);
                consulta = retornaConsulta(dtHrConsulta);

                if (consulta == null)
                {
                    Console.WriteLine("Agendamento não encontrado!\n");
                    return null;
                }
            }

            return consulta;
        }

        public void cancelaConsulta()
        {
            Paciente paciente;
            Console.WriteLine("Insira o CPF do paciente:");
            paciente = retornaPaciente(Console.ReadLine());

            if (paciente == null)
            {
                Console.WriteLine("Paciente não encontrado!\n");
            }
            else
            {
                Consulta consulta = retornaConsultaCancelada();            

                if (consulta != null)
                {
                    paciente.removeConsulta(consulta);
                    Consultas.Remove(consulta);
                    Console.WriteLine("Consulta cancelada com sucesso!\n");
                }
            }            
        }
    }
}
