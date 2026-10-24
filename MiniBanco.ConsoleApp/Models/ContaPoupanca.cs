using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaPoupanca : ContaBancaria
    {
        public double TaxaRendimentoMensal { get; private set; }
        public int DiaAniversario { get; private set; }

        public ContaPoupanca(string numeroConta, string nomeTitular, string cpfTitular, decimal saldoConta, double taxaRendimentoMensal = 0.5, int diaAniversario = 1) : base(numeroConta, nomeTitular, cpfTitular, saldoConta)
        {
            if (taxaRendimentoMensal < 0 || double.IsNaN(taxaRendimentoMensal) || double.IsInfinity(taxaRendimentoMensal))
                throw new ArgumentOutOfRangeException(nameof(taxaRendimentoMensal));
            if (diaAniversario < 1 || diaAniversario > 31)
                throw new ArgumentOutOfRangeException(nameof(diaAniversario));

            TaxaRendimentoMensal = taxaRendimentoMensal;
            DiaAniversario = diaAniversario;
        }

        public override void AplicarFechamentoMensal()
        {
            decimal rendimento = Math.Round(SaldoConta * (decimal)TaxaRendimentoMensal / 100m, 2);
            if (rendimento > 0)
            {
                SaldoConta += rendimento;
                Transacoes.Add(new Transacao(TipoTransacao.Deposito, rendimento, "Rendimento mensal"));
            }
        }

        public override void AplicarRendimento() => AplicarFechamentoMensal();

        public override string ObterTipoConta() => "Conta Poupança";
    }
}
