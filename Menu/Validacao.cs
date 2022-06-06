using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AgendaConsultorio
{
    public class Validacao
    {
        public InvalidDataException Excecao;
        
        public Validacao(int id, String valorEntrada) 
        {
            this.Excecao = validaData(valorEntrada);
        }

        public InvalidDataException validaData(String dataNascimento)
        {
            if (dataNascimento[2] != '/' || dataNascimento[5] != '/')
            {
                return new InvalidDataException("Formato de data inválido.");
            }
            else
            {
                String[] subs = dataNascimento.Split('/');
                int dia, mes, ano;
                dia = Convert.ToInt32(subs[0]);
                mes = Convert.ToInt32(subs[1]);
                ano = Convert.ToInt32(subs[2]);

                if (ano < 1900 || ano > DateTime.Now.Year)
                {
                    return new InvalidDataException("Ano inválido.");
                }

                if (mes < 1 || mes > 12)
                {
                    return new InvalidDataException("Mês inválido.");
                }

                if (dia < 1)
                {
                    return new InvalidDataException("Dia inválido.");
                }
                else
                {
                    switch (mes)
                    {
                        case 2:
                            if ((DateTime.IsLeapYear(ano) && dia > 29) || (!DateTime.IsLeapYear(ano) && dia > 28))
                            {
                                return new InvalidDataException("Dia inválido.");
                            }
                            break;
                        case 1:
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                        case 12:
                            if (dia > 31)
                            {
                                return new InvalidDataException("Dia inválido.");
                            }
                            break;
                        case 4:
                        case 6:
                        case 9:
                        case 11:
                            if (dia > 30)
                            {
                                return new InvalidDataException("Dia inválido.");
                            }
                            break;
                    }

                }

            }

            return null;
        }

        public InvalidDataException validaNascimento(string dataNascimento)
        {
            String padrao = "dd/MM/yyyy";

            DateTime data = DateTime.ParseExact(dataNascimento, padrao, CultureInfo.InvariantCulture);

            TimeSpan diferenca = DateTime.Now - data;

            int anos = (int)(diferenca.TotalDays / 365.25);

            if (anos < 13)
            {
                return new InvalidDataException("Paciente tem apenas " + anos.ToString() + " anos.");
            }

            return null;
        }

    }
}
