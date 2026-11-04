using System;
using System.Collections.Generic;

namespace AlgoritmoGrafo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<string>> grafo = new Dictionary<string, List<string>>();

            Console.WriteLine("=== CONFIGURAÇÃO DO GRAFO ===");
            Console.WriteLine("[1] Grafo Não Direcionado");
            Console.WriteLine("[2] Grafo Direcionado");
            Console.Write("Escolha o tipo de grafo: ");

            bool ehDirecionado = Console.ReadLine()?.Trim() == "2";
            Console.WriteLine($"\nGrafo configurado como: {(ehDirecionado ? "DIRECIONADO" : "NÃO DIRECIONADO")}\n");

            int op = 0;
            do
            {
                Console.WriteLine($"=== LISTA DE ADJACÊNCIA ({(ehDirecionado ? "Direcionado" : "Não Direcionado")}) ===");
                Console.WriteLine("[1] Adicionar e remover vértices");
                Console.WriteLine("[2] Adicionar e remover arestas");
                Console.WriteLine("[3] Calcular e exibir o grau de cada vértice");
                Console.WriteLine("[4] Verificar se existe uma aresta entre dois vértices");
                Console.WriteLine("[5] Listar todos os vizinhos de um vértice");
                Console.WriteLine("[6] Verificar se um determinado percurso é possível");
                Console.WriteLine("[7] Visualizar Grafo");
                Console.WriteLine("[8] Finalizar sistema");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out op))
                {
                    Console.WriteLine("Opção inválida! Digite um número.\n");
                    continue;
                }

                switch (op)
                {
                    case 1:
                        Console.WriteLine("\n[1] Adicionar Vértice | [2] Remover Vértice");
                        Console.Write("Escolha: ");
                        string op1 = Console.ReadLine()?.Trim();

                        Console.Write("Nome do vértice: ");
                        string vertice = Console.ReadLine()?.Trim();

                        if (op1 == "1")
                        {
                            if (string.IsNullOrEmpty(vertice))
                            {
                                Console.WriteLine("Nome de vértice inválido.\n");
                            }
                            else if (!grafo.ContainsKey(vertice))
                            {
                                grafo.Add(vertice, new List<string>());
                                Console.WriteLine($"Vértice '{vertice}' adicionado.\n");
                            }
                            else
                            {
                                Console.WriteLine($"Vértice '{vertice}' já existe.\n");
                            }
                        }
                        else if (op1 == "2")
                        {
                            RemoverVertice(grafo, vertice);
                        }
                        else
                        {
                            Console.WriteLine("Opção inválida.\n");
                        }
                        break;

                    case 2:
                        Console.WriteLine("\n[1] Adicionar Aresta | [2] Remover Aresta");
                        Console.Write("Escolha: ");
                        string op2 = Console.ReadLine()?.Trim();

                        Console.Write("Origem: ");
                        string origem = Console.ReadLine()?.Trim();

                        Console.Write("Destino: ");
                        string destino = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(origem) || string.IsNullOrEmpty(destino))
                        {
                            Console.WriteLine("Os vértices informados são inválidos.\n");
                            break;
                        }

                        if (!grafo.ContainsKey(origem) || !grafo.ContainsKey(destino))
                        {
                            Console.WriteLine("Erro: Ambos os vértices precisam existir previamente no grafo.\n");
                            break;
                        }

                        if (op2 == "1")
                        {
                            AdicionarAresta(grafo, origem, destino, ehDirecionado);
                        }
                        else if (op2 == "2")
                        {
                            RemoverAresta(grafo, origem, destino, ehDirecionado);
                        }
                        else
                        {
                            Console.WriteLine("Opção inválida.\n");
                        }
                        break;

                    case 3:
                        if (grafo.Count == 0)
                        {
                            Console.WriteLine("Grafo sem vértices.\n");
                        }
                        else
                        {
                            Console.WriteLine("\n=== GRAU DOS VÉRTICES ===");
                            foreach (var par in grafo)
                            {
                                if (ehDirecionado)
                                {
                                    int grauSaida = par.Value.Count;
                                    int grauEntrada = 0;
                                    foreach (var v in grafo.Values)
                                    {
                                        if (v.Contains(par.Key)) grauEntrada++;
                                    }
                                    Console.WriteLine($"Vértice '{par.Key}': Grau de Saída = {grauSaida} | Grau de Entrada = {grauEntrada}");
                                }
                                else
                                {
                                    Console.WriteLine($"Vértice '{par.Key}': Grau {par.Value.Count}");
                                }
                            }
                            Console.WriteLine();
                        }
                        break;

                    case 4:
                        Console.Write("\nVértice de origem: ");
                        origem = Console.ReadLine()?.Trim();

                        Console.Write("Vértice de destino: ");
                        destino = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(origem) || string.IsNullOrEmpty(destino))
                        {
                            Console.WriteLine("Os vértices informados são inválidos.\n");
                            break;
                        }

                        string representacaoAresta = ehDirecionado ? $"{origem} -> {destino}" : $"{origem} - {destino}";

                        if (VerificarAresta(grafo, origem, destino))
                        {
                            Console.WriteLine($"Existe a aresta ({representacaoAresta}).\n");
                        }
                        else
                        {
                            Console.WriteLine($"NÃO existe a aresta ({representacaoAresta}).\n");
                        }
                        break;

                    case 5:
                        Console.Write("\nDigite o vértice que deseja consultar: ");
                        vertice = Console.ReadLine()?.Trim();

                        if (string.IsNullOrEmpty(vertice))
                        {
                            Console.WriteLine("Nome de vértice inválido.\n");
                            break;
                        }

                        List<string> vizinhos = ListarVizinhos(grafo, vertice);

                        if (!grafo.ContainsKey(vertice))
                        {
                            Console.WriteLine($"Vértice '{vertice}' não encontrado.\n");
                        }
                        else if (vizinhos.Count == 0)
                        {
                            Console.WriteLine($"O vértice '{vertice}' não possui vizinhos conexos de saída.\n");
                        }
                        else
                        {
                            Console.WriteLine($"Vizinhos de '{vertice}':");
                            Console.WriteLine(string.Join(", ", vizinhos));
                            Console.WriteLine();
                        }
                        break;

                    case 6:
                        Console.WriteLine("\n=== VERIFICAR PERCURSO ===");
                        Console.WriteLine("Digite os vértices do percurso separados por vírgulas.");
                        Console.WriteLine("Exemplo: A, B, D");
                        Console.Write("Percurso: ");

                        string entradaPercurso = Console.ReadLine()?.Trim();

                        if (string.IsNullOrWhiteSpace(entradaPercurso))
                        {
                            Console.WriteLine("Percurso inválido ou vazio.\n");
                            break;
                        }

                        List<string> percurso = new List<string>();
                        string[] verticesPercurso = entradaPercurso.Split(',');

                        foreach (string item in verticesPercurso)
                        {
                            string nomeVertice = item.Trim();
                            if (nomeVertice.Length == 0)
                            {
                                percurso.Clear();
                                break;
                            }
                            percurso.Add(nomeVertice);
                        }

                        if (percurso.Count == 0)
                        {
                            Console.WriteLine("Percurso inválido.\n");
                            break;
                        }

                        bool resultado = VerificarPercurso(grafo, percurso);

                        if (resultado)
                        {
                            Console.WriteLine("O percurso informado é possível.\n");
                        }
                        else
                        {
                            Console.WriteLine("O percurso informado NÃO é possível.\n");
                        }
                        break;

                    case 7:
                        Console.WriteLine("\n=== ESTRUTURA DO GRAFO (LISTA DE ADJACÊNCIA) ===");
                        if (grafo.Count == 0)
                        {
                            Console.WriteLine("Grafo sem vértices.\n");
                        }
                        else
                        {
                            foreach (var par in grafo)
                            {
                                string vizinhosStr = par.Value.Count > 0
                                    ? string.Join(", ", par.Value)
                                    : "Sem vizinhos";

                                Console.WriteLine($"Vértice '{par.Key}' -> [{vizinhosStr}]");
                            }
                            Console.WriteLine();
                        }
                        break;

                    case 8:
                        Console.WriteLine("Programa finalizado.");
                        break;

                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.\n");
                        break;
                }

            } while (op != 8);
        }

        static void RemoverVertice(Dictionary<string, List<string>> grafo, string vertice)
        {
            if (string.IsNullOrEmpty(vertice))
            {
                Console.WriteLine("Nome de vértice inválido.\n");
                return;
            }

            if (grafo.Remove(vertice))
            {
                foreach (var lista in grafo.Values)
                {
                    lista.Remove(vertice);
                }

                Console.WriteLine($"Vértice '{vertice}' e suas arestas foram removidos.\n");
            }
            else
            {
                Console.WriteLine($"Vértice '{vertice}' não encontrado.\n");
            }
        }

        static void AdicionarAresta(Dictionary<string, List<string>> grafo, string origem, string destino, bool ehDirecionado)
        {
            if (!grafo[origem].Contains(destino))
            {
                grafo[origem].Add(destino);
            }

            if (!ehDirecionado && !grafo[destino].Contains(origem))
            {
                grafo[destino].Add(origem);
            }

            string simbolo = ehDirecionado ? "->" : "-";
            Console.WriteLine($"Aresta ({origem} {simbolo} {destino}) adicionada.\n");
        }

        static void RemoverAresta(Dictionary<string, List<string>> grafo, string origem, string destino, bool ehDirecionado)
        {
            grafo[origem].Remove(destino);

            if (!ehDirecionado)
            {
                grafo[destino].Remove(origem);
            }

            string simbolo = ehDirecionado ? "->" : "-";
            Console.WriteLine($"Aresta ({origem} {simbolo} {destino}) removida.\n");
        }

        static bool VerificarAresta(Dictionary<string, List<string>> grafo, string origem, string destino)
        {
            return grafo.ContainsKey(origem) && grafo[origem].Contains(destino);
        }

        static List<string> ListarVizinhos(Dictionary<string, List<string>> grafo, string vertice)
        {
            if (grafo.ContainsKey(vertice))
            {
                return grafo[vertice];
            }

            return new List<string>();
        }

        static bool VerificarPercurso(Dictionary<string, List<string>> grafo, List<string> percurso)
        {
            if (percurso.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < percurso.Count - 1; i++)
            {
                string atual = percurso[i];
                string proximo = percurso[i + 1];

                if (!grafo.ContainsKey(atual) || !grafo.ContainsKey(proximo))
                {
                    return false;
                }

                if (!grafo[atual].Contains(proximo))
                {
                    return false;
                }
            }

            return grafo.ContainsKey(percurso[percurso.Count - 1]);
        }
    }
}