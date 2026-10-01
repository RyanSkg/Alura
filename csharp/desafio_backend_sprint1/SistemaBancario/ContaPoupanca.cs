// Importa tipos básicos, como Math.
using System;

// Define a conta poupança, que herda de ContaBancaria e cumpre o contrato IRentavel.
class ContaPoupanca : ContaBancaria, IRentavel
{
    // Define quanto a poupança rende por mês (0,5%).
    public const decimal TaxaDeRendimentoMensal = 0.005m;

    // Informa o tipo desta conta.
    public override string Tipo => "Poupança";

    // Método construtor: repassa número e titular para o construtor da classe ContaBancaria.
    public ContaPoupanca(int numeroConta, string titular) : base(numeroConta, titular)
    {
    }

    // Calcula quanto o saldo atual vai render (vem da interface IRentavel).
    public decimal CalcularRendimento()
    {
        // Multiplica o saldo pela taxa e arredonda para centavos.
        return Math.Round(Saldo * TaxaDeRendimentoMensal, 2);
    }

    // Coloca o rendimento no saldo (vem da interface IRentavel).
    public void AplicarRendimento()
    {
        // Calcula o rendimento.
        decimal rendimento = CalcularRendimento();

        // Verifica se não tem nada para render (saldo zero).
        if (rendimento <= 0)
        {
            // Sai do método sem mexer no saldo.
            return;
        }

        // Soma o rendimento ao saldo.
        Saldo += rendimento;

        // Registra o rendimento no extrato.
        RegistrarTransacao("Rendimento", rendimento);
    }

    // Completa a descrição padrão com a taxa de rendimento.
    public override string Descrever()
    {
        // Junta a descrição da classe ContaBancaria com o rendimento (P1 mostra como porcentagem).
        return $"{base.Descrever()} | Rendimento: {TaxaDeRendimentoMensal:P1} ao mês";
    }
}
