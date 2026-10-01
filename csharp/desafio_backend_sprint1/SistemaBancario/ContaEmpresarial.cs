// Importa tipos básicos, como ArgumentException e InvalidOperationException.
using System;

// Define a conta empresarial, que herda tudo da classe ContaBancaria.
class ContaEmpresarial : ContaBancaria
{
    // Guarda o valor máximo que a empresa pode pegar emprestado.
    public decimal LimiteDeEmprestimo { get; }

    // Guarda quanto do empréstimo a empresa já usou e ainda não pagou.
    public decimal EmprestimoUtilizado { get; private set; }

    // Calcula quanto do limite ainda está livre.
    public decimal LimiteDisponivel => LimiteDeEmprestimo - EmprestimoUtilizado;

    // Informa o tipo desta conta.
    public override string Tipo => "Empresarial";

    // Método construtor: repassa número e titular para a classe ContaBancaria e guarda o limite.
    public ContaEmpresarial(int numeroConta, string titular, decimal limiteDeEmprestimo)
        : base(numeroConta, titular)
    {
        // Verifica se o limite é zero ou negativo.
        if (limiteDeEmprestimo <= 0)
        {
            // Lança um erro avisando que o limite precisa ser positivo.
            throw new ArgumentException("O limite de empréstimo precisa ser maior que zero.");
        }

        // Guarda o limite validado.
        LimiteDeEmprestimo = limiteDeEmprestimo;
    }

    // Pega dinheiro emprestado do banco, até o limite disponível.
    public void SolicitarEmprestimo(decimal valor)
    {
        // Verifica se o valor é zero ou negativo.
        if (valor <= 0)
        {
            // Lança um erro avisando que o valor precisa ser positivo.
            throw new ArgumentException("O valor do empréstimo precisa ser maior que zero.");
        }

        // Verifica se o valor passa do limite que ainda está livre.
        if (valor > LimiteDisponivel)
        {
            // Lança um erro mostrando quanto ainda pode ser pedido.
            throw new InvalidOperationException($"Valor acima do limite disponível ({LimiteDisponivel:C}).");
        }

        // Soma o valor ao total de empréstimo usado.
        EmprestimoUtilizado += valor;

        // Coloca o dinheiro emprestado no saldo.
        Saldo += valor;

        // Registra o empréstimo no extrato.
        RegistrarTransacao("Empréstimo", valor);
    }

    // Devolve parte ou todo o empréstimo usando o saldo da conta.
    public void PagarEmprestimo(decimal valor)
    {
        // Verifica se o valor é zero ou negativo.
        if (valor <= 0)
        {
            // Lança um erro avisando que o valor precisa ser positivo.
            throw new ArgumentException("O valor do pagamento precisa ser maior que zero.");
        }

        // Verifica se está tentando pagar mais do que deve.
        if (valor > EmprestimoUtilizado)
        {
            // Lança um erro mostrando quanto ainda deve.
            throw new InvalidOperationException($"O valor é maior que a dívida ({EmprestimoUtilizado:C}).");
        }

        // Verifica se o saldo não cobre o pagamento.
        if (valor > Saldo)
        {
            // Lança um erro avisando que falta dinheiro.
            throw new InvalidOperationException($"Saldo insuficiente para pagar. Saldo: {Saldo:C}");
        }

        // Tira o valor do saldo.
        Saldo -= valor;

        // Diminui a dívida.
        EmprestimoUtilizado -= valor;

        // Registra o pagamento no extrato como saída (negativo).
        RegistrarTransacao("Pagamento de empréstimo", -valor);
    }

    // Completa a descrição padrão com o limite de empréstimo.
    public override string Descrever()
    {
        // Junta a descrição da classe ContaBancaria com o limite livre.
        return $"{base.Descrever()} | Limite de empréstimo: {LimiteDisponivel:C} de {LimiteDeEmprestimo:C}";
    }
}
