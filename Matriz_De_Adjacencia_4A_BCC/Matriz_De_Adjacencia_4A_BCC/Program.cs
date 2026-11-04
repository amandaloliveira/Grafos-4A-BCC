using System;
using System.Collections.Generic;

namespace MatrizDeAdjacencia_Grafos
{
    internal class Grafo
    {
        private int[,] matriz;
        private List<string> vertices;
        private bool ehDirecionado;

        public Grafo(bool ehDirecionado)
        {
            this.ehDirecionado = ehDirecionado;
            this.vertices = new List<string>();
            this.matriz = new int[0, 0];
        }

        public int GetNumVertices() => vertices.Count;
        public bool IsDirecionado() => ehDirecionado;

        // Retorna o índice correspondente ao nome do vértice na matriz
        private int ObterIndice(string nome)
        {
            return vertices.FindIndex(v => v.Equals(nome, StringComparison.OrdinalIgnoreCase));
        }

        public bool AddVertice(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("Erro: Nome de vértice inválido!");
                return false;
            }

            if (ObterIndice(nome) != -1)
            {
                Console.WriteLine($"Erro: Vértice '{nome}' já existe!");
                return false;
            }

            int novoNum = vertices.Count + 1;
            int[,] novaMatriz = new int[novoNum, novoNum];

            // Copia os dados existentes para a nova matriz expandida
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    novaMatriz[i, j] = matriz[i, j];
                }
            }

            matriz = novaMatriz;
            vertices.Add(nome);
            Console.WriteLine($"Vértice '{nome}' adicionado com sucesso!");
            return true;
        }

        public bool DelVertice(string nome)
        {
            int idx = ObterIndice(nome);
            if (idx == -1)
            {
                Console.WriteLine($"Erro: Vértice '{nome}' não encontrado!");
                return false;
            }

            int novoNum = vertices.Count - 1;
            int[,] novaMatriz = new int[novoNum, novoNum];

            int novaLinha = 0;
            for (int i = 0; i < vertices.Count; i++)
            {
                if (i == idx) continue;

                int novaColuna = 0;
                for (int j = 0; j < vertices.Count; j++)
                {
                    if (j == idx) continue;

                    novaMatriz[novaLinha, novaColuna] = matriz[i, j];
                    novaColuna++;
                }
                novaLinha++;
            }

            matriz = novaMatriz;
            vertices.RemoveAt(idx);
            Console.WriteLine($"Vértice '{nome}' removido com sucesso!");
            return true;
        }

        public void AddAresta(string origem, string destino)
        {
            int idxOrig = ObterIndice(origem);
            int idxDest = ObterIndice(destino);

            if (idxOrig == -1 || idxDest == -1)
            {
                Console.WriteLine("Erro: Um ou ambos os vértices não foram encontrados no grafo.");
                return;
            }

            matriz[idxOrig, idxDest] = 1;

            if (!ehDirecionado)
            {
                matriz[idxDest, idxOrig] = 1;
                Console.WriteLine($"Aresta ({vertices[idxOrig]} <-> {vertices[idxDest]}) adicionada com sucesso!");
            }
            else
            {
                Console.WriteLine($"Aresta ({vertices[idxOrig]} -> {vertices[idxDest]}) adicionada com sucesso!");
            }
        }

        public void DelAresta(string origem, string destino)
        {
            int idxOrig = ObterIndice(origem);
            int idxDest = ObterIndice(destino);

            if (idxOrig == -1 || idxDest == -1)
            {
                Console.WriteLine("Erro: Um ou ambos os vértices não foram encontrados no grafo.");
                return;
            }

            if (matriz[idxOrig, idxDest] == 0)
            {
                Console.WriteLine("AVISO: Não existe aresta entre esses vértices no sentido informado!");
                return;
            }

            matriz[idxOrig, idxDest] = 0;

            if (!ehDirecionado)
            {
                matriz[idxDest, idxOrig] = 0;
                Console.WriteLine($"Aresta ({vertices[idxOrig]} <-> {vertices[idxDest]}) removida com sucesso!");
            }
            else
            {
                Console.WriteLine($"Aresta ({vertices[idxOrig]} -> {vertices[idxDest]}) removida com sucesso!");
            }
        }

        public void GrauVertice()
        {
            if (vertices.Count == 0)
            {
                Console.WriteLine("O grafo está vazio.");
                return;
            }

            Console.WriteLine("\n--- GRAU DOS VÉRTICES ---");
            for (int i = 0; i < vertices.Count; i++)
            {
                if (ehDirecionado)
                {
                    int grauSaida = 0;
                    int grauEntrada = 0;

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
                    Console.WriteLine($"Vértice '{vertices[i]}': Grau {grau}");
                }
            }
            Console.WriteLine("-------------------------");
        }

        public bool ExisteAresta(string origem, string destino)
        {
            int idxOrig = ObterIndice(origem);
            int idxDest = ObterIndice(destino);

            if (idxOrig != -1 && idxDest != -1)
            {
                return matriz[idxOrig, idxDest] == 1;
            }

            Console.WriteLine("ERRO: Vértices não encontrados.");
            return false;
        }

        public void ExibirVizinhos(string nome)
        {
            int idx = ObterIndice(nome);
            if (idx == -1)
            {
                Console.WriteLine($"Erro: Vértice '{nome}' não encontrado!");
                return;
            }

            List<string> vizinhos = new List<string>();

            for (int j = 0; j < vertices.Count; j++)
            {
                if (matriz[idx, j] == 1)
                {
                    vizinhos.Add(vertices[j]);
                }
            }

            string labelVizinhos = ehDirecionado ? "Vizinhos de saída" : "Vizinhos";

            if (vizinhos.Count == 0)
            {
                Console.WriteLine($"{labelVizinhos} do vértice '{vertices[idx]}': Nenhum.");
            }
            else
            {
                Console.WriteLine($"{labelVizinhos} do vértice '{vertices[idx]}': {string.Join(", ", vizinhos)}");
            }
        }

        public bool VerificarPercurso(List<string> percursoNomes)
        {
            if (percursoNomes == null || percursoNomes.Count == 0)
            {
                return false;
            }

            List<int> percursoIndices = new List<int>();
            foreach (string nome in percursoNomes)
            {
                int idc = ObterIndice(nome);
                if (idc == -1) return false;
                percursoIndices.Add(idc);
            }

            for (int i = 0; i < percursoIndices.Count - 1; i++)
            {
                int atual = percursoIndices[i];
                int proximo = percursoIndices[i + 1];

                if (matriz[atual, proximo] == 0)
                {
                    return false;
                }
            }

            return true;
        }

        public void ImprimirMatriz()
        {
            if (vertices.Count == 0)
            {
                Console.WriteLine("\nO grafo está vazio.");
                return;
            }

            Console.WriteLine("\n--- MATRIZ DE ADJACÊNCIA ---");

            Console.Write("".PadRight(5));
            for (int j = 0; j < vertices.Count; j++)
            {
                Console.Write(vertices[j].PadRight(5));
            }
            Console.WriteLine();

            for (int i = 0; i < vertices.Count; i++)
            {
                Console.Write(vertices[i].PadRight(5));
                for (int j = 0; j < vertices.Count; j++)
                {
                    Console.Write(matriz[i, j].ToString().PadRight(5));
                }
                Console.WriteLine();
            }

            Console.WriteLine("----------------------------");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int op = 0;
            Grafo g = null;

            do
            {
                Console.WriteLine("\n=== MATRIZ DE ADJACÊNCIA ===");
                Console.WriteLine("[1] Criar um grafo");
                Console.WriteLine("[2] Adicionar e remover vértices");
                Console.WriteLine("[3] Adicionar e remover arestas");
                Console.WriteLine("[4] Calcular e exibir o grau de cada vértice");
                Console.WriteLine("[5] Verificar se existe uma aresta entre dois vértices");
                Console.WriteLine("[6] Listar todos os vizinhos de um vértice");
                Console.WriteLine("[7] Verificar se um determinado percurso é possível");
                Console.WriteLine("[8] Imprimir o grafo");
                Console.WriteLine("[9] Finalizar sistema");
                Console.Write("Escolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out op))
                {
                    Console.WriteLine("Opção inválida! Digite um número.");
                    continue;
                }

                switch (op)
                {
                    case 1:
                        Console.WriteLine("\nEscolha o tipo do grafo:");
                        Console.WriteLine("[1] Grafo Não Direcionado");
                        Console.WriteLine("[2] Grafo Direcionado");
                        Console.Write("Opção: ");

                        string tipoOp = Console.ReadLine()?.Trim();
                        bool ehDirecionado = tipoOp == "2";

                        g = new Grafo(ehDirecionado);

                        Console.Write("Deseja cadastrar vértices iniciais agora? (s/n): ");
                        string resp = Console.ReadLine()?.Trim().ToLower();

                        if (resp == "s")
                        {
                            Console.Write("Digite a quantidade de vértices iniciais: ");
                            if (int.TryParse(Console.ReadLine(), out int total) && total > 0)
                            {
                                for (int i = 0; i < total; i++)
                                {
                                    Console.Write($"Nome do vértice {i + 1}: ");
                                    string nomeV = Console.ReadLine()?.Trim();
                                    g.AddVertice(nomeV);
                                }
                            }
                        }

                        Console.WriteLine($"\nGrafo {(ehDirecionado ? "DIRECIONADO" : "NÃO DIRECIONADO")} criado com sucesso!");
                        break;

                    case 2:
                        if (!ValidarGrafo(g)) break;

                        Console.WriteLine("\n--- GERENCIAR VÉRTICES ---");
                        Console.WriteLine("[1] Adicionar Vértice");
                        Console.WriteLine("[2] Remover Vértice");
                        Console.Write("Escolha uma opção: ");

                        if (int.TryParse(Console.ReadLine(), out int subOpV))
                        {
                            if (subOpV == 1)
                            {
                                Console.Write("Digite o nome do vértice a adicionar: ");
                                string nomeAdd = Console.ReadLine()?.Trim();
                                g.AddVertice(nomeAdd);
                            }
                            else if (subOpV == 2)
                            {
                                Console.Write("Digite o nome do vértice a remover: ");
                                string nomeDel = Console.ReadLine()?.Trim();
                                g.DelVertice(nomeDel);
                            }
                            else
                            {
                                Console.WriteLine("Opção inválida!");
                            }
                        }
                        break;

                    case 3:
                        if (!ValidarGrafo(g)) break;

                        Console.WriteLine("\n--- GERENCIAR ARESTAS ---");
                        Console.WriteLine("[1] Adicionar Aresta");
                        Console.WriteLine("[2] Remover Aresta");
                        Console.Write("Escolha uma opção: ");

                        if (int.TryParse(Console.ReadLine(), out int subOpA))
                        {
                            Console.Write("Digite o vértice de origem: ");
                            string o = Console.ReadLine()?.Trim();
                            Console.Write("Digite o vértice de destino: ");
                            string d = Console.ReadLine()?.Trim();

                            if (subOpA == 1)
                            {
                                g.AddAresta(o, d);
                            }
                            else if (subOpA == 2)
                            {
                                g.DelAresta(o, d);
                            }
                            else
                            {
                                Console.WriteLine("Opção inválida!");
                            }
                        }
                        break;

                    case 4:
                        if (!ValidarGrafo(g)) break;
                        g.GrauVertice();
                        break;

                    case 5:
                        if (!ValidarGrafo(g)) break;

                        Console.Write("Digite o vértice de origem: ");
                        string orig = Console.ReadLine()?.Trim();
                        Console.Write("Digite o vértice de destino: ");
                        string dest = Console.ReadLine()?.Trim();

                        string relacao = g.IsDirecionado() ? $"{orig} -> {dest}" : $"{orig} <-> {dest}";

                        if (g.ExisteAresta(orig, dest))
                        {
                            Console.WriteLine($"Existe a aresta ({relacao}).");
                        }
                        else
                        {
                            Console.WriteLine($"NÃO existe a aresta ({relacao}).");
                        }
                        break;

                    case 6:
                        if (!ValidarGrafo(g)) break;

                        Console.Write("Exibir vizinhos de qual vértice? ");
                        string vertViz = Console.ReadLine()?.Trim();
                        g.ExibirVizinhos(vertViz);
                        break;

                    case 7:
                        if (!ValidarGrafo(g)) break;

                        Console.WriteLine("\n--- VERIFICAR PERCURSO ---");
                        Console.WriteLine("Digite os nomes dos vértices separados por vírgula (Exemplo: São Paulo, Rio, Curitiba)");
                        Console.Write("Percurso: ");

                        string entradaPercurso = Console.ReadLine()?.Trim();
                        if (string.IsNullOrWhiteSpace(entradaPercurso))
                        {
                            Console.WriteLine("Percurso inválido.");
                            break;
                        }

                        string[] partes = entradaPercurso.Split(',');
                        List<string> percurso = new List<string>();

                        foreach (string item in partes)
                        {
                            string nomeItem = item.Trim();
                            if (!string.IsNullOrEmpty(nomeItem))
                            {
                                percurso.Add(nomeItem);
                            }
                        }

                        if (percurso.Count == 0)
                        {
                            Console.WriteLine("Percurso inválido!");
                            break;
                        }

                        if (g.VerificarPercurso(percurso))
                        {
                            Console.WriteLine("O percurso informado é POSSÍVEL!");
                        }
                        else
                        {
                            Console.WriteLine("O percurso informado NÃO é possível!");
                        }
                        break;

                    case 8:
                        if (!ValidarGrafo(g)) break;
                        g.ImprimirMatriz();
                        break;

                    case 9:
                        Console.WriteLine("\nSistema finalizado. Até logo!");
                        break;

                    default:
                        Console.WriteLine("Opção inválida! Tente novamente.");
                        break;
                }
            }
            while (op != 9);
        }

        private static bool ValidarGrafo(Grafo g)
        {
            if (g == null)
            {
                Console.WriteLine("\nErro: Crie um grafo primeiro (Opção 1) antes de realizar operações!");
                return false;
            }
            return true;
        }
    }
}