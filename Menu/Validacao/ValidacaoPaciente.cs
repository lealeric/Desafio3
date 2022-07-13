using System.Globalization;
using AgendaConsultorio.Database;
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

        /// <summary>
        /// Valida os dados sobre o paciente inseridos pelo usuário.
        /// </summary>
        /// <param name="pacienteDAO">Contexto dos pacientes no banco de dados.</param>
        /// <returns>Um array com os dados validados.</returns>
        public static String[] dadosPacienteValidos(PacienteDAO pacienteDAO)
        {
            ValidacaoPaciente validacaoPaciente;
            Dictionary<string, string> dicErrosPaciente;
            string[] dadosEntradaPaciente = new string[3];

            do
            {
                dadosEntradaPaciente = Interface.Interface.solicitaDadosPaciente(dadosEntradaPaciente);
                validacaoPaciente = 
                    new ValidacaoPaciente(pacienteDAO.recuperaPaciente(Convert.ToInt64(dadosEntradaPaciente[1])),
                    dadosEntradaPaciente[0], dadosEntradaPaciente[1], dadosEntradaPaciente[2]);
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

        /// <summary>
        /// Confere se um paciente pode ser removido.
        /// </summary>
        /// <param name="pacienteDAO">Contexto dos pacientes no banco de dados.</param>
        /// <param name="paciente">Paciente a ser removido.</param>
        /// <returns>Verdadeiro se for possível a remoção e falso, caso contrário.</returns>
        public static bool validaRemocaoPaciente(PacienteDAO pacienteDAO, Paciente paciente)
        {
            if (paciente == null)
            {
                Console.WriteLine("Não é possível remover pacientes não cadastrados!\n");
                return false;
            }

            Consulta proxConsulta = pacienteDAO.retornaProximaConsulta(paciente.Id);

            if (proxConsulta != null)
            {
                Console.WriteLine("Não é possível remover pacientes com agendamentos futuros!\n");
                return false;
            }

            return true;
        }
    }
}
