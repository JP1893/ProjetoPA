using BusinessLayer;
using StudentDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentPerformanceDashboard
{
    public partial class StudentPerformanceViewModel
    {
        #region Inicialização de dados antigos / base do template

        private void SeedSampleData()
        {
            StudentsPerYear.Add(new StudentsPerYear { Year = 2021, Students = 650 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2022, Students = 690 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2023, Students = 730 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2024, Students = 715 });
            StudentsPerYear.Add(new StudentsPerYear { Year = 2025, Students = 700 });

            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2021,
                Scores = new SubjectScores { PhysEd = 82.5, English = 74.2, Maths = 79.1, Science = 76.8 }
            });

            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2022,
                Scores = new SubjectScores { PhysEd = 84.1, English = 75.5, Maths = 80.8, Science = 77.2 }
            });

            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2023,
                Scores = new SubjectScores { PhysEd = 85.7, English = 77.2, Maths = 82.5, Science = 77.8 }
            });

            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2024,
                Scores = new SubjectScores { PhysEd = 84.9, English = 76.1, Maths = 81.0, Science = 76.9 }
            });

            AverageSubjectScores.Add(new AverageSubjectScore
            {
                Year = 2025,
                Scores = new SubjectScores { PhysEd = 83.8, English = 75.0, Maths = 79.6, Science = 75.5 }
            });

            _genderTotalsByYear[2021] = new StudentsByGradeAndGender { Male = 320, Female = 300, Others = 30 };
            _genderTotalsByYear[2022] = new StudentsByGradeAndGender { Male = 330, Female = 320, Others = 40 };
            _genderTotalsByYear[2023] = new StudentsByGradeAndGender { Male = 340, Female = 340, Others = 50 };
            _genderTotalsByYear[2024] = new StudentsByGradeAndGender { Male = 335, Female = 330, Others = 50 };
            _genderTotalsByYear[2025] = new StudentsByGradeAndGender { Male = 325, Female = 325, Others = 50 };

            StudentsByGradeAndGender = _genderTotalsByYear[2021];

            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2021,
                Rates = new BranchRates { PhysEd = 95.0, Arts = 81.0, English = 75.5, Maths = 83.5, Science = 86.0 }
            });

            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2022,
                Rates = new BranchRates { PhysEd = 97.2, Arts = 83.0, English = 77.0, Maths = 85.2, Science = 87.5 }
            });

            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2023,
                Rates = new BranchRates { PhysEd = 98.8, Arts = 84.0, English = 78.1, Maths = 86.4, Science = 88.1 }
            });

            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2024,
                Rates = new BranchRates { PhysEd = 97.5, Arts = 83.2, English = 77.0, Maths = 85.0, Science = 86.8 }
            });

            StudentParticipationRateByBranch.Add(new StudentParticipationRateByBranch
            {
                Year = 2025,
                Rates = new BranchRates { PhysEd = 96.0, Arts = 82.0, English = 75.8, Maths = 83.6, Science = 85.2 }
            });

            ComputeExamResultsFromRatesAndScores();
        }

        private void ComputeExamResultsFromRatesAndScores()
        {
            ExaminationResultsByBranch.Clear();

            foreach (var yearInfo in StudentsPerYear)
            {
                var year = yearInfo.Year;
                int total = yearInfo.Students;

                var rates = StudentParticipationRateByBranch.FirstOrDefault(r => r.Year == year)?.Rates;
                var scores = AverageSubjectScores.FirstOrDefault(s => s.Year == year)?.Scores;

                if (rates == null || scores == null)
                    continue;

                var results = new Dictionary<string, ExaminationResult>();

                void AddFor(string subjectName, double participationRatePercent, double avgScore)
                {
                    int attended = (int)Math.Round(total * participationRatePercent / 100.0, MidpointRounding.AwayFromZero);
                    int notAttended = Math.Max(0, total - attended);

                    double passRate = Math.Clamp(0.4 + (avgScore / 100.0) * 0.58, 0.4, 0.98);
                    int pass = (int)Math.Round(attended * passRate, MidpointRounding.AwayFromZero);
                    int fail = Math.Max(0, attended - pass);

                    results[subjectName] = new ExaminationResult
                    {
                        Pass = pass,
                        Fail = fail,
                        NotAttended = notAttended
                    };
                }

                AddFor("PhysEd", rates.PhysEd, scores.PhysEd);
                AddFor("English", rates.English, scores.English);
                AddFor("Maths", rates.Maths, scores.Maths);
                AddFor("Science", rates.Science, scores.Science);

                ExaminationResultsByBranch.Add(new ExaminationResultsByBranch
                {
                    Year = year,
                    Results = results
                });
            }
        }

        #endregion

        #region Inicialização de filtros

        private void InitializeSubjects()
        {
            Subjects.Clear();

            string[] names = new[] { "All", "PhysEd", "English", "Maths", "Science" };

            foreach (var name in names)
            {
                var subject = new Subject { Name = name };
                _subjectsByName[name] = subject;
                Subjects.Add(subject);
            }
        }

        private void InitializeCategoriaProdutos()
        {
            CategoriaProdutos.Clear();
            _categoriaProdutosByName.Clear();

            var categoriaTodos = new StudentDashBoard.Models.CategoriaProduto
            {
                CategoriaID = 0,
                Name = "Todas"
            };

            _categoriaProdutosByName["Todas"] = categoriaTodos;
            CategoriaProdutos.Add(categoriaTodos);

            CategoriaProdutoCollection categoriaProdutos = BusinessLayer.CategoriaProduto.Listar();

            foreach (BusinessLayer.CategoriaProduto item in categoriaProdutos)
            {
                var categoriaProduto = new StudentDashBoard.Models.CategoriaProduto
                {
                    CategoriaID = item.CategoriaId,
                    Name = item.NomeCategoria
                };

                _categoriaProdutosByName[item.NomeCategoria] = categoriaProduto;
                CategoriaProdutos.Add(categoriaProduto);
            }
        }

        private void InitializePaises()
        {
            Paises.Clear();
            _paisesByName.Clear();

            var paisTodos = new StudentDashBoard.Models.Pais
            {
                PaisID = 0,
                Name = "All"
            };

            _paisesByName["All"] = paisTodos;
            Paises.Add(paisTodos);

            PaisCollection paises = BusinessLayer.Pais.Listar();

            foreach (BusinessLayer.Pais item in paises)
            {
                var pais = new StudentDashBoard.Models.Pais
                {
                    PaisID = item.PaisId,
                    Name = item.NomePais
                };

                _paisesByName[pais.Name] = pais;
                Paises.Add(pais);
            }
        }

        private void InitializeCidades(int paisID)
        {
            Cidades.Clear();
            _cidadesByName.Clear();

            var cidadeTodos = new StudentDashBoard.Models.Cidade
            {
                Name = "All"
            };

            _cidadesByName["All"] = cidadeTodos;
            Cidades.Add(cidadeTodos);

            if (paisID <= 0)
                return;

            CidadeCollection cidades = BusinessLayer.Cidade.Listar();

            IEnumerable<BusinessLayer.Cidade> cidadesPorPais = cidades
                .Where(c => c.PaisId == paisID);

            foreach (BusinessLayer.Cidade item in cidadesPorPais)
            {
                var cidade = new StudentDashBoard.Models.Cidade
                {
                    Name = item.NomeCidade
                };

                _cidadesByName[cidade.Name] = cidade;
                Cidades.Add(cidade);
            }
        }

        private void InitializeProdutos()
        {
            Produtos.Clear();
            _produtosByName.Clear();

            var produtoTodos = new StudentDashBoard.Models.Produto
            {
                Name = "All"
            };

            _produtosByName["All"] = produtoTodos;
            Produtos.Add(produtoTodos);

            AllProdutos = BusinessLayer.Produto.Listar();

            foreach (BusinessLayer.Produto item in AllProdutos)
            {
                var produto = new StudentDashBoard.Models.Produto
                {
                    Name = item.NomeProduto
                };

                _produtosByName[produto.Name] = produto;
                Produtos.Add(produto);
            }
        }

        private void InitializeClientes()
        {
            Clientes.Clear();
            _clientesByName.Clear();

            var clienteTodos = new StudentDashBoard.Models.Cliente
            {
                Name = "All"
            };

            _clientesByName["All"] = clienteTodos;
            Clientes.Add(clienteTodos);

            ClienteCollection clientes = BusinessLayer.Cliente.Listar();

            foreach (BusinessLayer.Cliente item in clientes)
            {
                var cliente = new StudentDashBoard.Models.Cliente
                {
                    Name = item.Nome
                };

                _clientesByName[cliente.Name] = cliente;
                Clientes.Add(cliente);
            }
        }

        private void InitializeCollections()
        {
            Years.Clear();

            var anosComVendas = AllProdutos
                .Where(p => p.DataVenda.HasValue)
                .Select(p => p.DataVenda.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            foreach (var year in anosComVendas)
            {
                Years.Add(year);
            }

            if (Years.Count > 0)
            {
                SelectedYear = Years[0];
            }
        }

        #endregion

        #region Atualização principal do dashboard

        private void UpdateFilteredData()
        {
            BuildProjections();

            AtualizarGraficoParticipacaoFiltrado();
            AtualizarGraficoProdutosPorEstadoFiltrado();
            AtualizarDadosAntigosDoTemplate();
        }

        private void BuildProjections()
        {
            if (AllProdutos == null)
                return;

            int categoriaId = ObterCategoriaSelecionadaId();
            int paisId = ObterPaisSelecionadoId();

            AtualizarKpis(categoriaId, paisId);

            BuildGraficoLucroPorAnoECategoria(categoriaId);
            BuildGraficoEvolucaoVendas(categoriaId, paisId);
            BuildGraficoDistribuicaoVendas(categoriaId, paisId);
            BuildGraficoParticipacaoNasVendas(categoriaId, paisId);
            BuildGraficoProdutosPorEstado(categoriaId, paisId);
            BuildGraficoEstadoDosProdutos(categoriaId, paisId);

            AtualizarDadosAntigosDoTemplate();
        }

        #endregion

        #region KPIs

        private void AtualizarKpis(int categoriaId, int paisId)
        {
            TotalVendas = AllProdutos.ObterTotalVendas(categoriaId, paisId);
            Lucro = AllProdutos.ObterLucro(categoriaId);

            ProdutosVendidos = AllProdutos.ObterProdutosVendidos(categoriaId, SelectedYear);
            StockDisponivel = AllProdutos.ObterStockDisponivel(categoriaId);
        }

        #endregion

        #region Gráfico 1 - Produtos por Estado

        private void BuildGraficoProdutosPorEstado(int categoriaId, int paisId)
        {
            ExamResultsBySubject.Clear();

            string produtoSelecionado = SelectedProduto?.Name ?? "All";

            bool temProdutoSelecionado = !string.Equals(
                produtoSelecionado,
                "All",
                StringComparison.OrdinalIgnoreCase);

            IEnumerable<BusinessLayer.Produto> produtosBase = AllProdutos.GetFilterProdutos(categoriaId, produtoSelecionado);

            //if (categoriaId > 0)
            //{
            //    produtosBase = produtosBase.Where(p => p.CategoriaId == categoriaId);
            //}

            //if (temProdutoSelecionado)
            //{
            //    produtosBase = produtosBase.Where(p =>
            //        string.Equals(p.NomeProduto, produtoSelecionado, StringComparison.OrdinalIgnoreCase));
            //}

            List<BusinessLayer.Produto> produtosFiltrados = produtosBase.ToList();

            if (temProdutoSelecionado)
            {
                AdicionarEstadoProduto(produtoSelecionado, produtosFiltrados, paisId);
            }
            else if (categoriaId > 0)
            {
                var produtosAgrupados = produtosFiltrados
                    .GroupBy(p => p.NomeProduto)
                    .OrderBy(g => g.Key);

                foreach (var grupo in produtosAgrupados)
                {
                    AdicionarEstadoProduto(grupo.Key, grupo, paisId);
                }
            }
            else
            {
                var categoriasAgrupadas = produtosFiltrados
                    .GroupBy(p => p.CategoriaId)
                    .OrderBy(g => g.Key);

                foreach (var grupo in categoriasAgrupadas)
                {
                    string nomeCategoria = CategoriaProdutos
                        .FirstOrDefault(c => c.CategoriaID == grupo.Key)?.Name
                        ?? $"Categoria {grupo.Key}";

                    AdicionarEstadoProduto(nomeCategoria, grupo, paisId);
                }
            }
        }

        private void AdicionarEstadoProduto(
            string nome,
            IEnumerable<BusinessLayer.Produto> produtos,
            int paisId)
        {
            List<BusinessLayer.Produto> lista = produtos.ToList();

            int vendidos = lista.Count(p =>
                p.DataVenda.HasValue &&
                p.DataVenda.Value.Year == SelectedYear &&
                (paisId == 0 || p.IsPaisId(paisId)));

            int emStock = lista
                .Where(p => p.Ativo && !p.DataVenda.HasValue)
                .Sum(p => p.Quantidade);

            int inativos = lista.Count(p => !p.Ativo);

            ExamResultsBySubject.Add(new SubjectExamResult
            {
                Subject = new Subject { Name = nome },
                Pass = vendidos,
                Fail = emStock,
                NotAttended = inativos
            });
        }

        private void AtualizarGraficoProdutosPorEstadoFiltrado()
        {
            FilteredExamResults.Clear();

            foreach (SubjectExamResult item in ExamResultsBySubject)
            {
                FilteredExamResults.Add(item);
            }
        }

        #endregion

        #region Gráfico 2 - Lucro por Ano e Categoria

        private void BuildGraficoLucroPorAnoECategoria(int categoriaId)
        {
            ValoresGrafico.Clear();

            LucroPorAnoCollection lucroPorAnos = AllProdutos.ObterLucroPorAno(categoriaId);

            if (lucroPorAnos == null)
                return;

            foreach (LucroPorAno lucroPorAno in lucroPorAnos)
            {
                ValoresGrafico.Add(new ValorGrafico
                {
                    ValorX = lucroPorAno.Ano.ToString(),
                    ValorY = (int)lucroPorAno.Lucro
                });
            }
        }

        #endregion

        #region Gráfico 3 - Distribuição de Vendas

        private void BuildGraficoDistribuicaoVendas(int categoriaId, int paisId)
        {
            GenderParticipationPie.Clear();

            var produtosVendidos = AllProdutos
                .Where(p => p.DataVenda.HasValue
                            && p.DataVenda.Value.Year == SelectedYear
                            && (paisId == 0 || p.IsPaisId(paisId)))
                .ToList();

            if (categoriaId > 0)
            {
                produtosVendidos = produtosVendidos
                    .Where(p => p.CategoriaId == categoriaId)
                    .ToList();

                var vendasPorProduto = produtosVendidos
                    .GroupBy(p => p.NomeProduto)
                    .Select(g => new LabelValue
                    {
                        Label = g.Key,
                        Value = (double)g.Sum(p => p.PrecoVenda)
                    })
                    .Where(x => x.Value > 0)
                    .OrderByDescending(x => x.Value);

                foreach (var item in vendasPorProduto)
                {
                    GenderParticipationPie.Add(item);
                }
            }
            else
            {
                var vendasPorCategoria = produtosVendidos
                    .GroupBy(p => p.CategoriaId)
                    .Select(g => new LabelValue
                    {
                        Label = CategoriaProdutos
                            .FirstOrDefault(c => c.CategoriaID == g.Key)?.Name
                            ?? $"Categoria {g.Key}",

                        Value = (double)g.Sum(p => p.PrecoVenda)
                    })
                    .Where(x => x.Value > 0)
                    .OrderByDescending(x => x.Value);

                foreach (var item in vendasPorCategoria)
                {
                    GenderParticipationPie.Add(item);
                }
            }
        }

        #endregion

        #region Gráfico 4 - Evolução de Vendas

        private void BuildGraficoEvolucaoVendas(int categoriaId, int paisId)
        {
            ValoresGraficoVendas.Clear();

            IEnumerable<BusinessLayer.Produto> produtosVendidos = AllProdutos
                .Where(p => p.DataVenda.HasValue);

            if (categoriaId > 0)
            {
                produtosVendidos = produtosVendidos
                    .Where(p => p.CategoriaId == categoriaId);
            }

            if (paisId > 0)
            {
                produtosVendidos = produtosVendidos
                    .Where(p => p.IsPaisId(paisId));
            }

            string produtoSelecionado = SelectedProduto?.Name ?? "All";

            if (!string.Equals(produtoSelecionado, "All", StringComparison.OrdinalIgnoreCase))
            {
                produtosVendidos = produtosVendidos
                    .Where(p => string.Equals(p.NomeProduto, produtoSelecionado, StringComparison.OrdinalIgnoreCase));
            }

            var vendasPorAno = produtosVendidos
                .GroupBy(p => p.DataVenda.Value.Year)
                .Select(g => new
                {
                    Ano = g.Key,
                    TotalVendas = g.Sum(p => p.PrecoVenda)
                })
                .OrderBy(x => x.Ano);

            foreach (var item in vendasPorAno)
            {
                ValoresGraficoVendas.Add(new ValorGrafico
                {
                    ValorX = item.Ano.ToString(),
                    ValorY = (int)item.TotalVendas
                });
            }
        }
        #endregion

        #region Gráfico 5 - Participação nas Vendas

        private void BuildGraficoParticipacaoNasVendas(int categoriaId, int paisId)
        {
            ParticipationBySubject.Clear();
            TaxaVendasGauge.Clear();

            var produtosVendidosAno = AllProdutos
                .Where(p => p.DataVenda.HasValue
                            && p.DataVenda.Value.Year == SelectedYear
                            && (paisId == 0 || p.IsPaisId(paisId)))
                .ToList();

            double totalVendasAno = (double)produtosVendidosAno.Sum(p => p.PrecoVenda);

            IEnumerable<StudentDashBoard.Models.CategoriaProduto> categoriasParaMostrar =
                CategoriaProdutos.Where(c => c.CategoriaID > 0);

            if (categoriaId > 0)
            {
                categoriasParaMostrar = categoriasParaMostrar
                    .Where(c => c.CategoriaID == categoriaId);
            }

            foreach (var categoria in categoriasParaMostrar)
            {
                double vendasCategoria = (double)produtosVendidosAno
                    .Where(p => p.CategoriaId == categoria.CategoriaID)
                    .Sum(p => p.PrecoVenda);

                double percentagem = totalVendasAno > 0
                    ? Math.Round((vendasCategoria / totalVendasAno) * 100, 1)
                    : 0;

                ParticipationBySubject.Add(new SubjectRate
                {
                    Subject = new Subject { Name = categoria.Name },
                    Rate = percentagem
                });
            }

            if (categoriaId > 0 && ParticipationBySubject.Count > 0)
            {
                var item = ParticipationBySubject[0];

                TaxaVendasNome = item.Subject.Name;
                TaxaVendasValor = item.Rate;

                TaxaVendasGauge.Add(new LabelValue
                {
                    Label = item.Subject.Name,
                    Value = item.Rate
                });

                TaxaVendasGauge.Add(new LabelValue
                {
                    Label = "Restante",
                    Value = Math.Max(0, 100 - item.Rate)
                });
            }
            else
            {
                TaxaVendasNome = "Todas";
                TaxaVendasValor = 100;
            }
        }

        private void AtualizarGraficoParticipacaoFiltrado()
        {
            FilteredParticipationRates.Clear();

            foreach (var item in ParticipationBySubject)
            {
                FilteredParticipationRates.Add(item);
            }

            SelectedParticipationRate = FilteredParticipationRates.Count == 1
                ? FilteredParticipationRates[0]
                : null;
        }

        #endregion

        #region Gráfico 6 - Estado dos Produtos

        private void BuildGraficoEstadoDosProdutos(int categoriaId, int paisId)
        {
            GradeDistributions.Clear();

            var produtosCategoria = AllProdutos
                .Where(p => categoriaId == 0 || p.CategoriaId == categoriaId)
                .ToList();

            int vendidos = produtosCategoria
                .Count(p => p.DataVenda.HasValue
                            && p.DataVenda.Value.Year == SelectedYear
                            && (paisId == 0 || p.IsPaisId(paisId)));

            int emStock = produtosCategoria
                .Count(p => p.Ativo && !p.DataVenda.HasValue);

            int inativos = produtosCategoria
                .Count(p => !p.Ativo);

            int total = vendidos + emStock + inativos;

            int percentVendidos = total > 0
                ? (int)Math.Round((double)vendidos * 100 / total)
                : 0;

            int percentStock = total > 0
                ? (int)Math.Round((double)emStock * 100 / total)
                : 0;

            int percentInativos = total > 0
                ? 100 - percentVendidos - percentStock
                : 0;

            GradeDistributions.Add(new GradeDistribution
            {
                Grade = "Vendidos",
                Percentage = percentVendidos,
                Color = "#00C851"
            });

            GradeDistributions.Add(new GradeDistribution
            {
                Grade = "Stock",
                Percentage = percentStock,
                Color = "#2196F3"
            });

            GradeDistributions.Add(new GradeDistribution
            {
                Grade = "Inativos",
                Percentage = percentInativos,
                Color = "#F44336"
            });
        }

        #endregion

        #region Métodos auxiliares

        private int ObterCategoriaSelecionadaId()
        {
            return SelectedCategoriaProduto?.CategoriaID ?? 0;
        }

        private int ObterPaisSelecionadoId()
        {
            return SelectedPais?.PaisID ?? 0;
        }

        private static bool IsAll(Subject subject)
        {
            return subject != null &&
                   string.Equals(subject.Name, "All", StringComparison.OrdinalIgnoreCase);
        }

        private static double GetRateBySubject(BranchRates rates, string subject) => subject switch
        {
            "PhysEd" => rates.PhysEd,
            "Arts" => rates.Arts,
            "English" => rates.English,
            "Maths" => rates.Maths,
            "Science" => rates.Science,
            _ => rates.Maths
        };

        private double GetScoreBySubject(SubjectScores scores, string subject) => subject switch
        {
            "PhysEd" => scores.PhysEd,
            "English" => scores.English,
            "Maths" => scores.Maths,
            "Science" => scores.Science,
            _ => scores.Maths
        };

        #endregion

        #region Código antigo do template

        private void AtualizarDadosAntigosDoTemplate()
        {
            var avgScores = AverageSubjectScores
                .FirstOrDefault(s => s.Year == SelectedYear)?.Scores
                ?? new SubjectScores();

            PhysEdScore = avgScores.PhysEd;

            if (_genderTotalsByYear.TryGetValue(SelectedYear, out var yearGender))
            {
                StudentsByGradeAndGender = yearGender;
            }

            SubjectScoresOverYears.Clear();

            var subjectForTrend = SelectedSubject is null || IsAll(SelectedSubject)
                ? "Maths"
                : SelectedSubject.Name;

            foreach (var score in AverageSubjectScores.OrderBy(s => s.Year))
            {
                double val = GetScoreBySubject(score.Scores, subjectForTrend);

                SubjectScoresOverYears.Add(new LabelValue
                {
                    Label = score.Year.ToString(),
                    Value = val
                });
            }

            SemesterGradeTrend.Clear();

            double baseScore = GetScoreBySubject(avgScores, subjectForTrend);

            for (int i = 1; i <= 6; i++)
            {
                double decay = i * Math.Max(0.6, baseScore * 0.012);
                double variation = Math.Sin(i) * 0.8;
                double value = Math.Max(0, baseScore - decay + variation);

                SemesterGradeTrend.Add(new LabelValue
                {
                    Label = i.ToString(),
                    Value = value
                });
            }

            FilteredSemesterTrend.Clear();

            foreach (var item in SemesterGradeTrend)
            {
                item.Value = Math.Round(item.Value, 1);
                FilteredSemesterTrend.Add(item);
            }
        }

        private void BuildGradeDistribution(double average)
        {
            average = Math.Max(0, Math.Min(100, average));

            const double sd = 12.0;

            static double Phi(double x)
            {
                double t = 1.0 / (1.0 + 0.2316419 * Math.Abs(x));
                double d = Math.Exp(-x * x / 2.0) / Math.Sqrt(2.0 * Math.PI);

                double p = 1 - d * (
                    0.319381530 * t
                    - 0.356563782 * Math.Pow(t, 2)
                    + 1.781477937 * Math.Pow(t, 3)
                    - 1.821255978 * Math.Pow(t, 4)
                    + 1.330274429 * Math.Pow(t, 5));

                return x >= 0 ? p : 1 - p;
            }

            double Cdf(double score) => Phi((score - average) / sd);

            double pBelow60 = Cdf(60);
            double pBelow70 = Cdf(70);
            double pBelow80 = Cdf(80);
            double pBelow90 = Cdf(90);

            double pD = Math.Max(0, pBelow70 - pBelow60);
            double pC = Math.Max(0, pBelow80 - pBelow70);
            double pB = Math.Max(0, pBelow90 - pBelow80);
            double pA = Math.Max(0, 1 - pBelow90);

            int iA = (int)Math.Round(pA * 100);
            int iB = (int)Math.Round(pB * 100);
            int iC = (int)Math.Round(pC * 100);
            int iD = (int)Math.Round(pD * 100);
            int iF = 100 - (iA + iB + iC + iD);

            GradeDistributions.Clear();
            GradeDistributions.Add(new GradeDistribution { Grade = "A", Percentage = iA, Color = "#00C851" });
            GradeDistributions.Add(new GradeDistribution { Grade = "B", Percentage = iB, Color = "#2196F3" });
            GradeDistributions.Add(new GradeDistribution { Grade = "C", Percentage = iC, Color = "#FF9800" });
            GradeDistributions.Add(new GradeDistribution { Grade = "D", Percentage = iD, Color = "#F44336" });
            GradeDistributions.Add(new GradeDistribution { Grade = "F", Percentage = iF, Color = "#9C27B0" });
        }

        #endregion
    }
}