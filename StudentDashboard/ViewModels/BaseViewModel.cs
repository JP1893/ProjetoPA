using StudentDashBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace StudentPerformanceDashboard
{
    public partial class StudentPerformanceViewModel : INotifyPropertyChanged
    {
        #region Coleções principais / dados base

        public BusinessLayer.ProdutoCollection AllProdutos { get; set; } = new();

        public ObservableCollection<int> Years { get; } = new();

        public ObservableCollection<CategoriaProduto> CategoriaProdutos { get; } = new();
        public ObservableCollection<Pais> Paises { get; } = new();
        public ObservableCollection<Cidade> Cidades { get; } = new();
        public ObservableCollection<Produto> Produtos { get; } = new();
        public ObservableCollection<Cliente> Clientes { get; } = new();

        #endregion

        #region Gráfico 1 - Produtos por Estado

        // XAML:
        // ItemsSource="{Binding FilteredExamResults}"
        //
        // Mostra:
        // Vendidos / Em Stock / Inativos
        public ObservableCollection<SubjectExamResult> ExamResultsBySubject { get; } = new();
        public ObservableCollection<SubjectExamResult> FilteredExamResults { get; } = new();

        #endregion

        #region Gráfico 2 - Lucro por Ano e Categoria

        // XAML:
        // ItemsSource="{Binding ValoresGrafico}"
        //
        // Mostra:
        // Lucro por ano = PrecoVenda - PrecoCusto
        public ObservableCollection<ValorGrafico> ValoresGrafico { get; } = new();

        #endregion

        #region Gráfico 3 - Distribuição de Vendas

        // XAML:
        // ItemsSource="{Binding GenderParticipationPie}"
        //
        // Mostra:
        // Categoria = Todas -> vendas por categoria
        // Categoria específica -> vendas por produto
        public ObservableCollection<LabelValue> GenderParticipationPie { get; } = new();

        #endregion

        #region Gráfico 4 - Evolução de Vendas

        // XAML:
        // ItemsSource="{Binding ValoresGraficoVendas}"
        //
        // Mostra:
        // Total de vendas por ano = SUM(PrecoVenda)
        public ObservableCollection<ValorGrafico> ValoresGraficoVendas { get; } = new();

        #endregion

        #region Gráfico 5 - Participação nas Vendas

        // XAML:
        // ItemsSource="{Binding FilteredParticipationRates}"
        // ItemsSource="{Binding TaxaVendasGauge}"
        //
        // Mostra:
        // Percentagem de participação de cada categoria nas vendas
        public ObservableCollection<SubjectRate> ParticipationBySubject { get; } = new();
        public ObservableCollection<SubjectRate> FilteredParticipationRates { get; } = new();

        public ObservableCollection<LabelValue> TaxaVendasGauge { get; } = new();

        private string _taxaVendasNome = string.Empty;
        public string TaxaVendasNome
        {
            get => _taxaVendasNome;
            set
            {
                if (_taxaVendasNome != value)
                {
                    _taxaVendasNome = value;
                    OnPropertyChanged(nameof(TaxaVendasNome));
                }
            }
        }

        private double _taxaVendasValor;
        public double TaxaVendasValor
        {
            get => _taxaVendasValor;
            set
            {
                if (_taxaVendasValor != value)
                {
                    _taxaVendasValor = value;
                    OnPropertyChanged(nameof(TaxaVendasValor));
                }
            }
        }

        private SubjectRate? _selectedParticipationRate;
        public SubjectRate? SelectedParticipationRate
        {
            get => _selectedParticipationRate;
            private set
            {
                if (!Equals(_selectedParticipationRate, value))
                {
                    _selectedParticipationRate = value;
                    OnPropertyChanged(nameof(SelectedParticipationRate));
                }
            }
        }

        #endregion

        #region Gráfico 6 - Estado dos Produtos

        // XAML:
        // ItemsSource="{Binding GradeDistributions}"
        //
        // Mostra:
        // Vendidos / Stock / Inativos em percentagem
        public ObservableCollection<GradeDistribution> GradeDistributions { get; } = new();

        #endregion

        #region KPIs do topo

        private double _totalVendas;
        public double TotalVendas
        {
            get => _totalVendas;
            private set
            {
                if (_totalVendas != value)
                {
                    _totalVendas = value;
                    OnPropertyChanged(nameof(TotalVendas));
                }
            }
        }

        private double _lucro;
        public double Lucro
        {
            get => _lucro;
            private set
            {
                if (_lucro != value)
                {
                    _lucro = value;
                    OnPropertyChanged(nameof(Lucro));
                }
            }
        }

        private int _produtosVendidos;
        public int ProdutosVendidos
        {
            get => _produtosVendidos;
            private set
            {
                if (_produtosVendidos != value)
                {
                    _produtosVendidos = value;
                    OnPropertyChanged(nameof(ProdutosVendidos));
                }
            }
        }

        private int _stockDisponivel;
        public int StockDisponivel
        {
            get => _stockDisponivel;
            private set
            {
                if (_stockDisponivel != value)
                {
                    _stockDisponivel = value;
                    OnPropertyChanged(nameof(StockDisponivel));
                }
            }
        }

        #endregion

        #region Filtros selecionados

        private int _selectedYear = 2021;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    UpdateFilteredData();
                }
            }
        }

        private CategoriaProduto? _selectedCategoriaProduto;
        public CategoriaProduto? SelectedCategoriaProduto
        {
            get => _selectedCategoriaProduto;
            set
            {
                if (value is null || _selectedCategoriaProduto == value)
                    return;

                _selectedCategoriaProduto = value;
                OnPropertyChanged(nameof(SelectedCategoriaProduto));
                UpdateFilteredData();
            }
        }

        private Pais? _selectedPais;
        public Pais? SelectedPais
        {
            get => _selectedPais;
            set
            {
                if (value is null || _selectedPais == value)
                    return;

                _selectedPais = value;

                InitializeCidades(_selectedPais.PaisID);

                OnPropertyChanged(nameof(SelectedPais));
                OnPropertyChanged(nameof(SelectedCidade));
                UpdateFilteredData();
            }
        }

        private Cidade? _selectedCidade;
        public Cidade? SelectedCidade
        {
            get => _selectedCidade;
            set
            {
                if (value is null || _selectedCidade == value)
                    return;

                _selectedCidade = value;
                OnPropertyChanged(nameof(SelectedCidade));
                UpdateFilteredData();
            }
        }

        private Produto? _selectedProduto;
        public Produto? SelectedProduto
        {
            get => _selectedProduto;
            set
            {
                if (value is null || _selectedProduto == value)
                    return;

                _selectedProduto = value;
                OnPropertyChanged(nameof(SelectedProduto));
                UpdateFilteredData();
            }
        }

        private Cliente? _selectedCliente;
        public Cliente? SelectedCliente
        {
            get => _selectedCliente;
            set
            {
                if (value is null || _selectedCliente == value)
                    return;

                _selectedCliente = value;
                OnPropertyChanged(nameof(SelectedCliente));
                UpdateFilteredData();
            }
        }

        #endregion

        #region Dados antigos do template

        // Ainda existem porque o template original dependia deles.
        // Podes apagar mais tarde se confirmares que já não são usados no XAML.

        public ObservableCollection<StudentsPerYear> StudentsPerYear { get; } = new();
        public ObservableCollection<AverageSubjectScore> AverageSubjectScores { get; } = new();

        public StudentsByGradeAndGender StudentsByGradeAndGender { get; private set; } = new();
        private readonly Dictionary<int, StudentsByGradeAndGender> _genderTotalsByYear = new();

        public ObservableCollection<StudentParticipationRateByBranch> StudentParticipationRateByBranch { get; } = new();
        public ObservableCollection<ExaminationResultsByBranch> ExaminationResultsByBranch { get; } = new();

        public ObservableCollection<Subject> Subjects { get; } = new();
        public ObservableCollection<LabelValue> SubjectScoresOverYears { get; } = new();
        public ObservableCollection<LabelValue> SemesterGradeTrend { get; } = new();
        public ObservableCollection<LabelValue> FilteredSemesterTrend { get; } = new();

        private Subject? _selectedSubject;
        public Subject? SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                if (value is null || _selectedSubject == value)
                    return;

                _selectedSubject = value;
                OnPropertyChanged(nameof(SelectedSubject));
                UpdateFilteredData();
            }
        }

        private double _physEdScore;
        public double PhysEdScore
        {
            get => _physEdScore;
            private set
            {
                if (_physEdScore != value)
                {
                    _physEdScore = value;
                    OnPropertyChanged(nameof(PhysEdScore));
                }
            }
        }

        private double _englishScore;
        public double EnglishScore
        {
            get => _englishScore;
            private set
            {
                if (_englishScore != value)
                {
                    _englishScore = value;
                    OnPropertyChanged(nameof(EnglishScore));
                }
            }
        }

        private double _mathsScore;
        public double MathsScore
        {
            get => _mathsScore;
            private set
            {
                if (_mathsScore != value)
                {
                    _mathsScore = value;
                    OnPropertyChanged(nameof(MathsScore));
                }
            }
        }

        private double _scienceScore;
        public double ScienceScore
        {
            get => _scienceScore;
            private set
            {
                if (_scienceScore != value)
                {
                    _scienceScore = value;
                    OnPropertyChanged(nameof(ScienceScore));
                }
            }
        }

        #endregion

        #region Dicionários auxiliares

        private readonly Dictionary<string, Subject> _subjectsByName = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, CategoriaProduto> _categoriaProdutosByName = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Pais> _paisesByName = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Cidade> _cidadesByName = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Produto> _produtosByName = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, Cliente> _clientesByName = new(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Constructor

        public StudentPerformanceViewModel()
        {
            SeedSampleData();
            InitializeSubjects();

            InitializeCategoriaProdutos();
            InitializePaises();

            int paisID = 0;
            InitializeCidades(paisID);

            InitializeProdutos();
            InitializeClientes();

            InitializeCollections();

            SelectedSubject = _subjectsByName["Maths"];
            SelectedCategoriaProduto = _categoriaProdutosByName.Values.FirstOrDefault();
            SelectedPais = _paisesByName["All"];
            SelectedCidade = _cidadesByName["All"];
            SelectedProduto = _produtosByName["All"];
            SelectedCliente = _clientesByName["All"];

            UpdateFilteredData();
        }

        #endregion

        #region Property Changed Event

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}