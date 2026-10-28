using CRUDSqlite6133261.Models;

namespace CRUDSqlite6133261
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editClienteId; 

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => await CargarLista()); 
        }

        private async Task CargarLista()
        {
            var clientes = await _dbService.GetClientes();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                listaClientes.ItemsSource = clientes;
            });
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                await DisplayAlert("Error", "Nombre y Email son obligatorios", "OK");
                return;
            }

            if (_editClienteId == 0)
            {
                await _dbService.CreateCliente(new Cliente
                {
                    NombreCliente = txtNombre.Text,
                    Correo = txtEmail.Text,
                    Telefono = txtTelefono.Text
                });
            }
            else
            {
                var clienteAEditar = await _dbService.GetClienteById(_editClienteId);
                if (clienteAEditar != null)
                {
                    clienteAEditar.NombreCliente = txtNombre.Text;
                    clienteAEditar.Correo = txtEmail.Text;
                    clienteAEditar.Telefono = txtTelefono.Text;
                    await _dbService.UpdateCliente(clienteAEditar);
                }
            }

            LimpiarFormulario();
            await CargarLista();
        }

        private void listaClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Cliente clienteSeleccionado)
            {
                _editClienteId = clienteSeleccionado.Id;
                txtNombre.Text = clienteSeleccionado.NombreCliente;
                txtEmail.Text = clienteSeleccionado.Correo;
                txtTelefono.Text = clienteSeleccionado.Telefono;
                ((CollectionView)sender).SelectedItem = null;
            }
        }

        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            var boton = (Button)sender;
            var cliente = (Cliente)boton.CommandParameter;

            bool confirmar = await DisplayAlert("Eliminar", $"¿Borrar a {cliente.NombreCliente}?", "Sí", "No");
            if (confirmar)
            {
                await _dbService.DeleteCliente(cliente);
                await CargarLista();
            }
        }

        private void btnLimpiar_Clicked(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            _editClienteId = 0;
            txtNombre.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtTelefono.Text = string.Empty;
        }
    }
}
