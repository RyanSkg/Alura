// Define a conta corrente, que herda tudo da classe ContaBancaria.
class ContaCorrente : ContaBancaria
{
    // Define a taxa fixa cobrada em cada saque.
    public const decimal TaxaPorSaque = 2.50m;

    // Informa o tipo desta conta.
    public override string Tipo => "Corrente";

    // Método construtor: repassa número e titular para o construtor da classe ContaBancaria.
    public ContaCorrente(int numeroConta, string titular) : base(numeroConta, titular)
    {
    }

    // Muda a regra da taxa: a conta corrente cobra uma taxa a cada saque.
    protected override decimal CalcularTaxaDeSaque(decimal valor)
    {
        // Retorna a taxa fixa.
        return TaxaPorSaque;
    }

    // Completa a descrição padrão com a taxa.
    public override string Descrever()
    {
        // Junta a descrição da classe ContaBancaria com a taxa.
        return $"{base.Descrever()} | Taxa por saque: {TaxaPorSaque:C}";
    }
}
