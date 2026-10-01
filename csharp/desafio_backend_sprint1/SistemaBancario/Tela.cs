// Importa tipos básicos, como Console, FormatException e OverflowException.
using System;

// Importa IReadOnlyList, usado para receber a lista de contas.
using System.Collections.Generic;

// Importa Thread, usado para fazer pausas entre telas.
using System.Threading;

// Define a classe responsável por mostrar mensagens e ler entradas no console.
class Tela
{
    // Define a pausa padrão entre telas em milissegundos.
    private const int PausaEntreTelasEmMs = 2000;

    // Limpa a tela do console.
    private static void LimparTela()
    {
        // Só limpa se a saída for uma tela de verdade; em testes com a saída redirecionada, o Clear dá erro.
        if (!Console.IsOutputRedirected)
        {
            // Apaga tudo que está escrito no console.
            Console.Clear();
        }
    }

    // Mostra o logo do sistema.
    public static void MostrarLogo()
    {
        // Escreve o desenho em texto na tela.
        Console.WriteLine(@"
 ____
| __ )  __ _ _ __   ___ ___
|  _ \ / _` | '_ \ / __/ _ \
| |_) | (_| | | | | (_| (_) |
|____/ \__,_|_| |_|\___\___/
");
    }

    // Mostra o menu principal com todas as opções.
    public static void MostrarMenu(string nomeDoBanco)
    {
        // Limpa a tela antes de desenhar o menu.
        LimparTela();

        // Mostra o logo no topo do menu.
        MostrarLogo();

        // Mostra o nome do banco.
        Console.WriteLine($"Boas vindas ao {nomeDoBanco}!");

        // Mostra cada opção do menu.
        Console.WriteLine("\nDigite 1 para abrir uma conta");
        Console.WriteLine("Digite 2 para listar todas as contas");
        Console.WriteLine("Digite 3 para depositar");
        Console.WriteLine("Digite 4 para sacar");
        Console.WriteLine("Digite 5 para transferir");
        Console.WriteLine("Digite 6 para ver o extrato de uma conta");
        Console.WriteLine("Digite 7 para aplicar o rendimento das poupanças");
        Console.WriteLine("Digite 8 para solicitar empréstimo (conta empresarial)");
        Console.WriteLine("Digite 9 para pagar empréstimo (conta empresarial)");
        Console.WriteLine("Digite 0 para sair\n");
    }

    // Mostra um título cercado de asteriscos, igual no Screen Sound.
    public static void ExibirTituloDaOpcao(string titulo)
    {
        // Limpa a tela antes de mostrar a nova opção.
        LimparTela();

        // Cria uma linha de asteriscos do mesmo tamanho do título.
        string asteriscos = string.Empty.PadLeft(titulo.Length, '*');

        // Mostra a linha de cima.
        Console.WriteLine(asteriscos);

        // Mostra o título.
        Console.WriteLine(titulo);

        // Mostra a linha de baixo e pula uma linha.
        Console.WriteLine(asteriscos + "\n");
    }

    // Lê um texto e só aceita se o usuário digitar alguma coisa.
    public static string LerTexto(string mensagem)
    {
        // Repete até o usuário digitar um texto válido.
        while (true)
        {
            // Mostra a mensagem antes de ler.
            Console.Write(mensagem);

            // Lê o texto; se vier nulo, usa texto vazio.
            string texto = Console.ReadLine() ?? "";

            // Verifica se o usuário digitou alguma coisa além de espaços.
            if (!string.IsNullOrWhiteSpace(texto))
            {
                // Retorna o texto digitado.
                return texto;
            }

            // Avisa que o campo não pode ficar vazio.
            MostrarErro("Esse campo não pode ficar vazio.");
        }
    }

    // Lê um número inteiro dentro de um intervalo, tratando entradas inválidas com try/catch.
    public static int LerInteiro(string mensagem, int minimo, int maximo)
    {
        // Repete até o usuário digitar um número válido.
        while (true)
        {
            // Mostra a mensagem antes de ler.
            Console.Write(mensagem);

            // Tenta executar a conversão; se der erro, cai em um dos catch.
            try
            {
                // Converte o texto digitado para int.
                int valor = int.Parse(Console.ReadLine() ?? "");

                // Verifica se o número está dentro do intervalo permitido.
                if (valor >= minimo && valor <= maximo)
                {
                    // Retorna o número válido.
                    return valor;
                }

                // Avisa que o número está fora do intervalo.
                MostrarErro($"Digite um número entre {minimo} e {maximo}.");
            }
            // Cai aqui quando o usuário digita letras ou deixa vazio.
            catch (FormatException)
            {
                // Avisa que só números inteiros são aceitos.
                MostrarErro("Digite apenas números inteiros.");
            }
            // Cai aqui quando o número é grande demais para caber em um int.
            catch (OverflowException)
            {
                // Avisa que o número é grande demais.
                MostrarErro("Esse número é grande demais.");
            }
        }
    }

    // Lê um valor em dinheiro, tratando entradas inválidas com try/catch.
    public static decimal LerValor(string mensagem)
    {
        // Repete até o usuário digitar um valor válido.
        while (true)
        {
            // Mostra a mensagem antes de ler.
            Console.Write(mensagem);

            // Tenta executar a conversão; se der erro, cai em um dos catch.
            try
            {
                // Converte o texto digitado para decimal e já retorna.
                return decimal.Parse(Console.ReadLine() ?? "");
            }
            // Cai aqui quando o usuário digita letras ou deixa vazio.
            catch (FormatException)
            {
                // Avisa o formato esperado.
                MostrarErro("Digite apenas números (ex.: 150,75).");
            }
            // Cai aqui quando o número é grande demais para caber em um decimal.
            catch (OverflowException)
            {
                // Avisa que o número é grande demais.
                MostrarErro("Esse valor é grande demais.");
            }
        }
    }

    // Mostra todas as contas, uma por linha.
    public static void MostrarContas(IReadOnlyList<ContaBancaria> contas)
    {
        // Verifica se a lista está vazia.
        if (contas.Count == 0)
        {
            // Avisa que ainda não tem contas.
            Console.WriteLine("Nenhuma conta aberta ainda.");

            // Sai do método porque não tem nada para mostrar.
            return;
        }

        // Percorre cada conta da lista.
        foreach (ContaBancaria conta in contas)
        {
            // Mostra a descrição da conta; cada tipo descreve do seu jeito (polimorfismo).
            Console.WriteLine(conta.Descrever());
        }
    }

    // Mostra o extrato de uma conta.
    public static void MostrarExtrato(ContaBancaria conta)
    {
        // Mostra a conta no topo do extrato.
        Console.WriteLine($"{conta.Descrever()}\n");

        // Verifica se a conta ainda não tem movimentações.
        if (conta.Extrato.Count == 0)
        {
            // Avisa que não há movimentações.
            Console.WriteLine("Nenhuma movimentação ainda.");

            // Sai do método porque não tem mais nada para mostrar.
            return;
        }

        // Percorre as movimentações usando o índice para numerar.
        for (int i = 0; i < conta.Extrato.Count; i++)
        {
            // Mostra o número (começando em 1) e a movimentação.
            Console.WriteLine($"{i + 1}. {conta.Extrato[i].Descrever()}");
        }
    }

    // Mostra uma mensagem de erro em vermelho.
    public static void MostrarErro(string mensagem)
    {
        // Muda a cor do texto para vermelho.
        Console.ForegroundColor = ConsoleColor.Red;

        // Mostra a mensagem de erro.
        Console.WriteLine(mensagem);

        // Volta a cor do texto para o padrão.
        Console.ResetColor();
    }

    // Mostra uma mensagem de sucesso em verde.
    public static void MostrarSucesso(string mensagem)
    {
        // Muda a cor do texto para verde.
        Console.ForegroundColor = ConsoleColor.Green;

        // Mostra a mensagem de sucesso.
        Console.WriteLine(mensagem);

        // Volta a cor do texto para o padrão.
        Console.ResetColor();
    }

    // Espera o usuário apertar Enter para voltar ao menu.
    public static void Pausar()
    {
        // Mostra a instrução para voltar.
        Console.WriteLine("\nAperte Enter para voltar ao menu principal");

        // Espera o Enter (ReadLine funciona até quando a entrada vem de um arquivo de teste).
        Console.ReadLine();
    }

    // Mostra a mensagem de saída do sistema.
    public static void MostrarDespedida()
    {
        // Limpa a tela.
        LimparTela();

        // Mostra a mensagem de tchau.
        Console.WriteLine("Obrigado por usar o nosso banco. Tchau tchau :)");

        // Pausa um pouco antes de fechar.
        Thread.Sleep(PausaEntreTelasEmMs);
    }
}
