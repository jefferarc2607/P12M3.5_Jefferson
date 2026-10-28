using SumaCRUD6133261.Models;

namespace SumaCRUD6133261
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editResultadoId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => await CargarLista());
        }

        private async Task CargarLista()
        {
            var resultados = await _dbService.GetResultados();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                listaResultados.ItemsSource = resultados;
            });
        }
        private async void btnCalcular_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNumero1.Text) || string.IsNullOrWhiteSpace(txtNumero2.Text))
            {
                await DisplayAlert("Error", "Por favor ingresa ambos números", "OK");
                return;
            }

            if (!double.TryParse(txtNumero1.Text, out double num1) || !double.TryParse(txtNumero2.Text, out double num2))
            {
                await DisplayAlert("Error", "Ingresa solo valores numéricos", "OK");
                return;
            }

            double suma = num1 + num2;

            if (_editResultadoId == 0)
            {
                await _dbService.CreateResultado(new Resultado
                {
                    Numero1 = txtNumero1.Text,
                    Numero2 = txtNumero2.Text,
                    Suma = suma.ToString()
                });
            }
            else
            {
                var resultadoAEditar = await _dbService.GetResultadoById(_editResultadoId);
                if (resultadoAEditar != null)
                {
                    resultadoAEditar.Numero1 = txtNumero1.Text;
                    resultadoAEditar.Numero2 = txtNumero2.Text;
                    resultadoAEditar.Suma = suma.ToString();
                    await _dbService.UpdateResultado(resultadoAEditar);
                }
            }

            LimpiarFormulario();
            await CargarLista();
        }

        private void listaResultados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Resultado resultadoSeleccionado)
            {
                _editResultadoId = resultadoSeleccionado.Id;
                txtNumero1.Text = resultadoSeleccionado.Numero1;
                txtNumero2.Text = resultadoSeleccionado.Numero2;

                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            var boton = (Button)sender;
            var resultado = (Resultado)boton.CommandParameter;

            bool confirmar = await DisplayAlert("Eliminar", $"¿Borrar la suma de {resultado.Numero1} + {resultado.Numero2}?", "Sí", "No");
            if (confirmar)
            {
                await _dbService.DeleteResultado(resultado);
                await CargarLista();
            }
        }

        private void LimpiarFormulario()
        {
            _editResultadoId = 0;
            txtNumero1.Text = string.Empty;
            txtNumero2.Text = string.Empty;
        }
    }
}