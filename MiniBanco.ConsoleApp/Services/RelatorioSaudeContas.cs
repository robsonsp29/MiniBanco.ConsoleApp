using System.Globalization;
using MiniBanco.ConsoleApp.Models;

namespace MiniBanco.ConsoleApp.Services;

public static class RelatorioSaudeContas
{
    private static readonly CultureInfo CulturaBrasileira = CultureInfo.GetCultureInfo("pt-BR");

    public static void Imprimir(IReadOnlyList<ContaBancaria> contas)
    {
        Console.WriteLine("MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS");
        Console.WriteLine(new string('=', 68));

        if (contas.Count == 0)
        {
            Console.WriteLine("Nenhuma conta cadastrada para análise.");
            return;
        }

        for (int indice = 0; indice < contas.Count; indice++)
        {
            ImprimirConta(contas[indice], indice + 1, contas.Count);
        }

        Console.WriteLine(new string('=', 68));
        Console.WriteLine("FIM DO PROCESSAMENTO DE CONTAS");
    }

    private static void ImprimirConta(ContaBancaria conta, int indice, int totalContas)
    {
        int pontuacaoObtida = 0;
        int quantidadeTarifas = 0;
        int quantidadeSaques = 0;

        foreach (Transacao transacao in conta.Transacoes)
        {
            if (transacao.Tipo == TipoTransacao.Deposito)
            {
                pontuacaoObtida += 10;
            }
            else if (transacao.Tipo == TipoTransacao.Saque)
            {
                pontuacaoObtida += 5;
                quantidadeSaques++;
            }
            else if (transacao.Tipo == TipoTransacao.Tarifa)
            {
                quantidadeTarifas++;
            }
        }

        int pontuacaoMaxima = conta.Transacoes.Count * 10;
        double percentual = pontuacaoMaxima == 0
            ? 0
            : (double)pontuacaoObtida / pontuacaoMaxima * 100;

        string classificacao;
        string acao;
        if (percentual >= 90)
        {
            classificacao = "CONTA SAUDÁVEL";
            acao = "Oferecer investimentos e avaliar aumento de limite.";
        }
        else if (percentual >= 60)
        {
            classificacao = "CONTA EM ATENÇÃO";
            acao = "Contatar o cliente para orientação financeira e revisar tarifas.";
        }
        else
        {
            classificacao = "CONTA EM RISCO";
            acao = "Bloquear aumento de limite e abrir análise de crédito.";
        }

        Console.WriteLine();
        Console.WriteLine($"[{indice}/{totalContas}] PROCESSANDO CONTA");
        Console.WriteLine("DADOS DA CONTA:");
        Console.WriteLine($"• Tipo: {conta.ObterTipoConta()}");
        Console.WriteLine($"• Titular: {conta.NomeTitular} | Conta: {conta.NumeroConta}");
        Console.WriteLine($"• Atributo Específico: {ObterAtributosEspecificos(conta)}");
        Console.WriteLine($"• Saldo Atual: {conta.SaldoConta.ToString("C", CulturaBrasileira)}");
        Console.WriteLine($"MOVIMENTAÇÕES REGISTRADAS ({conta.Transacoes.Count} TRANSAÇÕES):");

        foreach (Transacao transacao in conta.Transacoes)
        {
            int pontos = ObterPontuacao(transacao.Tipo);
            string indicador = transacao.Tipo == TipoTransacao.Tarifa ? "[ X ]" :
                transacao.Tipo == TipoTransacao.Saque ? "[ ! ]" : "[OK]";
            string descricao = string.IsNullOrWhiteSpace(transacao.Descricao)
                ? transacao.Tipo.ToString()
                : transacao.Descricao;
            Console.WriteLine($"{indicador} {descricao} — {transacao.Tipo}: {transacao.Valor.ToString("C", CulturaBrasileira)} ({pontos} pts)");
        }

        Console.WriteLine("RESUMO DA PONTUAÇÃO:");
        Console.WriteLine($"• Pontuação Atingida: {pontuacaoObtida} de {pontuacaoMaxima} pontos possíveis");
        Console.WriteLine($"• Índice de Saúde: {percentual.ToString("N1", CulturaBrasileira)}%");
        Console.WriteLine($"• Classificação Final: [ {classificacao} ]");
        Console.WriteLine("RELATÓRIO DE PENDÊNCIAS E RECOMENDAÇÕES DA GERÊNCIA:");

        if (quantidadeTarifas == 0 && quantidadeSaques == 0)
        {
            Console.WriteLine("🟢 Nenhuma pendência identificada.");
        }
        else
        {
            if (quantidadeTarifas > 0)
            {
                Console.WriteLine("🔴 ITENS CRÍTICOS (TARIFAS):");
                foreach (Transacao transacao in conta.Transacoes)
                {
                    if (transacao.Tipo == TipoTransacao.Tarifa)
                    {
                        Console.WriteLine($"- {transacao.Descricao}: revisar cobrança e avaliar isenção ou migração de pacote ({transacao.Valor.ToString("C", CulturaBrasileira)}).");
                    }
                }
            }

            if (quantidadeSaques > 0)
            {
                Console.WriteLine("🟡 ITENS DE ATENÇÃO (SAQUES):");
                foreach (Transacao transacao in conta.Transacoes)
                {
                    if (transacao.Tipo == TipoTransacao.Saque)
                    {
                        Console.WriteLine($"- Saque de {transacao.Valor.ToString("C", CulturaBrasileira)}: acompanhar movimentação e contatar o cliente, se necessário.");
                    }
                }
            }
        }

        Console.WriteLine($"Ação prioritária: {acao}");
        if (quantidadeTarifas > 0)
        {
            Console.WriteLine("Recomendação: revisar o pacote de tarifas e negociar cobranças com o cliente.");
        }
        if (quantidadeSaques > 0)
        {
            Console.WriteLine("Recomendação: contatar o cliente para entender a frequência dos saques e avaliar o tipo de conta mais adequado.");
        }
    }

    private static int ObterPontuacao(TipoTransacao tipo)
    {
        if (tipo == TipoTransacao.Deposito) return 10;
        if (tipo == TipoTransacao.Saque) return 5;
        return 0;
    }

    private static string ObterAtributosEspecificos(ContaBancaria conta)
    {
        if (conta is ContaCorrente contaCorrente)
        {
            return $"Limite Cheque Especial: {contaCorrente.LimiteChequeEspecial.ToString("C", CulturaBrasileira)}";
        }
        if (conta is ContaPoupanca contaPoupanca)
        {
            return $"Rendimento: {contaPoupanca.TaxaRendimentoMensal.ToString("N1", CulturaBrasileira)}% ao mês | Aniversário: dia {contaPoupanca.DiaAniversario}";
        }
        if (conta is ContaSalario contaSalario)
        {
            return $"Empregador: {contaSalario.Empregador} | Limite de saques mensais: {contaSalario.LimiteSaquesMensais}";
        }

        return "Nenhum";
    }
}
