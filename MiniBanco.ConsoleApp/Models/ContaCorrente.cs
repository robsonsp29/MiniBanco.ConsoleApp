using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaCorrente : ContaBancaria
    {
        private const decimal TarifaManutencao = 10m;

        public decimal LimiteChequeEspecial { get; private set; }

        public ContaCorrente(string numeroConta, string nomeTitular, string cpfTitular, decimal saldoConta, decimal limite) : base(numeroConta, nomeTitular, cpfTitular, saldoConta)
        {
            if (limite < 0) throw new ArgumentOutOfRangeException(nameof(limite));
            LimiteChequeEspecial = limite;
        }

        public override bool Sacar(decimal valor)
        {
            if (valor <= 0 || valor > SaldoConta + LimiteChequeEspecial) return false;

            SaldoConta -= valor;
            Transacoes.Add(new Transacao(TipoTransacao.Saque, valor, "Saque"));
            return true;
        }

        public override void AplicarFechamentoMensal()
        {
            SaldoConta -= TarifaManutencao;
            Transacoes.Add(new Transacao(TipoTransacao.Tarifa, TarifaManutencao, "Tarifa de manutenção"));
        }

        public override string ObterTipoConta() => "Conta Corrente";

    }
}

