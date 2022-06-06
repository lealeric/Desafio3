using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace AgendaConsultorio
{
    /// <summary>
    /// Valida os dados de entrada do paciente.
    /// </summary>

    public class ValidacaoPaciente
    {
        public Dictionary<String, String> DicionarioErrosPaciente
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
        public ValidacaoPaciente (Paciente paciente, String nome, String cpf, String dtNascimento)
        {
            DicionarioErrosPaciente = new Dictionary<string, string>();

            validaNome(nome);
            validaCpf(cpf, paciente);
            validaNascimento(dtNascimento);
        }
        private void validaNome(String nome)
        {
            if (nome.Length < 5)
            {
                DicionarioErrosPaciente.Add("Nome", "Nome muito curto.\n");
            }
        }
        private void validaCpf(String cpf, Paciente paciente)
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
        private bool testeCpfIgual(String s)
        {
            for (int i = 1; i < s.Length; i++)
                if (s[i] != s[0])
                    return false;

            return true;
        }
        private bool testeDigito1Cpf(String s)
        {
            bool saida = false;
            int soma = 0;

            for (int i = 0, j = 10; i <= 8; i++, j--)
            {
                soma += (int.Parse(s[i].ToString()) * j);
            }

            int resto = soma % 11;

            if (resto <= 1)
            {
                if (int.Parse(s[9].ToString()) == 0) { saida = true; }
            }
            else
            {
                if (int.Parse(s[9].ToString()) == (11 - resto)) { saida = true; }
            }

            return saida;
        }
        private bool testeDigito2Cpf(String s)
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
                if (int.Parse(s[10].ToString()) == (11 - resto)) { saida = true; }
            }

            return saida;
        }
        private void validaNascimento(String dataNascimento)
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
    }
}
