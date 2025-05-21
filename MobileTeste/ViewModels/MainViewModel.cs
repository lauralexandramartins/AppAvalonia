using System;//biblioteca base
using System.Collections.ObjectModel; //fornece coleções observaveis, como ObservableCollection<T>
using System.ComponentModel; //fornece a interface InotifyPropertyChanged, Necessário para INotifyPropertyChanged, que permite que a UI reaja a mudanças nas propriedades do ViewModel.
using System.Linq;//Traz a LINQ (Language Integrated Query), que permite fazer consultas elegantes em coleções, como listas.
using System.Runtime.CompilerServices;//Necessário para usar [CallerMemberName], Isso faz com que o nome da propriedade que chamou OnPropertyChanged() seja preenchido automaticamente, sem você escrever manualmente.
using System.Windows.Input;//Fornece a interface ICommand, usada para comandos em botões e ações da UI. Permite ligar um botão a um método no ViewModel (ex: SalvarCommand).

namespace MobileTeste.ViewModels// Define o espaço de nomes, agrupando as classes relacionadas à lógica da interface (ViewModel) do app MobileTeste.
{
    public class MainViewModel : INotifyPropertyChanged //Classe publica declarada como MainViewModel e implementa a interface INotifyPropertyChanged
    {
        private string _nomeDigitado = string.Empty; //Campo privado que armazena o valor digitado. Começa com string.Empty (equivalente a "").
        public string NomeDigitado //propriedade pública ligada ao TextBox da interface. Ela representa o nome que o usuário digita em um campo de texto na interface gráfica.
        {
            get => _nomeDigitado; // retorna o valor atual 

            set //permite atualizar o valor atual
            {
                //eviat que o código fique em loop infinito, quando o nome digitado for diferente que está no valor armazenado ele executa
                if (_nomeDigitado != value)
                {
                    //a variavel _nomeDigitado recebe o valor dado a value
                    _nomeDigitado = value;
                    OnPropertyChanged(); //Notifica a interface que o campo nome mudou.
                    // Aqui notificamos que pode alterar o estado do botão
                    SalvarCommand.RaiseCanExecuteChanged(); //Quando o valor muda: Chama OnPropertyChanged() → avisa a interface que ela deve se atualizar. Chama RaiseCanExecuteChanged() → diz ao botão "Salvar" 
                    // para verificar se deve estar habilitado (baseado em PodeSalvar()).
                }
            }
        }
        private string _filtro = string.Empty; ///Campo privado que armazena o valor digitado. Começa com string.Empty (equivalente a "").

        public string Filtro //propriedade pública ligada ao TextBox da interface. Ela representa o nome que o usuário digita em um campo de texto na interface gráfica.
        {
            get => _filtro; //retorna o valor atual
            ///Esse é o setter: é chamado quando você altera o valor da propriedade Filtro
            set
            {
                //Se _filtro já é "ana" e você tentar fazer Filtro = "ana", o bloco não roda, evitando repetição.
                if (_filtro != value)
                {
                    //value é uma palavra-chave especial no set que representa o valor que está sendo atribuído.
                    _filtro = value;
                    OnPropertyChanged(); //Notifica a UI (interface gráfica) que a propriedade Filtro mudou. Notifica a UI (interface gráfica) que a propriedade Filtro mudou.
                    FiltrarNomes(); //Toda vez que o filtro muda, você quer mostrar apenas os nomes que combinam com ele.
                }
            }
        }
        //O que é ObservableCollection<T>?
        // É uma coleção que avisa automaticamente quando itens são adicionados, removidos ou alterados.
        // Muito usada em aplicações com interface gráfica que usam data binding, porque a UI escuta essas mudanças para atualizar a tela automaticamente.
        // Aqui, o tipo T é string, ou seja, essa coleção vai armazenar uma lista de textos (nomes).

        public ObservableCollection<string> Nomes { get; } = new ObservableCollection<string>(); //Quando o MainViewModel é criado, ele já terá uma lista Nomes pronta para ser usada. Essa lista vai guardar todos os nomes que o usuário cadastrar.
                                                                                                 //Como é ObservableCollection, qualquer alteração nessa lista vai refletir automaticamente na interface do usuário, que estiver ligada a essa proprieda

        public RelayCommand SalvarCommand { get; } //Serve para que o botão na interface saiba qual método executar e quando pode executar (habilitar/desabilitar). 
        // Tipo: RelayCommand — representa uma ação que a interface pode “disparar” (comando para o botão, por exemplo).
        //{ get; } — só tem getter, então você pode acessar essa propriedade, mas não pode substituir ela diretamente de fora da classe.

        public MainViewModel()
        {
            SalvarCommand = new RelayCommand(Salvar, PodeSalvar);
        }
        //É o construtor da classe MainViewModel.
        // O que o construtor faz? Inicializa objetos e propriedades quando a classe é criada.
        // Aqui, inicializa a propriedade SalvarCommand com um novo objeto RelayCommand.
        // new RelayCommand(Salvar, PodeSalvar) cria o comando que executa o método Salvar e usa o método PodeSalvar para saber se o comando está habilitado.

        private void Salvar()//Método privado que contém o que acontece quando o comando SalvarCommand for executado (por exemplo, quando clicar no botão "Salvar").
        {
            System.Diagnostics.Debug.WriteLine("Botão clicado!");//Escreve uma mensagem no console de depuração (útil para ver que o botão foi clicado durante o desenvolvimento).
            if (!string.IsNullOrWhiteSpace(NomeDigitado)) //Verifica se a string NomeDigitado não está vazia, nula ou só com espaços. Garante que só vai salvar nomes válidos.
            {
                Nomes.Add(NomeDigitado.Trim()); //adiciona o nome digitado à coleção Nomes. .Trim() remove espaços extras antes e depois do texto
                NomeDigitado = string.Empty; //Limpa o campo NomeDigitado (deixa a caixa de texto vazia para digitar outro nome).
                FiltrarNomes(); //Chama o método para atualizar a lista filtrada, já que o conteúdo mudou.
            }
        }


        private bool PodeSalvar() //Define quando o botão "Salvar" deve estar habilitado. Retorna true se NomeDigitado tem algum texto válido (não vazio, não só espaços). Essa lógica controla o estado do botão.
        {
            return !string.IsNullOrWhiteSpace(NomeDigitado); //Significa: “retornar o resultado” desta linha como o valor final do método.
                                                             // O método PodeSalvar() tem tipo bool, então ele precisa retornar um valor verdadeiro (true) ou falso (false).
                                                             // A palavra return indica exatamente isso: "o que vem depois será o valor final que o método vai entregar".
        }
        public ObservableCollection<string> NomesFiltrados { get; } = new ObservableCollection<string>(); //Declara outra coleção observável pública chamada NomesFiltrados.
        // Essa coleção vai conter os nomes que passam pelo filtro de busca (filtragem).
        // Inicializada já vazia, pronta para receber dados.


        private void FiltrarNomes() //Método que filtra os nomes da coleção original Nomes e atualiza NomesFiltrados. Sempre que o filtro muda ou a lista muda, chamamos ele para atualizar o que será mostrado
        {
            var nomesFiltrados = Nomes.Where(n => string.IsNullOrWhiteSpace(Filtro) || n.Contains(Filtro, StringComparison.OrdinalIgnoreCase))
            .ToList();
            //Usa LINQ (Where) para filtrar os nomes da lista original.
            // Condição: se o filtro está vazio, retorna todos os nomes.
            // Se tem filtro, só retorna nomes que contenham o texto do filtro, ignorando maiúsculas/minúsculas.
            // .ToList() converte o resultado para uma lista normal.

            NomesFiltrados.Clear();//Limpa a lista NomesFiltrados para atualizar do zero.
            foreach (var nome in nomesFiltrados)
            {
                NomesFiltrados.Add(nome);
            }
        }
        //diciona os nomes filtrados, um a um, em NomesFiltrados.Como NomesFiltrados é uma ObservableCollection, isso vai atualizar a interface.

        public event PropertyChangedEventHandler PropertyChanged;  //Declara um evento chamado PropertyChanged. É parte da interface INotifyPropertyChanged. Esse evento avisa a interface que alguma propriedade mudou, para que a UI atualize o que estiver ligado a essa propriedade.
        protected void OnPropertyChanged([CallerMemberName] string nome = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nome)); //Verifica se alguém está ouvindo o evento (?.). Se sim, dispara o evento dizendo qual propriedade mudou. 
            // Isso faz a interface saber que deve atualizar aquele dado na tela.
        }
    }
//Método que dispara o evento PropertyChanged.
// [CallerMemberName] é um atributo especial: se você chamar OnPropertyChanged() sem argumentos, o compilador já passa o nome da propriedade que chamou.
// Isso evita você ter que escrever o nome da propriedade manualmente, diminuindo erros.
    public class RelayCommand : ICommand // Essa classe serve para implementar comandos reutilizáveis e fáceis de ligar a botões na interface.
    {
        private readonly Action _execute; //o que o comando deve fazer (ação).
        private readonly Func<bool> _canExecute; //ondição que indica se o comando está habilitado (retorna true ou false).
        public event EventHandler CanExecuteChanged; //evento para avisar quando o estado do comando mudou (habilitado/desabilitado).

        public RelayCommand(Action execute, Func<bool> canExecute = null) //construtor
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute)); //ação que será executada ao disparar o comando (ex: método Salvar).
            _canExecute = canExecute; //função que retorna se o comando pode ser executado (ex: método PodeSalvar).


        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(); //Retorna true se o comando está habilitado. Se não tiver condição (_canExecute == null), sempre retorna true. 
        // Se tiver, chama a função _canExecute para decidir.




        public void Execute(object parameter) => _execute(); //Executa a ação atribuída (ex: chama o método Salvar).

        public void RaiseCanExecuteChanged() //Dispara o evento para avisar que o estado do comando pode ter mudado. Isso faz a interface atualizar o botão (habilitado/desabilitado).
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
