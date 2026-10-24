using Xunit;
using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Services;

namespace MiniBanco.ConsoleApp.Tests
{
    public class ContaTests
    {
        [Fact]
        public void Deposito_AumentaSaldo()
        {
            var c = new ContaCorrente("000001","Test","123",100m,50m);
            c.Depositar(50m);
            Assert.Equal(150m, c.SaldoConta);
        }

        [Fact]
        public void Saque_ContaCorrente_UsaChequeEspecialSemTarifaDeSaque()
        {
            var c = new ContaCorrente("000002", "Test", "123", 100m, 50m);
            bool ok = c.Sacar(120m);
            Assert.True(ok);
            Assert.Equal(-20m, c.SaldoConta);
            Assert.Equal(TipoTransacao.Saque, c.Transacoes[0].Tipo);
        }

        [Fact]
        public void Poupanca_AplicaRendimentoComoDepositoNoFechamentoMensal()
        {
            var p = new ContaPoupanca("000003", "Test", "123", 1000m, 0.5, 10);
            p.AplicarFechamentoMensal();
            Assert.Equal(1005m, p.SaldoConta);
            Assert.Equal(TipoTransacao.Deposito, p.Transacoes[0].Tipo);
            Assert.Equal(10, p.DiaAniversario);
        }

        [Fact]
        public void Saque_Insuficiente()
        {
            var p = new ContaPoupanca("000004", "Test", "123", 100m);
            bool ok = p.Sacar(200m);
            Assert.False(ok);
            Assert.Equal(100m, p.SaldoConta);
        }

        [Fact]
        public void ContaCorrente_FechamentoMensalRegistraTarifaDeManutencao()
        {
            var c = new ContaCorrente("000005", "Test", "123", 100m, 0m);
            c.AplicarFechamentoMensal();
            Assert.Equal(90m, c.SaldoConta);
            Assert.Equal(TipoTransacao.Tarifa, c.Transacoes[0].Tipo);
            Assert.Equal(10m, c.Transacoes[0].Valor);
        }

        [Fact]
        public void ContaSalario_CobraTarifaAposLimiteEResetaContagemMensal()
        {
            var c = new ContaSalario("000006", "Test", "123", 100m, "Empresa", 1);
            Assert.True(c.Sacar(10m));
            Assert.True(c.Sacar(10m));
            Assert.Equal(75m, c.SaldoConta);
            Assert.Equal(2, c.SaquesNoMes);
            Assert.Equal(TipoTransacao.Tarifa, c.Transacoes[2].Tipo);

            c.AplicarFechamentoMensal();
            Assert.Equal(0, c.SaquesNoMes);
            Assert.True(c.Sacar(10m));
            Assert.Equal(65m, c.SaldoConta);
        }

        [Fact]
        public void Relatorio_TrataContaSemTransacoesSemDividirPorZero()
        {
            var output = new StringWriter();
            TextWriter outputOriginal = Console.Out;
            try
            {
                Console.SetOut(output);
                RelatorioSaudeContas.Imprimir(new List<ContaBancaria>
                {
                    new ContaPoupanca("000007", "Test", "123", 0m)
                });
            }
            finally
            {
                Console.SetOut(outputOriginal);
            }

            Assert.Contains("Índice de Saúde: 0,0%", output.ToString());
            Assert.Contains("CONTA EM RISCO", output.ToString());
        }

        [Fact]
        public void Relatorio_ClassificaPontuacaoEListaTarifasSeparadamente()
        {
            var conta = new ContaCorrente("000008", "Test", "123", 100m, 0m);
            conta.Depositar(10m);
            conta.Depositar(10m);
            conta.Depositar(10m);
            conta.AplicarFechamentoMensal();
            conta.AplicarFechamentoMensal();

            var output = new StringWriter();
            TextWriter outputOriginal = Console.Out;
            try
            {
                Console.SetOut(output);
                RelatorioSaudeContas.Imprimir(new List<ContaBancaria> { conta });
            }
            finally
            {
                Console.SetOut(outputOriginal);
            }

            Assert.Contains("Pontuação Atingida: 30 de 50 pontos possíveis", output.ToString());
            Assert.Contains("Índice de Saúde: 60,0%", output.ToString());
            Assert.Contains("CONTA EM ATENÇÃO", output.ToString());
            Assert.Contains("ITENS CRÍTICOS (TARIFAS)", output.ToString());
            Assert.Contains("revisar o pacote de tarifas", output.ToString());
        }
    }
}
