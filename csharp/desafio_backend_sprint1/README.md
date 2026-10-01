# Desafio 1 — Sistema Bancário (C# POO)

Sistema de console em C# que gerencia diferentes tipos de contas bancárias. Toda conta tem número, titular e saldo, mas as regras de saque, taxas, rendimento e empréstimo mudam conforme o tipo da conta.

## Problema escolhido (Passo 1)

Um banco precisa gerenciar diferentes tipos de contas. Todas as contas têm saldo e titular, mas as regras de saque e taxas funcionam de forma diferente dependendo do tipo da conta:

- **Conta Corrente:** cobra uma taxa a cada saque.
- **Conta Poupança:** não tem taxa, mas tem rendimento.
- **Conta Empresarial:** tem um limite de empréstimo extra.

## Como rodar

Abra o arquivo `SistemaBancario.sln` no Visual Studio e aperte F5, ou rode pelo terminal (.NET 10):

```
cd SistemaBancario
dotnet run
```

O sistema já começa com uma conta de cada tipo para facilitar os testes.

## Classes (Passos 3 e 4)

### `ContaBancaria` (classe pai, abstrata)
**Atributos:**
- `NumeroConta`
- `Titular`
- `Saldo` (só pode ser alterado pela própria conta e pelas filhas)
- `Extrato` (lista de `Transacao`)
- `Tipo` (abstrato)

**Métodos:**
- `Depositar`
- `Sacar`: cobra a taxa definida por `CalcularTaxaDeSaque`.
- `Transferir`
- `CalcularTaxaDeSaque` (virtual, padrão sem taxa)
- `Descrever` (virtual)

### `ContaCorrente` (herda de `ContaBancaria`)
- Cobra uma taxa fixa de R$ 2,50 a cada saque, sobrescrevendo `CalcularTaxaDeSaque`.

### `ContaPoupanca` (herda de `ContaBancaria` e implementa `IRentavel`)
- Não tem taxa de saque.
- Rende 0,5% ao mês sobre o saldo (`CalcularRendimento` e `AplicarRendimento`).

### `ContaEmpresarial` (herda de `ContaBancaria`)
**Atributos:**
- `LimiteDeEmprestimo`
- `EmprestimoUtilizado`
- `LimiteDisponivel`

**Métodos:**
- `SolicitarEmprestimo`: até o limite disponível.
- `PagarEmprestimo`

### `IRentavel` (interface)
Contrato para contas que rendem: `CalcularRendimento()` e `AplicarRendimento()`.

### `Transacao`
Uma movimentação do extrato.
- **Atributos:** `Descricao`, `Valor`, `Data`
- **Método:** `Descrever`

### `Banco`
**Atributos:**
- `Nome`
- a lista de contas

**Métodos:**
- `AbrirContaCorrente`, `AbrirContaPoupanca` e `AbrirContaEmpresarial`, que geram o número da conta
- `BuscarConta`
- `AplicarRendimentos`: aplica o rendimento só nas contas `IRentavel`.
- `CalcularSaldoTotal`

### `Sistema`
Controla o menu interativo e trata as exceções lançadas pelas regras das contas.

### `Tela`
Concentra a entrada e a saída do console. As leituras de números usam `try/catch` para tratar entradas inválidas.

## Relacionamentos (Passo 5)

```
                 «abstract» ContaBancaria ◆── List<Transacao>
                  ▲          ▲           ▲
          herda   │          │ herda     │  herda
     ContaCorrente   ContaPoupanca   ContaEmpresarial
                          │
                          └── implementa «interface» IRentavel

   Banco ◆── List<ContaBancaria>     (composição)
   Sistema ──► Banco, Tela
```

## Menu

```
Digite 1 para abrir uma conta
Digite 2 para listar todas as contas
Digite 3 para depositar
Digite 4 para sacar
Digite 5 para transferir
Digite 6 para ver o extrato de uma conta
Digite 7 para aplicar o rendimento das poupanças
Digite 8 para solicitar empréstimo (conta empresarial)
Digite 9 para pagar empréstimo (conta empresarial)
Digite 0 para sair
```

## Testes realizados (Ação 3)

| Teste | Resultado esperado | Resultado |
|---|---|---|
| Digitar letras no menu | "Digite apenas números inteiros." | ok |
| Abrir conta corrente | conta criada com número novo | ok |
| Valor de saque "abc" | pede o valor de novo | ok |
| Sacar R$ 100 da corrente com R$ 1.500 | saldo R$ 1.397,50 (taxa de R$ 2,50) | ok |
| Sacar de conta com saldo zero | "Saldo insuficiente" | ok |
| Transferir R$ 200 da corrente para a poupança | sai R$ 202,50 de uma, entra R$ 200 na outra | ok |
| Aplicar rendimento | só a poupança rende 0,5% | ok |
| Pedir empréstimo em conta corrente | "Empréstimo só está disponível para conta empresarial." | ok |
| Pedir empréstimo acima do limite | "Valor acima do limite disponível" | ok |
| Pedir R$ 5.000 e pagar R$ 2.000 | dívida restante R$ 3.000 | ok |
| Ver extrato | mostra depósitos, saques, taxas e transferências | ok |

## Referências

MICROSOFT. **Documentação do C#**. Disponível em: https://learn.microsoft.com/pt-br/dotnet/csharp/. Acesso em: 1 out. 2026.

MICROSOFT. **Exceções e tratamento de exceções**. Disponível em: https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/exceptions/. Acesso em: 1 out. 2026.

MICROSOFT. **Interfaces: definir comportamento para vários tipos**. Disponível em: https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/types/interfaces. Acesso em: 1 out. 2026.

MICROSOFT. **Programação orientada a objetos (C#)**. Disponível em: https://learn.microsoft.com/pt-br/dotnet/csharp/fundamentals/tutorials/oop. Acesso em: 1 out. 2026.
