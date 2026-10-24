using System;
using System.Collections.Generic;
using System.Text;

namespace MiniBanco.ConsoleApp.Models
{
    public class ContaSalario : ContaBancaria
    {
        private const decimal TarifaSaqueExcedente = 5m;
        private int saquesNoMes;

        public string Empregador { get; private set; }
        public int LimiteSaquesMensais { get; private set; }
        public int SaquesNoMes => saquesNoMes;

        public ContaSalario(string numeroConta, string nomeTitular, string cpfTitular, decimal saldoConta, string empregador, int limiteSaquesMensais) : base(numeroConta, nomeTitular, cpfTitular, saldoConta)
        {
            if (string.IsNullOrWhiteSpace(empregador)) throw new ArgumentException("O empregador é obrigatório.", nameof(empregador));
            if (limiteSaquesMensais < 0) throw new ArgumentOutOfRangeException(nameof(limiteSaquesMensais));

            Empregador = empregador;
            LimiteSaquesMensais = limiteSaquesMensais;
        }

        public override bool Sacar(decimal valor)
        {
            if (valor <= 0) return false;

            bool excedeuLimite = saquesNoMes >= LimiteSaquesMensais;
            decimal tarifa = excedeuLimite ? TarifaSaqueExcedente : 0m;
            if (SaldoConta < valor + tarifa) return false;

            SaldoConta -= valor + tarifa;
            Transacoes.Add(new Transacao(TipoTransacao.Saque, valor, "Saque"));
            saquesNoMes++;
            if (tarifa > 0)
            {
                Transacoes.Add(new Transacao(TipoTransacao.Tarifa, tarifa, "Tarifa por saque excedente"));
            }
            return true;
        }

        public override void AplicarFechamentoMensal()
        {
            saquesNoMes = 0;
        }

        public override string ObterTipoConta() => "Conta Salário";
    }
}
