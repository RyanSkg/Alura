// Importa tipos básicos, como ArgumentException e InvalidOperationException.
using System;

// Importa List e IReadOnlyList, usados para guardar o extrato.
using System.Collections.Generic;

// Define a classe pai de todas as contas; é abstrata porque toda conta precisa ter um tipo.
abstract class ContaBancaria
{
    // Guarda as movimentações da conta; é privada para ninguém mexer no extrato por fora.
    private readonly List<Transacao> extrato = new List<Transacao>();

    // Guarda o número da conta.
    public int NumeroConta { get; }

    // Guarda o nome do titular da conta.
    public string Titular { get; }

    // Guarda o saldo; só a própria conta e as classes filhas podem mudar.
    public decimal Saldo { get; protected set; }

    // Deixa ler o extrato por fora, mas sem poder adicionar ou remover direto na lista.
    public IReadOnlyList<Transacao> Extrato => extrato;

    // Tipo da conta ("Corrente", "Poupança"...); cada classe filha é obrigada a informar o seu.
    public abstract string Tipo { get; }

    // Método construtor: roda quando uma classe filha cria uma nova conta.
    protected ContaBancaria(int numeroConta, string titular)
    {
        // Verifica se o número da conta é zero ou negativo.
        if (numeroConta <= 0)
        {
            // Lança um erro avisando que o número precisa ser positivo.
            throw new ArgumentException("O número da conta precisa ser maior que zero.");
        }

        // Verifica se o titular veio vazio.
        if (string.IsNullOrWhiteSpace(titular))
        {
            // Lança um erro avisando que o titular é obrigatório.
            throw new ArgumentException("O nome do titular não pode ficar vazio.");
        }

        // Guarda o número da conta.
        NumeroConta = numeroConta;

        // Guarda o titular sem espaços sobrando.
        Titular = titular.Trim();

        // Toda conta nova começa com saldo zero.
        Saldo = 0;
    }

    // Coloca dinheiro na conta.
    public void Depositar(decimal valor)
    {
        // Usa o método que coloca dinheiro, escrevendo "Depósito" no extrato.
        Creditar(valor, "Depósito");
    }

    // Tira dinheiro da conta, cobrando a taxa de saque de cada tipo de conta.
    public void Sacar(decimal valor)
    {
        // Usa o método que tira dinheiro, escrevendo "Saque" no extrato.
        Debitar(valor, "Saque");
    }

    // Manda dinheiro desta conta para outra; funciona como um saque aqui e um depósito lá.
    public void Transferir(ContaBancaria destino, decimal valor)
    {
        // Verifica se estão tentando transferir para a própria conta.
        if (destino.NumeroConta == NumeroConta)
        {
            // Lança um erro avisando que não faz sentido transferir para si mesmo.
            throw new InvalidOperationException("Não é possível transferir para a mesma conta.");
        }

        // Tira desta conta; se faltar saldo, o erro acontece aqui e o destino não recebe nada.
        Debitar(valor, $"Transferência para a conta {destino.NumeroConta}");

        // Coloca na conta de destino.
        destino.Creditar(valor, $"Transferência da conta {NumeroConta}");
    }

    // Coloca dinheiro no saldo e registra no extrato com a descrição recebida.
    private void Creditar(decimal valor, string descricao)
    {
        // Verifica se o valor é zero ou negativo.
        if (valor <= 0)
        {
            // Lança um erro avisando que o valor precisa ser positivo.
            throw new ArgumentException("O valor precisa ser maior que zero.");
        }

        // Soma o valor ao saldo.
        Saldo += valor;

        // Registra a entrada no extrato.
        RegistrarTransacao(descricao, valor);
    }

    // Tira dinheiro do saldo (mais a taxa) e registra no extrato com a descrição recebida.
    private void Debitar(decimal valor, string descricao)
    {
        // Verifica se o valor é zero ou negativo.
        if (valor <= 0)
        {
            // Lança um erro avisando que o valor precisa ser positivo.
            throw new ArgumentException("O valor precisa ser maior que zero.");
        }

        // Pergunta para a conta quanto de taxa ela cobra (cada tipo responde diferente).
        decimal taxa = CalcularTaxaDeSaque(valor);

        // Verifica se o saldo não cobre o saque mais a taxa.
        if (valor + taxa > Saldo)
        {
            // Lança um erro avisando que falta dinheiro.
            throw new InvalidOperationException($"Saldo insuficiente. Saldo: {Saldo:C} | Saque + taxa: {valor + taxa:C}");
        }

        // Tira o valor do saque do saldo.
        Saldo -= valor;

        // Registra a saída no extrato (negativo).
        RegistrarTransacao(descricao, -valor);

        // Verifica se essa conta cobra taxa.
        if (taxa > 0)
        {
            // Tira a taxa do saldo.
            Saldo -= taxa;

            // Registra a taxa no extrato como saída (negativo).
            RegistrarTransacao("Taxa de saque", -taxa);
        }
    }

    // Diz quanto de taxa é cobrado em cada saque; por padrão não tem taxa.
    protected virtual decimal CalcularTaxaDeSaque(decimal valor)
    {
        // Retorna zero; as classes filhas podem mudar isso.
        return 0;
    }

    // Guarda uma movimentação no extrato; as classes filhas também podem usar.
    protected void RegistrarTransacao(string descricao, decimal valor)
    {
        // Cria a transação e coloca no final do extrato.
        extrato.Add(new Transacao(descricao, valor));
    }

    // Monta a descrição padrão da conta; é virtual para as filhas poderem completar.
    public virtual string Descrever()
    {
        // Retorna número, tipo, titular e saldo.
        return $"Conta {NumeroConta} | {Tipo} | {Titular} | Saldo: {Saldo:C}";
    }
}
