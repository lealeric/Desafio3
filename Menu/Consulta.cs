using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AgendaConsultorio
{
    public class Consulta : IComparable<Consulta>
    {
        public Paciente Paciente { get; private set; }

        public DateTime DtHrInicio { get; private set; }

        public DateTime DtHrFim { get; private set; }

        private TimeSpan DuracaoConsulta
        {
            get
            {
                return DtHrFim - DtHrInicio;
            }
        }

        public Consulta(Paciente paciente, DateTime dtHrInicio, DateTime dtHrFim)
        {
            DtHrInicio = dtHrInicio;
            DtHrFim = dtHrFim;
            Paciente = paciente;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || !obj.GetType().Equals(this.GetType()))
            {
                return false;
            }

            Consulta other = (Consulta)obj;
            return this.DtHrInicio.Equals(other.DtHrInicio);
        }

        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3} {4}", DtHrInicio.ToString("HH:mm"), DtHrFim.ToString("HH:mm"), DuracaoConsulta.ToString("hh\\:mm"),
                                Paciente.Nome.PadRight(20), Paciente.DtNascimento.ToString("dd/MM/yyyy"));
        }

        public int CompareTo(Consulta other)
        {
            return this.DtHrInicio.CompareTo(other.DtHrInicio);
        }
    }
}
