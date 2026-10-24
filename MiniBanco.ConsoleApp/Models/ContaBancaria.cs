using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBanco.ConsoleApp.Models
{
    public abstract class ContaBancaria
    {
        public string NumeroConta { get; private set; }
        public string NomeTitular { get; private set; }
        public string CpfTitular { get; private set; }
        public decimal SaldoConta { get; protected set; }
        public List<Transacao> Transacoes { get; private set; }

        protected ContaBancaria(string numeroConta, string nomeTitular, string cpfTitular, decimal saldoConta)
        {
            NumeroConta = numeroConta;
            NomeTitular = nomeTitular;
            CpfTitular = cpfTitular;
            SaldoConta = saldoConta;
            Transacoes = new List<Transacao>();
        }

        public virtual void Depositar(decimal valor)
        {
            if (valor <= 0) throw new ArgumentException("   Valor deve ser maior que zero.");
            SaldoConta += valor;
            Transacoes.Add(new Transacao(TipoTransacao.Deposito, valor, "Depósito"));
        }

        public virtual bool Sacar(decimal valor)
        {
            if (valor <= 0 || SaldoConta < valor) return false;

            SaldoConta -= valor;
            Transacoes.Add(new Transacao(TipoTransacao.Saque, valor, "Saque"));
            return true;
        }

        public virtual void AplicarFechamentoMensal() { }

        public virtual void AplicarRendimento() { }

        public abstract string ObterTipoConta();

        public string ObterExtrato()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"   Extrato - Conta {NumeroConta} - Titular: {NomeTitular}");
            sb.AppendLine($"   Saldo atual: {SaldoConta:C}");
            sb.AppendLine("----------------------------------------------------------");
            foreach (var t in Transacoes)
            {
                string descricao = string.IsNullOrWhiteSpace(t.Descricao) ? t.Tipo.ToString() : t.Descricao;
                sb.AppendLine($"   {t.Data:G} - {descricao} - {t.Tipo} - {t.Valor:C}");
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"   {NumeroConta} - {NomeTitular} - {ObterTipoConta()} - {SaldoConta:C}";
        }
    }
}
