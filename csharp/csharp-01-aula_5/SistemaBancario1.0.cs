int valorRecebido = 0;

void Menu()
{
    Console.WriteLine("Bem-vindo ao sistema de doações!");
    Console.WriteLine("1. Registrar doação");
    Console.WriteLine("2. Sair");
    Console.Write("Escolha uma opção: ");

    int respostaMenu = int.Parse(Console.ReadLine()!);

    switch (respostaMenu)
    {
        case 1:
            Console.WriteLine("Você escolheu registrar uma doação.");
            TelaDeDoacao();
            break;

        case 2:
            Console.WriteLine("Saindo do sistema. Obrigado!");
            Environment.Exit(0);
            break;
    }
}

void TelaDeDoacao()
{
    console.WriteLine("Tela de Doação");
    Console.Write("Digite o valor da doação: ");
    valorRecebido = int.Parse(Console.ReadLine()!);

    Console.WriteLine($"O valor de {valorRecebido} foi recebido com sucesso!");

    Console.WriteLine("Deseja que a doação seja anônima? (S/N)");
    string respostaDoacao = Console.ReadLine()!;

    bool doacaoAnonima = respostaDoacao == "S" || respostaDoacao == "s";

    if (doacaoAnonima)
    {
        Console.WriteLine("A doação será anônima.");
    }
    else
    {
        Console.WriteLine("A doação não será anônima.");
    }

    Console.Write("Digite o tipo da Conta (Corrente/C ou Poupança/P): ");
    string tipoConta = Console.ReadLine()!;

    string tipoContaFinal;

    if (tipoConta == "C" || tipoConta == "c" || tipoConta == "Corrente" || tipoConta == "corrente")
    {
        tipoContaFinal = "Corrente";
        Console.WriteLine("A doação será feita em uma Conta Corrente.");
    }
    else if (tipoConta == "P" || tipoConta == "p" || tipoConta == "Poupança" || tipoConta == "poupança")
    {
        tipoContaFinal = "Poupança";
        Console.WriteLine("A doação será feita em uma Conta Poupança.");
    }
    else
    {
        tipoContaFinal = "Inválido";
        Console.WriteLine("Tipo de conta inválido.");
    }

    Console.WriteLine($"Valor recebido: R${valorRecebido}");
    Console.WriteLine($"Doação anônima: {(doacaoAnonima ? "Sim" : "Não")}");
    Console.WriteLine($"Tipo de Conta: {tipoContaFinal}");
}

Menu();