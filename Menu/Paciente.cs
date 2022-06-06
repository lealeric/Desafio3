using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaConsultorio
{
    /// <summary>
    /// Define um novo paciente e gerencia pacientes existentes.
    /// </summary>
    public class Paciente
    {
        public String Nome { get; }
        public long Cpf { get; }
        public DateTime DtNascimento { get; }        
        public List<Consulta> Consultas
        {
            get;
        }
        public int Idade
        {
            get 
            {
                return (int) ((DateTime.Now - DtNascimento).TotalDays / 365.25); 
            }
        }
       
        /// <summary>
        /// Cria uma nova instância de paciente com nome, CPF e data de nascimento.
        /// </summary>
        /// <param name="novoNome">Representa a propriedade <see cref="Nome"/> e deve ter mais que 5 caracteres</param>
        /// <param name="novoCpf">Representa a propriedade <see cref="Cpf" /> e deve atender às regras de definição do CPF</param>
        /// <param name="novaDtNascimento">Representa a propriedade <see cref="DtNascimento"/> e deve ser uma data válida para uma pessoa com 13 anos ou mais.</param>
        public Paciente(String novoNome, long novoCpf, DateTime novaDtNascimento)
        {
            this.Nome = novoNome;
            this.Cpf = novoCpf;
            this.DtNascimento = novaDtNascimento;
            this.Consultas = new List<Consulta>();
        }                

        /// <summary>
        /// Adiciona um novo agendamento de consulta ao paciente.
        /// </summary>
        /// <param name="consulta">Representa um elemento da propriedade <see cref="Consultas"/>.</param>
        public void addConsulta(Consulta consulta)
        {
            this.Consultas.Add(consulta);
        }

        /// <summary>
        /// Remove todos os agendamentos passados do paciente.
        /// </summary>
        public void removeTodasConsultas()
        {
            Consultas.Clear();
        }

        /// <summary>
        /// Remove uma consulta do paciente, quando o agendamento for cancelado.
        /// </summary>
        /// <param name="consulta">Consulta cancelada a remover da lista do paciente.</param>
        public void removeConsulta(Consulta consulta)
        {
            this.Consultas.Remove(consulta);
        }

        /// <summary>
        /// Verifica a existência de um agendamento futuro para o paciente.
        /// </summary>
        /// <returns>Verdadeiro se existe uma consulta após a data/hora atual, caso contrário retorna falso.</returns>
        public Consulta retornaProximaConsulta()
        {
            foreach (Consulta consulta in Consultas)
            {
                if (consulta.DtHrInicio > DateTime.Now)
                {
                    return consulta;
                }
            }            

            return null;
        }

        /// <summary>
        /// Pesquisa um agendamento baseado em data/hora específica.
        /// </summary>
        /// <param name="dtHrInicio">Data/hora inicial da consulta.</param>
        /// <param name="dtHrFim">Data/hora final da consulta.</param>
        /// <returns>Consulta no intervalo de tempo informado, caso contrário retorna null.</returns>
        public Consulta retornaConsulta(DateTime dtHrInicio, DateTime dtHrFim)
        {
            foreach (Consulta consulta in Consultas)
            {
                if (consulta.DtHrInicio.Equals(dtHrInicio) && consulta.DtHrFim.Equals(dtHrFim))
                {
                    return consulta;
                }
            }
            return null;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Paciente)) return false;

            Paciente other = (Paciente) obj;
            return this.Cpf.Equals(other.Cpf);
        }

        public override string ToString()
        {
            String saidaPrint = String.Format("{0} {1} {2} {3}", Cpf.ToString().PadRight(11), Nome.PadRight(32),
                DtNascimento.ToString("dd/MM/yyyy"), Idade.ToString().PadLeft(4));

            if (retornaProximaConsulta() != null)
            {
                Consulta consulta = this.retornaProximaConsulta();

                saidaPrint = String.Format( "{0} {1} {2} {3}\n" +
                                            "{4} Agendado para: {5}\n" +
                                            "{6} {7} às {8}", Cpf.ToString().PadRight(11), Nome.PadRight(32),
                                            DtNascimento.ToString("dd/MM/yyyy"), Idade.ToString().PadLeft(4),"".PadRight(11), consulta.DtHrInicio.ToString("dd/MM/yyy"),
                                            "".PadRight(11), consulta.DtHrInicio.ToString("HH:mm"), consulta.DtHrFim.ToString("HH:mm"));
            }

            return saidaPrint;
        }
    }
}
