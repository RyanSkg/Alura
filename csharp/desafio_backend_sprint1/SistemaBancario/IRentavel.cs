// Define o "contrato" de tudo que pode render dinheiro com o tempo.
interface IRentavel
{
    // Obriga quem implementar a interface a saber calcular quanto vai render.
    decimal CalcularRendimento();

    // Obriga quem implementar a interface a ter um jeito de colocar o rendimento no saldo.
    void AplicarRendimento();
}
