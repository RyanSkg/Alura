// Importa tipos básicos, como DateTime.
using System;

// Define a classe que representa uma movimentação da conta (depósito, saque, taxa...).
class Transacao
{
    // Guarda o que aconteceu (ex.: "Depósito", "Saque").
    public string Descricao { get; }

    // Guarda o valor movimentado; positivo entra, negativo sai.
    public decimal Valor { get; }

    // Guarda a data e hora da movimentação.
    public DateTime Data { get; }

    // Método construtor: roda quando criamos uma nova transação.
    public Transacao(string descricao, decimal valor)
    {
        // Guarda a descrição recebida.
        Descricao = descricao;

        // Guarda o valor recebido.
        Valor = valor;

        // Marca a data e hora atuais.
        Data = DateTime.Now;
    }

    // Monta a linha do extrato.
    public string Descrever()
    {
        // Retorna data, descrição e valor formatado em reais.
        return $"{Data:dd/MM/yyyy HH:mm} | {Descricao} | {Valor:C}";
    }
}
