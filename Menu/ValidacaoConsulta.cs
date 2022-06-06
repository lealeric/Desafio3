using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AgendaConsultorio
{
    /// <summary>
    /// Valida os dados do agendamento de consulta.
    /// </summary>
    public class ValidacaoConsulta
    {
        private DateTime Aberto;
        private DateTime Fechado;

        public Dictionary<String, String> DicionarioErrosConsulta
        {
            get;
        }

        /// <summary>
        /// Cria uma instância de validação da consulta.
        /// </summary>
        /// <param name="paciente"></param>
        /// <param name="consultas"></param>
        /// <param name="data"></param>
        /// <param name="hrInicial"></param>
        /// <param name="hrFinal"></param>
        public ValidacaoConsulta(Paciente paciente, List<Consulta> consultas, String data, String hrInicial, String hrFinal)
        {
            DicionarioErrosConsulta = new Dictionary<string, string>();

            validaPaciente(paciente);
            
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
                validaIntervalos(dtHrInicial, dtHrFinal, consultas);
                
            }            
        }

        /// <summary>
        /// Cria uma instância de validaçãoapenas para a data/hora inicial da consulta.
        /// </summary>
        /// <param name="dataInicial"></param>
        /// <param name="horaInicial"></param>
        public ValidacaoConsulta(String dataInicial, String horaInicial)
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

        private void validaPaciente(Paciente paciente)
        {
            if (paciente == null)
            {
                DicionarioErrosConsulta.Add("Paciente", "Paciente não cadastrado!\n");

                return;
            }
            else if (paciente.retornaProximaConsulta() != null)
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

        private void validaIntervalos(DateTime dtHrInicio, DateTime dtHrFim, List<Consulta> consultas)
        {
            if (dtHrInicio >= dtHrFim)
            {
                DicionarioErrosConsulta.Add("Inicial >= Final", "Hora do fim da consulta deve ser após o horário inicial do agendamento!\n");

                return;
            }
            if(temIntersecao(dtHrInicio, dtHrFim, consultas))
            {
                DicionarioErrosConsulta.Add("Sobreposição", "Existe conflito de horário com outro agendamento!\n");

                return;
            }
        }

        private bool temIntersecao(DateTime dtHrInicio, DateTime dtHrFim, List<Consulta> consultas)
        {
            foreach (Consulta consulta in consultas)
            {
                if ((dtHrInicio > consulta.DtHrInicio && dtHrInicio <= consulta.DtHrInicio) ||
                    (dtHrFim > consulta.DtHrInicio && dtHrFim <= consulta.DtHrFim) ||
                    dtHrFim == consulta.DtHrInicio || dtHrInicio == consulta.DtHrFim)
                {
                    return true;
                }
            }            

            return false;
        }



    }
}
