using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MobileTeste.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _nomeDigitado = string.Empty;
        public string NomeDigitado
        {
            get => _nomeDigitado;
            set
            {
                if (_nomeDigitado != value)
                {
                    _nomeDigitado = value;
                    OnPropertyChanged();
                    // Aqui notificamos que pode alterar o estado do botão
                    SalvarCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private string _filtro = string.Empty;
        public string Filtro
        {
            get => _filtro;
            set
            {
                if (_filtro != value)
                {
                    _filtro = value;
                    OnPropertyChanged();
                    FiltrarNomes();
                }
            }
        }

        public ObservableCollection<string> Nomes { get; } = new ObservableCollection<string>();

        public RelayCommand SalvarCommand { get; }

        public MainViewModel()
        {
            SalvarCommand = new RelayCommand(Salvar, PodeSalvar);
        }

      private void Salvar()
{
    System.Diagnostics.Debug.WriteLine("Botão clicado!");
            if (!string.IsNullOrWhiteSpace(NomeDigitado))
            {
                Nomes.Add(NomeDigitado.Trim());
                NomeDigitado = string.Empty;
                FiltrarNomes();
    }
}


        private bool PodeSalvar()
        {
            return !string.IsNullOrWhiteSpace(NomeDigitado);
        }
        public ObservableCollection<string> NomesFiltrados { get; } = new ObservableCollection<string>();
        private void FiltrarNomes()
        {
            var nomesFiltrados = Nomes.Where(n => string.IsNullOrWhiteSpace(Filtro) || n.Contains(Filtro, StringComparison.OrdinalIgnoreCase))
            .ToList();

            NomesFiltrados.Clear();
            foreach (var nome in nomesFiltrados)
            {
                NomesFiltrados.Add(nome);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nome = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute();

        public void Execute(object parameter) => _execute();

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
