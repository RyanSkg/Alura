Console.Write("Crie o código de desarme da bomba: ");
string codigoDesarme = Console.ReadLine()!;

int digitos = codigoDesarme.Length;

Console.Write("Digite a quantidade máxima de tentativas: ");
int maxTentativas = int.Parse(Console.ReadLine()!);

Console.Clear();
Console.WriteLine("Bomba ativada!");
Console.WriteLine($"A senha tem {digitos} digitos.");
Console.WriteLine($"Você tem {maxTentativas} tentativas.");
Thread.Sleep(2000);

int contador = 600;
int tentativasUsadas = 0;

string tentativa = "";
bool bombaDesarmada = false;
string dica = "";

while (contador >= 0 && !bombaDesarmada && tentativasUsadas < maxTentativas)
{
    Console.Clear();
    Console.WriteLine($"Tempo restante: {contador}");
    Console.WriteLine($"A senha tem {digitos} digitos.");
    Console.WriteLine($"Tentativas: {tentativasUsadas}/{maxTentativas}");
    Console.WriteLine($"Digite o código de desarme: {tentativa}");
    Console.WriteLine(dica);

    DateTime inicioSegundo = DateTime.Now;

    while ((DateTime.Now - inicioSegundo).TotalSeconds < 1)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo tecla = Console.ReadKey(true);

            if (tecla.Key == ConsoleKey.Enter)
            {
                if (tentativa.Length > 0)
                {
                    tentativasUsadas++;

                    if (tentativa == codigoDesarme)
                    {
                        bombaDesarmada = true;
                        break;
                    }

                    dica = GerarDica(codigoDesarme, tentativa);
                    tentativa = "";
                }
            }
            else if (tecla.Key == ConsoleKey.Backspace && tentativa.Length > 0)
            {
                tentativa = tentativa.Substring(0, tentativa.Length - 1);
            }
            else if (tentativa.Length < digitos)
            {
                tentativa += tecla.KeyChar;
            }

            Console.Clear();
            Console.WriteLine($"Tempo restante: {contador}");
            Console.WriteLine($"A senha tem {digitos} digitos.");
            Console.WriteLine($"Tentativas: {tentativasUsadas}/{maxTentativas}");
            Console.WriteLine($"Digite o código de desarme: {tentativa}");
            Console.WriteLine(dica);
        }
    }

    if (!bombaDesarmada)
    {
        contador--;
    }
}

Console.Clear();

if (bombaDesarmada)
{
    Console.WriteLine("Código correto! Bomba desarmada.");
}
else if (tentativasUsadas >= maxTentativas)
{
    Console.WriteLine("Booom! Você usou todas as tentativas.");
}
else
{
    Console.WriteLine("Booom! A contagem chegou ao fim!");
}

string GerarDica(string codigo, string tentativa)
{
    int certosNoLugarCerto = 0;
    int certosNoLugarErrado = 0;

    for (int i = 0; i < tentativa.Length && i < codigo.Length; i++)
    {
        if (tentativa[i] == codigo[i])
        {
            certosNoLugarCerto++;
        }
        else if (codigo.Contains(tentativa[i]))
        {
            certosNoLugarErrado++;
        }
    }

    if (certosNoLugarCerto == 0 && certosNoLugarErrado == 0)
    {
        return "Nenhum caractere está correto.";
    }

    return $"Certos no lugar certo: {certosNoLugarCerto} | Certos no lugar errado: {certosNoLugarErrado}";
}