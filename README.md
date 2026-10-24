
# MiniBanco.ConsoleApp - [M1S07] Mini-Projeto Avaliativo

Aplicativo de console em C#/.NET para simular operações bancárias básicas e gerar um relatório de saúde das contas. O projeto demonstra conceitos de programação orientada a objetos, 
herança, sobrescrita de métodos, coleções e testes automatizados.

## Visão geral

O sistema permite cadastrar contas correntes, poupanças e salários; registrar depósitos e saques; consultar extratos; listar as contas cadastradas; e analisar as movimentações por meio 
de um relatório de pontuação, classificação e recomendações para a gerência.
Os dados são mantidos em memória durante a execução. Ao encerrar o programa, as contas e movimentações cadastradas não são salvas em arquivo ou banco de dados.

## Requisitos

- .NET SDK 10.0 ou compatível com o target `net10.0` dos projetos.
- Windows, macOS ou Linux com terminal capaz de executar aplicativos .NET.

Confira a instalação do SDK com:

```
bash
dotnet --version
```

## Como executar

Na pasta raiz do repositório, execute:

```
bash
dotnet run --project MiniBanco.ConsoleApp/MiniBanco.ConsoleApp.csproj
```

O menu principal apresenta estas opções:

1. Criar uma nova conta.
2. Fazer um depósito.
3. Realizar um saque.
4. Gerar um extrato.
5. Listar contas ativas.
6. Gerar o relatório de saúde das contas.
7. Sair.

Os números de conta são gerados automaticamente, sequencialmente e com seis dígitos. Os valores monetários são informados como números decimais, conforme a cultura numérica configurada no sistema operacional. Os dados permanecem apenas na memória do processo.


### Telas de exemplo e como utilizá-las

Os exemplos abaixo representam a interação com o terminal. O texto de cabeçalho e o espaçamento podem variar com a largura da janela; os prompts e opções correspondem ao fluxo do aplicativo. Os dados de exemplo são digitados pelo usuário, exceto o número sequencial da conta e as mensagens geradas pelo sistema.


#### 1. Tela do Menu Principal

```
                                 [M1S07] Mini-Projeto Avaliativo


================================================================

                 Motor de Gestão de Contas Bancárias
                      ( MiniBanco.ConsoleApp )

================================================================


Selecione a Opção Desejada abaixo:

  1 - Criar uma Nova Conta
  2 - Fazer um Depósito
  3 - Realizar um Saque
  4 - Gerar um Extrato
  5 - Listar Contas Ativas
  6 - Relatório de Saúde das Contas
  7 - Sair

Digite a opção desejada:

```


Ao iniciar, o programa apresenta esse menu e aguarda que seja digitado o número de uma opção. Uma opção inválida ou uma entrada que não seja numérica não executa uma operação; 
o programa informa o erro e volta ao menu. Depois de cada operação, exceto a saída, o programa aguarda Enter antes de retornar ao menu. A opção 7 encerra a aplicação.



#### 2. Cadastro de Conta

```
                                [M1S07] Mini-Projeto Avaliativo

================================================================

                 Motor de Gestão de Contas Bancárias
                      ( Cadastro de Conta )

================================================================

Digite o Nome do Titular: Robson Alessandro Fernandes
Digite o CPF do Titular: 12345678900

Selecione o Tipo de Conta Desejada:
  1 - Conta Corrente
  2 - Conta Poupança
  3 - Conta Salário
Escolha o Tipo de Conta: 1

Saldo inicial: 0
Limite do cheque especial: 500
Conta criada: 000001 - Robson Alessandro Fernandes

```

O cadastro começa solicitando NOME e CPF DO TITULAR, depois pede o TIPO DE CONTA e o SALDO INICIAL. O número é criado automaticamente, em sequência, e apresentado na confirmação; 
neste exemplo, `000001` é o número que deverá ser informado nas operações seguintes. Os campos finais variam conforme o tipo escolhido:

- CONTA CORRENTE: informa o limite do cheque especial. Esse limite permite que saques ultrapassem o saldo positivo, mas somente até o TOTAL DO SALDO + LIMITE.
- CONTA POUPANÇA: informa a taxa de rendimento mensal como percentual e o dia de aniversário entre 1 e 31. O fechamento mensal usa a taxa para calcular o rendimento.
- CONTA SALÁRIO: informa o empregador e quantos saques no mês podem ser feitos antes da tarifa de R$ 5,00 por saque excedente.

A confirmação informa o número e o titular cadastrados. Os dados não são persistidos após o encerramento do programa. Entradas numéricas inválidas são lidas pelo 
programa como valores padrão em alguns campos; por isso, confira os valores digitados.


#### 3. Realizando um Depósito

```
                                 [M1S07] Mini-Projeto Avaliativo

================================================================

                 Motor de Gestão de Contas Bancárias
                     ( Realizar Depósito )

================================================================

Número da conta: 000001
Conta encontrada: 000001 - Robson Alessandro Fernandes - R$ 0,00
Valor a depositar: 250,00

Depósito realizado com sucesso.
Saldo atual: R$ 250,00

```

Informe o número da conta e o valor a depositar. O sistema procura a conta na lista em memória, aceita o número com ou sem zeros à esquerda se for numérico,
e mostra os dados da conta encontrada. Um depósito deve ser maior que zero: quando aceito, aumenta o saldo e registra uma transação do tipo `Deposito`; 
se o texto não puder ser interpretado como número, ou o valor for inválido, a operação não é concluída e uma mensagem é exibida.


#### 4. Efetuando um Saque


```
                                 [M1S07] Mini-Projeto Avaliativo

================================================================

                 Motor de Gestão de Contas Bancárias
                     ( Efetuar Saque )

================================================================

Número da conta: 000001
Conta encontrada: 000001 - Robson Alessandro Fernandes - R$ 250,00
Valor a sacar: 100,00
Saque realizado com sucesso.

```

Após localizar a conta, informe o valor. O método `Sacar` da classe correspondente verifica as regras do tipo de conta. 
Na conta corrente, por exemplo, o saldo pode ficar negativo até o limite do cheque especial. Na conta salário, o saque pode também gerar tarifa quando ultrapassar o limite mensal. 
Se o saque não puder ser realizado, o sistema informa que o saldo é insuficiente; valores que não possam ser convertidos para número são rejeitados antes da operação.


#### 5. Extrato e listagem de contas

A opção **4 - Gerar um Extrato** pede o número da conta e apresenta o histórico dessa conta. O cabeçalho identifica o titular e o número, seguido pelo saldo atual e uma linha por transação, contendo data, descrição, tipo e valor. Exemplos de movimentações são depósitos, saques, tarifa de manutenção, tarifa por saque excedente e rendimento mensal.

```
                                 [M1S07] Mini-Projeto Avaliativo

================================================================

                 Motor de Gestão de Contas Bancárias
                     ( Extrato )

================================================================

Extrato - Conta 000001 - Titular: Robson Alessandro Fernandes
Saldo atual: R$ 150,00
----------------------------------------------------------
24/10/2026 17:00 - Depósito - Deposito - R$ 250,00
24/10/2026 17:01 - Saque - Saque - R$ 100,00

    Pressione Enter para continuar...

```

A opção **5 - Listar Contas Ativas** mostra cada conta cadastrada com número, nome do titular, saldo e tipo. Se ainda não houver contas, informa que nenhuma conta foi cadastrada. 
A busca das operações também informa quando o número solicitado não foi encontrado.


#### 6. Relatório de saúde (exemplo ilustrativo)

A opção 6 - RELATÓRIO DE SAÚDE DAS CONTAS: analisa todas as contas cadastradas em sequência. O título `[1/2]`, por exemplo, indica que está sendo processada a primeira de duas contas. 
O bloco de dados identifica titular e tipo, apresenta o atributo específico da subclasse e mostra o saldo atualizado no momento da análise.

A seção de movimentações imprime o histórico considerado no cálculo. `[OK]` identifica movimentações sem apontamento específico, `[ ! ]` identifica saques que merecem acompanhamento 
e `[ X ]` identifica tarifas que devem ser revisadas. O valor entre parênteses é a pontuação atribuída àquela transação.

```

MINIBANCO .NET - MOTOR DE GESTÃO DE CONTAS
====================================================================
[1/1] PROCESSANDO CONTA
DADOS DA CONTA:
• Tipo: Conta Corrente
• Titular: Robson Alessandro Fernandes | Conta: 000001
• Atributo Específico: Limite Cheque Especial: R$ 500,00
• Saldo Atual: R$ -510,00
MOVIMENTAÇÕES REGISTRADAS (4 TRANSAÇÕES):
[OK] Depósito — Deposito: R$ 2.000,00 (10 pts)
[OK] Depósito — Deposito: R$ 300,00 (10 pts)
[ ! ] Saque — Saque: R$ 2.800,00 (5 pts)
[ X ] Tarifa de manutenção — Tarifa: R$ 10,00 (0 pts)
RESUMO DA PONTUAÇÃO:
• Pontuação Atingida: 25 de 40 pontos possíveis
• Índice de Saúde: 62,5%
• Classificação Final: [ CONTA EM ATENÇÃO ]
RELATÓRIO DE PENDÊNCIAS E RECOMENDAÇÕES DA GERÊNCIA:
🔴 ITENS CRÍTICOS (TARIFAS):
- Tarifa de manutenção: revisar cobrança e avaliar pacote de serviços.
🟡 ITENS DE ATENÇÃO (SAQUES):
- Saque de R$ 2.800,00: acompanhar movimentação e contatar o cliente.
Ação prioritária: Contatar o cliente para orientação financeira e revisar tarifas.
====================================================================
FIM DO PROCESSAMENTO DE CONTAS

```

No resumo, a pontuação atingida é a soma dos pontos das movimentações e a pontuação possível é o total de transações multiplicado por 10. Neste exemplo, `10 + 10 + 5 + 0 = 25` pontos 
obtidos de `4 × 10 = 40`, portanto o índice é `25 / 40 × 100 = 62,5%`. O percentual determina a classificação e a ação corporativa sugerida. Tarifas aparecem em uma lista crítica própria;
saques aparecem separadamente como itens de atenção. A recomendação final pode incluir revisão do pacote de tarifas e contato com o cliente.
Se a conta ainda não tiver movimentações, o relatório mostra índice de 0% e classificação de risco, sem tentar dividir por zero. Se não houver nenhuma conta cadastrada, 
a opção 6 informa isso e encerra a impressão do relatório sem gerar blocos de contas.


## Regras de negócio

### Detalhes das funções

| Função / método | O que faz |
|---|---|
| `Program.Main()` | Exibe o menu em loop e direciona a opção escolhida até o usuário sair. |
| `Program.CriarConta()` | Lê os dados do titular e os atributos específicos, cria a subclasse selecionada e a adiciona à lista em memória. |
| `Program.GerarNumeroConta()` | Gera números sequenciais de conta com seis dígitos, como `000001`. |
| `BuscarConta()` | Solicita o número da conta, normaliza uma entrada numérica para seis dígitos e percorre a lista para localizar a conta. |
| `RealizarDeposito()` | Localiza a conta, valida o valor numérico e chama `Depositar`; informa sucesso ou erro de valor inválido. |
| `RealizarSaque()` | Localiza a conta, lê o valor e chama o `Sacar` sobrescrito pelo tipo concreto. Informa sucesso ou saldo insuficiente. |
| `MostrarExtrato()` / `ContaBancaria.ObterExtrato()` | Exibe saldo e movimentações registradas com data, descrição, tipo e valor. |
| `ListarContas()` | Percorre e imprime as contas cadastradas e seus saldos. |
| `AplicarFechamentoMensal()` | Executa a regra de fechamento da subclasse (tarifa, rendimento ou reinício da contagem). Não é chamado automaticamente no menu. |
| `RelatorioSaudeContas.Imprimir()` | Percorre as contas e encaminha cada uma para cálculo e impressão do parecer. |
| `RelatorioSaudeContas.ImprimirConta()` | Calcula pontos, percentual e classificação, imprime movimentações e separa tarifas/saques em pendências. |


### Conta base

`ContaBancaria` concentra o número, titular, CPF, saldo e histórico de transações. `Depositar(valor)` rejeita valores menores ou iguais a zero com `ArgumentException`,
aumenta o saldo e registra uma transação `Deposito`. `Sacar(valor)` retorna `false` para valor inválido ou saldo insuficiente; quando aprovado, debita o saldo e registra `Saque`. 
As subclasses sobrescrevem o saque quando as regras diferem.


### Conta Corrente

- Tem `LimiteChequeEspecial`.
- Permite sacar até o saldo disponível somado ao limite do cheque especial.
- O fechamento mensal debita R$ 10,00 e registra uma transação do tipo `Tarifa`, descrita como tarifa de manutenção.
- Não é cobrada tarifa específica por saque.


### Conta Poupança

- Tem `TaxaRendimentoMensal` (em percentual) e `DiaAniversario`.
- O fechamento mensal calcula o rendimento com a fórmula `saldo × taxa / 100`, arredonda para duas casas decimais e, quando o rendimento é positivo, credita o valor no saldo.
- O rendimento é registrado como uma transação do tipo `Deposito`, com a descrição “Rendimento mensal”.
- A taxa padrão é 0,5% ao mês e o dia de aniversário padrão é 1 quando a conta é instanciada sem esses argumentos.


### Conta Salário

- Tem `Empregador` e `LimiteSaquesMensais`.
- Os saques dentro do limite não têm tarifa. Cada saque realizado após o limite ter sido atingido cobra também R$ 5,00, registrado como transação do tipo `Tarifa`.
- A tarifa só é debitada quando o saque é aprovado; se o saldo não cobrir o saque e a tarifa aplicável, a operação falha sem alterar o saldo ou a contagem.
- O fechamento mensal reinicia a contagem de saques, sem cobrança de tarifa de manutenção.
- A implementação não concede cheque especial para a conta salário.


### Fechamento mensal

O método `AplicarFechamentoMensal()` aplica a regra do tipo de conta. Ele está disponível nas classes de domínio, mas não é acionado automaticamente pelo menu atual: 
deve ser chamado pelo código cliente ou por uma futura opção de fechamento mensal.


## Relatório de saúde

A opção **6 - Relatório de Saúde das Contas** analisa todas as contas cadastradas e exibe os dados da conta, os atributos específicos do tipo, as transações, a pontuação,
o percentual de saúde, a classificação e as recomendações.


### Pontuação por movimentação

| Tipo de transação | Pontos |
|---|---:|
| `Deposito` | 10 |
| `Saque`    |  5 |
| `Tarifa`   |  0 |
| Outros tipos, como `Transferencia` | 0 |

A pontuação máxima possível é calculada como `quantidade de transações × 10`. O índice é:

```
(pontuação obtida / pontuação máxima possível) × 100
```

A divisão converte a pontuação obtida para `double`, evitando truncamento de inteiros. Quando não há transações, o índice é definido como 0% para evitar divisão por zero.


### Classificações e ações

| Índice | Classificação | Ação recomendada |
|---|---|---|
| 90% a 100% | Conta saudável | Oferecer investimentos e avaliar aumento de limite. |
| 60% a menos de 90% | Conta em atenção | Contatar o cliente para orientação financeira e revisar tarifas. |
| Abaixo de 60% | Conta em risco | Bloquear aumento de limite e abrir análise de crédito. |

O relatório também separa tarifas como **itens críticos** e saques como **itens de atenção**, com orientações para revisar cobranças, contatar o cliente ou avaliar um tipo de conta
mais adequado. Os valores são formatados em reais usando a cultura `pt-BR`.

> Observação: cada transação registrada entra no total usado para calcular a pontuação máxima. Por isso, uma tarifa vale zero pontos, mas também aumenta o total de transações e, 
consequentemente, a pontuação máxima possível.


## Estrutura do repositório

```

MiniBanco.ConsoleApp/
├── Exceptions/
│   └── SaldoInsuficienteException.cs
├── Models/
│   ├── ContaBancaria.cs
│   ├── ContaCorrente.cs
│   ├── ContaPoupanca.cs
│   ├── ContaSalario.cs
│   └── Transacao.cs
├── Services/
│   └── RelatorioSaudeContas.cs
├── Program.cs
└── MiniBanco.ConsoleApp.csproj
│
├── REAME.md


```

## Tecnologias

- C#
- .NET 10
- Aplicação de console
- xUnit
