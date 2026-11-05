using System;
using System.Collections.Generic;
using System.Linq;

namespace ListaDeAdjacencia
{
    public class Vertice<G>
    {
        public G Origem { get; set; }
        public G Destino { get; set; }

        public Vertice(G origem, G destino)
        {
            Origem = origem;
            Destino = destino;
        }
    }

    public class Grafo<G>
    {
        public List<G> Vertices { get; private set; }
        public List<Vertice<G>> Arestas { get; private set; }
        public bool EhDirecionado { get; private set; }

        public Grafo(bool ehDirecionado = false)
        {
            Vertices = new List<G>();
            Arestas = new List<Vertice<G>>();
            EhDirecionado = ehDirecionado;
        }

        public bool AdicionarVertice(G vertice)
        {
            if (Vertices.Contains(vertice))
                return false;

            Vertices.Add(vertice);
            return true;
        }

        public bool RemoverVertice(G vertice)
        {
            if (!Vertices.Contains(vertice))
                return false;

            Vertices.Remove(vertice);
            Arestas.RemoveAll(a =>
                EqualityComparer<G>.Default.Equals(a.Origem, vertice) ||
                EqualityComparer<G>.Default.Equals(a.Destino, vertice));

            return true;
        }

        public bool AdicionarAresta(G origem, G destino)
        {
            if (!Vertices.Contains(origem) || !Vertices.Contains(destino))
                return false;

            if (ExisteAresta(origem, destino))
                return false;

            Arestas.Add(new Vertice<G>(origem, destino));
            return true;
        }

        public bool RemoverAresta(G origem, G destino)
        {
            int removidos = Arestas.RemoveAll(a =>
                (EqualityComparer<G>.Default.Equals(a.Origem, origem) && EqualityComparer<G>.Default.Equals(a.Destino, destino)) ||
                (!EhDirecionado && EqualityComparer<G>.Default.Equals(a.Origem, destino) && EqualityComparer<G>.Default.Equals(a.Destino, origem))
            );

            return removidos > 0;
        }

        public bool ExisteAresta(G origem, G destino)
        {
            return Arestas.Any(a =>
                (EqualityComparer<G>.Default.Equals(a.Origem, origem) && EqualityComparer<G>.Default.Equals(a.Destino, destino)) ||
                (!EhDirecionado && EqualityComparer<G>.Default.Equals(a.Origem, destino) && EqualityComparer<G>.Default.Equals(a.Destino, origem))
            );
        }

        public void ExibirGrauVertice()
        {
            if (Vertices.Count == 0)
            {
                Console.WriteLine("Grafo sem vértices.\n");
                return;
            }

            Console.WriteLine("\n=== GRAU DOS VÉRTICES ===");
            foreach (var v in Vertices)
            {
                if (EhDirecionado)
                {
                    int grauSaida = Arestas.Count(a => EqualityComparer<G>.Default.Equals(a.Origem, v));
                    int grauEntrada = Arestas.Count(a => EqualityComparer<G>.Default.Equals(a.Destino, v));
                    Console.WriteLine($"Vértice '{v}': Grau de Saída = {grauSaida} | Grau de Entrada = {grauEntrada}");
                }
                else
                {
                    int grau = Arestas.Count(a =>
                        EqualityComparer<G>.Default.Equals(a.Origem, v) ||
                        EqualityComparer<G>.Default.Equals(a.Destino, v));
                    Console.WriteLine($"Vértice '{v}': Grau {grau}");
                }
            }
            Console.WriteLine();
        }

        public void BuscarVizinhos(G vertice)
        {
            if (!Vertices.Contains(vertice))
            {
                Console.WriteLine($"Vértice '{vertice}' não encontrado.\n");
                return;
            }

            Console.WriteLine($"Para '{vertice}', são vizinhos:");
            bool encontrou = false;

            foreach (var aresta in Arestas)
            {
                if (EqualityComparer<G>.Default.Equals(vertice, aresta.Origem))
                {
                    Console.WriteLine($" -> {aresta.Destino}");
                    encontrou = true;
                }
                else if (!EhDirecionado && EqualityComparer<G>.Default.Equals(vertice, aresta.Destino))
                {
                    Console.WriteLine($" -> {aresta.Origem}");
                    encontrou = true;
                }
            }

            if (!encontrou)
            {
                Console.WriteLine(" (Nenhum vizinho encontrado)");
            }
            Console.WriteLine();
        }

        public bool VerificarPercurso(List<G> percurso)
        {
            if (percurso == null || percurso.Count < 2)
                return false;

            foreach (var v in percurso)
            {
                if (!Vertices.Contains(v))
                    return false;
            }

            for (int i = 0; i < percurso.Count - 1; i++)
            {
                G atual = percurso[i];
                G proximo = percurso[i + 1];

                if (!ExisteAresta(atual, proximo))
                    return false;
            }

            return true;
        }

        public void ImprimirListaArestas()
        {
            Console.WriteLine(EhDirecionado
                ? "\n=== ESTRUTURA DO GRAFO (DIRECIONADO) ==="
                : "\n=== ESTRUTURA DO GRAFO (NÃO-DIRECIONADO) ===");

            Console.WriteLine($"Vértices ({Vertices.Count}): [{string.Join(", ", Vertices)}]");
            Console.WriteLine("Arestas:");

            if (Arestas.Count == 0)
            {
                Console.WriteLine(" (Nenhuma aresta cadastrada)");
            }
            else
            {
                string simbolo = EhDirecionado ? "->" : "<->";
                foreach (var aresta in Arestas)
                {
                    Console.WriteLine($" ({aresta.Origem} {simbolo} {aresta.Destino})");
                }
            }
            Console.WriteLine();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== CONFIGURAÇÃO DO GRAFO ===");
            Console.WriteLine("[1] Grafo Não Direcionado");
            Console.WriteLine("[2] Grafo Direcionado");
            Console.Write("Escolha o tipo de grafo: ");

            bool ehDirecionado = Console.ReadLine()?.Trim() == "2";
            Grafo<string> grafo = new Grafo<string>(ehDirecionado);

            Console.WriteLine($"\nGrafo configurado como: {(ehDirecionado ? "DIRECIONADO" : "NÃO DIRECIONADO")}\n");

            int op = 0;
            do
            {
                Console.WriteLine($"=== LISTA DE ARESTAS ({(grafo.EhDirecionado ? "Direcionado" : "Não Direcionado")}) ===");
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
                        string v = Console.ReadLine()?.ToLower().Trim();

                        if (string.IsNullOrEmpty(v))
                        {
                            Console.WriteLine("Nome de vértice inválido.\n");
                            break;
                        }

                        if (op1 == "1")
                        {
                            if (grafo.AdicionarVertice(v))
                                Console.WriteLine($"Vértice '{v}' adicionado.\n");
                            else
                                Console.WriteLine($"Vértice '{v}' já existe.\n");
                        }
                        else if (op1 == "2")
                        {
                            if (grafo.RemoverVertice(v))
                                Console.WriteLine($"Vértice '{v}' e suas arestas foram removidos.\n");
                            else
                                Console.WriteLine($"Vértice '{v}' não encontrado.\n");
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
                        string v1 = Console.ReadLine()?.ToLower().Trim();
                        Console.Write("Destino: ");
                        string v2 = Console.ReadLine()?.ToLower().Trim();

                        if (string.IsNullOrEmpty(v1) || string.IsNullOrEmpty(v2))
                        {
                            Console.WriteLine("Vértices inválidos.\n");
                            break;
                        }

                        if (op2 == "1")
                        {
                            if (grafo.AdicionarAresta(v1, v2))
                                Console.WriteLine($"Aresta ({v1} -> {v2}) adicionada.\n");
                            else
                                Console.WriteLine("Erro: Verifique se ambos os vértices existem ou se a aresta já foi cadastrada.\n");
                        }
                        else if (op2 == "2")
                        {
                            if (grafo.RemoverAresta(v1, v2))
                                Console.WriteLine($"Aresta entre '{v1}' e '{v2}' removida.\n");
                            else
                                Console.WriteLine("Aresta não encontrada.\n");
                        }
                        else
                        {
                            Console.WriteLine("Opção inválida.\n");
                        }
                        break;

                    case 3:
                        grafo.ExibirGrauVertice();
                        break;

                    case 4:
                        Console.Write("\nVértice de origem: ");
                        string vOrigem = Console.ReadLine()?.ToLower().Trim();
                        Console.Write("Vértice de destino: ");
                        string vDestino = Console.ReadLine()?.ToLower().Trim();

                        string simbolo = grafo.EhDirecionado ? "->" : "<->";

                        if (grafo.ExisteAresta(vOrigem, vDestino))
                            Console.WriteLine($"Existe a aresta ({vOrigem} {simbolo} {vDestino}).\n");
                        else
                            Console.WriteLine($"NÃO existe a aresta ({vOrigem} {simbolo} {vDestino}).\n");
                        break;

                    case 5:
                        Console.Write("\nDigite o vértice para consultar os vizinhos: ");
                        string vConsulta = Console.ReadLine()?.ToLower().Trim();
                        grafo.BuscarVizinhos(vConsulta);
                        break;

                    case 6:
                        Console.WriteLine("\n=== VERIFICAR PERCURSO ===");
                        Console.WriteLine("Digite os vértices do percurso separados por vírgulas (Exemplo: a, b, c)");
                        Console.Write("Percurso: ");

                        string entradaPercurso = Console.ReadLine()?.Trim();
                        if (string.IsNullOrWhiteSpace(entradaPercurso))
                        {
                            Console.WriteLine("Percurso inválido.\n");
                            break;
                        }

                        List<string> percurso = entradaPercurso
                            .Split(',')
                            .Select(p => p.ToLower().Trim())
                            .Where(p => !string.IsNullOrEmpty(p))
                            .ToList();

                        if (grafo.VerificarPercurso(percurso))
                            Console.WriteLine("O percurso informado é POSSÍVEL!\n");
                        else
                            Console.WriteLine("O percurso informado NÃO é possível.\n");
                        break;

                    case 7:
                        grafo.ImprimirListaArestas();
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
    }
}