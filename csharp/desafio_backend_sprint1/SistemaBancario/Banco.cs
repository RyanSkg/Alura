// Importa tipos básicos, como ArgumentException.
using System;

// Importa List e IReadOnlyList, usados para guardar as contas.
using System.Collections.Generic;

// Define a classe que guarda e organiza todas as contas do banco.
class Banco
{
    // Define o número da primeira conta aberta.
    private const int PrimeiroNumeroDeConta = 1001;

    // Guarda as contas do banco; é privada para ninguém mexer na lista por fora.
    private readonly List<ContaBancaria> contas = new List<ContaBancaria>();

    // Guarda qual número a próxima conta vai receber.
    private int proximoNumero = PrimeiroNumeroDeConta;

    // Guarda o nome do banco.
    public string Nome { get; }

    // Deixa ler as contas por fora, mas sem poder adicionar ou remover direto na lista.
    public IReadOnlyList<ContaBancaria> Contas => contas;

    // Método construtor: roda quando criamos um novo banco.
    public Banco(string nome)
    {
        // Guarda o nome do banco.
        Nome = nome;
    }

    // Abre uma conta corrente e devolve a conta criada.
    public ContaCorrente AbrirContaCorrente(string titular)
    {
        // Cria a conta com o próximo número livre.
        ContaCorrente conta = new ContaCorrente(GerarNumeroDaConta(), titular);

        // Guarda a conta na lista.
        contas.Add(conta);

        // Devolve a conta criada.
        return conta;
    }

    // Abre uma conta poupança e devolve a conta criada.
    public ContaPoupanca AbrirContaPoupanca(string titular)
    {
        // Cria a conta com o próximo número livre.
        ContaPoupanca conta = new ContaPoupanca(GerarNumeroDaConta(), titular);

        // Guarda a conta na lista.
        contas.Add(conta);

        // Devolve a conta criada.
        return conta;
    }

    // Abre uma conta empresarial e devolve a conta criada.
    public ContaEmpresarial AbrirContaEmpresarial(string titular, decimal limiteDeEmprestimo)
    {
        // Cria a conta com o próximo número livre.
        ContaEmpresarial conta = new ContaEmpresarial(GerarNumeroDaConta(), titular, limiteDeEmprestimo);

        // Guarda a conta na lista.
        contas.Add(conta);

        // Devolve a conta criada.
        return conta;
    }

    // Procura uma conta pelo número.
    public ContaBancaria BuscarConta(int numeroConta)
    {
        // Percorre cada conta do banco.
        foreach (ContaBancaria conta in contas)
        {
            // Verifica se o número bate.
            if (conta.NumeroConta == numeroConta)
            {
                // Retorna a conta encontrada.
                return conta;
            }
        }

        // Se o laço terminou sem encontrar, lança um erro.
        throw new ArgumentException($"A conta {numeroConta} não foi encontrada!");
    }

    // Aplica o rendimento em todas as contas que rendem e devolve quantas renderam.
    public int AplicarRendimentos()
    {
        // Começa a contagem em zero.
        int quantidade = 0;

        // Percorre cada conta do banco.
        foreach (ContaBancaria conta in contas)
        {
            // Verifica se a conta cumpre o contrato IRentavel (só a poupança, por enquanto).
            if (conta is IRentavel contaRentavel)
            {
                // Aplica o rendimento usando a interface.
                contaRentavel.AplicarRendimento();

                // Conta mais uma conta que rendeu.
                quantidade++;
            }
        }

        // Devolve quantas contas renderam.
        return quantidade;
    }

    // Soma o saldo de todas as contas.
    public decimal CalcularSaldoTotal()
    {
        // Começa a soma em zero.
        decimal total = 0;

        // Percorre cada conta do banco.
        foreach (ContaBancaria conta in contas)
        {
            // Soma o saldo da conta.
            total += conta.Saldo;
        }

        // Devolve o total.
        return total;
    }

    // Gera o número da próxima conta.
    private int GerarNumeroDaConta()
    {
        // Guarda o número atual para devolver.
        int numero = proximoNumero;

        // Prepara o número da próxima conta.
        proximoNumero++;

        // Devolve o número gerado.
        return numero;
    }
}
