using System.Globalization;
using AgendaConsultorio.Model;

namespace AgendaConsultorio.Validacao
{
    /// <summary>
    /// Valida os dados referentes ao paciente.
    /// </summary>

    public class ValidacaoPaciente
    {
        public Dictionary<string, string> DicionarioErrosPaciente
        {
            get;
        }

        /// <summary>
        /// Cria uma instância de validação dos dados do paciente.
        /// </summary>
        /// <param name="paciente"></param>
        /// <param name="nome"></param>
        /// <param name="cpf"></param>
        /// <param name="dtNascimento"></param>
        public ValidacaoPaciente(Paciente paciente, string nome, string cpf, string dtNascimento)
        {
            DicionarioErrosPaciente = new Dictionary<string, string>();

            validaNome(nome);
            validaCpf(cpf, paciente);
            validaNascimento(dtNascimento);
        }
        public static String[] dadosPacienteValidos(Agenda agenda)
        {
            ValidacaoPaciente validacaoPaciente;
            Dictionary<string, string> dicErrosPaciente;
            string[] dadosEntradaPaciente = new string[3];

            do
            {
                dadosEntradaPaciente = Interface.Interface.solicitaDadosPaciente(dadosEntradaPaciente);
                validacaoPaciente = new ValidacaoPaciente(agenda.retornaPaciente(dadosEntradaPaciente[1]), dadosEntradaPaciente[0],
                    dadosEntradaPaciente[1], dadosEntradaPaciente[2]);
                dicErrosPaciente = validacaoPaciente.DicionarioErrosPaciente;

                foreach (KeyValuePair<string, string> item in dicErrosPaciente)
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

                    Console.WriteLine(string.Format("Erro: {0}\n", item.Value));
                }
            } while (dicErrosPaciente.Count > 0);

            return dadosEntradaPaciente;
        }
        private void validaNome(string nome)
        {
            if (nome.Length < 5)
            {
                DicionarioErrosPaciente.Add("Nome", "Nome muito curto.\n");
            }
        }
        private void validaCpf(string cpf, Paciente paciente)
        {
            if (cpf.Length != 11 || testeCpfIgual(cpf) || !testeDigito1Cpf(cpf) || !testeDigito2Cpf(cpf))
            {
                DicionarioErrosPaciente.Add("CPF", "CPF inválido.\n");
            }
            else if (paciente != null)
            {
                DicionarioErrosPaciente.Add("CPF", "Paciente já cadastrado.\n");
            }
        }
        private bool testeCpfIgual(string s)
        {
            for (int i = 1; i < s.Length; i++)
                if (s[i] != s[0])
                    return false;

            return true;
        }
        private bool testeDigito1Cpf(string s)
        {
            bool saida = false;
            int soma = 0;

            for (int i = 0, j = 10; i <= 8; i++, j--)
            {
                soma += int.Parse(s[i].ToString()) * j;
            }

            int resto = soma % 11;

            if (resto <= 1)
            {
                if (int.Parse(s[9].ToString()) == 0) { saida = true; }
            }
            else
            {
                if (int.Parse(s[9].ToString()) == 11 - resto) { saida = true; }
            }

            return saida;
        }
        private bool testeDigito2Cpf(string s)
        {
            bool saida = false;
            int soma = 0;

            for (int i = 0, j = 11; i <= 9; i++, j--)
            {
                soma += int.Parse(s[i].ToString()) * j;
            }

            int resto = soma % 11;

            if (resto <= 1)
            {
                if (int.Parse(s[10].ToString()) == 0) { saida = true; }
            }
            else
            {
                if (int.Parse(s[10].ToString()) == 11 - resto) { saida = true; }
            }

            return saida;
        }
        private void validaNascimento(string dataNascimento)
        {
            DateTime data;

            bool dataNascimentoValida = DateTime.TryParseExact(dataNascimento, "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out data);

            if (!dataNascimentoValida)
            {
                DicionarioErrosPaciente.Add("Data de Nascimento", "Data de nascimento em formato inválido!\n");
            }
            else
            {
                int anos = (int)((DateTime.Now - data).TotalDays / 365.25);

                if (anos < 13)
                {
                    DicionarioErrosPaciente.Add("Data de Nascimento", "Paciente tem apenas " + anos.ToString() + " anos!\n");
                }
            }
        }
        public static bool validaRemocaoPaciente(Paciente paciente, IList<Consulta> consultas)
        {
            if (paciente == null)
            {
                Console.WriteLine("Não é possível remover pacientes não cadastrados!\n");
                return false;
            }

            /*if (paciente.retornaProximaConsulta() != null)
            {
                Console.WriteLine("Não é possível remover pacientes com agendamentos futuros!\n");
                return false;
            }*/

            /*if (paciente.Consultas != null)
            {
                Consulta consultaPaciente, consultaAgenda;
                for (int i = 0; i < paciente.Consultas.Count; i++)
                {
                    consultaPaciente = paciente.Consultas[i];

                    for (int j = 0; j < consultas.Count; j++)
                    {
                        consultaAgenda = consultas[j];
                        if (consultaPaciente.Equals(consultaAgenda))
                        {
                            consultas.Remove(consultaAgenda);
                        }
                    }
                }
                paciente.removeTodasConsultas();
            }*/

            return true;
        }
    }
}
