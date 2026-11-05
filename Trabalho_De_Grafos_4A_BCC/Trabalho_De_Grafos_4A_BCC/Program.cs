using System;
using System.Collections.Generic;
using System.Linq;

namespace Trabalho_De_Grados_4A_BCC
{
    public interface IGrafo
    {
        bool EhDirecionado { get; }
        string NomeEstrutura { get; }
        bool AddVertice(string nome);
        bool DelVertice(string nome);
        void AddAresta(string origem, string destino);
        void DelAresta(string origem, string destino);
        void ExibirGrauVertice();
        bool ExisteAresta(string origem, string destino);
        void ExibirVizinhos(string vertice);
        bool VerificarPercurso(List<string> percurso);
        void ImprimirEstrutura();
    }

    //LISTA DE ADJACÊNCIA
    public class GrafoListaAdjacencia : IGrafo
    {
        private Dictionary<string, List<string>> adj;
        public bool EhDirecionado { get; private set; }
        public string NomeEstrutura => "Lista de Adjacência";

        public GrafoListaAdjacencia(bool ehDirecionado)
        {
            EhDirecionado = ehDirecionado;
            adj = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }

        public bool AddVertice(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome) || adj.ContainsKey(nome))
                return false;

            adj.Add(nome, new List<string>());
            return true;
        }

        public bool DelVertice(string nome)
        {
            if (!adj.Remove(nome))
                return false;

            foreach (var lista in adj.Values)
            {
                lista.RemoveAll(v => v.Equals(nome, StringComparison.OrdinalIgnoreCase));
            }
            return true;
        }

        public void AddAresta(string origem, string destino)
        {
            if (!adj.ContainsKey(origem) || !adj.ContainsKey(destino))
            {
                Console.WriteLine("Erro: Ambos os vértices precisam existir previamente no grafo.\n");
                return;
            }

            if (!adj[origem].Contains(destino, StringComparer.OrdinalIgnoreCase))
            {
                adj[origem].Add(destino);
            }

            if (!EhDirecionado && !adj[destino].Contains(origem, StringComparer.OrdinalIgnoreCase))
            {
                adj[destino].Add(origem);
            }

            string simbolo = EhDirecionado ? "->" : "<->";
            Console.WriteLine($"Aresta ({origem} {simbolo} {destino}) adicionada.\n");
        }

        public void DelAresta(string origem, string destino)
        {
            if (!adj.ContainsKey(origem) || !adj.ContainsKey(destino))
            {
                Console.WriteLine("Erro: Vértices não encontrados.\n");
                return;
            }

            adj[origem].RemoveAll(v => v.Equals(destino, StringComparison.OrdinalIgnoreCase));
            if (!EhDirecionado)
            {
                adj[destino].RemoveAll(v => v.Equals(origem, StringComparison.OrdinalIgnoreCase));
            }

            string simbolo = EhDirecionado ? "->" : "<->";
            Console.WriteLine($"Aresta ({origem} {simbolo} {destino}) removida.\n");
        }

        public void ExibirGrauVertice()
        {
            if (adj.Count == 0)
            {
                Console.WriteLine("Grafo sem vértices.\n");
                return;
            }

            Console.WriteLine("\n=== GRAU DOS VÉRTICES ===");
            foreach (var par in adj)
            {
                if (EhDirecionado)
                {
                    int grauSaida = par.Value.Count;
                    int grauEntrada = adj.Values.Count(lista => lista.Contains(par.Key, StringComparer.OrdinalIgnoreCase));
                    Console.WriteLine($"Vértice '{par.Key}': Grau de Saída = {grauSaida} | Grau de Entrada = {grauEntrada}");
                }
                else
                {
                    Console.WriteLine($"Vértice '{par.Key}': Grau = {par.Value.Count}");
                }
            }
            Console.WriteLine();
        }

        public bool ExisteAresta(string origem, string destino)
        {
            return adj.ContainsKey(origem) && adj[origem].Contains(destino, StringComparer.OrdinalIgnoreCase);
        }

        public void ExibirVizinhos(string vertice)
        {
            if (!adj.ContainsKey(vertice))
            {
                Console.WriteLine($"Vértice '{vertice}' não encontrado.\n");
                return;
            }

            var vizinhos = adj[vertice];
            string label = EhDirecionado ? "Vizinhos de saída" : "Vizinhos";

            if (vizinhos.Count == 0)
                Console.WriteLine($"{label} de '{vertice}': Nenhum.\n");
            else
                Console.WriteLine($"{label} de '{vertice}': {string.Join(", ", vizinhos)}\n");
        }

        public bool VerificarPercurso(List<string> percurso)
        {
            if (percurso == null || percurso.Count < 2) return false;

            for (int i = 0; i < percurso.Count - 1; i++)
            {
                string atual = percurso[i];
                string proximo = percurso[i + 1];

                if (!ExisteAresta(atual, proximo))
                    return false;
            }
            return true;
        }

        public void ImprimirEstrutura()
        {
            Console.WriteLine("\n=== ESTRUTURA DO GRAFO (LISTA DE ADJACÊNCIA) ===");
            if (adj.Count == 0)
            {
                Console.WriteLine("Grafo sem vértices.\n");
                return;
            }

            foreach (var par in adj)
            {
                string vizinhosStr = par.Value.Count > 0 ? string.Join(", ", par.Value) : "Sem vizinhos";
                Console.WriteLine($"Vértice '{par.Key}' -> [{vizinhosStr}]");
            }
            Console.WriteLine();
        }
    }

    //MATRIZ DE ADJACÊNCIA
    public class GrafoMatrizAdjacencia : IGrafo
    {
        private int[,] matriz;
        private List<string> vertices;
        public bool EhDirecionado { get; private set; }
        public string NomeEstrutura => "Matriz de Adjacência";

        public GrafoMatrizAdjacencia(bool ehDirecionado)
        {
            EhDirecionado = ehDirecionado;
            vertices = new List<string>();
            matriz = new int[0, 0];
        }

        private int ObterIndice(string nome)
        {
            return vertices.FindIndex(v => v.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public bool AddVertice(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome) || ObterIndice(nome) != -1)
                return false;

            int novoTam = vertices.Count + 1;
            int[,] novaMatriz = new int[novoTam, novoTam];

            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    novaMatriz[i, j] = matriz[i, j];
                }
            }

            matriz = novaMatriz;
            vertices.Add(nome);
            return true;
        }

        public bool DelVertice(string nome)
        {
            int idx = ObterIndice(nome);
            if (idx == -1) return false;

            int novoTam = vertices.Count - 1;
            int[,] novaMatriz = new int[novoTam, novoTam];

            int novaLinha = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (i == idx) continue;
                int novaCol = 0;
                for (int j = 0; j < vertices.Count; j++)
                {
                    if (j == idx) continue;
                    novaMatriz[novaLinha, novaCol] = matriz[i, j];
                    novaCol++;
                }
                novaLinha++;
            }

            matriz = novaMatriz;
            vertices.RemoveAt(idx);
            return true;
        }

        public void AddAresta(string origem, string destino)
        {
            int i = ObterIndice(origem);
            int j = ObterIndice(destino);

            if (i == -1 || j == -1)
            {
                Console.WriteLine("Erro: Vértices não encontrados.\n");
                return;
            }

            matriz[i, j] = 1;
            if (!EhDirecionado) matriz[j, i] = 1;

            string simbolo = EhDirecionado ? "->" : "<->";
            Console.WriteLine($"Aresta ({origem} {simbolo} {destino}) adicionada.\n");
        }

        public void DelAresta(string origem, string destino)
        {
            int i = ObterIndice(origem);
            int j = ObterIndice(destino);

            if (i == -1 || j == -1)
            {
                Console.WriteLine("Erro: Vértices não encontrados.\n");
                return;
            }

            matriz[i, j] = 0;
            if (!EhDirecionado) matriz[j, i] = 0;

            string simbolo = EhDirecionado ? "->" : "<->";
            Console.WriteLine($"Aresta ({origem} {simbolo} {destino}) removida.\n");
        }

        public void ExibirGrauVertice()
        {
            if (vertices.Count == 0)
            {
                Console.WriteLine("Grafo sem vértices.\n");
                return;
            }

            Console.WriteLine("\n=== GRAU DOS VÉRTICES ===");
            for (int i = 0; i < vertices.Count; i++)
            {
                if (EhDirecionado)
                {
                    int grauSaida = 0, grauEntrada = 0;
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        if (matriz[i, j] == 1) grauSaida++;
                        if (matriz[j, i] == 1) grauEntrada++;
                    }
                    Console.WriteLine($"Vértice '{vertices[i]}': Grau de Saída = {grauSaida} | Grau de Entrada = {grauEntrada}");
                }
                else
                {
                    int grau = 0;
                    for (int j = 0; j < vertices.Count; j++)
                    {
                        if (matriz[i, j] == 1) grau++;
                    }
                    Console.WriteLine($"Vértice '{vertices[i]}': Grau = {grau}");
                }
            }
            Console.WriteLine();
        }

        public bool ExisteAresta(string origem, string destino)
        {
            int i = ObterIndice(origem);
            int j = ObterIndice(destino);
            return i != -1 && j != -1 && matriz[i, j] == 1;
        }

        public void ExibirVizinhos(string vertice)
        {
            int i = ObterIndice(vertice);
            if (i == -1)
            {
                Console.WriteLine($"Vértice '{vertice}' não encontrado.\n");
                return;
            }

            List<string> vizinhos = new List<string>();
            for (int j = 0; j < vertices.Count; j++)
            {
                if (matriz[i, j] == 1) vizinhos.Add(vertices[j]);
            }

            string label = EhDirecionado ? "Vizinhos de saída" : "Vizinhos";
            if (vizinhos.Count == 0)
                Console.WriteLine($"{label} de '{vertice}': Nenhum.\n");
            else
                Console.WriteLine($"{label} de '{vertice}': {string.Join(", ", vizinhos)}\n");
        }

        public bool VerificarPercurso(List<string> percurso)
        {
            if (percurso == null || percurso.Count < 2) return false;

            for (int k = 0; k < percurso.Count - 1; k++)
            {
                if (!ExisteAresta(percurso[k], percurso[k + 1]))
                    return false;
            }
            return true;
        }

        public void ImprimirEstrutura()
        {
            if (vertices.Count == 0)
            {
                Console.WriteLine("Grafo sem vértices.\n");
                return;
            }

            Console.WriteLine("\n=== ESTRUTURA DO GRAFO (MATRIZ DE ADJACÊNCIA) ===");
            Console.Write("".PadRight(10));
            for (int j = 0; j < vertices.Count; j++)
                Console.Write(vertices[j].PadRight(8));
            Console.WriteLine();

            for (int i = 0; i < vertices.Count; i++)
            {
                Console.Write(vertices[i].PadRight(10));
                for (int j = 0; j < vertices.Count; j++)
                {
                    Console.Write(matriz[i, j].ToString().PadRight(8));
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    //LISTA DE ARESTAS
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

    public class GrafoListaArestas : IGrafo
    {
        private List<string> vertices;
        private List<Vertice<string>> arestas;
        public bool EhDirecionado { get; private set; }
        public string NomeEstrutura => "Lista de Arestas";

        public GrafoListaArestas(bool ehDirecionado)
        {
            EhDirecionado = ehDirecionado;
            vertices = new List<string>();
            arestas = new List<Vertice<string>>();
        }

        public bool AddVertice(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome) || vertices.Contains(nome, StringComparer.OrdinalIgnoreCase))
                return false;

            vertices.Add(nome);
            return true;
        }

        public bool DelVertice(string nome)
        {
            string verticeEncontrado = vertices.FirstOrDefault(v => v.Equals(nome, StringComparison.OrdinalIgnoreCase));
            if (verticeEncontrado == null) return false;

            vertices.Remove(verticeEncontrado);
            arestas.RemoveAll(a => a.Origem.Equals(nome, StringComparison.OrdinalIgnoreCase) ||
                                   a.Destino.Equals(nome, StringComparison.OrdinalIgnoreCase));
            return true;
        }

        public void AddAresta(string origem, string destino)
        {
            if (!vertices.Contains(origem, StringComparer.OrdinalIgnoreCase) ||
                !vertices.Contains(destino, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine("Erro: Ambos os vértices precisam existir previamente no grafo.\n");
                return;
            }

            if (!ExisteAresta(origem, destino))
            {
                arestas.Add(new Vertice<string>(origem, destino));
                if (!EhDirecionado && !origem.Equals(destino, StringComparison.OrdinalIgnoreCase))
                {
                    arestas.Add(new Vertice<string>(destino, origem));
                }
                string simbolo = EhDirecionado ? "->" : "<->";
                Console.WriteLine($"Aresta ({origem} {simbolo} {destino}) adicionada.\n");
            }
            else
            {
                Console.WriteLine("Aresta já existente.\n");
            }
        }

        public void DelAresta(string origem, string destino)
        {
            int qtd = arestas.RemoveAll(a =>
                (a.Origem.Equals(origem, StringComparison.OrdinalIgnoreCase) && a.Destino.Equals(destino, StringComparison.OrdinalIgnoreCase)) ||
                (!EhDirecionado && a.Origem.Equals(destino, StringComparison.OrdinalIgnoreCase) && a.Destino.Equals(origem, StringComparison.OrdinalIgnoreCase))
            );

            if (qtd > 0)
                Console.WriteLine($"Aresta ({origem} - {destino}) removida.\n");
            else
                Console.WriteLine("Aresta não encontrada.\n");
        }

        public void ExibirGrauVertice()
        {
            if (vertices.Count == 0)
            {
                Console.WriteLine("Grafo sem vértices.\n");
                return;
            }

            Console.WriteLine("\n=== GRAU DOS VÉRTICES ===");
            foreach (var v in vertices)
            {
                if (EhDirecionado)
                {
                    int grauSaida = arestas.Count(a => a.Origem.Equals(v, StringComparison.OrdinalIgnoreCase));
                    int grauEntrada = arestas.Count(a => a.Destino.Equals(v, StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine($"Vértice '{v}': Grau de Saída = {grauSaida} | Grau de Entrada = {grauEntrada}");
                }
                else
                {
                    int grau = arestas.Count(a => a.Origem.Equals(v, StringComparison.OrdinalIgnoreCase));
                    Console.WriteLine($"Vértice '{v}': Grau = {grau}");
                }
            }
            Console.WriteLine();
        }

        public bool ExisteAresta(string origem, string destino)
        {
            return arestas.Any(a => a.Origem.Equals(origem, StringComparison.OrdinalIgnoreCase) &&
                                    a.Destino.Equals(destino, StringComparison.OrdinalIgnoreCase));
        }

        public void ExibirVizinhos(string vertice)
        {
            if (!vertices.Contains(vertice, StringComparer.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Vértice '{vertice}' não encontrado.\n");
                return;
            }

            var vizinhos = arestas
                .Where(a => a.Origem.Equals(vertice, StringComparison.OrdinalIgnoreCase))
                .Select(a => a.Destino)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            string label = EhDirecionado ? "Vizinhos de saída" : "Vizinhos";
            if (vizinhos.Count == 0)
                Console.WriteLine($"{label} de '{vertice}': Nenhum.\n");
            else
                Console.WriteLine($"{label} de '{vertice}': {string.Join(", ", vizinhos)}\n");
        }

        public bool VerificarPercurso(List<string> percurso)
        {
            if (percurso == null || percurso.Count < 2) return false;

            for (int i = 0; i < percurso.Count - 1; i++)
            {
                if (!ExisteAresta(percurso[i], percurso[i + 1]))
                    return false;
            }
            return true;
        }

        public void ImprimirEstrutura()
        {
            Console.WriteLine("\n=== ESTRUTURA DO GRAFO (LISTA DE ARESTAS) ===");
            Console.WriteLine($"Vértices ({vertices.Count}): [{string.Join(", ", vertices)}]");
            Console.WriteLine("Arestas:");

            if (arestas.Count == 0)
            {
                Console.WriteLine(" (Nenhuma aresta cadastrada)");
            }
            else
            {
                string simbolo = EhDirecionado ? "->" : "<->";
                foreach (var a in arestas)
                {
                    Console.WriteLine($" ({a.Origem} {simbolo} {a.Destino})");
                }
            }
            Console.WriteLine();
        }
    }

    //PROGRAMA PRINCIPAL
    internal class Program
    {
        static void Main(string[] args)
        {
            IGrafo grafo = ConfigurarNovoGrafo();

            int op = 0;
            do
            {
                Console.WriteLine($"=== SISTEMA DE GRAFOS ===");
                Console.WriteLine($"Estrutura: {grafo.NomeEstrutura} | Modelo: {(grafo.EhDirecionado ? "Direcionado" : "Não Direcionado")}");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("[1] Adicionar e remover vértices");
                Console.WriteLine("[2] Adicionar e remover arestas");
                Console.WriteLine("[3] Calcular e exibir o grau de cada vértice");
                Console.WriteLine("[4] Verificar se existe uma aresta entre dois vértices");
                Console.WriteLine("[5] Listar todos os vizinhos de um vértice");
                Console.WriteLine("[6] Verificar se um determinado percurso é possível");
                Console.WriteLine("[7] Visualizar Grafo");
                Console.WriteLine("[8] Alterar Configuração do Grafo (Estrutura/Modelo)");
                Console.WriteLine("[9] Finalizar sistema");
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
                        string v = Console.ReadLine()?.Trim();

                        if (op1 == "1")
                        {
                            if (grafo.AddVertice(v))
                                Console.WriteLine($"Vértice '{v}' adicionado.\n");
                            else
                                Console.WriteLine($"Erro: Vértice '{v}' inválido ou já existente.\n");
                        }
                        else if (op1 == "2")
                        {
                            if (grafo.DelVertice(v))
                                Console.WriteLine($"Vértice '{v}' e suas conexões foram removidos.\n");
                            else
                                Console.WriteLine($"Erro: Vértice '{v}' não encontrado.\n");
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
                        string o = Console.ReadLine()?.Trim();
                        Console.Write("Destino: ");
                        string d = Console.ReadLine()?.Trim();

                        if (op2 == "1")
                        {
                            grafo.AddAresta(o, d);
                        }
                        else if (op2 == "2")
                        {
                            grafo.DelAresta(o, d);
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
                        string vOrig = Console.ReadLine()?.Trim();
                        Console.Write("Vértice de destino: ");
                        string vDest = Console.ReadLine()?.Trim();

                        string simbolo = grafo.EhDirecionado ? "->" : "<->";

                        if (grafo.ExisteAresta(vOrig, vDest))
                            Console.WriteLine($"Existe a aresta ({vOrig} {simbolo} {vDest}).\n");
                        else
                            Console.WriteLine($"NÃO existe a aresta ({vOrig} {simbolo} {vDest}).\n");
                        break;

                    case 5:
                        Console.Write("\nDigite o vértice que deseja consultar: ");
                        string vViz = Console.ReadLine()?.Trim();
                        grafo.ExibirVizinhos(vViz);
                        break;

                    case 6:
                        Console.WriteLine("\n=== VERIFICAR PERCURSO ===");
                        Console.WriteLine("Digite os vértices do percurso separados por vírgula (Exemplo: A, B, C)");
                        Console.Write("Percurso: ");

                        string entrada = Console.ReadLine()?.Trim();
                        if (string.IsNullOrWhiteSpace(entrada))
                        {
                            Console.WriteLine("Percurso inválido.\n");
                            break;
                        }

                        List<string> percurso = entrada.Split(',')
                            .Select(p => p.Trim())
                            .Where(p => !string.IsNullOrEmpty(p))
                            .ToList();

                        if (grafo.VerificarPercurso(percurso))
                            Console.WriteLine("O percurso informado é POSSÍVEL!\n");
                        else
                            Console.WriteLine("O percurso informado NÃO é possível.\n");
                        break;

                    case 7:
                        grafo.ImprimirEstrutura();
                        break;

                    case 8:
                        grafo = ConfigurarNovoGrafo();
                        break;

                    case 9:
                        Console.WriteLine("\nSistema finalizado. Até logo!");
                        break;

                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.\n");
                        break;
                }

            } while (op != 9);
        }

        private static IGrafo ConfigurarNovoGrafo()
        {
            Console.WriteLine("\n=== CONFIGURAÇÃO INICIAL DO GRAFO ===");
            Console.WriteLine("Escolha a representação de dados:");
            Console.WriteLine("[1] Lista de Adjacência");
            Console.WriteLine("[2] Matriz de Adjacência");
            Console.WriteLine("[3] Lista de Arestas");
            Console.Write("Opção: ");
            string opEstrutura = Console.ReadLine()?.Trim();

            Console.WriteLine("\nEscolha o modelo de direcionamento:");
            Console.WriteLine("[1] Grafo Não Direcionado");
            Console.WriteLine("[2] Grafo Direcionado");
            Console.Write("Opção: ");
            bool ehDirecionado = Console.ReadLine()?.Trim() == "2";

            IGrafo novoGrafo;
            switch (opEstrutura)
            {
                case "2":
                    novoGrafo = new GrafoMatrizAdjacencia(ehDirecionado);
                    break;
                case "3":
                    novoGrafo = new GrafoListaArestas(ehDirecionado);
                    break;
                case "1":
                default:
                    novoGrafo = new GrafoListaAdjacencia(ehDirecionado);
                    break;
            }

            Console.WriteLine($"\nGrafo recriado usando [{novoGrafo.NomeEstrutura}] - Mode: {(ehDirecionado ? "DIRECIONADO" : "NÃO DIRECIONADO")}\n");
            return novoGrafo;
        }
    }

}