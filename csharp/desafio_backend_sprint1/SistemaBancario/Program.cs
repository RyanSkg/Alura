// Importa Console.
using System;

// Importa CultureInfo, usado para configurar o padrão brasileiro.
using System.Globalization;

// Importa Encoding, usado para o console mostrar acentos corretamente.
using System.Text;

// Faz o console escrever usando UTF-8.
Console.OutputEncoding = Encoding.UTF8;

// Faz o programa usar o padrão brasileiro: vírgula nos decimais e R$ nos valores.
CultureInfo.CurrentCulture = new CultureInfo("pt-BR");

// Cria o banco.
Banco banco = new Banco("Banco Sprint");

// Abre uma conta de cada tipo para o sistema não começar vazio.
banco.AbrirContaCorrente("Ana Souza").Depositar(1500m);
banco.AbrirContaPoupanca("Bruno Lima").Depositar(3000m);
banco.AbrirContaEmpresarial("Padaria Pão Quente", 20000m).Depositar(8000m);

// Cria um objeto da classe Sistema, entregando o banco.
Sistema sistema = new Sistema(banco);

// Inicia o sistema.
sistema.Iniciar();
