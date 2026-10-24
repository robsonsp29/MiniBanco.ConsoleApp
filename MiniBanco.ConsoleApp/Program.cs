using MiniBanco.ConsoleApp.Models;
using MiniBanco.ConsoleApp.Services;
using System;
using System.Collections.Generic;
namespace MiniBanco.ConsoleApp;

public class Program
{
    public static List<ContaBancaria> contas = new();
    public static int nextAccountNumber = 1;

    public static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("                                                                                        [M1S07] Mini-Projeto Avaliativo ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("========================================================================================================================");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("                                           Motor de  Gestão de Contas Bancárias                                         ");
            Console.WriteLine("                                                 ( MiniBanco.ConsoleApp )                                               ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("========================================================================================================================");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("  Selecione a Opção Desejada abaixo:                                                                                    ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("     1 - Criar uma Nova Conta                                                                                           ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("     2 - Fazer um Depósito                                                                                              ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("     3 - Realizar un Sacar                                                                                              ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("     4 - Gerar um Extrato                                                                                               ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("     5 - Listar Contas Ativas                                                                                           ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("     6 - Relatório de Saúde das Contas                                                                                  ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("     7 - Sair da Conta                                                                                                  ");
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine("                                                                                                                        ");            
            Console.WriteLine("                                                                                                                        ");
            Console.Write("  Digite a opção desejada: ");

            if (!int.TryParse(Console.ReadLine(), out int opcao))
            {
                Console.WriteLine("  Opção inválida, digite uma opção válida.");
                Console.WriteLine();
                Console.WriteLine("  Pressione Enter para continuar...");
                Console.ReadLine();
                continue;
            }

            switch (opcao)
            {
                case 1: CriarConta(); break;
                case 2: RealizarDeposito(); break;
                case 3: RealizarSaque(); break;
                case 4: MostrarExtrato(); break;
                case 5: ListarContas(); break;
                case 6: RelatorioSaudeContas.Imprimir(contas); break;
                case 7: Console.WriteLine("  Saindo..."); return;
                default: break;
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("  " + "  Pressione Enter para continuar...");
            Console.ReadLine();
        }
    }

    public static string GerarNumeroConta() => (nextAccountNumber++).ToString("D6");    
    public static void CriarConta()
    {
        Console.Clear();
        Console.WriteLine("                                                                                        [M1S07] Mini-Projeto Avaliativo ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                        Motor de Gestão de Contas Bancárias                                             ");
        Console.WriteLine("                                               ( Cadastro de Conta )                                                    ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        Console.Write("    Digite o Nome do Titular: "); string nome = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("                                                                                                                        ");
        Console.Write("    Digite o CPF do Titular: "); string cpf = Console.ReadLine() ?? string.Empty;
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("    Selecione o Tipo de Conta Desejada:                                                                                 ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("        - Digite 1 para Conta Corrente                                                                                  ");
        Console.WriteLine("        - Digite 2 para Conta Poupança                                                                                  ");
        Console.WriteLine("        - Digite 3 para Conta Salário                                                                                   ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        Console.Write("    Escolha o Tipo de Conta: ");

        if (!int.TryParse(Console.ReadLine(), out int tipo) || tipo < 1 || tipo > 3)
        {
            Console.WriteLine("  Tipo inválido.");
            return;
        }
        Console.WriteLine("");
        Console.Write("    Saldo inicial: ");
        decimal saldoInicial = 0m;
        decimal.TryParse(Console.ReadLine(), out saldoInicial);

        string numero = GerarNumeroConta();
        ContaBancaria conta;

        if (tipo == 1)
        {
            Console.WriteLine("");
            Console.Write("    Limite do cheque especial: ");
            decimal.TryParse(Console.ReadLine(), out decimal limite);
            conta = new ContaCorrente(numero, nome, cpf, saldoInicial, limite);
            Console.WriteLine("");
        }
        else if (tipo == 2)
        {
            Console.Write("    Taxa de rendimento mensal (%): ");
            double.TryParse(Console.ReadLine(), out double taxaRendimento);
            Console.Write("    Dia de aniversário (1 a 31): ");
            int.TryParse(Console.ReadLine(), out int diaAniversario);
            conta = new ContaPoupanca(numero, nome, cpf, saldoInicial, taxaRendimento, diaAniversario);
        }
        else
        {
            Console.Write("    Empregador: ");
            string empregador = Console.ReadLine() ?? string.Empty;
            Console.Write("    Limite de saques mensais: ");
            int.TryParse(Console.ReadLine(), out int limiteSaques);
            conta = new ContaSalario(numero, nome, cpf, saldoInicial, empregador, limiteSaques);
        }

        contas.Add(conta);
        Console.WriteLine($"    Conta criada: {conta.NumeroConta} - {conta.NomeTitular}");
    }

    static ContaBancaria? BuscarConta()
    {
        Console.Write("   Número da conta: ");
        string input = (Console.ReadLine() ?? string.Empty).Trim();

        string numero = input;
        if (int.TryParse(input, out int numeric))
        {
            numero = numeric.ToString("D6");
        }

        ContaBancaria? conta = null;
        foreach (ContaBancaria item in contas)
        {
            if (item.NumeroConta == numero)
            {
                conta = item;
                break;
            }
        }

        if (conta == null)
        {
            Console.WriteLine();
            Console.WriteLine("   Conta não encontrada.");
            return null;
        }

        Console.WriteLine();
        Console.WriteLine($"   Conta encontrada: {conta.NumeroConta} - {conta.NomeTitular} - {conta.SaldoConta:C}");
        return conta;
    }

    static void RealizarDeposito()
    {
        Console.Clear();
        Console.WriteLine("                                                                                        [M1S07] Mini-Projeto Avaliativo ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                      Motor de  Gestão de Contas Bancárias                                              ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                              ( Realizar Depósito )                                                     ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        var conta = BuscarConta();
        if (conta == null) return;
        Console.WriteLine("");
        Console.Write("   Valor a depositar: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal valor))
        {
            Console.WriteLine("");
            Console.WriteLine("   Valor inválido.");
            return;
        }

        try
        {
            conta.Depositar(valor);
            Console.WriteLine("");
            Console.WriteLine("   Depósito realizado com sucesso.");
            Console.WriteLine("");
            Console.WriteLine($"   Saldo atual: {conta.SaldoConta:C}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"   Erro: {ex.Message}");
        }
    }


    static void RealizarSaque()
    {
        Console.Clear();
        Console.WriteLine("                                                                                        [M1S07] Mini-Projeto Avaliativo ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                        Motor de Gestão de Contas Bancárias                                             ");
        Console.WriteLine("                                                 ( Efetuar Saque )                                                      ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");

        var conta = BuscarConta();
        if (conta == null) return;
        Console.WriteLine("");
        Console.Write("   Valor a sacar: ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal valor)) { Console.WriteLine("   Valor inválido."); return; }
        bool resultado = conta.Sacar(valor);
        Console.WriteLine(resultado ? "   Saque realizado com sucesso." : "   Saldo insuficiente.");
    }

    static void MostrarExtrato()
    {
        Console.Clear();
        Console.WriteLine("                                                                                        [M1S07] Mini-Projeto Avaliativo ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                        Motor de Gestão de Contas Bancárias                                             ");
        Console.WriteLine("                                                   ( Extrato )                                                          ");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("========================================================================================================================");
        Console.WriteLine("                                                                                                                        ");
        Console.WriteLine("                                                                                                                        ");
        var conta = BuscarConta();
        if (conta == null) return;
        Console.WriteLine();
        Console.WriteLine(conta.ObterExtrato());
    }

    static void ListarContas()
    {
        if (contas.Count == 0) { Console.WriteLine("   Nenhuma conta cadastrada."); return; }
        Console.WriteLine("   Contas cadastradas:");
        foreach (var c in contas)
        {
            Console.WriteLine("                                                                                                                        ");
            Console.WriteLine($"   {c.NumeroConta} - {c.NomeTitular} - {c.SaldoConta:C} - {c.GetType().Name}");
        }
    }
}
