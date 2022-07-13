using System.Globalization;
using AgendaConsultorio.Model;

namespace AgendaConsultorio.Validacao
{
    /// <summary>
    /// Valida os dados do referentes às consultas.
    /// </summary>
    public class ValidacaoConsulta
    {
        private DateTime Aberto;
        private DateTime Fechado;

        public Dictionary<string, string> DicionarioErrosConsulta
        {
            get;
        }

        /// <summary>
        /// Cria uma instância de validação da consulta.
        /// </summary>
        /// <param name="agenda">Objeto gerenciador dos contextos de agendamentos e pacientes.</param>
        /// <param name="cpf"></param>
        /// <param name="data"></param>
        /// <param name="hrInicial"></param>
        /// <param name="hrFinal"></param>
        public ValidacaoConsulta(Agenda agenda, string cpf, string data, string hrInicial, string hrFinal)
        {
            DicionarioErrosConsulta = new Dictionary<string, string>();

            validaPaciente(agenda, Convert.ToInt64(cpf));

            DateTime dtHrInicial, dtHrFinal;
            bool dataHoraInicialValida = DateTime.TryParseExact(data + " " + hrInicial, "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None, out dtHrInicial);
            bool dataHoraFinalValida = DateTime.TryParseExact(data + " " + hrFinal, "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None, out dtHrFinal);

            if (!dataHoraInicialValida)
            {
                DicionarioErrosConsulta.Add("Data/Hora Inicial", "Data/Hora inicial em formato inválido");
            }
            else if (!dataHoraFinalValida)
            {
                DicionarioErrosConsulta.Add("Data/Hora Final", "Data/Hora final em formato inválido");
            }
            else
            {
                Aberto = DateTime.ParseExact(data + " 0800", "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);
                Fechado = DateTime.ParseExact(data + " 1900", "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);
                validaDataHoraInicial(dtHrInicial);
                validaDataHoraFinal(dtHrFinal);
                validaIntervalos(dtHrInicial, dtHrFinal, agenda.ConsultaDAO.Consultas()); ;
            }
        }

        /// <summary>
        /// Realiza a validação dos dados referentes a um agendamento, digitados pelo usuário.
        /// </summary>
        /// <param name="agenda">Objeto gerenciador dos contextos de agendamentos e pacientes.</param>
        /// <returns>Um array com os dados validados.</returns>
        public static String[] validaDadosConsulta(Agenda agenda)
        {
            ValidacaoConsulta validacaoConsulta;
            Dictionary<string, string> dicErrosConsulta;
            string[] dadosConsulta = new string[4];

            do
            {
                dadosConsulta = Interface.Interface.solicitaDadosConsulta(dadosConsulta);
                validacaoConsulta = new ValidacaoConsulta(agenda, dadosConsulta[0], dadosConsulta[1], dadosConsulta[2], dadosConsulta[3]);
                dicErrosConsulta = validacaoConsulta.DicionarioErrosConsulta;

                foreach (KeyValuePair<string, string> item in dicErrosConsulta)
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
            
            return dadosConsulta;
        }

        /// <summary>
        /// Cria uma instância de validaçãoapenas para a data/hora inicial da consulta.
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="horaInicial"></param>
        public ValidacaoConsulta(string dataInicial, string horaInicial)
        {
            DicionarioErrosConsulta = new Dictionary<string, string>();
            DateTime dtHrInicial;
            bool dataHoraInicialValida = DateTime.TryParseExact(dataInicial + " " + horaInicial, "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None, out dtHrInicial);

            if (!dataHoraInicialValida)
            {
                DicionarioErrosConsulta.Add("Data/Hora Inicial", "Data/Hora inicial em formato inválido");
            }
            else
            {
                Aberto = DateTime.ParseExact(dataInicial + " 0800", "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);
                Fechado = DateTime.ParseExact(dataInicial + " 1900", "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);

                validaDataHoraInicial(dtHrInicial);
            }
        }

        private void validaPaciente(Agenda agenda, long cpf)
        {
            Paciente paciente = agenda.PacienteDAO.recuperaPaciente(cpf);
            if ( paciente == null)
            {
                DicionarioErrosConsulta.Add("Paciente", "Paciente não cadastrado!\n");

                return;
            }
            else if (agenda.PacienteDAO.retornaProximaConsulta(paciente.Id) != null)
            {
                DicionarioErrosConsulta.Add("Paciente", "Paciente já possui consulta agendada!\n");

                return;
            }
        }

        private void validaDataHoraInicial(DateTime dtHr)
        {

            if (dtHr > Fechado || dtHr < Aberto || dtHr.DayOfWeek == DayOfWeek.Sunday || dtHr.DayOfWeek == DayOfWeek.Saturday)
            {
                DicionarioErrosConsulta.Add("Horário Inicial", "Hora inicial fora do horário de funcionamento do consultório.\n");

                return;
            }
            else if (dtHr < DateTime.Now)
            {
                DicionarioErrosConsulta.Add("Horário Inicial", "Hora inicial da consulta deve ser futura.\n");

                return;
            }
            else if (dtHr.Minute % 15 != 0)
            {
                DicionarioErrosConsulta.Add("Horário Inicial", "Hora inicial da consulta deve ser a cada 15 minutos.\n");

                return;
            }
        }

        private void validaDataHoraFinal(DateTime dtHr)
        {

            if (dtHr > Fechado || dtHr < Aberto || dtHr.DayOfWeek == DayOfWeek.Sunday || dtHr.DayOfWeek == DayOfWeek.Saturday)
            {
                DicionarioErrosConsulta.Add("Horário Final", "Hora final fora do horário de funcionamento do consultório.\n");

                return;
            }
            else if (dtHr < DateTime.Now)
            {
                DicionarioErrosConsulta.Add("Horário Final", "Hora final da consulta deve ser futura.\n");

                return;
            }
            else if (dtHr.Minute % 15 != 0)
            {
                DicionarioErrosConsulta.Add("Horário Final", "Hora final da consulta deve ser a cada 15 minutos.\n");

                return;
            }
        }

        private void validaIntervalos(DateTime dtHrInicio, DateTime dtHrFim, IList<Consulta> consultas)
        {
            if (dtHrInicio >= dtHrFim)
            {
                DicionarioErrosConsulta.Add("Inicial >= Final", "Hora do fim da consulta deve ser após o horário inicial do agendamento!\n");

                return;
            }
            if (temIntersecao(dtHrInicio, dtHrFim, consultas))
            {
                DicionarioErrosConsulta.Add("Sobreposição", "Existe conflito de horário com outro agendamento!\n");

                return;
            }
        }

        private bool temIntersecao(DateTime dtHrInicio, DateTime dtHrFim, IList<Consulta> consultas)
        {
            dtHrInicio = dtHrInicio;
            dtHrFim = dtHrFim;

            foreach (Consulta consulta in consultas)
            {
                if (dtHrInicio >= consulta.DtHrInicio && dtHrInicio <= consulta.DtHrFim ||
                    dtHrFim >= consulta.DtHrInicio && dtHrFim <= consulta.DtHrFim)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Recupera a data para consultar, na tabela de agendamentos, qual será cancelado.
        /// </summary>
        /// <returns>Uma data em formato padronizado.</returns>
        public static DateTime retornaDataConsultaCancelada()
        {
            Consulta consulta;
            string[] dadosConsulta;
            ValidacaoConsulta validacaoConsulta;
            DateTime dtHrConsulta;

            do
            {
                dadosConsulta = Interface.Interface.solicitaDadosConsultaCancelada();
                validacaoConsulta = new ValidacaoConsulta(dadosConsulta[0], dadosConsulta[1]);

                foreach (KeyValuePair<string, string> item in validacaoConsulta.DicionarioErrosConsulta)
                {
                    Console.WriteLine(item.Value + "\n");
                }
                
            } while (validacaoConsulta.DicionarioErrosConsulta.Count != 0);

            return DateTime.ParseExact(dadosConsulta[0] + " " + dadosConsulta[1], "dd/MM/yyyy HHmm", new CultureInfo("pt-BR"), DateTimeStyles.None);
        }
    }
}
