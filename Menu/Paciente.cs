using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaConsultorio
{
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

        public Consulta getProxConsulta()
        {
            return Consultas.ElementAt(Consultas.Count - 1);
        }        

        public Paciente(String novoNome, long novoCpf, DateTime novaDtNascimento)
        {
            this.Nome = novoNome;
            this.Cpf = novoCpf;
            this.DtNascimento = novaDtNascimento;
            this.Consultas = new List<Consulta>();
        }
                
        public void addConsulta(Consulta consulta)
        {
            this.Consultas.Add(consulta);
        }

        public void removeTodasConsultas()
        {
            Consultas.Clear();
        }

        public void removeConsulta(Consulta consulta)
        {
            this.Consultas.Remove(consulta);
        }

        public bool temConsultaFutura()
        {
            if (Consultas != null)
            {
                foreach (Consulta consulta in Consultas)
                {
                    if (consulta.DtHrInicio > DateTime.Now)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

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

            if (this.temConsultaFutura())
            {
                Consulta consulta = this.getProxConsulta();

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
