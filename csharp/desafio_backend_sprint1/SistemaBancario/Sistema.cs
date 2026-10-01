// Importa tipos básicos, como Console, ArgumentException e InvalidOperationException.
using System;

// Define a classe responsável por controlar o menu e as ações do sistema.
class Sistema
{
    // Guarda o banco que o sistema vai controlar.
    private readonly Banco banco;

    // Método construtor: roda quando criamos um novo objeto Sistema.
    public Sistema(Banco banco)
    {
        // Guarda no atributo da classe o banco recebido pelo construtor.
        this.banco = banco;
    }

    // Método público que inicia e controla o fluxo principal do sistema.
    public void Iniciar()
    {
        // Começa com uma opção que não é "sair" para entrar no while.
        int opcao = -1;

        // Continua mostrando o menu até o usuário escolher 0.
        while (opcao != 0)
        {
            // Mostra o menu principal.
            Tela.MostrarMenu(banco.Nome);

            // Lê a opção escolhida, aceitando só de 0 a 9.
            opcao = Tela.LerInteiro("Digite a sua opção: ", 0, 9);

            // Tenta executar a opção; se alguma regra for quebrada, cai em um dos catch.
            try
            {
                // Executa a ação da opção escolhida.
                ExecutarOpcao(opcao);
            }
            // Cai aqui quando algum dado é inválido (valor negativo, conta que não existe...).
            catch (ArgumentException erro)
            {
                // Mostra a mensagem do erro que foi lançado.
                Tela.MostrarErro($"\n{erro.Message}");

                // Espera o usuário ler antes de voltar ao menu.
                Tela.Pausar();
            }
            // Cai aqui quando a operação não pode ser feita (saldo insuficiente, limite estourado...).
            catch (InvalidOperationException erro)
            {
                // Mostra a mensagem do erro que foi lançado.
                Tela.MostrarErro($"\n{erro.Message}");

                // Espera o usuário ler antes de voltar ao menu.
                Tela.Pausar();
            }
        }

        // Mostra a mensagem de saída.
        Tela.MostrarDespedida();
    }

    // Decide qual método chamar de acordo com a opção escolhida.
    private void ExecutarOpcao(int opcao)
    {
        // Compara a opção com cada caso possível.
        switch (opcao)
        {
            // Opção 1: abrir conta.
            case 1: AbrirConta();
                break;
            // Opção 2: listar contas.
            case 2: ListarContas();
                break;
            // Opção 3: depositar.
            case 3: Depositar();
                break;
            // Opção 4: sacar.
            case 4: Sacar();
                break;
            // Opção 5: transferir.
            case 5: Transferir();
                break;
            // Opção 6: ver extrato.
            case 6: VerExtrato();
                break;
            // Opção 7: aplicar rendimento.
            case 7: AplicarRendimentos();
                break;
            // Opção 8: solicitar empréstimo.
            case 8: SolicitarEmprestimo();
                break;
            // Opção 9: pagar empréstimo.
            case 9: PagarEmprestimo();
                break;
        }
    }

    // Abre uma conta nova do tipo escolhido.
    private void AbrirConta()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Abrir conta");

        // Mostra os tipos de conta disponíveis.
        Console.WriteLine($"1. Corrente (taxa de {ContaCorrente.TaxaPorSaque:C} a cada saque)");
        Console.WriteLine($"2. Poupança (sem taxa, rende {ContaPoupanca.TaxaDeRendimentoMensal:P1} ao mês)");
        Console.WriteLine("3. Empresarial (tem limite de empréstimo extra)");

        // Lê o tipo escolhido.
        int tipo = Tela.LerInteiro("\nEscolha o tipo da conta: ", 1, 3);

        // Lê o nome do titular.
        string titular = Tela.LerTexto("Digite o nome do titular: ");

        // Cria a variável da conta; o tipo real vai depender da escolha.
        ContaBancaria conta;

        // Verifica se a conta escolhida é corrente.
        if (tipo == 1)
        {
            // Abre uma conta corrente.
            conta = banco.AbrirContaCorrente(titular);
        }
        // Verifica se a conta escolhida é poupança.
        else if (tipo == 2)
        {
            // Abre uma conta poupança.
            conta = banco.AbrirContaPoupanca(titular);
        }
        // Se não é 1 nem 2, só pode ser empresarial.
        else
        {
            // Lê o limite de empréstimo da empresa.
            decimal limite = Tela.LerValor("Digite o limite de empréstimo (R$): ");

            // Abre uma conta empresarial.
            conta = banco.AbrirContaEmpresarial(titular, limite);
        }

        // Avisa que deu tudo certo e mostra o número da conta.
        Tela.MostrarSucesso($"\nConta {conta.Tipo} número {conta.NumeroConta} aberta para {conta.Titular}!");

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Mostra todas as contas do banco.
    private void ListarContas()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Exibindo todas as contas do banco");

        // Mostra as contas.
        Tela.MostrarContas(banco.Contas);

        // Mostra o saldo somado de todas as contas.
        Console.WriteLine($"\nSaldo total no banco: {banco.CalcularSaldoTotal():C}");

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Coloca dinheiro em uma conta.
    private void Depositar()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Depósito");

        // Pede para o usuário escolher a conta.
        ContaBancaria conta = EscolherConta("Digite o número da conta: ");

        // Lê o valor do depósito.
        decimal valor = Tela.LerValor("Digite o valor do depósito (R$): ");

        // Faz o depósito; se o valor for inválido, a conta lança um erro.
        conta.Depositar(valor);

        // Avisa que deu tudo certo e mostra o novo saldo.
        Tela.MostrarSucesso($"\nDepósito feito! Novo saldo: {conta.Saldo:C}");

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Tira dinheiro de uma conta.
    private void Sacar()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Saque");

        // Pede para o usuário escolher a conta.
        ContaBancaria conta = EscolherConta("Digite o número da conta: ");

        // Lê o valor do saque.
        decimal valor = Tela.LerValor("Digite o valor do saque (R$): ");

        // Faz o saque; cada tipo de conta cobra a taxa do seu jeito.
        conta.Sacar(valor);

        // Avisa que deu tudo certo e mostra o novo saldo.
        Tela.MostrarSucesso($"\nSaque feito! Novo saldo: {conta.Saldo:C}");

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Manda dinheiro de uma conta para outra.
    private void Transferir()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Transferência");

        // Pede a conta de onde o dinheiro vai sair.
        ContaBancaria origem = EscolherConta("Digite o número da conta de origem: ");

        // Pede a conta que vai receber o dinheiro.
        ContaBancaria destino = EscolherConta("Digite o número da conta de destino: ");

        // Lê o valor da transferência.
        decimal valor = Tela.LerValor("Digite o valor da transferência (R$): ");

        // Faz a transferência.
        origem.Transferir(destino, valor);

        // Avisa que deu tudo certo e mostra os dois saldos.
        Tela.MostrarSucesso($"\nTransferência feita! Saldo da conta {origem.NumeroConta}: {origem.Saldo:C} | Saldo da conta {destino.NumeroConta}: {destino.Saldo:C}");

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Mostra as movimentações de uma conta.
    private void VerExtrato()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Extrato");

        // Pede para o usuário escolher a conta.
        ContaBancaria conta = EscolherConta("Digite o número da conta: ");

        // Pula uma linha antes do extrato.
        Console.WriteLine();

        // Mostra o extrato da conta.
        Tela.MostrarExtrato(conta);

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Aplica o rendimento em todas as poupanças.
    private void AplicarRendimentos()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Rendimento das poupanças");

        // Aplica o rendimento e guarda quantas contas renderam.
        int quantidade = banco.AplicarRendimentos();

        // Verifica se nenhuma conta rendeu.
        if (quantidade == 0)
        {
            // Avisa que não existe poupança no banco.
            Console.WriteLine("Nenhuma conta poupança encontrada.");
        }
        // Se pelo menos uma rendeu.
        else
        {
            // Avisa quantas contas receberam rendimento.
            Tela.MostrarSucesso($"Rendimento aplicado em {quantidade} conta(s) poupança!");
        }

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Pega dinheiro emprestado em uma conta empresarial.
    private void SolicitarEmprestimo()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Solicitar empréstimo");

        // Pede para o usuário escolher uma conta empresarial.
        ContaEmpresarial conta = EscolherContaEmpresarial();

        // Mostra quanto ainda pode ser pedido.
        Console.WriteLine($"Limite disponível: {conta.LimiteDisponivel:C}");

        // Lê o valor do empréstimo.
        decimal valor = Tela.LerValor("Digite o valor do empréstimo (R$): ");

        // Faz o empréstimo; se passar do limite, a conta lança um erro.
        conta.SolicitarEmprestimo(valor);

        // Avisa que deu tudo certo e mostra o novo saldo.
        Tela.MostrarSucesso($"\nEmpréstimo liberado! Novo saldo: {conta.Saldo:C}");

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Devolve parte ou todo o empréstimo de uma conta empresarial.
    private void PagarEmprestimo()
    {
        // Mostra o título da opção.
        Tela.ExibirTituloDaOpcao("Pagar empréstimo");

        // Pede para o usuário escolher uma conta empresarial.
        ContaEmpresarial conta = EscolherContaEmpresarial();

        // Mostra quanto ainda falta pagar.
        Console.WriteLine($"Dívida atual: {conta.EmprestimoUtilizado:C}");

        // Lê o valor do pagamento.
        decimal valor = Tela.LerValor("Digite o valor do pagamento (R$): ");

        // Faz o pagamento; se for inválido, a conta lança um erro.
        conta.PagarEmprestimo(valor);

        // Avisa que deu tudo certo e mostra quanto ainda deve.
        Tela.MostrarSucesso($"\nPagamento feito! Dívida restante: {conta.EmprestimoUtilizado:C}");

        // Espera o usuário antes de voltar ao menu.
        Tela.Pausar();
    }

    // Mostra as contas e devolve a que o usuário escolher pelo número.
    private ContaBancaria EscolherConta(string mensagem)
    {
        // Verifica se o banco ainda não tem contas.
        if (banco.Contas.Count == 0)
        {
            // Lança um erro avisando que é preciso abrir uma conta antes.
            throw new InvalidOperationException("Nenhuma conta aberta. Abra uma conta primeiro (opção 1).");
        }

        // Mostra as contas.
        Tela.MostrarContas(banco.Contas);

        // Lê o número da conta escolhida.
        int numero = Tela.LerInteiro($"\n{mensagem}", 1, int.MaxValue);

        // Busca a conta; se não existir, o banco lança um erro.
        return banco.BuscarConta(numero);
    }

    // Pede uma conta e garante que ela é empresarial.
    private ContaEmpresarial EscolherContaEmpresarial()
    {
        // Pede para o usuário escolher a conta.
        ContaBancaria conta = EscolherConta("Digite o número da conta empresarial: ");

        // Verifica se a conta escolhida é empresarial.
        if (conta is ContaEmpresarial contaEmpresarial)
        {
            // Devolve a conta já tratada como empresarial.
            return contaEmpresarial;
        }

        // Se não é empresarial, lança um erro.
        throw new InvalidOperationException("Empréstimo só está disponível para conta empresarial.");
    }
}
